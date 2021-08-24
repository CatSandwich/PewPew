using System;
using Enemy.Data;
using UnityEngine;

namespace Singletons
{
    public static class ScoreKeeper
    {
        public static bool IsFrozen { get; private set; }
        public static float TotalScore => CurrentScore + CurrentDistance;
        public static float CurrentDistance => Time.time - ResetTime;
        public static float CurrentScore { get; private set; }
        public static float ResetTime { get; private set; }
        public static int Kills { get; private set; }
        public static int KillsNormalOnly { get; private set; }
        public static int KillsBonusOnly { get; private set; }
        public static int KillsBossOnly { get; private set; }

        public static void ResetRunScores()
        {
            IsFrozen = false;
            ResetTime = Time.time;
            CurrentScore = 0f;

            Kills = 0;
            KillsNormalOnly = 0;
            KillsBonusOnly = 0;
            KillsBossOnly = 0;
        }

        public static void FreezeRunScores() => IsFrozen = true;

        public static void AddKill(EnemyFormationWaveType type)
        {
            if (IsFrozen) return;
            Kills++;
            switch (type)
            {
                case EnemyFormationWaveType.Normal:
                    KillsNormalOnly++;
                    break;
                case EnemyFormationWaveType.Bonus:
                    KillsBonusOnly++;
                    break;
                case EnemyFormationWaveType.Boss:
                    KillsBossOnly++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static void AddScore(float score)
        {
            if (IsFrozen) return;
            CurrentScore += score;
        }
    }
}
