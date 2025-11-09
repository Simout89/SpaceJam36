using UnityEngine;
using Users.FateX.Scripts;
using Users.FateX.Scripts.Enemys;
using Users.FateX.Scripts.Entity;
using Users.FateX.Scripts.View;
using Zenject;

namespace Скриптерсы.Zenject
{
    public class GamePlayInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EnemySpawnArea>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<SnakeSpawner>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<HealthView>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<ChoiceCardMenu>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyManager>().FromComponentsInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<EnemySpawner>().FromNewComponentOnNewGameObject().AsSingle();
            
            Container.BindInterfacesAndSelfTo<ActiveEntities>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameTimer>().AsSingle();
            Container.BindInterfacesAndSelfTo<GamePlaySceneEntryPoint>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySpawnDirector>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerExperience>().AsSingle();
            Container.BindInterfacesAndSelfTo<LootManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<DeathHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<UserInfo>().AsSingle();
        }
    }
}