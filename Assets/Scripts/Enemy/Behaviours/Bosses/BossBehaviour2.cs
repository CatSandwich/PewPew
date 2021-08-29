using System.Collections.Generic;
using Enemy.Data;
using Singletons;
using UnityEngine;

namespace Enemy.Behaviours.Bosses
{
    [CreateAssetMenu(menuName = "Enemies/Behaviours/Unique/BossBehaviour2")]
    // ReSharper disable once UnusedMember.Global
    public class BossBehaviour2 : AbstractBehaviour
    {
        public override float GetLeftBounds() => WaveController.LeftBounds + 1f;
        public override float GetRightBounds() => WaveController.RightBounds - 1f;
        public float GetTopBounds() => WaveController.TopBounds;

        public Vector2 SubordinateDelayRange = new Vector2(1f, 5f);

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
                    if (target.transform.position.y <= GetTopBounds())
                    {
                        SetNewPosition(target);
                        data.MovementPhase = 1;
                    }
                    break;
                }

                case 1:
                {
                    target.transform.Translate(target.Speed * Time.deltaTime * (data.TargetPosition - target.transform.position).normalized);
                    if (Vector3.Distance(target.transform.position, data.TargetPosition) < 0.01f)
                    {
                        SetNewPosition(target);
                        data.MovementPhase = 1;
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

            if (Time.time < data.NextSubordinate) return;

            var bossData = (BossData) target.Data;

            WaveController.Instance.SpawnSubordinate(target, bossData.Subordinates[0], target.transform.position);
            data.NextSubordinate = Time.time + Random.Range(SubordinateDelayRange.x, SubordinateDelayRange.y);
        }

        public override void ClearData(AbstractEnemyScript target)
        {
            if (_data.ContainsKey(target)) _data.Remove(target);
        }

        private void SetNewPosition(AbstractEnemyScript target)
        {
            var top = GetTopBounds();
            var half = top / 2f;
            _data[target].TargetPosition = new Vector3(Random.Range(-GetLeftBounds(), GetLeftBounds()), Random.Range(half, top), 0f);
        }

        private class UnitData
        {
            public int MovementPhase;
            public float NextSubordinate;
            public Vector3 TargetPosition;
        }
    }
}
