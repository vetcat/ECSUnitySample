using NUnit.Framework;
using Tests.Editor.SetUp;
using UnityEngine;

namespace Tests.Editor.Systems
{
    public class PlayerFactorySystemTests : EcsTestsBase
    {
        [Test]
        public void Create_NotNull()
        {
            var player = PlayerFactorySystem.Create(Vector3.zero, Quaternion.identity);
            Assert.NotNull(player);
            Assert.AreEqual(1, PlayerPool.NumActive);

            PlayerFactorySystem.Destroy(player);
            Assert.AreEqual(0, PlayerPool.NumActive);
        }

        [Test]
        public void Create_Position_Equal()
        {
            var position = new Vector3(3f,3f,3f);
            var player = PlayerFactorySystem.Create(position, Quaternion.identity);

            Assert.AreEqual(position, player.transform.position);
        }
    }
}