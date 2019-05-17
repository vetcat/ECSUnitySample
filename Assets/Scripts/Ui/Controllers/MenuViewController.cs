using Libs.UiCore;
using Signals;
using Ui.Views;
using UniRx;
using Zenject;

namespace Ui.Controllers
{
    public class MenuViewController : UiController<UiMenuView>
    {
        private readonly SignalBus _signalBus;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public MenuViewController(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public override void Initialize()
        {
            View.buttonStart.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    View.Hide();
                    _signalBus.Fire(new SignalStartGame());
                })
                .AddTo(_disposables);
        }

        public override void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}
