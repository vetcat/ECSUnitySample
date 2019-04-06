using Components;
using Installers;
using UniRx;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    [DisableAutoCreation]
    public class PlayerFactorySystem : ComponentSystem, IPrioritySystem
    {
        public int Priority { get; }
        public readonly ReactiveCollection<PlayerFacade> SpawnList = new ReactiveCollection<PlayerFacade>();

        private readonly PlayerFacade.Pool _pool;
        private readonly GameSettings _settings;

        public PlayerFactorySystem(int priority, PlayerFacade.Pool pool, GameSettings settings)
        {
            Priority = priority;
            _pool = pool;
            _settings = settings;
        }

        protected override void OnUpdate()
        {
        }

        public PlayerFacade Create(Vector3 position, Quaternion rotation)
        {
            var spawn = _pool.Spawn(position, rotation);
            SpawnList.Add(spawn);

            var entity = spawn.gameObject.GetComponent<GameObjectEntity>().Entity;
            EntityManager.AddComponentData(entity, new PlayerComponent());
            EntityManager.AddComponentData(entity, new HealthComponent { Value = _settings.constants.healthPlayer });
            EntityManager.AddComponentData(entity, new InputComponent());

            return spawn;
        }

        public void Destroy(PlayerFacade playerFacade)
        {
            if (SpawnList.Count == 0)
                return;

            _pool.Despawn(playerFacade);
            SpawnList.Remove(playerFacade);
        }
    }
}