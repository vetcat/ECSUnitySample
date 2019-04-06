using Systems;
using NUnit.Framework;
using Tests.Editor.SetUp;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Tests.Editor.Systems
{
    public class CharacterControllerMovementSystemTests : EcsTestsBase
    {
        [Test]
        public void MoveCharacter_OnFrame_EqualPosition()
        {
            Container.BindInterfacesAndSelfTo<CharacterControllerMovementSystem>().AsSingle().WithArguments(510).NonLazy();
            var system = Container.Resolve<CharacterControllerMovementSystem>();
            World.Active.AddManager(system);

            var player = PlayerFactorySystem.Create(Vector3.zero, quaternion.identity);
            TimeProvider.SetDeltaTime(0.5f);
            var speed = GameSettings.constants.SpeedPlayerMove;
            var forward = player.transform.forward;
            player.GetComponent<CharacterController>().SimpleMove(forward * speed * TimeProvider.DeltaTime);
            var nextPosition = player.transform.position;

            //set back to zero
            player.transform.position = Vector3.zero;

            //apply position from system
            system.Update();

            Assert.AreEqual(nextPosition, player.transform.position);
        }
    }
}
