using Components;
using Interfaces;
using Unity.Entities;
using Zenject;

namespace Systems
{
    [DisableAutoCreation]
    public class PlayerCalculateCountSystem : ComponentSystem, IPrioritySystem
    {
        private readonly SignalBus _signalBus;
        private ComponentGroup _playerGroup;

        public PlayerCalculateCountSystem(int priority, SignalBus signalBus)
        {
            _signalBus = signalBus;
            Priority = priority;
        }

        public int Priority { get; }

        protected override void OnCreateManager()
        {
            _playerGroup = GetComponentGroup(ComponentType.ReadOnly<PlayerComponent>());
        }

        protected override void OnUpdate()
        {
            var entities = _playerGroup.GetEntityArray();
        }
    }
}