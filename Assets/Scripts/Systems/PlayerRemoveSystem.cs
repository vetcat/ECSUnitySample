using Components;
using Signals;
using UniRx;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Zenject;

namespace Systems
{
    [DisableAutoCreation]
    public class PlayerRemoveSystem : ComponentSystem, IPrioritySystem
    {
        public int Priority { get; }

        private readonly SignalBus _signalBus;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private ComponentGroup _group;

        public PlayerRemoveSystem(SignalBus signalBus, int priority)
        {
            _signalBus = signalBus;
            Priority = priority;
        }

        protected override void OnCreateManager()
        {
            _group = GetComponentGroup(
                ComponentType.ReadOnly<Transform>(),
                ComponentType.ReadOnly<PlayerComponent>(),
                ComponentType.Exclude<DestroyEntityComponent>());

            _signalBus.GetStream<SignalUiLayerWantsRemovePlayer>()
                .Subscribe(Remove)
                .AddTo(_disposables);
        }

        protected override void OnDestroyManager()
        {
            _disposables?.Dispose();
        }

        protected override void OnUpdate()
        {
        }

        private void Remove(SignalUiLayerWantsRemovePlayer data)
        {
            if (_group.CalculateLength() == 0)
            {
                return;
            }

            var entities = _group.ToEntityArray(Allocator.TempJob);
            EntityManager.AddComponent(entities[0], typeof(DestroyEntityComponent));
            entities.Dispose();
        }
    }
}