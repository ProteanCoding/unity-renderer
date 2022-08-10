using DCL;
using DCL.Chat.Channels;
using DCL.Chat.HUD;
using DCL.HelpAndSupportHUD;
using DCL.Huds.QuestsPanel;
using DCL.Huds.QuestsTracker;
using DCL.Interface;
using DCL.SettingsCommon;
using DCL.SettingsPanelHUD;
using SignupHUD;
using SocialFeaturesAnalytics;
using UnityEngine;

public class HUDFactory : IHUDFactory
{
    public virtual IHUD CreateHUD(HUDElementID hudElementId)
    {
        IHUD hudElement = null;
        switch (hudElementId)
        {
            case HUDElementID.NONE:
                break;
            case HUDElementID.MINIMAP:
                hudElement = new MinimapHUDController();
                break;
            case HUDElementID.PROFILE_HUD:
                hudElement = new ProfileHUDController(new UserProfileWebInterfaceBridge());
                break;
            case HUDElementID.NOTIFICATION:
                hudElement = new NotificationHUDController();
                break;
            case HUDElementID.AVATAR_EDITOR:
                hudElement = new AvatarEditorHUDController(DataStore.i.featureFlags,
                    Environment.i.platform.serviceProviders.analytics);
                break;
            case HUDElementID.SETTINGS_PANEL:
                hudElement = new SettingsPanelHUDController();
                break;
            case HUDElementID.PLAYER_INFO_CARD:
                hudElement = new PlayerInfoCardHUDController(
                    // TODO (lazy loading): Pass FriendsController.i after kernel integration
                    new LazyLoadingFriendsControllerMock(FriendsController.i,
                        UserProfileController.i),
                    Resources.Load<StringVariable>("CurrentPlayerInfoCardId"),
                    new UserProfileWebInterfaceBridge(),
                    new WearablesCatalogControllerBridge(),
                    new SocialAnalytics(
                        Environment.i.platform.serviceProviders.analytics,
                        new UserProfileWebInterfaceBridge()),
                    ProfanityFilterSharedInstances.regexFilter,
                    DataStore.i,
                    CommonScriptableObjects.playerInfoCardVisibleState);
                break;
            case HUDElementID.AIRDROPPING:
                hudElement = new AirdroppingHUDController();
                break;
            case HUDElementID.TERMS_OF_SERVICE:
                hudElement = new TermsOfServiceHUDController();
                break;
            case HUDElementID.FRIENDS:
                hudElement = new FriendsHUDController(DataStore.i,
                    // TODO (lazy loading): Pass FriendsController.i after kernel integration
                    new LazyLoadingFriendsControllerMock(FriendsController.i, UserProfileController.i),
                    new UserProfileWebInterfaceBridge(),
                    new SocialAnalytics(
                        Environment.i.platform.serviceProviders.analytics,
                        new UserProfileWebInterfaceBridge()),
                    // TODO (channels): Pass ChatController.i after kernel integration
                    new ChatChannelsControllerMock(ChatController.i, UserProfileController.i));
                break;
            case HUDElementID.WORLD_CHAT_WINDOW:
                hudElement = new WorldChatWindowController(
                    new UserProfileWebInterfaceBridge(),
                    // TODO (lazy loading): Pass FriendsController.i after kernel integration
                    new LazyLoadingFriendsControllerMock(FriendsController.i, UserProfileController.i),
                    // TODO (channels): Pass ChatController.i after kernel integration
                    new ChatChannelsControllerMock(ChatController.i, UserProfileController.i));
                break;
            case HUDElementID.PRIVATE_CHAT_WINDOW:
                hudElement = new PrivateChatWindowController(
                    DataStore.i,
                    new UserProfileWebInterfaceBridge(),
                    // TODO (channels): Pass ChatController.i after kernel integration
                    new ChatChannelsControllerMock(ChatController.i, UserProfileController.i),
                    // TODO (lazy loading): Pass FriendsController.i after kernel integration
                    new LazyLoadingFriendsControllerMock(FriendsController.i, UserProfileController.i),
                    new SocialAnalytics(
                        Environment.i.platform.serviceProviders.analytics,
                        new UserProfileWebInterfaceBridge()),
                    SceneReferences.i.mouseCatcher,
                    Resources.Load<InputAction_Trigger>("ToggleWorldChat"));
                break;
            case HUDElementID.PUBLIC_CHAT:
                hudElement = new PublicChatWindowController(
                    // TODO (channels): Pass ChatController.i after kernel integration
                    new ChatChannelsControllerMock(ChatController.i, UserProfileController.i),
                    new UserProfileWebInterfaceBridge(),
                    DataStore.i,
                    ProfanityFilterSharedInstances.regexFilter,
                    SceneReferences.i.mouseCatcher,
                    Resources.Load<InputAction_Trigger>("ToggleWorldChat"));
                break;
            case HUDElementID.CHANNELS_CHAT:
                hudElement = new ChatChannelHUDController(
                    DataStore.i,
                    new UserProfileWebInterfaceBridge(),
                    // TODO (channels): Pass ChatController.i after kernel integration
                    new ChatChannelsControllerMock(ChatController.i, UserProfileController.i),
                    SceneReferences.i.mouseCatcher,
                    Resources.Load<InputAction_Trigger>("ToggleWorldChat"));
                break;
            case HUDElementID.CHANNELS_SEARCH:
                hudElement = new SearchChannelsWindowController(
                    // TODO (channels): Pass ChatController.i after kernel integration
                    new ChatChannelsControllerMock(ChatController.i, UserProfileController.i));
                break;
            case HUDElementID.CHANNELS_CREATE:
                hudElement = new CreateChannelWindowController(
                    // TODO (channels): Pass ChatController.i after kernel integration
                    new ChatChannelsControllerMock(ChatController.i, UserProfileController.i));
                break;
            case HUDElementID.CHANNELS_LEAVE_CONFIRMATION:
                hudElement = new LeaveChannelConfirmationWindowController(
                    // TODO (channels): Pass ChatController.i after kernel integration
                    new ChatChannelsControllerMock(ChatController.i, UserProfileController.i));
                break;
            case HUDElementID.TASKBAR:
                hudElement = new TaskbarHUDController();
                break;
            case HUDElementID.OPEN_EXTERNAL_URL_PROMPT:
                hudElement = new ExternalUrlPromptHUDController();
                break;
            case HUDElementID.NFT_INFO_DIALOG:
                hudElement = new NFTPromptHUDController();
                break;
            case HUDElementID.TELEPORT_DIALOG:
                hudElement = new TeleportPromptHUDController();
                break;
            case HUDElementID.CONTROLS_HUD:
                hudElement = new ControlsHUDController();
                break;
            case HUDElementID.HELP_AND_SUPPORT_HUD:
                hudElement = new HelpAndSupportHUDController();
                break;
            case HUDElementID.USERS_AROUND_LIST_HUD:
                hudElement = new VoiceChatWindowController(
                    new UserProfileWebInterfaceBridge(),
                    FriendsController.i,
                    new SocialAnalytics(
                        Environment.i.platform.serviceProviders.analytics,
                        new UserProfileWebInterfaceBridge()),
                    DataStore.i,
                    Settings.i);
                break;
            case HUDElementID.GRAPHIC_CARD_WARNING:
                hudElement = new GraphicCardWarningHUDController();
                break;
            case HUDElementID.BUILDER_IN_WORLD_MAIN:
                break;
            case HUDElementID.QUESTS_PANEL:
                hudElement = new QuestsPanelHUDController();
                break;
            case HUDElementID.QUESTS_TRACKER:
                hudElement = new QuestsTrackerHUDController();
                break;
            case HUDElementID.SIGNUP:
                hudElement = new SignupHUDController();
                break;
            case HUDElementID.BUILDER_PROJECTS_PANEL:
                break;
            case HUDElementID.LOADING:
                hudElement = new LoadingHUDController();
                break;
        }

        return hudElement;
    }

    public void Dispose()
    {
    }

    public void Initialize()
    {
    }
}