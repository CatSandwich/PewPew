using System;
using System.Collections.Generic;
using Enemy.Formations;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class WaveController : MonoBehaviour
    {
        public static WaveController Instance => _instance ??= FindObjectOfType<WaveController>();
        private static WaveController _instance;

        public static float LeftBounds => Instance._leftBounds;
        public static float RightBounds => Instance._rightBounds;
        public static float TopBounds => Instance._topBounds;
        public static bool RunIsAlive => Instance._runIsAlive;

        private bool _runIsAlive;

        public Text DistanceDisplay;
        public GenericFormation[] WaveList;

        private GenericFormation _currentFormation;
        private float _nextSpawn;

        private readonly System.Random Random = new System.Random();

        private float _leftBounds;
        private float _rightBounds;
        private float _topBounds;
        private Vector3 _bottomLeftBounds;
        private Vector3 _topRightBounds;

        private IEnumerator<EnemyFormationPlacement[]> _enemies;
        private int _wave;
        private int _lastBossWave;
        private bool _isBossWaveActive;
        private List<EnemyScript> _currentBosses = new List<EnemyScript>();

        private float _distanceScore;

        public void OnEnemyHitsEndZone(EnemyScript enemy)
        {
            _runIsAlive = false;
        }

        public void OnNormalEnemyDestroyed(EnemyScript enemy)
        {
            // A normal enemy has been defeated
        }
        public void OnBonusEnemyDestroyed(EnemyScript enemy)
        {
            // A bonus enemy has been defeated
        }
        public void OnBossEnemyDestroyed(EnemyScript enemy)
        {
            if (!_currentBosses.Contains(enemy)) return;
            _currentBosses.Remove(enemy);

            // A boss has been defeated, add points

            if (!_isBossWaveActive) return;
            if (_currentBosses.Count > 0) return;
            _isBossWaveActive = false;
            _nextSpawn = Time.time + 5f;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            _runIsAlive = true;
            _nextSpawn = Time.time + 5f;

            _bottomLeftBounds = Camera.main.ScreenToWorldPoint(Vector3.zero);
            _topRightBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

            _leftBounds = _bottomLeftBounds.x + 1f;
            _rightBounds = _topRightBounds.x + -1f;
            _topBounds = _topRightBounds.y - 1f;

            if (WaveList.Length < 1)
                Debug.LogError("WaveList not found! Was the WaveController deleted from the scene?");
        }

        private void Update()
        {
            if (!_runIsAlive)
            {
                DistanceDisplay.text = $"Distance: {_distanceScore:N1}km\nWave: {_wave}\nBoss Wave: {_isBossWaveActive}\n" + (_isBossWaveActive ? $"Bosses Remaining: {_currentBosses.Count}\n" : "");
                return;
            }

            _distanceScore = Time.time;

            DistanceDisplay.text = $"Distance: {_distanceScore:N1}km\nWave: {_wave}\nBoss Wave: {_isBossWaveActive}\n" + (_isBossWaveActive ? $"Bosses Remaining: {_currentBosses.Count}\n" : "");

            if (Time.time > _nextSpawn)
                SpawnEnemy();
        }

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
                    var spawn = new Vector3(GetSpawnXClamped(placement), _topRightBounds.y + 1f + placement.Offset.y, 0f);

                    var go = Instantiate(placement.Enemy.GetPrefab());
                    go.transform.position = spawn;

                    var enemy = go.GetComponent<EnemyScript>();
                    enemy.SpawnPoint = spawn;
                    enemy.Speed = _currentFormation.GetSpeed();
                    enemy.Movement = _currentFormation.Movement;
                    enemy.Wave = _wave;
                    enemy.WaveID = waveID;

                    switch (_currentFormation.GetWaveType())
                    {
                        case EnemyFormationWaveType.Undefined:
                        case EnemyFormationWaveType.Normal:
                            enemy.WaveType = EnemyScript.EnemyWaveType.Normal;
                            break;
                        case EnemyFormationWaveType.Bonus:
                            enemy.WaveType = EnemyScript.EnemyWaveType.Bonus;
                            break;
                        case EnemyFormationWaveType.Boss:
                            _currentBosses.Add(enemy);
                            enemy.WaveType = EnemyScript.EnemyWaveType.Boss;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    waveID++;
                }
            }

            if (_currentFormation.GetWaveType() == EnemyFormationWaveType.Boss)
            {
                _isBossWaveActive = true;
            }

            // If this wave is over, cool off
            if (!_enemies.MoveNext())
            {
                _currentFormation = null;
                _nextSpawn = Time.time + 5f;
                return;
            }

            _nextSpawn = Time.time + _currentFormation.GetSpacing();
        }
        
        private void GetNextFormation()
        {
            var loopLimit = 0;
            while (_currentFormation == null)
            {
                loopLimit++;
                if (loopLimit > 500)
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
    }
}
