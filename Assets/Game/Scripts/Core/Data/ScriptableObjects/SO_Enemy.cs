using UnityEngine;
using Game.Core.Data;


[CreateAssetMenu(fileName = "SO_Enemy", menuName = "Game/SO_Enemy", order = 0)]
public class SO_Enemy : ScriptableObject
{
    public EnemyData EnemyData;
}
