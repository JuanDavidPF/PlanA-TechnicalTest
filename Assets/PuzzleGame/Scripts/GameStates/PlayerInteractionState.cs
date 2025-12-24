using PlanA.Architecture.DataBinding;
using PlanA.Architecture.EventBus;

namespace PlanA.PuzzleGame.GameStates
{
    public class PlayerInteractionState : IGameState
    {
        public void OnEnter()
        {
            GameManager.Instance.RuntimeGameData.Moves.Bind(OnMovesChanged, true);
        }

        static private void OnMovesChanged(IntDataBind moves)
        {
            if (moves.Value <= 0)
            {
                GameManager.Instance.StateMachine.ChangeState(new GameOverState());
            }
        }

        public void OnTick()
        {
        }

        public void OnExit()
        {
            GameManager.Instance?.RuntimeGameData.Moves.UnBind(OnMovesChanged);
        }
    }
}