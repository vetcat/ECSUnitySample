using System;
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

        private readonly ITimeProvider _timeProvider;
        private readonly GameSettings _settings;
        private ComponentGroup _group;
        private const float InputFilter = 0.01f;

        public CharacterControllerMovementSystem(int priority, ITimeProvider timeProvider, GameSettings settings)
        {
            _timeProvider = timeProvider;
            _settings = settings;
            Priority = priority;
        }

        protected override void OnCreateManager()
        {
            _group = GetComponentGroup(
                ComponentType.ReadOnly<Transform>(),
                ComponentType.ReadOnly<CharacterController>(),
                ComponentType.ReadOnly<InputComponent>());
        }

        protected override void OnUpdate()
        {
            Entities.With(_group).ForEach(
                (Entity entity, ref InputComponent inputComponent, CharacterController controller, Transform transform) =>
                {
                    var speed = _settings.constants.SpeedPlayerMove * inputComponent.Vertical;

                    if (Math.Abs(speed) > InputFilter)
                    {
                        controller.Move(transform.forward * speed * _timeProvider.DeltaTime);
                    }
                });
        }
    }
}