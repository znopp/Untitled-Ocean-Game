using System.Collections;
using Highscore;
using UI_Scripts;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemySpawning : MonoBehaviour
    {
        
        // Time between spawning
        public float spawningCooldown; 
        
        // Local difficulty variable for edgecases
        private string _difficulty;
        
        // Amount of enemies to spawn per attempt
        public float spawnAmount;
        
        // Spawnable areas
        public Transform[] spawningArea;
        
        // Transform of the Enemies object in scene
        public Transform enemiesParent;
        
        // Enemy GameObjects
        public GameObject shark;
        public GameObject bigShark;
        
        // Random position within spawnable area
        private int _randomSpawnpoint;
        
        // Boolean to determine if enemies can spawn yet
        private bool _spawnable;
        
        // Used to regulate difficulty (by changing spawnAmount and spawningCooldown)
        public TimePlayedCounter timePlayedCounter;
        


        private void Start()
        {
            _spawnable = true;
            spawningCooldown = 1;
            spawnAmount = 1;
            
            // Find and store the reference to the "Enemies" GameObject
            var enemiesGameObject = GameObject.Find("Enemies");
            
            if (enemiesGameObject)
            {
                enemiesParent = enemiesGameObject.transform;
            }
            else
            {
                Debug.LogError("Enemies GameObject not found in the scene.");
            }
            
        }

        private void Update()
        {
            if (_spawnable)
            {
                SpawnEnemy();
            }
        }

        private Vector2 GetRandomPositionInSpawnpoint()
        {
            _randomSpawnpoint = Random.Range(0, spawningArea.Length);

            Transform spawnpoint = spawningArea[_randomSpawnpoint];
            
            // Get the bounds of the spawnpoint
            Bounds spawnpointBounds = spawnpoint.GetComponent<SpriteRenderer>().bounds;

            // Generate a random position within the spawnpoints boundaries
            var position = spawnpoint.position;
            float randomX = Random.Range(position.x - spawnpointBounds.extents.x, position.x + spawnpointBounds.extents.x);
            float randomY = Random.Range(position.y - spawnpointBounds.extents.y, position.y + spawnpointBounds.extents.y);

            return new Vector2(randomX, randomY);
        }

        private IEnumerator Cooldown()
        {
            _difficulty = !HighscoreManager.Instance ? "normal" : HighscoreManager.Instance.GetCurrentDifficulty();

            // Base values
            var baseCooldown = 5.0f;
            var baseSpawnAmount = 1;

            // Difficulty adjustments
            var difficultyMultiplier = _difficulty switch
            {
                "easy" => 3.0f,
                "normal" => 1.5f,
                "hard" => 0.5f,
                _ => 1.5f
            };

            var spawnAmountFactor = _difficulty switch
            {
                "easy" => 0.01f,
                "normal" => 0.015f,
                "hard" => 0.02f,
                _ => 0.015f
            };
            

            // Exponential scaling based on time played with a minimum cap on cooldown
            var minCooldown = 1.0f; // Set a minimum cooldown to prevent approaching zero
            spawningCooldown = Mathf.Max(minCooldown, baseCooldown * Mathf.Exp(-0.005f * timePlayedCounter.timer) * difficultyMultiplier);

            // Adjusted spawn amount for more gradual growth
            spawnAmount = Mathf.FloorToInt(baseSpawnAmount + Mathf.Log(1 + spawnAmountFactor * timePlayedCounter.timer) * difficultyMultiplier);

            yield return new WaitForSeconds(spawningCooldown);
            _spawnable = true;
        }



        private void SpawnEnemy()
        {
            _randomSpawnpoint = Random.Range(0, spawningArea.Length);

            for (var i = 0; i < spawnAmount; i++)
            {
                // Get a random position within the selected spawnpoint
                var randomPosition = GetRandomPositionInSpawnpoint();
                
                // min inclusive, max exclusive -> 0 - 9 = 10 digits, checking a digit = 10% chance

                if (Random.Range(0, 10) == 9)
                {
                    Instantiate(bigShark, randomPosition, quaternion.identity, enemiesParent).name ="Big Shark";
                }
                else
                {
                    Instantiate(shark, randomPosition, Quaternion.identity, enemiesParent).name = "Shark";
                }
                
                
            }
            
            _spawnable = false;
            StartCoroutine(Cooldown());
        }

        public void SpawnEnemyNoCooldown(GameObject enemy, Vector3 position)
        {
            position.x += Random.Range(-0.4f, 0.4f);
            position.y += Random.Range(-0.4f, 0.4f);
            
            Instantiate(enemy, position, Quaternion.identity, enemiesParent).name ="Shark";
        }
        
    }
}
