using System;
using System.Collections.Generic;
using Interfaces;
using Unity.Entities;
using UnityEngine;
using Zenject;

public class Bootstrap : IInitializable, IDisposable
{
    private readonly List<IPrioritySystem> _systems;
    private EntityManager _entityManager;
    private World _world;

    public Bootstrap(List<IPrioritySystem> systems, string worldName)
    {
        _systems = systems;
        _world = new World(worldName);
        World.Active = _world;
    }

    public void Initialize()
    {
        _entityManager = _world.AddManager(new EntityManager());

        _systems.Sort((x, y) => x.Priority.CompareTo(y.Priority));

        foreach (var system in _systems)
        {
            _world.AddManager((ComponentSystemBase) system);
            Debug.Log("add system " + system.GetType() + "; Priority = " + system.Priority);
        }

        ScriptBehaviourUpdateOrder.UpdatePlayerLoop(_world);
    }

    public void Dispose()
    {
        Debug.LogFormat("World: {0} dispose", _world.Name);

        var entities = _entityManager.GetAllEntities();
        _entityManager.DestroyEntity(entities);
        entities.Dispose();

        World.DisposeAllWorlds();
        WordStorage.Instance.Dispose();
        WordStorage.Instance = null;
        ScriptBehaviourUpdateOrder.UpdatePlayerLoop();
    }
}