using PlanA.Architecture.EventBus;
using UnityEngine;

namespace PlanA.Architecture.StateManagement
{
    public class StateMachine
    {
        private IGameState _currentGameState;

        public StateMachine(IGameState entryState)
        {
            _currentGameState = entryState;
        }

        public void StartMachine()
        {
            ChangeState(_currentGameState);
        }

        public void StopMachine()
        {
            _currentGameState?.OnExit();
            _currentGameState = null;
        }

        public void ChangeState(IGameState newState)
        {
            Debug.Log($"StateMachine::ChangeState: {newState}");
            _currentGameState?.OnExit();
            _currentGameState = newState;
            _currentGameState?.OnEnter();
        }

        public void Tick()
        {
            _currentGameState?.OnTick();
        }
    }
}