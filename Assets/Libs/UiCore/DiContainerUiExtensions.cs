using UnityEngine;
using Zenject;

namespace Libs.UiCore
{
    public static class DiContainerUiExtensions
    {
        public static void BindViewController<TView, TController>(this DiContainer container, UiView view, Canvas parent) where TView : UiView where TController : UiController
        {
            container.Bind<TView>().FromComponentInNewPrefab(view).UnderTransform(parent.transform).AsSingle();
            container.BindController<TController>();
        }

        public static void BindViewControllerWithArgument<TView, TController, T>(this DiContainer container, GameObject viewPrefab, Transform parent, T paramArgument) where TView : UiView where TController : UiController
        {
            container.Bind<TView>().FromComponentInNewPrefab(viewPrefab).UnderTransform(parent).AsSingle();
            container.BindControllerWithArgument<TController, T>(paramArgument);
        }

        private static void BindController<TController>(this DiContainer container) where TController : UiController
        {
            container.BindInterfacesAndSelfTo<TController>().AsSingle().NonLazy();
        }

        private static void BindControllerWithArgument<TController, T>(this DiContainer container, T paramArgument) where TController : UiController
        {
            container.BindInterfacesAndSelfTo<TController>().AsSingle().WithArguments(paramArgument).NonLazy();
        }
    }
}
