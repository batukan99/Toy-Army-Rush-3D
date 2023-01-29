using System.Collections.Generic;
using UnityEngine;
using Game.Core.Data;
using DG.Tweening;
using Game.Core.Army;
using Game.Core.Vehicle;
using Game.StateMachines;
using Game.Core.Events;
using EventType = Game.Core.Enums.EventType;

namespace Game.Managers
{
    public class ArmyManager : MonoBehaviour, IProvidable
    {
        //public List<ArmyPlacementData> armyPlaces = new List<ArmyPlacementData>();

        [SerializeField] private Color armyColor;
        public List<AllyAI> armies = new List<AllyAI>();
        public List<AllyAI> deathArmies = new List<AllyAI>();
        protected List<Sequence> runningSequences = new List<Sequence>();

        private VehicleManager _vehicleManager;
        public int _currentPieceIndex = 0;
        private List<Piece> _pieces => _vehicleManager.GetCurrentVehiclePieces();
        private int _pieceCapacity => _pieces.Count > 0 ? _pieces[0].Capacity : 0;
        private float distanceBetweenArmies => (float)(_pieces[_currentPieceIndex].Length / _pieceCapacity) + 0.3f;
        
        

        private void Awake() 
        {
            ManagerProvider.Register(this);
            _vehicleManager = ManagerProvider.GetManager<VehicleManager>();
            ChangeLayerOfVehicleBase();
        }
        
        public bool IsFull()
        {
            return armies.Count >= _pieces.Count * _pieceCapacity;
        }

        public void ChangeLayerOfVehicleBase() 
        {
            /*if(armies.Count > 0) 
            {
                _vehicleManager.SetLayerOfVehicleBase("IgnoreBullet");
            } 
            else 
            {
                _vehicleManager.SetLayerOfVehicleBase("AllyVehicle");
            }*/
        }
        public virtual void AddArmyToVehicle(AllyAI army)
        {
            if(IsFull()) 
            {
                print(_currentPieceIndex);
                return;
            }
            armies.Add(army);
            army.meshRenderer.material.DOColor(armyColor, 1f).SetEase(Ease.OutQuart);
            
            /*
            army.PieceIndex = _currentPieceIndex;
            if(deathArmies.Count > 0) 
            {
                army.transform.SetParent(deathArmies[0].transform.parent);
                army.IsPlaced = true;
                PlayPlacementAnimation(army, deathArmies[0].GetTransform().localPosition.x, 0);
                _pieces[_currentPieceIndex].numberOfArmyAdded += 1;
                deathArmies.RemoveAt(0);
            } else
            */

            if (armies.Count >= 1)
            {
                Rearrange();
            }
            ChangeLayerOfVehicleBase();
        }

        public void AddArmiesToVehicle(List<AllyAI> armies)
        {
            this.armies.AddRange(armies);
        }


        protected void Rearrange()
        {
           foreach (Sequence seq in runningSequences)
            {
                if(seq.IsComplete()) {
                    seq.Kill();
                    runningSequences.Remove(seq);
                }
            }
            //runningSequences.Clear();

            
            for(int a = _currentPieceIndex * _pieceCapacity; a < armies.Count; a++) 
            {
                AllyAI army = armies[a];
                army.transform.SetParent(_pieces[_currentPieceIndex].ArmyPlace);

                
                float shiftAmount = CalculateArmyPlacement(army, a);

                PlayPlacementAnimation(army, shiftAmount, a);
                    
                if(_pieces[_currentPieceIndex].numberOfArmyAdded >= _pieceCapacity) 
                {
                    _currentPieceIndex += 1;
                    break;
                }
            }
                //armies[i].SkinnedMeshRenderer.material.color = colors.GetColor(armyPlaces[i].color);
                //armies[i].Animator.SetTrigger(AnimationHash.TryGetPoseHash(armyPlaces[i].pose));
        }

        private void PlayPlacementAnimation(AllyAI army, float shiftAmount, int placeIndex) 
        {
            army.transform.DOKill();

            Sequence seq = DOTween.Sequence();
            if(placeIndex == armies.Count - 1)
                seq.Append(army.transform.DOLocalMove(new Vector3(shiftAmount, 0.75f, 0), 0.25f));
            seq.Append(army.transform.DOLocalMove(new Vector3(shiftAmount, 0, 0), 0.25f));
            seq.Join(army.transform.DOLocalRotate(Vector3.zero, 0.25f));
            
            seq.Play().OnComplete(() => army.IsJumpedIntoVehicle = true);
            runningSequences.Add(seq);
        }

        private float CalculateArmyPlacement(AllyAI army, int index)
        {
            float shiftX = 0;
            if (army.IsPlaced == false)
            {
                _pieces[_currentPieceIndex].numberOfArmyAdded += 1;
                army.IsPlaced = true;
            }
            else 
            {
                int shiftSide = index % 2 == 0 ? 1: -1;
                shiftX = (distanceBetweenArmies + Mathf.Abs(army.LastShiftedValue)) * shiftSide;
                shiftX = army.IsShifted ? army.LastShiftedValue: shiftX;
                //if(index == 1) print("a: "+ index + " shiftX: " + shiftX + " IsShifted: " + army.IsShifted);
                army.IsShifted = !army.IsShifted;
           }
           army.LastShiftedValue = shiftX;
           return shiftX;
        }

        private void OnPieceDestroyedEvent(Piece piece) 
        {
            Vector3 movePos;
            float duration = 0.75f;
            bool containsArmy = false;

            foreach(AllyAI army in piece.ArmyPlace.GetComponentsInChildren<AllyAI>())
            {
                if(armies.Contains(army))
                {
                    //deathArmies.Add(army);
                    containsArmy = true;
                    armies.Remove(army);
                // _pieces[ally.PieceIndex].numberOfArmyAdded -= 1;
                }

                army.transform.SetParent(null);
                army.GetComponent<AllyAI>().Kill();

                movePos = army.transform.position + Random.onUnitSphere * Random.Range(1f, 2.5f);
                movePos = new Vector3(movePos.x, 0.75f, movePos.z);
                army.transform.DOMove(movePos, duration);
            }
            
            if(containsArmy)
            {
                _currentPieceIndex -= 1;
                if(_currentPieceIndex < 0)
                    _currentPieceIndex = 0;
            }
            
            //ChangeLayerOfVehicleBase();
        }

        private void OnTriggerEnter(Collider other)
        {
            AllyAI army = other.GetComponent<AllyAI>();

            if (army != null)
            {
                AddArmyToVehicle(army);

                //MMVibrationManager.Haptic(HapticTypes.LightImpact);

                // Move progress indicator.
                //uiManager.gamePanel.progressUI.MoveArrow(PieceCount);
            }
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
