using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu()]
    public class GenericEnemy : ScriptableObject
    {
        public GameObject Prefab;
        public GameObject GetPrefab() => Prefab;
    }
}
