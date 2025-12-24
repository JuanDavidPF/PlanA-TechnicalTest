using PlanA.Architecture.EventBus;
using PlanA.Architecture.Services;
using PlanA.PuzzleGame.GameEvents;

namespace PlanA.PuzzleGame.GameStates
{
    public class GameOverState : IGameState
    {
        public void OnEnter()
        {
            EventBusService eventBusService = ServiceLocator.Get<EventBusService>();
            eventBusService.Raise(new OnGameOver());
            eventBusService.Subscribe<OnReplayRequested>(ReplayGame);
        }

        public void OnTick()
        {
        }

        public void OnExit()
        {
            if (ServiceLocator.TryGet(out EventBusService eventBusService))
            {
                eventBusService.Unsubscribe<OnReplayRequested>(ReplayGame);
            }
        }

        static private void ReplayGame(OnReplayRequested onReplayRequested)
        {
            GameManager.Instance.StateMachine.ChangeState(new GameStartedState());
        }
    }
}