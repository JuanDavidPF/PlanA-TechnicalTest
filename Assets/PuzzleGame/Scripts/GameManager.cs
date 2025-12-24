using PlanA.Architecture;
using PlanA.Architecture.EventBus;
using PlanA.Architecture.Services;
using PlanA.Architecture.StateManagement;
using PlanA.PuzzleGame.GameStates;
using UnityEngine;

namespace PlanA.PuzzleGame
{
    public class GameManager : Singleton<GameManager>
    {
        [field: SerializeField] public GameData EditorGameData { get; private set; }

        [field: SerializeField] public GameData RuntimeGameData { get; private set; }

        public StateMachine StateMachine { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Register(typeof(EventBusService), new EventBusService());
            StateMachine = new StateMachine(new GameStartedState());
        }

        private void Start()
        {
            StateMachine.StartMachine();
        }

        private void Update()
        {
            StateMachine.Tick();
        }

        private void OnDestroy()
        {
            StateMachine.StopMachine();
            ServiceLocator.CleanUp();
            RuntimeGameData.Assign(EditorGameData);
        }
    }
}