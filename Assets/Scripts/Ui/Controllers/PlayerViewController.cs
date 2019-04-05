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
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private const int AddPayerCount = 1;

        public PlayerViewController(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public override void Initialize()
        {
            View.buttonAdd.OnClickAsObservable()
                .Subscribe(_=>{ _signalBus.Fire(new SignalUiLayerWantsAddPlayer(AddPayerCount));})
                .AddTo(_disposables);

            View.buttonRemove.OnClickAsObservable()
                .Subscribe(_=>{_signalBus.Fire<SignalUiLayerWantsRemovePlayer>();})
                .AddTo(_disposables);

            _signalBus.GetStream<SignalEcsLayerPlayerCountUpdate>()
                .Subscribe(UpdatePlayerCount)
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

        private void UpdatePlayerCount(SignalEcsLayerPlayerCountUpdate data)
        {
            View.textPlayerCount.text = GetFormattedText(data.Count);
        }
    }
}
