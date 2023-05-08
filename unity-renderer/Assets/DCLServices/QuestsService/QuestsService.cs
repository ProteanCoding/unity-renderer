﻿using Cysharp.Threading.Tasks;
using Decentraland.Quests;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DCLServices.QuestsService
{
    public class QuestsService: IDisposable
    {
        public event Action<QuestStateUpdate> OnQuestUpdated;
        public IReadOnlyDictionary<string, QuestStateUpdate> CurrentState => stateCache;

        private readonly IClientQuestsService clientQuestsService;
        private readonly Dictionary<string, UniTaskCompletionSource<ProtoQuest>> definitionCache = new ();
        private readonly Dictionary<string, QuestStateUpdate> stateCache = new ();
        private string userId = null;
        private CancellationTokenSource userSubscribeCt = null;

        public QuestsService(IClientQuestsService clientQuestsService)
        {
            this.clientQuestsService = clientQuestsService;
        }

        public void SetUserId(string userId)
        {
            if(userId == this.userId)
                return;

            this.userId = userId;
            // Definitions are not user specific, so we only need to clear the state cache
            stateCache.Clear();
            userSubscribeCt?.Cancel();
            userSubscribeCt?.Dispose();
            userSubscribeCt = new CancellationTokenSource();
            Subscribe(userSubscribeCt.Token).Forget();
        }

        private async UniTaskVoid Subscribe(CancellationToken ct)
        {
            var  enumerator = clientQuestsService.Subscribe(new UserAddress { UserAddress_ = userId }).GetAsyncEnumerator(ct);
            try
            {
                while (await enumerator.MoveNextAsync())
                {
                    var userUpdate = enumerator.Current;
                    if(userUpdate.MessageCase != UserUpdate.MessageOneofCase.QuestState)
                        continue;

                    stateCache[userUpdate.QuestState.QuestInstanceId] = userUpdate.QuestState;
                    OnQuestUpdated?.Invoke(userUpdate.QuestState);
                }
            }
            finally
            {
                await enumerator.DisposeAsync();
            }
        }

        public async UniTask<StartQuestResponse> StartQuest(string questId)
        {
            return await clientQuestsService.StartQuest(new StartQuestRequest { QuestId = questId });
        }

        public async UniTask<AbortQuestResponse> AbortQuest(string questInstanceId)
        {
            return await clientQuestsService.AbortQuest(new AbortQuestRequest { QuestInstanceId = questInstanceId });
        }

        public async UniTask<ProtoQuest> GetDefinition(string questId, CancellationToken cancellationToken = default)
        {
            UniTaskCompletionSource<ProtoQuest> definitionCompletionSource;
            async UniTask<ProtoQuest> RetrieveTask()
            {
                var definition = await clientQuestsService.GetQuestDefinition(new QuestDefinitionRequest{QuestId = questId});
                definitionCompletionSource.TrySetResult(definition);
                return definition;
            }

            if (!definitionCache.TryGetValue(questId, out definitionCompletionSource))
            {
                definitionCompletionSource = new UniTaskCompletionSource<ProtoQuest>();
                definitionCache[questId] = definitionCompletionSource;
                RetrieveTask().Forget();
            }

            return await definitionCompletionSource.Task.AttachExternalCancellation(cancellationToken);
        }

        public void Dispose()
        {
            userSubscribeCt?.Cancel();
            userSubscribeCt?.Dispose();
            userSubscribeCt = null;

        }
    }
}