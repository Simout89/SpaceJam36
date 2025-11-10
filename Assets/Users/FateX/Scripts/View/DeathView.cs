using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Users.FateX.Scripts.View
{
    public class DeathView: MonoBehaviour
    {
        [SerializeField] private GameObject deathView;
        [SerializeField] private Button _button;


        public void Awake()
        {
            _button.onClick.AddListener(HandleClick);
        }

        private void HandleClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ShowDeathView()
        {
            deathView.SetActive(true);
        }
    }
}