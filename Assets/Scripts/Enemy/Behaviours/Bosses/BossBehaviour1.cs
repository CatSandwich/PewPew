using System.Collections.Generic;
using Enemy.Data;
using Singletons;
using UnityEngine;

namespace Enemy.Behaviours.Bosses
{
    [CreateAssetMenu(menuName = "Enemies/Behaviours/Unique/BossBehaviour1")]
    // ReSharper disable once UnusedMember.Global
    public class BossBehaviour1 : AbstractBehaviour
    {
        public override float GetLeftBounds() => WaveController.LeftBounds + 1f;
        public override float GetRightBounds() => WaveController.RightBounds - 1f;
        public float GetTopBounds() => WaveController.TopBounds;

        private readonly Dictionary<AbstractEnemyScript, UnitData> _data = new Dictionary<AbstractEnemyScript, UnitData>();

        public override void PrepareBehaviour(AbstractEnemyScript target)
        {
            if (_data.ContainsKey(target)) _data.Remove(target);
            _data.Add(target, new UnitData { NextSubordinate = Time.time + 5f });

        }

        public override void DoBehaviour(AbstractEnemyScript target) => DoBehaviour((WaveEnemyScript)target);
        public void DoBehaviour(WaveEnemyScript target)
        {
            if (!WaveController.RunIsAlive) return;
            var data = _data[target];

            switch (data.MovementPhase)
            {
                // Move Down
                case 0:
                {
                    target.transform.Translate(target.Speed * Time.deltaTime * Vector3.down);
                    if (target.transform.position.y <= GetTopBounds()) data.MovementPhase = 1;
                    break;
                }
                // Move Left
                case 1:
                {
                    target.transform.Translate(target.Speed * Time.deltaTime * Vector3.left);
                    if (target.transform.position.x <= GetLeftBounds())
                    {
                        data.MovementPhase = 2;
                        data.TurnTime = Time.time;
                    }
                    break;
                }
                // Move Right
                case 2:
                {
                    target.transform.Translate(target.Speed * Time.deltaTime * Vector3.right);
                    if (target.transform.position.x >= GetRightBounds())
                    {
                        data.MovementPhase = 1;
                        data.TurnTime = Time.time;
                    }
                    break;
                }
                default:
                {
                    Debug.LogError($"Unknown Phase #{data.MovementPhase} for {GetType()}!");
                    data.MovementPhase = 0;
                    break;
                }
            }

            var bossData = (BossData)target.Data;

            if (data.AttackPhase != 1 && Mathf.Abs(Time.time - data.TurnTime) > 1f) return;
            if (Time.time < data.NextSubordinate) return;

            WaveController.Instance.SpawnSubordinate(target, bossData.Subordinates[0], target.transform.position);
            if (data.SubordinateCounter < 4)
            {
                data.NextSubordinate = Time.time + 0.5f;
                data.SubordinateCounter++;
                data.AttackPhase = 1;
            }
            else
            {
                data.NextSubordinate = Time.time + 10f;
                data.SubordinateCounter = 0;
                data.AttackPhase = 0;
            }
        }
        public override void ClearData(AbstractEnemyScript target)
        {
            if (_data.ContainsKey(target)) _data.Remove(target);
        }

        private class UnitData
        {
            public int MovementPhase;
            public int AttackPhase;
            public float NextSubordinate;
            public int SubordinateCounter;
            public float TurnTime;
        }
    }
}
