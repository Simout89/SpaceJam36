using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Скриптерсы;
using Random = UnityEngine.Random;

namespace Users.FateX.Scripts.View
{
    public class ChoiceCardMenu: MonoBehaviour
    {
        [SerializeField] private ChoiceCard[] choiceCards;
        [SerializeField] private ChoiceCard choiceCardStats;
        [SerializeField] private Transform cardContainer;
        [SerializeField] private GameObject menu;

        private const int maxCards = 3;
        private const int MaxEnemies = 50;

        [Inject] private SnakeSpawner _snakeSpawner;
        [Inject] private GameStateManager _gameStateManager;

        public void ShowMenu()
        {
            menu.SetActive(true);
            _gameStateManager.ChangeState(GameStates.CardMenu);
        }

        public void HideMenu()
        {
            menu.SetActive(false);
            _gameStateManager.ChangeState(GameStates.Play);
        }

        public void SpawnCards(int amount, int statsCards = 0)
        {
            ShowMenu();
            
            List<ChoiceCard> randomCards = new List<ChoiceCard>(choiceCards);

            for (int i = 0; i < amount; i++)
            {
                var card = randomCards[Random.Range(0, randomCards.Count)];

                var newCard = Instantiate(card, cardContainer);
                
                ChoiceCard capturedCard = newCard;
                newCard.Button.onClick.AddListener(() => HandleClick(capturedCard));
                
                randomCards.Remove(card);
            }

            if (statsCards > 0)
            {
                var newCardStats = Instantiate(choiceCardStats, cardContainer);
                newCardStats.Button.onClick.AddListener(() => HandleClick(newCardStats));
                if(newCardStats.TryGetComponent(out ChoiceCardStats choiceCard)) choiceCard.Init(_snakeSpawner.SnakeStats);
            }

        }

        private void HandleClick(ChoiceCard capturedCard)
        {
            if(capturedCard.SnakeBodyPart != null)
                _snakeSpawner.Snake.Grow(capturedCard.SnakeBodyPart);
            DeleteCards();
        }

        public void DeleteCards()
        {
            foreach (Transform child in cardContainer)
            {
                Destroy(child.gameObject);
            }
            
            HideMenu();
        }
    }
}