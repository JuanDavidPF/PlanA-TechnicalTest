namespace PlanA.Architecture.EventBus
{
    public interface IGameState
    {
        public void OnEnter();
        public void OnTick();

        public void OnExit();
    }
}