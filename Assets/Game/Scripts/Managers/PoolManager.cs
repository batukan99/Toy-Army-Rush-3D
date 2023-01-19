using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Vehicle;
using Game.Core.Army;

namespace Game.Managers 
{
    public class PoolManager : MonoSingleton<PoolManager>, IProvidable
    {
        public ObjectPool objectPool;

        public Piece GetPieceObject(Vector3 position, Quaternion rotation, Transform parent) 
        {
            return objectPool.GetPooledObject(0, position, rotation, parent).GetComponent<Piece>();
        }
        public AllyAI GetAllyObject(Vector3 position, Quaternion rotation, Transform parent) 
        {
            return objectPool.GetPooledObject(1, position, rotation, parent).GetComponent<AllyAI>();
        }
        public Bullet GetBulletObject(Vector3 position, Quaternion rotation, Transform parent) 
        {
            return objectPool.GetPooledObject(2, position, rotation, parent).GetComponent<Bullet>();
        }
        public Bullet GetHomingBulletObject(Vector3 position, Quaternion rotation, Transform parent) 
        {
            return objectPool.GetPooledObject(3, position, rotation, parent).GetComponent<HomingBullet>();
        }
        public RectTransform GetMoneyPopUpObject(Vector3 position, Quaternion rotation, Transform parent) 
        {
            return objectPool.GetPooledObject(4, position, rotation, parent).GetComponent<RectTransform>();
        }

        public void ReturnPieceToPool(GameObject piece) 
        {
            objectPool.AddToPool(0, piece);
        }
        public void ReturnArmyToPool(GameObject armyBase) 
        {
            objectPool.AddToPool(1, armyBase);
        }
        public void ReturnBulletToPool(GameObject bullet) 
        {
            objectPool.AddToPool(2, bullet);
        }
        public void ReturnHomingBulletToPool(GameObject bullet) 
        {
            objectPool.AddToPool(3, bullet);
        }
        public void ReturnMoneyPopUpToPool(GameObject MoneyPopUp) 
        {
            objectPool.AddToPool(4, MoneyPopUp);
        }

        private void Awake() {
            ManagerProvider.Register(this);
        }

    }
}
