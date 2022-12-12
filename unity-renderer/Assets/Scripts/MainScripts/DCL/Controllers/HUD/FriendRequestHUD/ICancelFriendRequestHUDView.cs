using DCL.Helpers;
using System;

namespace DCL.Social.Friends
{
    public interface ICancelFriendRequestHUDView
    {
        event Action OnCancel;
        event Action OnClose;
        event Action OnOpenProfile;

        void SetRecipientName(string userName);
        void SetRecipientProfilePicture(ILazyTextureObserver textureObserver);
        void SetSenderProfilePicture(ILazyTextureObserver textureObserver);
        void Show();
        void Close();
        void Dispose();
        void ShowPendingToCancel();
        void ShowCancelFailed();
        void SetBodyMessage(string messageBody);
        void SetTimestamp(DateTime date);
    }
}