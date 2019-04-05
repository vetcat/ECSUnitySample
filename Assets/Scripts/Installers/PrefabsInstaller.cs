using System;
using Unity.Entities;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PrefabsInstaller", menuName = "Installers/PrefabsInstaller")]
public class PrefabsInstaller : ScriptableObjectInstaller<PrefabsInstaller>
{
    [Serializable]
    public class Settings
    {
        public GameObjectEntity player;
        public GameObject enemy;
        public int healthEnemy = 50;
        public int healthPlayer = 100;
    }

    public Settings settings;

    public override void InstallBindings()
    {
        Container.BindInstances(settings);
    }
}