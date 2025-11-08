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
            
            Container.BindInterfacesAndSelfTo<EnemySpawner>().FromNewComponentOnNewGameObject().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyManager>().FromNewComponentOnNewGameObject().AsSingle();
            
            Container.BindInterfacesAndSelfTo<ActiveEntities>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameTimer>().AsSingle();
            Container.BindInterfacesAndSelfTo<GamePlaySceneEntryPoint>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySpawnDirector>().AsSingle();
        }
    }
}