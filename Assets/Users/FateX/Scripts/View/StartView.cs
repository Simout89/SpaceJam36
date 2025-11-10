using System;
using UnityEngine;
using Zenject;

namespace Users.FateX.Scripts.View
{
    public class StartView: MonoBehaviour
    {
        [Inject] private UserInfo _userInfo;
        [SerializeField] private GameObject view;

        private void Awake()
        {
            if (_userInfo.firstPlay)
            {
                view.SetActive(true);
            }
            else
            {
                view.SetActive(false);
            }
        }
    }
}