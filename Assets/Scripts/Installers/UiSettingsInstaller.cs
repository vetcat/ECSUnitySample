using Libs.UiCore;
using Ui.Controllers;
using Ui.Views;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "UiSettingsInstaller", menuName = "Installers/UiSettingsInstaller")]
    public class UiSettingsInstaller : ScriptableObjectInstaller<UiSettingsInstaller>
    {
        public UiPlayerView uiPlayerView;
        public UiMenuView uiMenuView;

        public override void InstallBindings()
        {
            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("Canvas not found");
                return;
            }

            BindViews(canvas);
        }

        private void BindViews(Canvas canvas)
        {
            Container.BindViewController<UiPlayerView, PlayerViewController>(uiPlayerView, canvas);
            Container.BindViewController<UiMenuView, MenuViewController>(uiMenuView, canvas);
        }
    }
}