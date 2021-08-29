using Enemy.Behaviours;
using Enemy.Behaviours.Subordinates;
using UnityEngine;

namespace Enemy.Data
{
    [CreateAssetMenu(menuName = "Enemies/SubordinateData")]
    public class SubordinateData : EnemyDataBase
    {
        public SubordinateBehaviour Behaviour;

        public float Speed = 1f;

        public float SpawnTimer = 1f;
    }
}