using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using Game.Core.Vehicle;
using Game.Core.Events;
using Game.Commands;
using EventType = Game.Core.Enums.EventType;
using DG.Tweening;

namespace Game.Controllers 
{ 
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform Target;
        [SerializeField] private Transform DollyCamera;
        [SerializeField] private Transform StartCamera;
        [SerializeField] private float Speed;

        private Stack<CameraZoomOutCommand> _zoomOutCommands = new Stack<CameraZoomOutCommand>();

        private void LateUpdate()
        {
            Vector3 pos = Target.position;

            pos.x = transform.position.x;
            pos.x = Mathf.Lerp(transform.position.x, Target.position.x, Speed * Time.deltaTime);

            transform.position = pos;
        }
        private void ShakeDollyCamera(float duration, float amount) 
        {
            DollyCamera.transform.DOKill();
            DollyCamera.transform.DOShakePosition(duration, amount, 15).SetEase(Ease.OutSine);
        }
        private void OnPieceDestroyedEvent(Piece piece)
        {
            ShakeDollyCamera(0.25f, 0.15f);
            CameraZoomOutCommand oldZoomOutCommand = _zoomOutCommands.Pop();
            oldZoomOutCommand.Undo();
        }
        private void OnPieceCollectedEvent(Piece piece)
        {
            ShakeDollyCamera(0.15f, 0.05f);
            CameraZoomOutCommand newZoomOutCommand = new CameraZoomOutCommand(DollyCamera);
            _zoomOutCommands.Push(newZoomOutCommand);
            newZoomOutCommand.Execute();
        }

        private void OnHelicopterDestroyedEvent()
        {
            ShakeDollyCamera(0.25f, 0.35f);
        }
        private void OnRocketExplodedEvent()
        {
            ShakeDollyCamera(0.25f, 0.07f);
        }

        private void OnGameStartedEvent() 
        {
            StartCamera.gameObject.SetActive(false);
        }
        private void OnGameOverEvent(bool status) 
        {
        }
        private void OnEnable() 
        {
            EventBase.StartListening(EventType.PieceDestroyed, OnPieceDestroyedEvent);
            EventBase.StartListening(EventType.PieceCollected, OnPieceCollectedEvent);
            EventBase.StartListening(EventType.GameStarted, OnGameStartedEvent);
            EventBase.StartListening(EventType.HelicopterDestroyed, OnHelicopterDestroyedEvent);
            EventBase.StartListening(EventType.RocketExploded, OnRocketExplodedEvent);
            EventBase.StartListening(EventType.GameOver, (UnityAction<bool>)OnGameOverEvent);
        }

        private void OnDisable() 
        {
            EventBase.StopListening(EventType.PieceDestroyed, OnPieceDestroyedEvent);
            EventBase.StopListening(EventType.PieceCollected, OnPieceCollectedEvent);
            EventBase.StopListening(EventType.GameStarted, OnGameStartedEvent);
            EventBase.StopListening(EventType.HelicopterDestroyed, OnHelicopterDestroyedEvent);
            EventBase.StopListening(EventType.RocketExploded, OnRocketExplodedEvent);
            EventBase.StopListening(EventType.GameOver, (UnityAction<bool>)OnGameOverEvent);
        }
    }
}

