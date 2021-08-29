using UnityEngine;

namespace Enemy.Data
{
    [CreateAssetMenu(menuName = "Enemies/BossData")]
    public class BossData : WaveEnemyData
    {
        public SubordinateData[] Subordinates;
    }
}
