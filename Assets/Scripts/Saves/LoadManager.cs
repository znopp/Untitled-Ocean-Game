using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Highscore;
using Item_Scripts;
using Player;
using UI_Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Weapon_Scripts;

namespace Saves
{
    public class LoadManager : MonoBehaviour
    {

        private Shop.Shop _shop;
        private Controls _controls;
        private AoeAttack _aoeAttack;
        private Harpoon _harpoon;
        private BulletTime _bulletTime;
        
        private static LoadManager Instance { get; set; }
        
        [Header("Prefabs")]
        public GameObject sharkPrefab;
        public GameObject bigSharkPrefab;
        public GameObject heartPrefab;
        public GameObject coinPrefab;

        private readonly Dictionary<string, GameObject> _enemyPrefabs = new Dictionary<string, GameObject>();
        private string _savePath;
        private SaveData _cachedSaveData;
        private bool _shouldLoadSaveData;

        // Parent transform references
        private Transform _enemiesParent;
        private Transform _coinsParent;
        private Transform _healthParent;

        private void Awake()
        {
            // Singleton pattern to persist between scenes
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeManager();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeManager()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "gamesave.dat");
            
            // Initialize dictionary with inspector-assigned prefabs
            _enemyPrefabs["Shark"] = sharkPrefab;
            _enemyPrefabs["Big Shark"] = bigSharkPrefab;

            // Register for scene load events
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void FindParentObjects()
        {
            var enemiesObject = GameObject.Find("Enemies");
            var coinsObject = GameObject.Find("Coins");
            var healthObject = GameObject.Find("Health");

            _enemiesParent = enemiesObject ? enemiesObject.transform : null;
            _coinsParent = coinsObject ? coinsObject.transform : null;
            _healthParent = healthObject ? healthObject.transform : null;

            if (!_enemiesParent) Debug.LogError("Enemies GameObject not found in the scene.");
            if (!_coinsParent) Debug.LogError("Coins GameObject not found in the scene.");
            if (!_healthParent) Debug.LogError("Health GameObject not found in the scene.");
        }
        
        public void LoadGame()
        {
            if (!File.Exists(_savePath))
            {
                Debug.LogWarning("No save file found!"); // File not found error
                return;
            }

            // Log the save path to ensure consistency
            Debug.Log($"Loading save file from: {_savePath}");

            // Load and cache the save data

            var formatter = new BinaryFormatter();
            using (var stream = new FileStream(_savePath, FileMode.Open))
            {
                _cachedSaveData = formatter.Deserialize(stream) as SaveData;
                HighscoreManager.Instance.SetDifficulty(_cachedSaveData.difficulty);
            }
            
            if (_cachedSaveData == null)
            {
                Debug.LogError("Failed to load save data. Deserialized data is null.");
                return;
            }

            Debug.Log("Save data loaded successfully.");

            _shouldLoadSaveData = true;
    
            // Load the game scene
            SceneManager.LoadScene("Game");
        }


        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Game" && _shouldLoadSaveData && _cachedSaveData != null)
            {
                StartCoroutine(SetupLoadedGame(_cachedSaveData));
                _shouldLoadSaveData = false;
            }
        }

        private IEnumerator SetupLoadedGame(SaveData saveData)
        {
            // Wait for next frame to ensure all scene objects are initialized
            yield return new WaitForEndOfFrame();
    
            // Wait one more frame to ensure all Start() methods have run
            yield return new WaitForEndOfFrame();

            // Find parent objects in the scene
            FindParentObjects();

            // Clear existing objects
            ClearExistingObjects("enemy");
            ClearExistingObjects("bigEnemy");
            ClearExistingObjects("health");
            ClearExistingObjects("coin");
            
            // Load player data
            var player = GameObject.FindGameObjectWithTag("player");
            if (player)
            {
                player.transform.position = new Vector3(saveData.playerX, saveData.playerY, saveData.playerZ);

                var healthScript = player.GetComponent<PlayerHealth>();
                var coinScript = player.GetComponent<InteractCoin>();
                var timeScript = player.GetComponent<TimePlayedCounter>();

                if (healthScript) 
                {
                    StartCoroutine(SetPlayerHealthDelayed(healthScript, saveData.hp));
                }
                if (coinScript) coinScript.coins = saveData.playerCoins;
                if (timeScript) timeScript.timer = saveData.timePlayed;
            }
            
            // Spawn saved objects
            foreach (var enemyData in saveData.enemies)
            {
                if (_enemyPrefabs.TryGetValue(enemyData.enemyType, out var prefab))
                {
                    SpawnObject(prefab, enemyData, _enemiesParent, enemyData.enemyType);
                }
                else
                {
                    Debug.LogWarning($"Enemy prefab not found for type: {enemyData.enemyType}");
                }
            }

            foreach (var heartData in saveData.hearts)
                SpawnObject(heartPrefab, heartData, _healthParent, "Health");

            foreach (var coinData in saveData.coins)
                SpawnObject(coinPrefab, coinData, _coinsParent, "Coin");

            yield return new WaitForEndOfFrame();

            _bulletTime = GameObject.FindGameObjectWithTag("player").GetComponent<BulletTime>();
            _aoeAttack = GameObject.FindGameObjectWithTag("player").GetComponent<AoeAttack>();
            _controls = GameObject.FindGameObjectWithTag("player").GetComponent<Controls>();
            _harpoon = GameObject.FindGameObjectWithTag("harpoon").GetComponent<Harpoon>();
            _shop = GameObject.FindGameObjectWithTag("shop").GetComponent<Shop.Shop>();


            if (_aoeAttack)
            {
                _aoeAttack.radius = saveData.aoeRadius;
                _aoeAttack.aoeCooldown = saveData.aoeCooldown;
                _aoeAttack.indicatorTimer = saveData.aoeIndicatorTimer;
                _aoeAttack.canUse = saveData.aoeCanUse;
            }
            
            if (_harpoon)
            {
                _harpoon.spearheadVelocity = saveData.spearheadVelocity;
            }

            if (_controls)
            {
                _controls.speed = saveData.swimSpeed;
            }
            
            if (_shop)
            {
                _shop.harpoon.currentHarpoon = saveData.harpoonLevel;
                _shop.suitHealthPurchased = saveData.suitHealthLevel;
                _shop.swimSpeedPurchased = saveData.swimSpeedLevel;
                _shop.aoePurchased = saveData.aoeLevel;
                
                _shop.bulletTimePurchased = saveData.btLevel;

                if (_bulletTime)
                {
                    _shop.bulletTimePurchased = saveData.btLevel;
                    _bulletTime.maxDuration = saveData.btMaxDuration;
                    _bulletTime.regenerationFactor = saveData.btRegenFactor;
                    _bulletTime.duration = saveData.btDuration;
                }

                // Update the shop UI to reflect loaded upgrade levels
                _shop.UpdateShopUI();
            }
            
            Debug.Log("Game loaded successfully!");
        }
        
        private IEnumerator SetPlayerHealthDelayed(PlayerHealth healthScript, float savedHealth)
        {
            // Wait for next frame to ensure Start() has completed
            yield return null;
            healthScript.hp = savedHealth;
        }

        private void ClearExistingObjects(string notSystemTag)
        {
            var objects = GameObject.FindGameObjectsWithTag(notSystemTag);
            foreach (var obj in objects)
                Destroy(obj);
        }

        private void SpawnObject(GameObject prefab, Vector3Data data, Transform parent, string objectName)
        {
            if (prefab)
            {
                var obj = Instantiate(prefab, new Vector3(data.x, data.y, data.z), 
                    Quaternion.Euler(0, 0, data.rotation), parent);
                obj.name = objectName; // Set the name without "(Clone)"
            }
        }

        public bool SaveFileExists()
        {
            return File.Exists(_savePath);
        }
    }
}