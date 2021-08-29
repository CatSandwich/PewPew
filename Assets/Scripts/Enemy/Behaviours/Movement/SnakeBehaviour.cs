using System;
using System.Collections.Generic;
using Singletons;
using UnityEngine;

namespace Enemy.Behaviours.Movement
{
    [CreateAssetMenu(menuName = "Enemies/Behaviours/Generic/SnakeBehaviour")]
    // ReSharper disable once UnusedMember.Global
    public class SnakeBehaviour : AbstractBehaviour
    {
        public override float GetLeftBounds() => WaveController.LeftBounds - 2;
        public override float GetRightBounds() => WaveController.RightBounds + 2;

        private readonly Dictionary<AbstractEnemyScript, UnitData> _data = new Dictionary<AbstractEnemyScript, UnitData>();

        public float Height = 2f;
        public float SinewaveSpeed = 1f;

        public override void PrepareBehaviour(AbstractEnemyScript target)
        {
            if (_data.ContainsKey(target)) _data.Remove(target);
            var spawnedLeft = target.SpawnPoint.x < 0;
            _data.Add(target, new UnitData
            {
                MovementPhase = spawnedLeft ? 0 : 1,
                UnitY = target.SpawnPoint.y
            });
        }
        public override void DoBehaviour(AbstractEnemyScript target)
        {
            if (!WaveController.RunIsAlive) return;
            var data = _data[target];

            switch (data.MovementPhase)
            {
                case 0:
                {
                    target.transform.position = new Vector3(target.transform.position.x + (target.Speed * Time.deltaTime), data.UnitY + Mathf.Cos(target.LifeTime * SinewaveSpeed) * Height, target.transform.position.z);
                    if (target.transform.position.x > GetRightBounds())
                    {
                        data.UnitY -= 1;
                        data.MovementPhase = 1;
                    }
                    break;
                }
                case 1:
                {
                    target.transform.position = new Vector3(target.transform.position.x - (target.Speed * Time.deltaTime), data.UnitY + Mathf.Cos(target.LifeTime * SinewaveSpeed) * Height, target.transform.position.z);
                    if (target.transform.position.x < GetLeftBounds())
                    {
                        data.UnitY -= 1;
                        data.MovementPhase = 0;
                    }
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

        private class UnitData
        {
            public int MovementPhase;
            public float UnitY;
        }
    }
}