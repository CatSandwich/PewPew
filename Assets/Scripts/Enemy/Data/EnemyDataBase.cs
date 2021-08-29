using UnityEngine;

namespace Enemy.Data
{
    public abstract class EnemyDataBase : ScriptableObject
    {
        public GameObject Prefab;
        public GameObject GetPrefab() => Prefab;

        [Tooltip("The maximum health of this enemy - note some weapons do extra damage")]
        public int MaxHealth = 1;

        [Tooltip("The score added when this enemy is killed.")]
        public float ScoreValue = 5;

        [Tooltip("The total value of Coins dropped when this enemy is killed.")]
        public int CoinValue = 1;
    }
}
