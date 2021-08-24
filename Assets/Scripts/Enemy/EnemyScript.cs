using System;
using Enemy.Movement;
using UnityEngine;

namespace Enemy
{
    public class EnemyScript : MonoBehaviour
    {

        /// <summary> The number of seconds this Enemy has been alive, in game time. </summary>
        public float LifeTime => Time.time - SpawnTime;

        public EnemyWaveType WaveType;
        public GenericMovement Movement;
        public float Speed;
        public Vector3 SpawnPoint;
        public float SpawnTime;
        public int Wave;
        public int WaveID;
        public int MovementPhase;
        public int AttackPhase;

        private void Start()
        {
            SpawnTime = Time.time;
        }

        private void Update()
        {
            Movement.DoMovement(this);
        }

        private void OnDestroy()
        {
            switch (WaveType)
            {
                case EnemyWaveType.Normal:
                    WaveController.Instance.NormalEnemyDestroyed(this);
                    break;
                case EnemyWaveType.Bonus:
                    WaveController.Instance.BonusEnemyDestroyed(this);
                    break;
                case EnemyWaveType.Boss:
                    WaveController.Instance.BossEnemyDestroyed(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public enum EnemyWaveType
        {
            Normal,
            Bonus,
            Boss
        }
    }
}
