using Cysharp.Threading.Tasks;
using DCL.Helpers;
using System;
using UnityEngine;
using DCL.NotificationModel;
using System.Threading;
using Type = DCL.NotificationModel.Type;

namespace DCL.LoadingScreen
{
    /// <summary>
    /// Controls the state of the loading screen. It's responsibility is to update the view depending on the SceneController state
    /// Creates and provides the controllers associated to the LoadingScreen: TipsController and PercentageController
    /// </summary>
    public class LoadingScreenController : IDisposable
    {
        private readonly ILoadingScreenView view;
        private readonly ISceneController sceneController;
        private readonly DataStore_Player playerDataStore;
        private readonly DataStore_Common commonDataStore;
        private readonly DataStore_LoadingScreen loadingScreenDataStore;
        private readonly DataStore_Realm realmDataStore;
        private readonly IWorldState worldState;
        private readonly NotificationsController notificationsController;

        private Vector2Int currentDestination;
        private string currentRealm;
        private bool currentRealmIsWorld;
        private readonly LoadingScreenTipsController tipsController;
        private readonly LoadingScreenPercentageController percentageController;
        private bool onSignUpFlow;

        private CancellationTokenSource timeoutCTS;

        public LoadingScreenController(ILoadingScreenView view, ISceneController sceneController, IWorldState worldState, NotificationsController notificationsController,
            DataStore_Player playerDataStore, DataStore_Common commonDataStore, DataStore_LoadingScreen loadingScreenDataStore, DataStore_Realm realmDataStore)
        {
            this.view = view;
            this.sceneController = sceneController;
            this.playerDataStore = playerDataStore;
            this.commonDataStore = commonDataStore;
            this.worldState = worldState;
            this.loadingScreenDataStore = loadingScreenDataStore;
            this.realmDataStore = realmDataStore;
            this.notificationsController = notificationsController;

            tipsController = new LoadingScreenTipsController(view.GetTipsView());
            percentageController = new LoadingScreenPercentageController(sceneController, view.GetPercentageView(), commonDataStore);

            this.playerDataStore.lastTeleportPosition.OnChange += TeleportRequested;
            this.commonDataStore.isSignUpFlow.OnChange += OnSignupFlow;
            this.sceneController.OnReadyScene += ReadyScene;
            view.OnFadeInFinish += FadeInFinished;
        }

        public void Dispose()
        {
            view.Dispose();
            percentageController.Dispose();
            playerDataStore.lastTeleportPosition.OnChange -= TeleportRequested;
            commonDataStore.isSignUpFlow.OnChange -= OnSignupFlow;
            sceneController.OnReadyScene -= ReadyScene;
            view.OnFadeInFinish -= FadeInFinished;
        }

        private void FadeInFinished(ShowHideAnimator obj)
        {
            loadingScreenDataStore.decoupledLoadingHUD.visible.Set(true);
        }

        private void ReadyScene(int obj)
        {
            //We have to check that the latest scene loaded is the one from our current destination
            if (worldState.GetSceneNumberByCoords(currentDestination).Equals(obj))
            {
                //We have to check if the player is loaded
                if(commonDataStore.isPlayerRendererLoaded.Get())
                    FadeOutView();
                else
                {
                    percentageController.SetAvatarLoadingMessage();
                    commonDataStore.isPlayerRendererLoaded.OnChange += PlayerLoaded;
                }
            }
        }

        //We have to add one more check not to show the loadingScreen unless the player is loaded
        private void PlayerLoaded(bool loaded, bool _)
        {
            if(loaded)
                FadeOutView();

            commonDataStore.isPlayerRendererLoaded.OnChange -= PlayerLoaded;
        }

        private void OnSignupFlow(bool current, bool previous)
        {
            onSignUpFlow = current;
            if (current)
                FadeOutView();
            else
                view.FadeIn(false, false);
        }

        private void TeleportRequested(Vector3 current, Vector3 previous)
        {
            if (onSignUpFlow) return;

            Vector2Int currentDestinationCandidate = Utils.WorldToGridPosition(current);

            if (IsNewRealm() || IsNewScene(currentDestinationCandidate))
            {
                currentDestination = currentDestinationCandidate;

                //On a teleport, to copy previos behaviour, we disable tips entirely and show the teleporting screen
                //This is probably going to change with the integration of WORLDS loading screen
                //Temporarily removing tips until V2
                //tipsController.StopTips();
                percentageController.StartLoading(currentDestination);

                view.FadeIn(false, true);

                timeoutCTS = new CancellationTokenSource();
                StartTimeoutCounter(timeoutCTS.Token);
            }
        }

        private bool IsNewRealm()
        {
            //Realm has not been set yet, so we are not in a new realm
            if (realmDataStore.playerRealmAboutConfiguration.Get() == null)
                return false;

            bool realmChangeRequiresLoadingScreen;

            if (commonDataStore.isWorld.Get())
                realmChangeRequiresLoadingScreen = string.IsNullOrEmpty(currentRealm) || !currentRealm.Equals(realmDataStore.playerRealmAboutConfiguration.Get().RealmName);
            else
                realmChangeRequiresLoadingScreen = currentRealmIsWorld;

            currentRealm = realmDataStore.playerRealmAboutConfiguration.Get().RealmName;
            currentRealmIsWorld = commonDataStore.isWorld.Get();
            return realmChangeRequiresLoadingScreen;
        }

        //If the destination scene is not loaded, we show the teleport screen. THis is called in the POSITION_UNSETTLED
        //On the other hand, the POSITION_SETTLED event is called; but since the scene will already be loaded, the loading screen wont be shown
        private bool IsNewScene(Vector2Int currentDestinationCandidate) =>
             worldState.GetSceneNumberByCoords(currentDestinationCandidate).Equals(-1);

        private void CheckSceneTimeout(Vector2Int currentDestinationCandidate)
        {
            //If we are settling on the destination position, but loading is not complete, this means that kernel is calling for a timeout.
            //For now, we hide the loading screen and add a notification
            if (currentDestinationCandidate.Equals(currentDestination) &&
                worldState.GetScene(worldState.GetSceneNumberByCoords(currentDestination))?.loadingProgress < 100)
            {
                notificationsController.ShowNotification(new Model
                {
                    message = "Loading scene timeout",
                    type = Type.GENERIC,
                    timer = 10f,
                    destroyOnFinish = true
                });
                FadeOutView();
            }
        }

        private void FadeOutView()
        {
            view.FadeOut();
            loadingScreenDataStore.decoupledLoadingHUD.visible.Set(false);

            if (timeoutCTS != null)
            {
                timeoutCTS.Cancel();
                timeoutCTS.Dispose();
            }
        }

        public async UniTask StartTimeoutCounter(CancellationToken token)
        {
            try
            {
                // Wait for 120 seconds
                await UniTask.Delay(120000, cancellationToken: token);
                CheckSceneTimeout(currentDestination);
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation
                Debug.Log("Timeout was aborted");
            }
        }
    }
}
