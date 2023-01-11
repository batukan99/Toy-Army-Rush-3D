using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Data;
using DG.Tweening;
using Game.Core.Army;
using Game.Core.Events;
using EventType = Game.Core.Enums.EventType;

namespace Game.Core.Vehicle
{
    public class VehicleBase : IDamageable
    {
        public List<PiecePlacementData> piecePlaces = new List<PiecePlacementData>();
        public List<Piece> pieces = new List<Piece>();
        protected List<Sequence> runningSequences = new List<Sequence>();

        private bool _isDeath = false;

        public virtual void AddPieceToVehicle(Piece piece)
        {
            pieces.Add(piece);
            
            if (pieces.Count >= 1)
            {
                Rearrange();
            }
        }

        public void AddPiecesToVehicle(List<Piece> pieces)
        {
            this.pieces.AddRange(pieces);
        }

        protected void Rearrange()
        {
            foreach (Sequence seq in runningSequences)
            {
                seq.Kill();
            }
            runningSequences.Clear();

            for (int i = 0; i < piecePlaces.Count; i++)
            {
                if (i >= pieces.Count)
                {
                    piecePlaces[i].isUsed = false;
                    return;
                }

                piecePlaces[i].isUsed = true;
                pieces[i].transform.SetParent(piecePlaces[i].position);

                pieces[i].transform.DOKill();

                Sequence seq = DOTween.Sequence();
                seq.Append(pieces[i].transform.DOLocalMove(Vector3.zero, 0.25f));
                seq.Join(pieces[i].transform.DOLocalRotate(Vector3.zero, 0.25f));
                seq.Play();
                runningSequences.Add(seq);

                //pieces[i].SkinnedMeshRenderer.material.color = colors.GetColor(piecePlaces[i].color);
                //pieces[i].Animator.SetTrigger(AnimationHash.TryGetPoseHash(piecePlaces[i].pose));
            }
        }

        public override void TakeDamage(float damage)
        {
            if(pieces.Count <= 0)
                return;

            Piece lastPiece = pieces[pieces.Count - 1];
            lastPiece.Health -= damage;

            if (lastPiece.Health <= 0)
                EventBase.NotifyListeners(EventType.PieceDestroyed, lastPiece);
        }
        public bool IsFull()
        {
            return pieces.Count >= piecePlaces.Count;
        }
        
        public override Transform GetTransform() => transform;

        public override bool IsAttackable() => !IsDeath();

        public override bool IsDeath() => _isDeath;

        
    }
}

