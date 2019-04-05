using Providers;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    [DisableAutoCreation]
    public class DeltaTimeUpdateSystem : ComponentSystem , IPrioritySystem
    {
        public int Priority { get; }

        private readonly ITimeProvider _timeProvider;

        public DeltaTimeUpdateSystem(int priority, ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
            Priority = priority;
        }

        protected override void OnUpdate()
        {
            _timeProvider.SetDeltaTime(Time.deltaTime);
        }
    }
}