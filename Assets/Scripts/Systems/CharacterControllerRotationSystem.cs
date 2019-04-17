using Components;
using Installers;
using Providers;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    [DisableAutoCreation]
    public class CharacterControllerRotationSystem : ComponentSystem, IPrioritySystem
    {
        public int Priority { get; }

        private readonly IInputProvider _inputProvider;
        private readonly ITimeProvider _timeProvider;
        private readonly GameSettings _settings;
        private EntityQuery _group;

        public CharacterControllerRotationSystem(int priority, IInputProvider inputProvider, ITimeProvider timeProvider, GameSettings settings)
        {
            _inputProvider = inputProvider;
            _timeProvider = timeProvider;
            _settings = settings;
            Priority = priority;
        }

        protected override void OnCreateManager()
        {
            _group = GetEntityQuery(
                ComponentType.ReadOnly<Transform>(),
                ComponentType.ReadOnly<CharacterController>(),
                ComponentType.ReadOnly<InputComponent>());
        }

        protected override void OnUpdate()
        {
            Entities.With(_group).ForEach(
                (Entity entity, CharacterController controller, Transform transform) =>
                {
                    transform.Rotate(0f, _inputProvider.Horizontal * _settings.constants.SpeedPlayerRotate * _timeProvider.DeltaTime, 0f);
                });
        }
    }
}