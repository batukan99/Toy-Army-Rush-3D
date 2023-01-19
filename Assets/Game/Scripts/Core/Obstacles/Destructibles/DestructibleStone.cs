using UnityEngine;
using Game.Managers;
using DG.Tweening;
using Game.Core.Events;
using Game.Core.Vehicle;
using EventType = Game.Core.Enums.EventType;

namespace Game.Core.Obstacles
{
    public class DestructibleStone : DestructibleBase
    {
        [SerializeField] private Transform[] itemsOnTop;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private bool isBonusObstacle = false;

        [Space, Header("Materials")]
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material greyMaterial;

        protected override void Awake()
        {
            base.Awake();

            healthUI.SetHealth(health);
        }


        public override void BreakApart()
        {
            if (itemsOnTop != null)
            {
                ThrowTheItemsDown();
            }

            base.BreakApart();
        }

        private void ThrowTheItemsDown()
        {
            foreach (Transform item in itemsOnTop)
            {
                Vector3 jumpPos;

                item.SetParent(null);
                
                if (item.GetComponent<Money>() != null)
                {
                    jumpPos = new Vector3(item.localPosition.x, 1, item.localPosition.z)  + Vector3.back * 1f;
                }
                else
                {
                    jumpPos = new Vector3(item.localPosition.x, 1, item.localPosition.z) + Vector3.forward * 4f;
                }

                item.DOLocalJump(jumpPos, 0.8f, 1, 0.37f).SetEase(Ease.InOutSine);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            VehicleManager vehicleManager = other.GetComponent<VehicleManager>();

            if (vehicleManager != null)
            {
                if (isBonusObstacle)
                {
                   /* for(int i = 0; i < vehicleManager.PieceCount; i++) 
                    {
                        print("pieceCount: " + vehicleManager.PieceCount);
                        Piece lastPiece = vehicleManager.GetLastVehiclePiece();
                        EventBase.NotifyListeners(EventType.PieceDestroyed, lastPiece);
                    }*/
                    vehicleManager.RemovePieces(vehicleManager.PieceCount);
                    GameManager.Instance.EndGame(true);
                }
                else
                {
                    print(other.gameObject.name);
                    Piece lastPiece = vehicleManager.GetLastVehiclePiece();
                    EventBase.NotifyListeners(EventType.PieceDestroyed, lastPiece);
                    //vehicleManager.RemovePiece();
                }
                Disappear();
            }
        }


        private void OnValidate()
        {
            healthUI?.SetHealth(health);
            
            if (meshRenderer != null)
            {
                meshRenderer.material = isBonusObstacle ? greyMaterial : defaultMaterial;
            }
        }
    }
}
