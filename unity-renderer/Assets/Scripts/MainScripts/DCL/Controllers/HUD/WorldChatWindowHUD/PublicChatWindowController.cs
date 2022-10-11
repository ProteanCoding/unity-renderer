using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DCL;
using DCL.Chat;
using DCL.Interface;
using SocialFeaturesAnalytics;
using UnityEngine;
using System.Collections.Generic;

public class PublicChatWindowController : IHUD
{
    public IPublicChatWindowView View { get; private set; }
    
    private const int FADEOUT_DELAY = 30000;

    public event Action OnBack;
    public event Action OnClosed;

    private readonly IChatController chatController;
    private readonly IUserProfileBridge userProfileBridge;
    private readonly DataStore dataStore;
    private readonly IProfanityFilter profanityFilter;
    private readonly IMouseCatcher mouseCatcher;
    private readonly InputAction_Trigger toggleChatTrigger;
    private ChatHUDController chatHudController;
    private string channelId;
    private CancellationTokenSource deactivateFadeOutCancellationToken = new CancellationTokenSource();

    private bool skipChatInputTrigger;
    private string lastPrivateMessageRecipient = string.Empty;

    private UserProfile ownProfile => userProfileBridge.GetOwn();
    internal BaseVariable<HashSet<string>> visibleTaskbarPanels => dataStore.HUDs.visibleTaskbarPanels;

    public PublicChatWindowController(IChatController chatController,
        IUserProfileBridge userProfileBridge,
        DataStore dataStore,
        IProfanityFilter profanityFilter,
        IMouseCatcher mouseCatcher,
        InputAction_Trigger toggleChatTrigger)
    {
        this.chatController = chatController;
        this.userProfileBridge = userProfileBridge;
        this.dataStore = dataStore;
        this.profanityFilter = profanityFilter;
        this.mouseCatcher = mouseCatcher;
        this.toggleChatTrigger = toggleChatTrigger;
    }

    public void Initialize(IPublicChatWindowView view = null)
    {
        view ??= PublicChatWindowComponentView.Create();
        View = view;
        view.OnClose += HandleViewClosed;
        view.OnBack += HandleViewBacked;

        if (mouseCatcher != null)
            mouseCatcher.OnMouseLock += Hide;

        chatHudController = new ChatHUDController(dataStore,
            userProfileBridge,
            true,
            profanityFilter);
        chatHudController.Initialize(view.ChatHUD);
        chatHudController.OnSendMessage += SendChatMessage;

        chatController.OnAddMessage -= HandleMessageReceived;
        chatController.OnAddMessage += HandleMessageReceived;

        toggleChatTrigger.OnTriggered += HandleChatInputTriggered;
    }

    public void Setup(string channelId)
    {
        if (string.IsNullOrEmpty(channelId) || channelId == this.channelId) return;
        this.channelId = channelId;

        var channel = chatController.GetAllocatedChannel(channelId);
        View.Configure(new PublicChatModel(this.channelId,
            channel.Name,
            channel.Description,
            channel.Joined,
            channel.MemberCount,
            false));

        ReloadAllChats().Forget();
    }

    public void Dispose()
    {
        View.OnClose -= HandleViewClosed;
        View.OnBack -= HandleViewBacked;

        if (chatController != null)
            chatController.OnAddMessage -= HandleMessageReceived;

        chatHudController.OnSendMessage -= SendChatMessage;

        if (mouseCatcher != null)
            mouseCatcher.OnMouseLock -= Hide;
        
        toggleChatTrigger.OnTriggered -= HandleChatInputTriggered;

        if (View != null)
        {
            View.Dispose();
        }
    }

    private void SendChatMessage(ChatMessage message)
    {
        var isValidMessage = !string.IsNullOrEmpty(message.body) && !string.IsNullOrWhiteSpace(message.body);
        var isPrivateMessage = message.messageType == ChatMessage.Type.PRIVATE;

        if (isValidMessage)
        {
            chatHudController.ResetInputField();
            chatHudController.FocusInputField();
        }
        else
        {
            HandleViewClosed();
            SetVisibility(false);
            return;
        }

        if (isPrivateMessage)
            message.body = $"/w {message.recipient} {message.body}";

        chatController.Send(message);
    }
    
    public void SetVisibility(bool visible, bool focusInputField)
    {
        SetVisiblePanelList(visible);
        if (visible)
        {
            View.Show();
            MarkChannelMessagesAsRead();
            
            if (focusInputField)
                chatHudController.FocusInputField();
        }
        else
        {   
            chatHudController.UnfocusInputField();
            View.Hide();
        }
    }

    private void SetVisiblePanelList(bool visible)
    {
        HashSet<string> newSet = visibleTaskbarPanels.Get();
        if (visible)
            newSet.Add("PublicChatChannel");
        else 
            newSet.Remove("PublicChatChannel");

        visibleTaskbarPanels.Set(newSet, true);
    }

    public void SetVisibility(bool visible) => SetVisibility(visible, false);

    private async UniTaskVoid ReloadAllChats()
    {
        chatHudController.ClearAllEntries();

        const int entriesPerFrame = 10;
        // TODO: filter entries by channelId
        var list = chatController.GetAllocatedEntries();
        if (list.Count == 0) return;

        for (var i = list.Count - 1; i >= 0; i--)
        {
            var message = list[i];
            if (i % entriesPerFrame == 0) await UniTask.NextFrame();
            HandleMessageReceived(message);
        }
    }

    internal void MarkChannelMessagesAsRead() => chatController.MarkChannelMessagesAsSeen(channelId);

    private void HandleViewClosed()
    {
        OnClosed?.Invoke();
    }

    private void HandleViewBacked() 
    {
        OnBack?.Invoke(); 
    }

    private void HandleMessageReceived(ChatMessage message)
    {
        if (message.messageType != ChatMessage.Type.PUBLIC
            && message.messageType != ChatMessage.Type.SYSTEM) return;
        if (!string.IsNullOrEmpty(message.recipient)) return;

        chatHudController.AddChatMessage(message, View.IsActive);

        if (View.IsActive)
            MarkChannelMessagesAsRead();
    }
    
    private async UniTaskVoid WaitThenFadeOutMessages(CancellationToken cancellationToken)
    {
        await UniTask.SwitchToMainThread(cancellationToken);
        if (cancellationToken.IsCancellationRequested) return;
        chatHudController.FadeOutMessages();
    }

    public void Hide()
    {
        SetVisibility(false);
    }

    private void HandleChatInputTriggered(DCLAction_Trigger action)
    {
        if (!View.IsActive) return;
        chatHudController.FocusInputField();
    }
}