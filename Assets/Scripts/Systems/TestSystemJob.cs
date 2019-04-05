﻿using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Systems
{
    [DisableAutoCreation]
    public class TestSystemJob : JobComponentSystem, IPrioritySystem
    {
        public TestSystemJob(int priority)
        {
            Priority = priority;
        }

        public int Priority { get; }

        protected override void OnCreateManager()
        {
        }

        protected override void OnDestroyManager()
        {
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            Debug.Log("[TestSystemJob] OnUpdate");
            return inputDeps;
        }
    }
}