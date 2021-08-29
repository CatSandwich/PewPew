using System;
using System.Collections.Generic;
using Singletons;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.Behaviours.Subordinates
{
    [CreateAssetMenu(menuName = "Enemies/Behaviours/Subordinates/ScrambleBehaviour")]
    public class SubordinateScrambleBehaviour : SubordinateBehaviour
    {
        public override float GetLeftBounds() => WaveController.LeftBounds + 1f;
        public override float GetRightBounds() => WaveController.RightBounds - 1f;
        public float GetTopBounds() => WaveController.TopBounds;

        public Vector2 TimeBeforeSpin = new Vector2(5f, 20f);
        public Vector2 TimeBeforeRun = new Vector2(5f, 20f);

        private readonly Dictionary<AbstractEnemyScript, UnitData> _data = new Dictionary<AbstractEnemyScript, UnitData>();

        public override void PrepareBehaviour(AbstractEnemyScript target)
        {
            if (_data.ContainsKey(target)) _data.Remove(target);
            _data.Add(target, new UnitData
            {
                TimeBeforeSpin = Random.Range(TimeBeforeSpin.x, TimeBeforeSpin.y),
                TimeBeforeRun = Random.Range(TimeBeforeRun.x, TimeBeforeRun.y)
            });
            SetNewPosition(target);
        }
        public override void DoBehaviour(AbstractEnemyScript target)
        {
            if (!WaveController.RunIsAlive) return;

            var data = _data[target];

            switch (_data[target].MovementPhase)
            {
                case 0:
                {
                    target.transform.Translate(target.Speed * Time.deltaTime * (data.TargetPosition - target.transform.position).normalized);

                    if (Vector3.Distance(target.transform.position, data.TargetPosition) < 0.01f)
                    {
                        if (target.LifeTime > _data[target].TimeBeforeSpin)
                        {
                            data.MovementPhase = 1;
                            data.Time = Time.time;
                            return;
                        }
                        SetNewPosition(target);
                    }
                    break;
                }

                case 1:
                {
                    var t = Time.time - data.Time;
                    var r = Mathf.Min(t, 0.5f);
                    target.transform.position = _data[target].TargetPosition + new Vector3(Mathf.Cos(t * 3) * r, Mathf.Sin(t * 3) * r, 0f);

                    if (target.LifeTime > data.TimeBeforeSpin + data.TimeBeforeRun)
                    {
                        data.MovementPhase = 2;
                    }
                    break;
                }

                case 2:
                {
                    target.transform.Translate(target.Speed * Time.deltaTime * Vector3.down);
                    break;
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
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
            public Vector3 TargetPosition;
            public float TimeBeforeSpin;
            public float TimeBeforeRun;
            public float Time;
        }
    }
}
