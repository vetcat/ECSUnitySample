using System;
using Zenject;

namespace Libs.UiCore
{
    public interface IUiController
    {
        void Show();
        void Hide();
        bool IsShow();
    }

    public abstract class UiController : IInitializable, IDisposable
    {
        public virtual void Initialize()
        {        
        }

        public virtual void Dispose()
        {        
        }
    }

    public abstract class UiController<T> : UiController, IUiController
        where T : UiView
    {
        [Inject]
        private T _view;

        public T View { get { return _view; } }
    
        public virtual void Show()
        {
            View.Show();
        }

        public virtual void Hide()
        {
            View.Hide();
        }

        public bool IsShow()
        {
            return View.IsShow();
        }
    }
}