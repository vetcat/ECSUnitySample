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
        public PlayerView playerView;
        public MenuView menuView;

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
            Container.BindViewController<PlayerView, PlayerViewController>(playerView, canvas);
            Container.BindViewController<MenuView, MenuViewController>(menuView, canvas);
        }
    }
}