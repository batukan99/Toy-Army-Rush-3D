using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Game.Core.Army;

namespace Game.Core.Vehicle.Enemy 
{
    public class EnemyHelicopter : MonoBehaviour
    {
        [SerializeField] private Transform Rotor;
        [SerializeField] public GameObject Body;
        [SerializeField] private EnemyAI enemyAI;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Color deathColor = new Color32(80, 80, 80, 255);
        [SerializeField] public float ForwardSpeed = 5.0f;
        [SerializeField] public float MaxHeight = 5.0f;
        [SerializeField] public float WeavingDistance = 1.5f;
        [SerializeField] public float DropDistance = 20.0f;
        [SerializeField] public float InboundRange = 20.0f;

        private List<IManeuverBehaviour> _strategies = new List<IManeuverBehaviour>();

        private float _enemyAILastPositionX;
        private bool _isDeath;
        private void Start() 
        {
            Rotor.DOLocalRotate(new Vector3(0, 360, 0), 1, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
            //transform.DOLocalMoveY(2, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            
            
            _strategies.Add(gameObject.AddComponent<BopplingManeuver>());
            _strategies.Add(gameObject.AddComponent<WeavingManeuver>());

            _enemyAILastPositionX = enemyAI.GetTransform().position.x;
        }

        public bool canMove;
        public void ApplyStrategy(IManeuverBehaviour strategy) 
        {
            if(!_isDeath)
                strategy.Maneuver(this);
        }

        public void DecideStrategy()
        {
            canMove = true;
            int index = Random.Range(0, _strategies.Count);
            
            ApplyStrategy(_strategies[index]);

        }

        public void EncounterStrategy() 
        {
            ApplyStrategy( gameObject.AddComponent<EncounterManeuver>() );
        }

        public void MoveEnemyToOtherSide() 
        {
            float newPositionX = _enemyAILastPositionX;
            newPositionX = newPositionX < 0 ? 1 : -1;
            _enemyAILastPositionX = newPositionX;

            enemyAI.GetTransform().DOLocalMoveX(newPositionX, 0.5f).SetEase(Ease.InOutSine);
        }

        private void Update()
        {
            if(enemyAI.IsDeath() && !_isDeath)
            {
                KillHelicopter();
                _isDeath = true;
            }

            if (canMove) 
            {
                MovementUpdate();
            }
        }

        private void MovementUpdate()
        {
            transform.position += Vector3.forward * ForwardSpeed * Time.deltaTime;
        }

        private void KillHelicopter()
        {
            transform.DOKill();
            transform.DOShakeRotation(1f, 1, 2, 1);
            transform.DOShakePosition(1f, 1, 2, 1).OnComplete( () => DropHelicopter() );
        }

        private void DropHelicopter()
        {
            meshRenderer.material.DOColor(deathColor, 1.5f).SetEase(Ease.OutQuart);

            transform.DOKill();

            Vector3 currentRotation = transform.eulerAngles;
            transform.DOMoveY(transform.position.y - DropDistance, 2f);
            transform.DORotate(new Vector3(50, currentRotation.y, currentRotation.z), 1.5f)
                .OnComplete( () =>  transform.DOScale(new Vector3(0, 0, 0), 0.5f)
                    .OnComplete( () => gameObject.SetActive(false) ));
        }
    }
}

