using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu()]
    public class EnemyBase : ScriptableObject
    {
        public GameObject Prefab;
        public GameObject GetPrefab() => Prefab;

        public int MaxHealth = 1;
    }
}
