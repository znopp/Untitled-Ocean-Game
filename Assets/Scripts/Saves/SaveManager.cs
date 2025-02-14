using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    [System.Serializable]
    public class SaveData
    {
        // Player Transform data
        public float playerX;
        public float playerY;
        public float playerZ;
        
        // Player stats
        public float hp;
        public int playerCoins;
        public float timePlayed;

        public string difficulty;

        // Collectibles and enemies positions
        public List<Vector3Data> hearts = new List<Vector3Data>();
        public List<Vector3Data> coins = new List<Vector3Data>();
        public List<EnemyData> enemies = new List<EnemyData>();
        
        // Shop upgrades
        public int harpoonLevel;
        public int suitHealthLevel;
        public int swimSpeedLevel;
        public int aoeLevel;
        public int btLevel;
        
        // Actual player stats
        public float swimSpeed;
        public float aoeRadius;
        public float aoeCooldown;
        public float aoeIndicatorTimer;
        public bool aoeCanUse;
        
        public float spearheadVelocity;
        public float btRegenFactor;
        public float btMaxDuration;
        public float btDuration;
    }

    [System.Serializable]
    public class Vector3Data
    {
        public float x;
        public float y;
        public float z;
        public float rotation;

        public Vector3Data(Transform transform)
        {
            x = transform.position.x;
            y = transform.position.y;
            z = transform.position.z;
            rotation = transform.rotation.eulerAngles.z;
        }
    }

    [System.Serializable]
    public class EnemyData : Vector3Data
    {
        public string enemyType;  // Store the name of the prefab

        public EnemyData(Transform transform, string type) : base(transform)
        {
            enemyType = type;
        }
    }

    public class SaveManager : MonoBehaviour
    {
        public Shop.Shop shop;
        public Controls controls;
        public AoeAttack aoeAttack;
        public Harpoon harpoon;
        public BulletTime bulletTime;
        
        [Header("Enemy Prefabs")]
        public GameObject sharkPrefab;
        public GameObject bigSharkPrefab;
        private string _savePath;

        private Dictionary<string, GameObject> _enemyPrefabs = new Dictionary<string, GameObject>();

        private void Awake()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "gamesave.dat");
        
            // Initialize dictionary with inspector-assigned prefabs
            _enemyPrefabs["Shark"] = sharkPrefab;
            _enemyPrefabs["Big Shark"] = bigSharkPrefab;
        }

        public void DeleteSave()
        {
            File.Delete(_savePath);
        }
        
        public void SaveGameThenTransition()
        {
            SaveGame();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        
        private GameObject _heartPrefab;
        private GameObject _coinPrefab;

        public void SaveTime()
        {
            if (!HighscoreManager.Instance) return;
            
            var player = GameObject.FindGameObjectWithTag("player");
            if (player)
            {
                var timeScript = player.GetComponent<TimePlayedCounter>();
                HighscoreManager.Instance.SaveBestTime(timeScript.timer, HighscoreManager.Instance.GetCurrentDifficulty());    
            }
        }

        private void SaveGame()
        {
            var saveData = new SaveData();
            
            // Get the player GameObject
            var player = GameObject.FindGameObjectWithTag("player");
            if (player)
            {
                // Save transform data
                saveData.playerX = player.transform.position.x;
                saveData.playerY = player.transform.position.y;
                saveData.playerZ = player.transform.position.z;

                // Save player stats
                var healthScript = player.GetComponent<PlayerHealth>();
                var coinScript = player.GetComponent<InteractCoin>();
                var timeScript = player.GetComponent<TimePlayedCounter>();

                if (healthScript) saveData.hp = healthScript.hp;
                if (coinScript) saveData.playerCoins = coinScript.coins;
                if (timeScript)
                {
                    saveData.timePlayed = timeScript.timer;
                    
                    if (HighscoreManager.Instance)
                    {
                        saveData.difficulty = HighscoreManager.Instance.GetCurrentDifficulty();
                        HighscoreManager.Instance.SaveBestTime(timeScript.timer, saveData.difficulty);
                    }
                }
                
            }

            // Save all enemies
            var enemies = GameObject.FindGameObjectsWithTag("enemy");
            var bigEnemies = GameObject.FindGameObjectsWithTag("bigEnemy");
    
            foreach (var enemy in enemies.Concat(bigEnemies))
            {
                var enemyType = GetEnemyType(enemy);
                saveData.enemies.Add(new EnemyData(enemy.transform, enemyType));
            }

            // Save all collectibles
            var hearts = GameObject.FindGameObjectsWithTag("health");
            foreach (var heart in hearts)
            {
                saveData.hearts.Add(new Vector3Data(heart.transform));
            }

            var coins = GameObject.FindGameObjectsWithTag("coin");
            foreach (var coin in coins)
            {
                saveData.coins.Add(new Vector3Data(coin.transform));
            }

            if (aoeAttack)
            {
                saveData.aoeRadius = aoeAttack.radius;
                saveData.aoeCooldown = aoeAttack.aoeCooldown;
                saveData.aoeIndicatorTimer = aoeAttack.indicatorTimer;
                saveData.aoeCanUse = aoeAttack.canUse;
            }

            if (controls)
            {
                saveData.swimSpeed = controls.speed;
            }

            if (harpoon)
            {
                saveData.spearheadVelocity = harpoon.spearheadVelocity;
            }
            
            if (shop)
            {
                saveData.harpoonLevel = shop.harpoon.currentHarpoon;
                saveData.suitHealthLevel = shop.suitHealthPurchased;
                saveData.swimSpeedLevel = shop.swimSpeedPurchased;
                saveData.aoeLevel = shop.aoePurchased;

                if (bulletTime)
                {
                    saveData.btLevel = shop.bulletTimePurchased;
                    saveData.btMaxDuration = bulletTime.maxDuration;
                    saveData.btRegenFactor = bulletTime.regenerationFactor;
                    saveData.btDuration = bulletTime.duration;
                }
                
            }

            // Write to file
            var formatter = new BinaryFormatter();
            using var stream = new FileStream(_savePath, FileMode.Create);
            formatter.Serialize(stream, saveData);
        }

        private string GetEnemyType(GameObject enemy)
        {
            // You'll need to implement this based on how you distinguish between enemy types
            // Could be based on the GameObject's name, a component, or a custom property
            if (enemy.name.Equals("Shark")) return "Shark";
            if (enemy.name.Equals("Big Shark")) return "Big Shark";
            // Add more enemy type checks as needed
            
            return enemy.name;
        }
        
        public string LoadDifficulty()
        {
            if (File.Exists(_savePath))
            {
                var formatter = new BinaryFormatter();
                using var stream = new FileStream(_savePath, FileMode.Open);
                var saveData = formatter.Deserialize(stream) as SaveData;
                return saveData?.difficulty ?? "easy"; // Default to "easy" if no difficulty is saved
            }
            return "easy";  // Default to easy if no save file
        }
    }
}