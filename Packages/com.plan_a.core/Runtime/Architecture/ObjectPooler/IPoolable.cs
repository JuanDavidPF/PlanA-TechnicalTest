namespace PlanA.Architecture.Architecture.ObjectPool
{
    public interface IPoolable
    {
        void OnEnqueue();
        void OnDequeue();
    }
}