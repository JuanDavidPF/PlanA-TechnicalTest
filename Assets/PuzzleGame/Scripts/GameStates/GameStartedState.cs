using Cysharp.Threading.Tasks;
using PlanA.Architecture.EventBus;
using PlanA.Architecture.Services;
using PlanA.PuzzleGame.GameEvents;
using UnityEngine;

namespace PlanA.PuzzleGame.GameStates
{
    public class GameStartedState : IGameState
    {
        public void OnEnter()
        {
            ServiceLocator.Get<EventBusService>().Subscribe<OnBoardCreated>(OnBoardCreated);
            LoadGame().Forget();
        }

        /// <summary>
        ///     This method could be expanded to do an API request or get the session data from a resource or any other place.
        /// </summary>
        /// <remarks>
        ///     Currently it copies the editor GameData that is used as the `default` rules.
        /// </remarks>
        static private UniTaskVoid LoadGame()
        {
            GameManager.Instance.RuntimeGameData.Assign(GameManager.Instance.EditorGameData);
            ServiceLocator.Get<EventBusService>().Raise(new OnGameStarted());
            return default;
        }

        static private void OnBoardCreated(OnBoardCreated onBoardCreated)
        {
            GameManager.Instance.StateMachine.ChangeState(new PlayerInteractionState());
        }

        public void OnTick()
        {
        }

        public void OnExit()
        {
            if (ServiceLocator.TryGet(out EventBusService eventBusService))
            {
                eventBusService.Unsubscribe<OnBoardCreated>(OnBoardCreated);
            }
        }
    }
}