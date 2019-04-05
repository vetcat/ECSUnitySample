using Libs.UiCore;
using Signals;
using Ui.Views;
using UniRx;
using Zenject;

namespace Ui.Controllers
{
    public class EnemyViewController : UiController<EnemyView>
    {
        private readonly SignalBus _signalBus;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private const int ADD_ENEMY_COUNT = 1;

        public EnemyViewController(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public override void Initialize()
        {
            View.buttonAdd.OnClickAsObservable()
                .Subscribe(_=>{ _signalBus.Fire(new SignalUiLayerWantsAddEnemy(ADD_ENEMY_COUNT));})
                .AddTo(_disposables);

            View.buttonRemove.OnClickAsObservable()
                .Subscribe(_=>{_signalBus.Fire<SignalUiLayerWantsRemoveEnemy>();})
                .AddTo(_disposables);

            _signalBus.GetStream<SignalEcsLayerEnemyCountUpdate>()
                .Subscribe(UpdateEnemyCount)
                .AddTo(_disposables);

        }

        public override void Dispose()
        {
            _disposables?.Dispose();
        }

        public string GetFormattedText(int count)
        {
            return $"Enemy count : {count:D}";
        }

        private void UpdateEnemyCount(SignalEcsLayerEnemyCountUpdate data)
        {
            View.textEnemyCount.text = GetFormattedText(data.Count);
        }
    }
}
