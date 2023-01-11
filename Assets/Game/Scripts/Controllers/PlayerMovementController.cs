using UnityEngine;
using UnityEngine.Events;
using Game.Core.Data;
using Game.Managers;
using Game.Core.Events;
using EventType = Game.Core.Enums.EventType;

public class PlayerMovementController : MonoBehaviour
{

    private Rigidbody _rigidBody;

    private bool canMove = false;
    private PlayerData _playerData;
    private InputManager inputManager;

    private PlayerData GetPlayerData() => Resources.Load<SO_Player>("Data/SO_Player").PlayerData;
    public void SetPlayerData() => _playerData = GetPlayerData();
    private void Update()
    {
        if (canMove) 
        {
            MovementUpdate();
        }
    }

    private void MovementUpdate()
    {
        transform.Translate(new Vector3(0, 0, _playerData.ForwardSpeed * Time.deltaTime));
        Vector3 pos = transform.position;
        pos.x += inputManager.Input.x * _playerData.SidewaySpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -_playerData.ClampPosition.x, _playerData.ClampPosition.x);
        transform.position = pos;
    }

    private void OnGameStartedEvent() 
    {
        canMove = true;
    }
    private void OnGameOverEvent(bool status) 
    {
        canMove = false;
    }
    private void OnEnable() {
        _rigidBody = GetComponent<Rigidbody>();
        SetPlayerData();
        inputManager = ManagerProvider.GetManager<InputManager>();

        EventBase.StartListening(EventType.GameStarted, OnGameStartedEvent);
        EventBase.StartListening(EventType.GameOver, (UnityAction<bool>)OnGameOverEvent);
    }

    private void OnDisable() {
        EventBase.StopListening(EventType.GameStarted, OnGameStartedEvent);
        EventBase.StopListening(EventType.GameOver, (UnityAction<bool>)OnGameOverEvent);
    }
}
