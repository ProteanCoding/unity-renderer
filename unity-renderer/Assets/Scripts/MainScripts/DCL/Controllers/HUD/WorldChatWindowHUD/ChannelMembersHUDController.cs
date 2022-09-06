using Cysharp.Threading.Tasks;
using DCL.Chat.Channels;
using System;
using System.Threading;

namespace DCL.Chat.HUD
{
    public class ChannelMembersHUDController : IDisposable
    {
        private const int LOAD_TIMEOUT = 2;
        private const int LOAD_PAGE_SIZE = 30;
        private const int MINUTES_FOR_AUTOMATIC_RELOADING = 5;
        private readonly IChatController chatController;
        private IChannelMembersComponentView view;
        private DateTime loadStartedTimestamp = DateTime.MinValue;
        private CancellationTokenSource loadingCancellationToken = new CancellationTokenSource();
        private CancellationTokenSource reloadingCancellationToken = new CancellationTokenSource();
        private string currentChannelId;
        private int lastLimitRequested;
        private bool isSearching;
        private bool isVisible;

        public IChannelMembersComponentView View => view;

        public ChannelMembersHUDController(IChannelMembersComponentView view, IChatController chatController)
        {
            this.view = view;
            this.chatController = chatController;
        }

        public void SetChannelId(string channelId)
        {
            if (string.IsNullOrEmpty(channelId))
                return;

            currentChannelId = channelId;
            lastLimitRequested = LOAD_PAGE_SIZE;

            if (isVisible)
            {
                LoadMembers();
                SetAutomaticReloadingActive(true);
            }
        }

        public void Dispose()
        {
            ClearListeners();
            view.Dispose();
            loadingCancellationToken.Cancel();
            loadingCancellationToken.Dispose();
            reloadingCancellationToken.Cancel();
            reloadingCancellationToken.Dispose();
        }

        public void SetVisibility(bool visible)
        {
            isVisible = visible;

            if (visible)
            {
                LoadMembers();
                SetAutomaticReloadingActive(true);
            }
            else
            {
                SetAutomaticReloadingActive(false);
                ClearListeners();
                view.Hide();
            }
        }

        private void LoadMembers()
        {
            ClearListeners();

            view.ClearSearchInput();

            view.OnSearchUpdated += SearchMembers;
            view.OnRequestMoreMembers += LoadMoreMembers;
            chatController.OnUpdateChannelMembers += UpdateChannelMembers;

            view.Show();
            view.ClearAllEntries();
            view.ShowLoading();

            loadStartedTimestamp = DateTime.Now;
            chatController.GetChannelInfo(currentChannelId);
            chatController.GetChannelMembers(currentChannelId, lastLimitRequested, 0);

            loadingCancellationToken.Cancel();
            loadingCancellationToken = new CancellationTokenSource();
            WaitTimeoutThenHideLoading(loadingCancellationToken.Token).Forget();
        }

        private void SearchMembers(string searchText)
        {
            loadStartedTimestamp = DateTime.Now;
            view.ClearAllEntries();
            view.HideLoadingMore();
            view.ShowLoading();

            isSearching = !string.IsNullOrEmpty(searchText);

            if (string.IsNullOrEmpty(searchText))
            {
                chatController.GetChannelMembers(currentChannelId, lastLimitRequested, 0);
                SetAutomaticReloadingActive(true);
            }
            else
            {
                chatController.GetChannelMembers(currentChannelId, LOAD_PAGE_SIZE, 0, searchText);
                SetAutomaticReloadingActive(false);
            }

            loadingCancellationToken.Cancel();
            loadingCancellationToken = new CancellationTokenSource();
            WaitTimeoutThenHideLoading(loadingCancellationToken.Token).Forget();
        }

        private void UpdateChannelMembers(string channelId, ChannelMember[] channelMembers)
        {
            if (!view.IsActive) return;
            view.HideLoading();
            view.ShowLoadingMore();

            foreach (ChannelMember member in channelMembers)
            {
                UserProfile memberProfile = UserProfileController.GetProfileByUserId(member.userId);

                if (memberProfile != null)
                {
                    ChannelMemberEntryModel userToAdd = new ChannelMemberEntryModel
                    {
                        isOnline = member.isOnline,
                        thumnailUrl = memberProfile.face256SnapshotURL,
                        userId = memberProfile.userId,
                        userName = memberProfile.userName
                    };

                    view.Set(userToAdd);
                }
            }
        }

        private void LoadMoreMembers()
        {
            if (IsLoading()) return;
            loadStartedTimestamp = DateTime.Now;
            view.HideLoadingMore();
            chatController.GetChannelMembers(currentChannelId, LOAD_PAGE_SIZE, view.EntryCount);

            if (!isSearching)
                lastLimitRequested = LOAD_PAGE_SIZE + view.EntryCount;
        }

        public void SetAutomaticReloadingActive(bool isActive)
        {
            reloadingCancellationToken.Cancel();

            if (isActive)
            {
                reloadingCancellationToken = new CancellationTokenSource();
                ReloadMembersPeriodically(reloadingCancellationToken.Token).Forget();
            }
        }

        private async UniTask ReloadMembersPeriodically(CancellationToken cancellationToken)
        {
            while (true)
            {
                await UniTask.Delay(MINUTES_FOR_AUTOMATIC_RELOADING * 60 * 1000, cancellationToken: cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                LoadMembers();
            }
        }

        private bool IsLoading() => (DateTime.Now - loadStartedTimestamp).TotalSeconds < LOAD_TIMEOUT;

        private void ClearListeners()
        {
            view.OnSearchUpdated -= SearchMembers;
            view.OnRequestMoreMembers -= LoadMoreMembers;
            chatController.OnUpdateChannelMembers -= UpdateChannelMembers;
        }

        private async UniTask WaitTimeoutThenHideLoading(CancellationToken cancellationToken)
        {
            await UniTask.Delay(LOAD_TIMEOUT * 1000, cancellationToken: cancellationToken);
            if (cancellationToken.IsCancellationRequested) return;
            view.HideLoading();
        }
    }
}