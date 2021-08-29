using System.Collections.Generic;
using System.Linq;
using Enemy;
using Enemy.Data;
using Enemy.Formations;
using Pooling;
using TMPro;
using UI.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Singletons
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
        public AbstractFormation[] WaveList;
        /// <summary> Returns an IEnumerator of the current enemies when called. </summary>
        public IEnumerable<AbstractEnemyScript> GetCurrentEnemies() => _currentEnemies;
        #endregion

        #region Private Fields
        /// <summary> The current Formation being used to spawn units. </summary>
        private AbstractFormation _currentFormation;
        /// <summary> The earliest time at which the next unit may spawn, in Game Time. </summary>
        private float _nextSpawn;
        /// <summary> A Vector3 which describes the position of the bottom left most pixel in world coordinates. </summary>
        private Vector3 _bottomLeftBounds;
        /// <summary> A Vector3 which describes the position of the top right most pixel in world coordinates. </summary>
        private Vector3 _topRightBounds;

        /// <summary> An IEnumerator which contains the current list of units being spawned. </summary>
        private IEnumerator<EnemyFormationPlacement[]> _enemies;
        /// <summary> The current wave number. </summary>
        private int _wave;
        /// <summary> The last wave a Boss was spawned on. </summary>
        private int _lastBossWave;
        /// <summary> True if the current wave is a Boss wave. </summary>
        private bool _isBossWaveActive;

        /// <summary> A list of all the currently alive Enemies. </summary>
        private readonly List<AbstractEnemyScript> _currentEnemies = new List<AbstractEnemyScript>();
        /// <summary> A List of all the currently alive Bosses. </summary>
        private readonly List<AbstractEnemyScript> _currentBosses = new List<AbstractEnemyScript>();

        private readonly System.Random Random = new System.Random();

        private Pool<WaveEnemyScript> WaveEnemyPool;
        private Pool<SubordinateEnemyScript> SubordinateEnemyPool;
        private readonly Dictionary<int, List<GameObject>> ModelPools = new Dictionary<int, List<GameObject>>();
        private Pool<ScoreDrop> ScoreDropPool;
        private Pool<CoinDrop> CoinDropPool;

        #endregion

        #region Public Methods
        /// <summary> Called when an Enemy Collides with the EndZone. </summary>
        public void OnEnemyHitsEndZone(AbstractEnemyScript enemy)
        {
            if (!_currentEnemies.Contains(enemy)) return;
            RunIsAlive = false;
            ScoreKeeper.FreezeRunScores();
        }
        /// <summary> Called when a Normal type Enemy is destroyed. </summary>
        public void OnNormalEnemyDestroyed(WaveEnemyScript enemy, bool wasKilled)
        {
            var waveType = enemy.WaveType;
            if (!RemoveEnemy(enemy, false)) return;
            if (wasKilled) HandleEnemyKilled(waveType, enemy);
        }
        /// <summary> Called when a Bonus type Enemy is destroyed. </summary>
        public void OnBonusEnemyDestroyed(WaveEnemyScript enemy, bool wasKilled)
        {
            var waveType = enemy.WaveType;
            if (!RemoveEnemy(enemy, false)) return;
            if (wasKilled) HandleEnemyKilled(waveType, enemy);
        }
        /// <summary> Called when a Boss type Enemy is destroyed. </summary>
        public void OnBossEnemyDestroyed(WaveEnemyScript enemy, bool wasKilled)
        {
            var subordinates = FindObjectsOfType<SubordinateEnemyScript>().Where(s => s.Leader == enemy && s.Data.DiesWithBoss);
            foreach (var subordinate in subordinates) subordinate.TakeDamage(float.MaxValue, false);

            var waveType = enemy.WaveType;
            if (!RemoveEnemy(enemy, true)) return;
            if (wasKilled) HandleEnemyKilled(waveType, enemy);

            // If this was the last boss for the round, continue on
            if (!_isBossWaveActive) return;
            if (_currentBosses.Count > 0) return;
            _isBossWaveActive = false;
            _nextSpawn = Time.time + 5f;
        }

        public void OnSubordinateEnemyDestroyed(SubordinateEnemyScript enemy, bool wasKilled)
        {
            if (!RemoveEnemy(enemy)) return;
            if (wasKilled) HandleEnemyKilled(enemy);
        }

        public void Release(ScoreDrop score) => ScoreDropPool.Release(score);
        public void Release(CoinDrop coin) => CoinDropPool.Release(coin);
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

            WaveEnemyPool = PoolManager.CreatePool(CreateWaveEnemy, ActivateWaveEnemy, DeactivateWaveEnemy);
            SubordinateEnemyPool = PoolManager.CreatePool(CreateSubordinateEnemy, ActivateSubordinateEnemy, DeactivateSubordinateEnemy);
            ScoreDropPool = PoolManager.CreatePool(CreateScoreDrop, ActivateScoreDrop, DeactivateScoreDrop);
            CoinDropPool = PoolManager.CreatePool(CreateCoinDrop, ActivateCoinDrop, DeactivateCoinDrop);
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
            var forcingIllegalWave = false;
            while (_currentFormation == null)
            {
                loopLimit++;
                if (loopLimit > 100)
                {
                    Debug.LogError("Unable to find a suitable wave!!");
                    forcingIllegalWave = true;
                }

                // Select a random wave
                var index = Random.Next(WaveList.Length);
                // If this wave is too difficult, skip it
                if (!forcingIllegalWave && WaveList[index].GetDifficultyMin() > Time.time || WaveList[index].GetDifficultyMax() < Time.time) continue;
                if (!forcingIllegalWave && WaveList[index].GetWaveType() == EnemyFormationWaveType.Boss && (_wave < 10 || _wave - _lastBossWave < 10)) continue;
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
            if (_currentFormation.Behaviour.GetLeftBounds() > 0) return 0;
            if (_currentFormation.Behaviour.GetRightBounds() < 0) return 0;

            // Fall back to clamping the movement range to be on screen
            return Mathf.Clamp( placement.Offset.x,
                                _currentFormation.Behaviour.GetLeftBounds(),
                                _currentFormation.Behaviour.GetRightBounds());
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
                    var spawn = new Vector3(GetSpawnXClamped(placement), _topRightBounds.y + 1f + placement.Offset.y, 0f);
                    var enemy = WaveEnemyPool.Get();
                    var model = GetModel(placement.Enemy.Prefab);
                    enemy.gameObject.transform.position = spawn;
                    model.transform.parent = enemy.gameObject.transform;
                    model.transform.localPosition = Vector3.zero;

                    model.GetComponent<ChildCollisionHandler>().Parent = enemy;
                    
                    enemy.Model = model;
                    enemy.ModelId = placement.Enemy.Prefab.GetInstanceID();

                    enemy.Data = placement.Enemy;

                    enemy.SpawnTime = Time.time;
                    enemy.SpawnPoint = spawn;
                    enemy.Speed = _currentFormation.GetSpeed();
                    enemy.Behaviour = _currentFormation.Behaviour;
                    enemy.Wave = _wave;
                    enemy.WaveId = waveID;
                    enemy.Health = enemy.MaxHealth = placement.Enemy.MaxHealth;
                    enemy.WaveType = _currentFormation.GetWaveType();

                    if (_currentFormation.GetWaveType() == EnemyFormationWaveType.Boss) _currentBosses.Add(enemy);
                    _currentEnemies.Add(enemy);

                    enemy.Behaviour.PrepareBehaviour(enemy);
                    
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

        public void SpawnSubordinate(AbstractEnemyScript leader, SubordinateData target, Vector3 position)
        {
            var enemy = SubordinateEnemyPool.Get();
            var model = GetModel(target.Prefab);
            enemy.gameObject.transform.position = position;
            model.transform.parent = enemy.gameObject.transform;
            model.transform.localPosition = Vector3.zero;

            model.GetComponent<ChildCollisionHandler>().Parent = enemy;

            enemy.Model = model;
            enemy.ModelId = target.Prefab.GetInstanceID();

            enemy.Data = target;

            enemy.SpawnTime = Time.time;
            enemy.SpawnPoint = position;
            enemy.Speed = target.Speed;
            enemy.Behaviour = target.Behaviour;

            enemy.Health = enemy.MaxHealth = target.MaxHealth;

            _currentEnemies.Add(enemy);

            enemy.Behaviour.PrepareBehaviour(enemy);

            enemy.Leader = leader;
        }

        private WaveEnemyScript CreateWaveEnemy() => CreateEnemy<WaveEnemyScript>("Enemy");
        private void ActivateWaveEnemy(WaveEnemyScript enemy) => ActivateEnemy(enemy);
        private void DeactivateWaveEnemy(WaveEnemyScript enemy) => DeactivateEnemy(enemy);

        private SubordinateEnemyScript CreateSubordinateEnemy() => CreateEnemy<SubordinateEnemyScript>("Subordinate");
        private void ActivateSubordinateEnemy(SubordinateEnemyScript enemy) => ActivateEnemy(enemy);
        private void DeactivateSubordinateEnemy(SubordinateEnemyScript enemy) => DeactivateEnemy(enemy);

        private T CreateEnemy<T>(string enemyName) where T : Component
        {
            var go = new GameObject(enemyName);
            go.SetActive(false);
            go.tag = "Enemy";
            go.hideFlags = HideFlags.HideInHierarchy;
            go.transform.parent = gameObject.transform;
            return go.AddComponent<T>();
        }

        private void ActivateEnemy(AbstractEnemyScript enemy)
        {
            enemy.gameObject.SetActive(true);
            enemy.gameObject.hideFlags = HideFlags.None;
        }
        private void DeactivateEnemy(AbstractEnemyScript enemy)
        {
            ReleaseModel(enemy.ModelId, enemy.Model);
            enemy.Model = null;
            enemy.ModelId = 0;
            enemy.gameObject.SetActive(false);
            enemy.gameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        private GameObject GetModel(GameObject prefab)
        {
            var key = prefab.GetInstanceID();
            if (!ModelPools.ContainsKey(key)) return Instantiate(prefab);
            if (ModelPools[key].Count <= 0) return Instantiate(prefab);

            var model = ModelPools[key][0];
            ModelPools[key].Remove(model);

            model.SetActive(true);
            model.hideFlags = HideFlags.None;
            return model;
        }

        private void ReleaseModel(int key, GameObject model)
        {
            model.SetActive(false);
            model.hideFlags = HideFlags.HideInHierarchy;
            model.transform.parent = gameObject.transform;
            if (!ModelPools.ContainsKey(key)) ModelPools.Add(key, new List<GameObject>());
            ModelPools[key].Add(model);
        }

        private ScoreDrop CreateScoreDrop()
        {
            var go = Instantiate(Assets.Instance.ScoreDrop);
            go.SetActive(false);
            go.hideFlags = HideFlags.HideInHierarchy;
            return go.GetComponent<ScoreDrop>();
        }

        private void ActivateScoreDrop(ScoreDrop item)
        {
            item.gameObject.SetActive(true);
            item.gameObject.hideFlags = HideFlags.None;
        }

        private void DeactivateScoreDrop(ScoreDrop item)
        {
            item.gameObject.SetActive(false);
            item.gameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        private CoinDrop CreateCoinDrop()
        {
            var go = Instantiate(Assets.Instance.CoinDrop);
            go.SetActive(false);
            go.hideFlags = HideFlags.HideInHierarchy;
            return go.GetComponent<CoinDrop>();
        }

        private void ActivateCoinDrop(CoinDrop item)
        {
            item.gameObject.SetActive(true);
            item.gameObject.hideFlags = HideFlags.None;
        }

        private void DeactivateCoinDrop(CoinDrop item)
        {
            item.gameObject.SetActive(false);
            item.gameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        private bool RemoveEnemy(WaveEnemyScript enemy, bool wasBoss)
        {
            if (!_currentEnemies.Contains(enemy)) return false;
            if (wasBoss) _currentBosses.Remove(enemy);
            _currentEnemies.Remove(enemy);
            WaveEnemyPool.Release(enemy);
            enemy.Behaviour.ClearData(enemy);
            return true;
        }
        private bool RemoveEnemy(SubordinateEnemyScript enemy)
        {
            SubordinateEnemyPool.Release(enemy);
            enemy.Behaviour.ClearData(enemy);
            return true;
        }

        private void HandleEnemyKilled(EnemyFormationWaveType type, WaveEnemyScript enemy)
        {
            ScoreKeeper.AddKill(type);
            ScoreKeeper.AddScore(enemy.Data.ScoreValue);
            DropScore(enemy.transform.position, enemy.Data.ScoreValue);
            DropCoins(enemy.transform.position, enemy.Data.CoinValue);
        }
        private void HandleEnemyKilled(SubordinateEnemyScript enemy)
        {
            ScoreKeeper.AddKill(EnemyFormationWaveType.Subordinate);
            ScoreKeeper.AddScore(enemy.Data.ScoreValue);
            DropScore(enemy.transform.position, enemy.Data.ScoreValue);
            DropCoins(enemy.transform.position, enemy.Data.CoinValue);
        }

        private void DropScore(Vector3 position, float score)
        {
            var scoreDrop = ScoreDropPool.Get();
            scoreDrop.gameObject.transform.position = position;
            scoreDrop.gameObject.GetComponent<TextMeshPro>().text = ((int)score).ToString();
        }

        private void DropCoins(Vector3 position, int count)
        {
            while (count > 0)
            {
                int coinType;
                if (count >= 10) coinType = Random.Next(3); // 0, 1, 2
                else if (count >= 5) coinType = Random.Next(2); // 0, 1
                else coinType = 0;
                count -= DropCoin(position, coinType);
            }
        }

        /// <summary>
        /// Drops a single coin at the given position.
        /// </summary>
        /// <returns>The value of the coin dropped.</returns>
        private int DropCoin(Vector3 position, int type)
        {
            CoinDrop coinDrop;
            switch (type)
            {
                case 0:
                {
                    coinDrop = CoinDropPool.Get();
                    coinDrop.gameObject.transform.position = position;
                    coinDrop.GetRenderer().sprite = Assets.Instance.Coin1;
                    coinDrop.Value = 1;
                    return 1;
                }
                case 1:
                {
                    coinDrop = CoinDropPool.Get();
                    coinDrop.gameObject.transform.position = position;
                    coinDrop.GetRenderer().sprite = Assets.Instance.Coin5;
                    coinDrop.Value = 5;
                    return 5;
                }
                case 2:
                {
                    coinDrop = CoinDropPool.Get();
                    coinDrop.gameObject.transform.position = position;
                    coinDrop.GetRenderer().sprite = Assets.Instance.Coin10;
                    coinDrop.Value = 10;
                    return 10;
                }
                default:
                {
                    Debug.LogError($"Couldn't handle coin type [{type}]");
                    return int.MaxValue;
                }
            }
        }
        #endregion
    }
}
