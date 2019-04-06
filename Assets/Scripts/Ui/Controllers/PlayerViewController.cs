using Systems;
using Libs.UiCore;
using Signals;
using Ui.Views;
using UniRx;
using Zenject;

namespace Ui.Controllers
{
    public class PlayerViewController : UiController<PlayerView>
    {
        private readonly SignalBus _signalBus;
        private readonly PlayerFactorySystem _playerFactorySystem;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private const int AddPayerCount = 1;

        //injection can be both signals and systems
        public PlayerViewController(SignalBus signalBus, PlayerFactorySystem playerFactorySystem)
        {
            _signalBus = signalBus;
            _playerFactorySystem = playerFactorySystem;
        }

        public override void Initialize()
        {
            View.Hide();

            View.buttonAdd.OnClickAsObservable()
                .Subscribe(_=>{ _signalBus.Fire(new SignalUiLayerWantsAddPlayer(AddPayerCount));})
                .AddTo(_disposables);

            View.buttonRemove.OnClickAsObservable()
                .Subscribe(_=>{_signalBus.Fire<SignalUiLayerWantsRemovePlayer>();})
                .AddTo(_disposables);

            _playerFactorySystem.SpawnList.ObserveCountChanged()
                .Subscribe(UpdatePlayerCount)
                .AddTo(_disposables);

            _signalBus.GetStream<SignalStartGame>()
                .Subscribe(_ =>
                {
                    View.Show();
                })
                .AddTo(_disposables);
        }

        public override void Dispose()
        {
            _disposables?.Dispose();
        }

        public string GetFormattedText(int count)
        {
            return $"Player count : {count:D}";
        }

        private void UpdatePlayerCount(int count)
        {
            View.textPlayerCount.text = GetFormattedText(count);
        }
    }
}
