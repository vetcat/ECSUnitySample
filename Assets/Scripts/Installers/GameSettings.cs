using System;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Installers/GameSettingsInstaller")]
    public class GameSettings : ScriptableObjectInstaller<GameSettings>
    {
        [Serializable]
        public class Prefabs
        {
            public GameObject player;
        }

        [Serializable]
        public class Constants
        {
            public int healthPlayer = 100;
            public float SpeedPlayerMove = 5f; //meter seconds
            public float SpeedPlayerRotate = 180; //grad seconds
        }

        public Constants constants;
        public Prefabs prefabs;

        public override void InstallBindings()
        {
            Container.BindInstances(this);
        }
    }
}