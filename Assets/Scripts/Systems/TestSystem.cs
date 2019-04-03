using Interfaces;
using Unity.Entities;

namespace Systems
{
    [DisableAutoCreation]
    public class TestSystem : ComponentSystem, IPrioritySystem
    {
        public TestSystem(int priority)
        {
            Priority = priority;
        }

        public int Priority { get; }

        protected override void OnCreateManager()
        {
        }

        protected override void OnDestroyManager()
        {
        }

        protected override void OnUpdate()
        {
            //Debug.Log("[TestSystem] OnUpdate");
        }
    }
}