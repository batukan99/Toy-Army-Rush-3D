using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Events;
using EventType = Game.Core.Enums.EventType;
public class Test : MonoBehaviour
{
    private void OnEnable()
        {
            EventBase.StartListening(EventType.GameStarted, OnGameStarted);
            //UISignals.Instance.onUpdateScore += OnScoreUpdated;
        
        }

        private void OnDisable()
        {
            EventBase.StopListening(EventType.GameStarted, OnGameStarted);
           /* if(UISignals.Instance != null)
                UISignals.Instance.onUpdateScore -= OnScoreUpdated;*/
        }

        private void OnGameStarted() 
        {
            Debug.Log("started");
        }

        private void OnScoreUpdated() 
        {
        }
}
