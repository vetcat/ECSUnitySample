using Systems;
using Zenject;

public class EcsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        BindSystems();

        Container.BindInterfacesAndSelfTo<Bootstrap>().AsSingle().WithArguments("SampleWorld").NonLazy();
    }

    private void BindSystems()
    {
        Container.BindInterfacesAndSelfTo<PlayerCalculateCountSystem>().AsSingle().WithArguments(10).NonLazy();
        Container.BindInterfacesAndSelfTo<TestSystem>().AsSingle().WithArguments(20).NonLazy();
        Container.BindInterfacesAndSelfTo<TestSystemJob>().AsSingle().WithArguments(30).NonLazy();
    }
}