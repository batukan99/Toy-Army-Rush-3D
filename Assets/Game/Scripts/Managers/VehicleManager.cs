using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Vehicle;
using Game.Core.Army;
using Game.Core.Events;
using EventType = Game.Core.Enums.EventType;
using DG.Tweening;

namespace Game.Managers
{
    public class VehicleManager : MonoBehaviour, IProvidable
    {
        [SerializeField] private AllyAI firstArmy;
        [SerializeField] private Piece firstPiece;
        [SerializeField] private Transform backFence;
        [SerializeField] private VehicleBase vehicleBase;
        public int PieceCount => _collectedPieces.Count;

        private Stack<Piece> _collectedPieces = new Stack<Piece>();

        private UIManager _uiManager;
        private PoolManager _poolManager;
        
        private void Awake()
        {
            ManagerProvider.Register(this);
            _uiManager = ManagerProvider.GetManager<UIManager>();
            _poolManager = ManagerProvider.GetManager<PoolManager>();
            
        }

        private void Start()
        {
            AddNewPiece(firstPiece);
            //AddNewArmy(firstArmy);
        }

        public List<Piece> GetCurrentVehiclePieces()
        {
            return vehicleBase.pieces;
        }
        public Piece GetLastVehiclePiece()
        {
            return vehicleBase.pieces[vehicleBase.pieces.Count - 1];;
        }
        public void InitializePieces(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Piece piece = _poolManager.GetPieceObject(transform.position, transform.rotation, transform);
                //piece.gameObject.SetActive(true);
                //piece.transform.position = transform.position;
                AddNewPiece(piece);
            }
        }
        public void AddNewPiece(Piece newPiece)
        {
            bool isExtra = false;

            newPiece.Collider.enabled = false;

            if (vehicleBase.gameObject.activeSelf)
            {
                if (vehicleBase.IsFull())
                {
                    //Vector3 origin = GameManager.Instance.mainCamera.WorldToScreenPoint(transform.position);
                    //moneyPopupHandler.ShowMoneyPopup(1, origin, () => LevelManager.Instance.AddMoney(1));
                    newPiece.gameObject.SetActive(false);
                    isExtra = true;
                }
                else
                {
                    vehicleBase.AddPieceToVehicle(newPiece);

                    if(!backFence.gameObject.activeSelf)
                        backFence.gameObject.SetActive(true);
                    backFence.DOComplete();
                    backFence.DOLocalMoveZ(backFence.localPosition.z - 1f, 0.25f);
                }
            }

            if (!isExtra)
            {
                _collectedPieces.Push(newPiece);
            }
        }
        public void RemovePieces(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                RemovePiece();
            }
        }

        public void RemovePiece()
        {
            if (_collectedPieces.TryPop(out Piece piece))
            {
                Vector3 movePos;
                float duration = 0.75f;

                vehicleBase.pieces.Remove(piece);
                
                Sequence seq = DOTween.Sequence();
            
                piece.transform.SetParent(null);
                movePos = transform.position + Random.onUnitSphere * Random.Range(1f, 2.5f);
                seq.Append(piece.transform.DOMove(movePos, duration));
                seq.Join(piece.transform.DORotate(Random.onUnitSphere * Random.Range(30f, 270f), duration));
                seq.Join(piece.transform.DOScale(0f, duration));
                seq.OnComplete(() => piece.gameObject.SetActive(false));

                backFence.DOComplete();
                backFence.DOLocalMoveZ(backFence.localPosition.z + 1f, 0.25f);
                // Move progress indicator.
                //uiManager.gamePanel.progressUI.MoveArrow(PieceCount);
            }

            if (PieceCount < 1)
            {
                if(!GameManager.Instance.IsFinishReached)
                    GameManager.Instance.EndGame(false);
                    
                backFence.gameObject.SetActive(false);
                return;
            }
        }

        public void SetLayerOfVehicleBase(string layer) 
        {
            vehicleBase.gameObject.layer = LayerMask.NameToLayer(layer);
        }

        private void OnTriggerEnter(Collider other)
        {
            Piece piece = other.GetComponent<Piece>();

            if (piece != null)
            {
                AddNewPiece(piece);

                //MMVibrationManager.Haptic(HapticTypes.LightImpact);

                // Move progress indicator.
                //uiManager.gamePanel.progressUI.MoveArrow(PieceCount);
            }
        }

        private void OnPieceDestroyedEvent(Piece destroyedPiece) 
        {
            RemovePiece();
        }
        private void OnEnable()
        {
            EventBase.StartListening(EventType.PieceDestroyed, OnPieceDestroyedEvent);
        }

        private void OnDisable()
        {
            EventBase.StopListening(EventType.PieceDestroyed, OnPieceDestroyedEvent);
        }

    }
}
