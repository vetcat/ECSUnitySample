using System.Collections;
using NUnit.Framework;
using Ui.Views;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Zenject;

namespace Tests.PlayTests
{
    public class SimplePlayTest : UITest
    {
        [TearDown]
        public void TearDown()
        {
            Object.Destroy(ProjectContext.Instance.gameObject);
        }

        [UnityTest]
        public IEnumerator SimpleTest()
        {
            SceneManager.LoadScene("SampleScene");

            yield return WaitFor(new ObjectAppeared<UiMenuView>());

            yield return new WaitForSeconds(1f);

            yield return Press("[UIMenuView] - ButtonStart");

            yield return WaitFor(new ObjectAppeared<UiPlayerView>());

            yield return AssertLabel("[UIPlayerView] - TextPlayerCount", "Player count : " + 1);

            var instancePlayerCount = 10;

            for (var i = 0; i < instancePlayerCount; i++)
            {
                yield return Press("[UIPlayerView] - ButtonAdd");
            }

            yield return new WaitForSeconds(1f);

            yield return AssertLabel("[UIPlayerView] - TextPlayerCount", "Player count : " + (instancePlayerCount + 1));

            for (var i = 0; i < instancePlayerCount; i++)
            {
                yield return Press("[UIPlayerView] - ButtonRemove");
            }

            yield return AssertLabel("[UIPlayerView] - TextPlayerCount", "Player count : " + 1);
        }
    }
}
