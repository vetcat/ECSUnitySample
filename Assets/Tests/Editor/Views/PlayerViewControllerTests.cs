using System;
using Libs.UiCore;
using NUnit.Framework;
using Signals;
using Tests.Editor.SetUp;
using Ui.Controllers;
using Ui.Views;
using UnityEngine;
using Zenject;

namespace Tests.Editor.Views
{
    public class PlayerViewControllerTests : EcsTestsBase
    {
        private PlayerViewController _controller;
        private PlayerView _view;
        private SignalBus _signalBus;

        protected override void Install()
        {
            base.Install();
            Container.DeclareSignal<SignalUiLayerWantsAddPlayer>();
            Container.DeclareSignal<SignalUiLayerWantsRemovePlayer>();

            SignalBusInstaller.Install(Container);
            _signalBus = Container.Resolve<SignalBus>();

            Container.BindViewController<PlayerView, PlayerViewController>(UiSettings.playerView, Canvas);

            _controller = Container.Resolve<PlayerViewController>();
            _controller.Initialize();

            _view = Container.Resolve<PlayerView>();
        }

        [Test]
        public void ButtonAdd_Click_FireSignal()
        {
            var received = false;
            Action callback = () => received = true;
            _signalBus.Subscribe<SignalUiLayerWantsAddPlayer>(callback);

            _view.buttonAdd.onClick.Invoke();
            Assert.AreEqual(true, received);
        }

        [Test]
        public void ButtonRemove_Click_FireSignal()
        {
            var received = false;
            Action callback = () => received = true;
            _signalBus.Subscribe<SignalUiLayerWantsRemovePlayer>(callback);

            _view.buttonRemove.onClick.Invoke();
            Assert.AreEqual(true, received);
        }

        [Test]
        public void PlayerCreateDestroy_TextPlayerCount_EqualCount()
        {
            var player = PlayerFactorySystem.Create(Vector3.zero, Quaternion.identity);

            Assert.AreEqual(_controller.GetFormattedText(1), _view.textPlayerCount.text);

            PlayerFactorySystem.Destroy(player);
            Assert.AreEqual(_controller.GetFormattedText(0), _view.textPlayerCount.text);
        }
    }
}