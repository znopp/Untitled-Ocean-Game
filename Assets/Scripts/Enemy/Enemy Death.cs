using Item_Scripts.GameObject_Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyDeath : MonoBehaviour
    {

        private Transform _coinsParent;
        private Transform _healthParent;
        
        private EnemySpawning _enemySpawning;
        private CoinGameObject _coinGameObject;
        private HealthGameObject _healthGameObject;

        public void Start()
        {
            _enemySpawning = FindObjectOfType<EnemySpawning>();
            _coinGameObject = FindObjectOfType<CoinGameObject>();
            _healthGameObject = FindObjectOfType<HealthGameObject>();
            
            var coinGameObject = GameObject.Find("Coins");
            var healthGameObject = GameObject.Find("Health");
            
            // Coin object in hierarchy
            if (coinGameObject)
            {
                _coinsParent = coinGameObject.transform;
            }
            else
            {
                Debug.LogError("Coin GameObject not found in the scene.");
            }
            
            // Health object in hierarchy
            if (healthGameObject)
            {
                _healthParent = healthGameObject.transform;
            }
            else
            {
                Debug.LogError("Health GameObject not found in the scene.");
            }
        }

        public void KillGeneric(GameObject enemy, Vector3 position)
        {
            Destroy(enemy);
            
            if (Random.Range(0, 9) <= 3)
            {
                SpawnCoin(_coinGameObject.coinGameObject, position);
            }
            
            if (Random.Range(0, 9) <= 1)
            {
                SpawnHealth(_healthGameObject.healthGameObject, position);
            }

        }
    
        public void KillBigShark(GameObject bigShark, Vector3 position)
        {
            Destroy(bigShark);
            
            if (Random.Range(0, 9) <= 3)
            {
                SpawnCoin(_coinGameObject.coinGameObject, position);
            }
            
            if (Random.Range(0, 9) <= 1)
            {
                SpawnHealth(_healthGameObject.healthGameObject, position);
            }
            
            _enemySpawning.SpawnEnemyNoCooldown(_enemySpawning.shark, position);
            _enemySpawning.SpawnEnemyNoCooldown(_enemySpawning.shark, position);
        }

        private void SpawnCoin(GameObject money, Vector3 position)
        {
            position.x += Random.Range(-0.2f, 0.2f);
            position.y += Random.Range(-0.2f, 0.2f);
            
            Instantiate(money, position, Quaternion.identity, _coinsParent).name = "Coin";
        }

        private void SpawnHealth(GameObject health, Vector3 position)
        {
            position.x += Random.Range(-0.2f, 0.2f);
            position.y += Random.Range(-0.2f, 0.2f);
            
            Instantiate(health, position, Quaternion.identity, _healthParent).name = "Health";
        }
    }
}
