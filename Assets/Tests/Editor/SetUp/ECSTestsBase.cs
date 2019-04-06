using Installers;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;
using Zenject;

namespace Tests.Editor.SetUp
{
    public abstract class EcsTestsBase
    {
        public DiContainer Container;
        public GameSettings GameSettings;
        public PlayerFacade.Pool PlayerPool;

        [SetUp]
        public void SetUp()
        {
            DefaultWorldInitialization.Initialize("TestWorld", false);

            Container = new DiContainer();
            GameSettings = Resources.Load("GameSettings") as GameSettings;
            Container.BindInstances(GameSettings);

            Container.BindMemoryPool<PlayerFacade, PlayerFacade.Pool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(GameSettings.prefabs.player)
                .UnderTransformGroup("Pool_Players");

            PlayerPool = Container.Resolve<PlayerFacade.Pool>();
        }
    }
}