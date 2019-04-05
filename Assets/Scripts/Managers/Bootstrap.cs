using System.Collections.Generic;
using Systems;
using Unity.Entities;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class Bootstrap : IInitializable
    {
        private readonly List<IPrioritySystem> _systems;

        public Bootstrap(List<IPrioritySystem> systems)
        {
            _systems = systems;
        }

        public void Initialize()
        {
            _systems.Sort((x, y) => x.Priority.CompareTo(y.Priority));
            var simulationSystemGroup = World.Active.GetOrCreateManager<SimulationSystemGroup>();

            foreach (var system in _systems)
            {
                World.Active.AddManager((ComponentSystemBase) system);
                simulationSystemGroup.AddSystemToUpdateList((ComponentSystemBase) system);
                Debug.Log("add system " + system.GetType() + "; Priority = " + system.Priority);
            }
        }
    }
}