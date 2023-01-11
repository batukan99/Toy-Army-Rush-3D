using UnityEngine;
using Game.Core.Data;
using Game.Core.Enums;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "SO_Bullet", menuName = "Game/SO_Bullet", order = 0)]
public class SO_Bullet : ScriptableObject
{
    public SerializedDictionary<AllyType, BulletData> BulletDatas;
/*
    private void OnEnable() 
    {
        BulletDatas.Add(AllyType.Rifle, new BulletData());
        BulletDatas.Add(AllyType.Sniper, new BulletData());
        BulletDatas.Add(AllyType.Rocket, new BulletData());

    }*/
    
}