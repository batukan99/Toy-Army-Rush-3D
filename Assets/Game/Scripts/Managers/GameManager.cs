using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Events;
using EventType = Game.Core.Enums.EventType;

namespace Game.Managers
{
    public class GameManager : MonoSingleton<GameManager>, IProvidable
    {
        public bool isGameStarted {get; private set;}
        public bool IsFinishReached { get; set; }
        public Camera mainCamera { get; private set; }
        private LevelManager _levelManager;

        private void Start()
        {
            _levelManager = ManagerProvider.GetManager<LevelManager>();
            Application.targetFrameRate = 60;

            isGameStarted = false;
            IsFinishReached = false;

            DataManager.Instance.GetDatas();
            _levelManager.CreateLevel();
            mainCamera = Camera.main;

            DG.Tweening.DOTween.SetTweensCapacity(200, 100);
        }

        void Update()
        {
            //UISignals.Instance.onUpdateScore?.Invoke();
        }

        public void StartGame()
        {
            if (!isGameStarted)
            {
                isGameStarted = true;
                EventBase.NotifyListeners(EventType.GameStarted);
            }
        }
        public void EndGame(bool status)
        {
            if (isGameStarted)
            {
                isGameStarted = false;
                EventBase.NotifyListeners(EventType.GameOver, status);
            }
        }

        private void OnEnable() {
            ManagerProvider.Register(this);
        }

    }

}