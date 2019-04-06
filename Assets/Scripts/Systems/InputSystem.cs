using Components;
using Providers;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    [DisableAutoCreation]
    public class InputSystem : ComponentSystem, IPrioritySystem
    {
        public int Priority { get; }

        private readonly IInputProvider _inputProvider;
        private ComponentGroup _group;

        public InputSystem(int priority, IInputProvider inputProvider)
        {
            _inputProvider = inputProvider;
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
                (Entity entity, ref InputComponent inputComponent) =>
                {
                    PostUpdateCommands.SetComponent(entity, new InputComponent
                    {
                        Vertical = _inputProvider.Vertical,
                        Horizontal = _inputProvider.Horizontal
                    });
                });
        }
    }
}