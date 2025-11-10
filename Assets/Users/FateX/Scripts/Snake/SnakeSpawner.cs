using UnityEngine;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts
{
    public class SnakeSpawner: MonoBehaviour
    {
        [Inject] private IInputService _inputService;
        
        [SerializeField] private Snake _snakePrefab;
        [SerializeField] private Transform spawnPoint;
        
        public Snake Snake { get; private set; }
        public SnakeStats SnakeStats { get; private set; }

        public Snake SpawnSnake()
        {
            Snake snake = Instantiate(_snakePrefab);
            snake.Init(_inputService);
            snake.transform.position = spawnPoint.position;
            Snake = snake;
            SnakeStats = snake.GetComponent<SnakeStats>();
            return snake;
        }
    }
}