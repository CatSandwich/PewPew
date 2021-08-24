using System;
using System.Collections.Generic;
using Enemy.Formations;
using UnityEngine;
using UnityEngine.UI;
using EnemyFormationPlacement = Enemy.Data.EnemyFormationPlacement;
using EnemyFormationWaveType = Enemy.Data.EnemyFormationWaveType;

namespace Enemy
{
    public class WaveController : MonoBehaviour
    {
        public static WaveController Instance => _instance ??= FindObjectOfType<WaveController>();
        private static WaveController _instance;

        /// <summary> Returns the left most X coordinate on screen allowed for unit placement. </summary>
        public static float LeftBounds { get; private set; }
        /// <summary> Returns the right most X coordinate on screen allowed for unit placement. </summary>
        public static float RightBounds { get; private set; }
        /// <summary> Returns the top most Y coordinate on screen allowed for unit pathing. </summary>
        public static float TopBounds { get; private set; }
        /// <summary> Returns true if the current run is still ongoing. </summary>
        public static bool RunIsAlive { get; private set; }

        #region Public Fields
        public Text DistanceDisplay;
        /// <summary> Contains a list of all Formations which can be selected. </summary>
        public GenericFormation[] WaveList;
        /// <summary> Returns an IEnumerator of the current enemies when called. </summary>
        public IEnumerable<EnemyScript> GetCurrentEnemies() => _currentEnemies;
        #endregion

        #region Private Fields
        /// <summary> The current Formation being used to spawn units. </summary>
        private GenericFormation _currentFormation;
        /// <summary> The earliest time at which the next unit may spawn, in Game Time. </summary>
        private float _nextSpawn;
        /// <summary> A Vector3 which describes the position of the bottom left most pixel in world coordinates. </summary>
        private Vector3 _bottomLeftBounds;
        /// <summary> A Vector3 which describes the position of the top right most pixel in world coordinates. </summary>
        private Vector3 _topRightBounds;

        /// <summary> An IEnumrator which contains the current list of units being spawned. </summary>
        private IEnumerator<EnemyFormationPlacement[]> _enemies;
        /// <summary> The current wave number. </summary>
        private int _wave;
        /// <summary> The last wave a Boss was spawned on. </summary>
        private int _lastBossWave;
        /// <summary> True if the current wave is a Boss wave. </summary>
        private bool _isBossWaveActive;

        /// <summary> A list of all the currently alive Enemies. </summary>
        private readonly List<EnemyScript> _currentEnemies = new List<EnemyScript>();
        /// <summary> A List of all the currently alive Bosses. </summary>
        private readonly List<EnemyScript> _currentBosses = new List<EnemyScript>();

        private readonly System.Random Random = new System.Random();
        #endregion

        #region Public Methods
        /// <summary> Called when an Enemy Collides with the EndZone. </summary>
        public void OnEnemyHitsEndZone(EnemyScript enemy)
        {
            RunIsAlive = false;
            ScoreKeeper.FreezeRunScores();
        }
        /// <summary> Called when a Normal type Enemy is destroyed. </summary>
        public void OnNormalEnemyDestroyed(EnemyScript enemy, bool wasKilled)
        {
            _currentEnemies.Remove(enemy);
            if (!wasKilled) return;
            ScoreKeeper.AddScore(10f);
            ScoreKeeper.AddKill(enemy.WaveType);
        }
        /// <summary> Called when a Bonus type Enemy is destroyed. </summary>
        public void OnBonusEnemyDestroyed(EnemyScript enemy, bool wasKilled)
        {
            _currentEnemies.Remove(enemy);
            if (!wasKilled) return;
            ScoreKeeper.AddScore(10f);
            ScoreKeeper.AddKill(enemy.WaveType);
        }
        /// <summary> Called when a Boss type Enemy is destroyed. </summary>
        public void OnBossEnemyDestroyed(EnemyScript enemy, bool wasKilled)
        {
            _currentEnemies.Remove(enemy);
            _currentBosses.Remove(enemy);
            if (wasKilled)
            {
                ScoreKeeper.AddScore(100f);
                ScoreKeeper.AddKill(enemy.WaveType);
            }

            // If this was the last boss for the round, continue on
            if (!_isBossWaveActive) return;
            if (_currentBosses.Count > 0) return;
            _isBossWaveActive = false;
            _nextSpawn = Time.time + 5f;
        }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
        }
        private void Start()
        {
            ScoreKeeper.ResetRunScores();
            RunIsAlive = true;
            _nextSpawn = Time.time + 5f;

            _bottomLeftBounds = Camera.main.ScreenToWorldPoint(Vector3.zero);
            _topRightBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

            LeftBounds = _bottomLeftBounds.x + 1f;
            RightBounds = _topRightBounds.x + -1f;
            TopBounds = _topRightBounds.y - 1f;

            if (WaveList.Length < 1)
                Debug.LogError("WaveList not found! Was the WaveController deleted from the scene?");
        }
        private void Update()
        {
            DistanceDisplay.text = $"Distance: {ScoreKeeper.CurrentDistance:N1}km\nWave: {_wave}\nBoss Wave: {_isBossWaveActive}\n" + (_isBossWaveActive ? $"Bosses Remaining: {_currentBosses.Count}\n" : "" + $"\nScore: {ScoreKeeper.TotalScore}");

            if (!RunIsAlive) return;

            if (Time.time > _nextSpawn)
                SpawnEnemy();
        }
        #endregion

        #region Private Methods
        /// <summary> Selects the next Formation which will be sent. </summary>
        private void GetNextFormation()
        {
            var loopLimit = 0;
            while (_currentFormation == null)
            {
                loopLimit++;
                if (loopLimit > 100)
                {
                    Debug.LogError("Unable to find a suitable wave!!");
                    _nextSpawn = Time.time + 10f;
                    break;
                }

                // Select a random wave
                var index = Random.Next(WaveList.Length);
                // If this wave is too difficult, skip it
                if (WaveList[index].GetDifficultyMin() > Time.time || WaveList[index].GetDifficultyMax() < Time.time) continue;
                if (WaveList[index].GetWaveType() == EnemyFormationWaveType.Boss && (_wave < 10 || _wave - _lastBossWave < 10)) continue;
                _enemies?.Dispose();
                // Grab the next formation and prepare it to spawn units
                _currentFormation = WaveList[index];
                _currentFormation.Initialize();
                _enemies = _currentFormation.GetNextEnemies().GetEnumerator();
                // Prime the Enumerator for reading the first element
                _enemies.MoveNext();
                _wave++;

                if (_currentFormation.GetWaveType() == EnemyFormationWaveType.Boss)
                {
                    _lastBossWave = _wave;
                }
            }
        }

        /// <summary>
        /// Clamps the X Placement range of the given <see cref="EnemyFormationPlacement"/> to on screen coordinates.
        /// Has a special fallback for when the range of the placement cannot be forced entirely on screen.
        /// </summary>
        private float GetSpawnXClamped(EnemyFormationPlacement placement)
        {
            // Check for out of bounds placement
            // This can happen when the movement range is too wide and it's forced to go off screen
            if (_currentFormation.Movement.GetLeftBounds() > 0) return 0;
            if (_currentFormation.Movement.GetRightBounds() < 0) return 0;

            // Fall back to clamping the movement range to be on screen
            return Mathf.Clamp( placement.Offset.x,
                                _currentFormation.Movement.GetLeftBounds(),
                                _currentFormation.Movement.GetRightBounds());
        }

        /// <summary> Spawns the next row of enemies. </summary>
        private void SpawnEnemy()
        {
            // If this is a boss wave, wait until the boss is dead
            if (_isBossWaveActive)
            {
                _nextSpawn = Time.time + 5f;
                return;
            }

            if (_currentFormation == null)
                GetNextFormation();

            // If there are enemies to spawn, spawn them
            if (_enemies.Current != null)
            {
                var waveID = 0;
                // Spawn all the enemies in this row
                foreach (var placement in _enemies.Current)
                {
                    // Pick a spawn point
                    var spawn = new Vector3(GetSpawnXClamped(placement), _topRightBounds.y + 1f + placement.Offset.y, 0f);
                    // Create a game object from a Prefab
                    var go = Instantiate(placement.Enemy.GetPrefab());
                    go.transform.position = spawn;
                    // Attach the Enemy script and prepare it
                    var enemy = go.GetComponent<EnemyScript>();
                    enemy.SpawnPoint = spawn;
                    enemy.Speed = _currentFormation.GetSpeed();
                    enemy.Movement = _currentFormation.Movement;
                    enemy.Wave = _wave;
                    enemy.WaveID = waveID;
                    enemy.Health = enemy.MaxHealth = placement.Enemy.MaxHealth;
                    enemy.WaveType = _currentFormation.GetWaveType();

                    _currentEnemies.Add(enemy);

                    waveID++;
                }
            }

            // If this is a boss wave, set the mode so no new mobs spawn during this wave
            if (_currentFormation.GetWaveType() == EnemyFormationWaveType.Boss)
                _isBossWaveActive = true;

            // If this wave is over, cool off
            if (!_enemies.MoveNext())
            {
                _currentFormation = null;
                _nextSpawn = Time.time + 5f;
                return;
            }

            _nextSpawn = Time.time + _currentFormation.GetSpacing();
        }
        #endregion
    }
}
