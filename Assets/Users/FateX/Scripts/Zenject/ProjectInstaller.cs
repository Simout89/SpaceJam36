using Users.FateX.Scripts;
using Users.FateX.Scripts.LeaderBoard;
using Zenject;
using Скриптерсы.Services;

namespace Скриптерсы.Zenject
{
    public class ProjectInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputService>()
                .AsSingle()
                .NonLazy();
            Container.BindInterfacesAndSelfTo<LeaderboardManager>()
                .AsSingle()
                .NonLazy();
            Container.BindInterfacesAndSelfTo<UserInfo>()
                .AsSingle()
                .NonLazy();
        }
    }
}