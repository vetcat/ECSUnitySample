using Components;
using Providers;
using Unity.Entities;
using Unity.Jobs;

namespace Systems
{
    [DisableAutoCreation]
    public class InputSystem : JobComponentSystem, IPrioritySystem
    {
        private readonly IInputProvider _inputProvider;
        public int Priority { get; }

        private EndSimulationEntityCommandBufferSystem _barrier;
        private EntityArchetype _inputUpdateArchetype;

        //only for test JobHandle/InputUpdateJob
        private static float _horizontal;
        private static float _vertical;

        public InputSystem(int priority, IInputProvider inputProvider)
        {
            _inputProvider = inputProvider;
            Priority = priority;
        }

        protected override void OnCreateManager()
        {
            _barrier = World.GetOrCreateManager<EndSimulationEntityCommandBufferSystem>();
            _inputUpdateArchetype = EntityManager.CreateArchetype(typeof(InputComponent));
        }

        private struct InputUpdateJob : IJobProcessComponentDataWithEntity<InputComponent>
        {
            public EntityCommandBuffer.Concurrent Ecb;

            public EntityArchetype InputUpdatedArchetype;

            public void Execute(Entity entity, int index, ref InputComponent inputData)
            {
                inputData.Horizontal = _horizontal;
                inputData.Vertical = _vertical;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            _horizontal = _inputProvider.Horizontal;
            _vertical = _inputProvider.Vertical;

            var job = new InputUpdateJob
            {
                Ecb = _barrier.CreateCommandBuffer().ToConcurrent(),
                InputUpdatedArchetype = _inputUpdateArchetype
            };
            inputDeps = job.Schedule(this, inputDeps);
            inputDeps.Complete();
            _barrier.AddJobHandleForProducer(inputDeps);
            return inputDeps;
        }
    }
}