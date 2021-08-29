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
            
            switch (_data[target].MovementPhase)
            {
                // Move Down
                case 0:
                {
                    target.transform.Translate(target.Speed * Time.deltaTime * Vector3.down);
                    if (target.transform.position.y <= GetTopBounds())
                    {
                        _data[target].NextSubordinate = Time.time + 1f;
                        _data[target].MovementPhase = 1;
                    }
                    break;
                }
                // Move Left
                case 1:
                {
                    target.transform.Translate(target.Speed * Time.deltaTime * Vector3.left);
                    if (target.transform.position.x <= GetLeftBounds()) _data[target].MovementPhase = 2;
                    break;
                }
                // Move Right
                case 2:
                {
                    target.transform.Translate(target.Speed * Time.deltaTime * Vector3.right);
                    if (target.transform.position.x >= GetRightBounds()) _data[target].MovementPhase = 1;
                    break;
                }

                default:
                    Debug.LogError($"Unknown Phase #{_data[target].MovementPhase} for {GetType()}!");
                    break;
            }

            if (Time.time > _data[target].NextSubordinate)
            {
                var data = (BossData) target.Data;
                WaveController.Instance.SpawnSubordinate(data.Subordinates[0], target.transform.position);
                _data[target].NextSubordinate = Time.time + data.Subordinates[0].SpawnTimer;
            }
        }
        public override void ClearData(AbstractEnemyScript target)
        {
            if (_data.ContainsKey(target)) _data.Remove(target);
        }

        private class UnitData
        {
            public int MovementPhase;
            public float NextSubordinate;
        }
    }
}
