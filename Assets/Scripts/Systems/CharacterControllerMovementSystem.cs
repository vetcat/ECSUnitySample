using Components;
using Installers;
using Providers;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    [DisableAutoCreation]
    public class CharacterControllerMovementSystem : ComponentSystem, IPrioritySystem
    {
        public int Priority { get; }

        private readonly IInputProvider _inputProvider;
        private readonly ITimeProvider _timeProvider;
        private readonly GameSettings _settings;

        private ComponentGroup _group;
        public CharacterControllerMovementSystem(int priority, IInputProvider inputProvider, ITimeProvider timeProvider, GameSettings settings)
        {
            _inputProvider = inputProvider;
            _timeProvider = timeProvider;
            _settings = settings;
            Priority = priority;
        }

        protected override void OnCreateManager()
        {
            _group = GetComponentGroup(
                ComponentType.ReadOnly<Transform>(),
                ComponentType.ReadOnly<CharacterController>(),
                ComponentType.ReadOnly<InputComponent>(),
                ComponentType.Exclude<DestroyEntityComponent>());
        }

        protected override void OnUpdate()
        {
            Entities.With(_group).ForEach(
                (Entity entity, CharacterController controller, Transform transform) =>
                {
                    var speed = _settings.constants.SpeedPlayerMove * _inputProvider.Vertical;
                    controller.Move(transform.forward * speed * _timeProvider.DeltaTime);
                });
        }
    }
}