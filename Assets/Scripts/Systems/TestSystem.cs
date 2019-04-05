using Interfaces;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    [DisableAutoCreation]
    public class TestSystem : ComponentSystem, IPrioritySystem
    {
        public int Priority { get; }
        public TestSystem(int priority)
        {
            Priority = priority;
        }

        protected override void OnCreateManager()
        {
            Debug.Log("[TestSystem] OnCreateManager");
        }

        protected override void OnDestroyManager()
        {
            Debug.Log("[TestSystem] OnDestroyManager");
        }

        protected override void OnUpdate()
        {
            Debug.Log("[TestSystem] OnUpdate");
        }
    }
}