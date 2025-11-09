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
        [SerializeField] private Transform cardContainer;
        [SerializeField] private GameObject menu;

        private const int maxCards = 3;

        [Inject] private SnakeSpawner _snakeSpawner;
        [Inject] private GameStateManager _gameStateManager;

        private void Awake()
        {
            SpawnCards(2);
        }

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

        public void SpawnCards(int amount)
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
        }

        private void HandleClick(ChoiceCard capturedCard)
        {
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