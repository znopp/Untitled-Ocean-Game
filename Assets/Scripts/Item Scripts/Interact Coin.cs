using System;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Item_Scripts
{
    public class InteractCoin : MonoBehaviour
    {
        public TextMeshProUGUI coinText;
        public TextMeshProUGUI coinTextInGUI;

        public int coins;
        
        private Rigidbody2D _coinRigidbody;
        
        private float _coinVelocity;

        private Vector2 _moveToPlayer;


        private void Update()
        {
            coinText.text = "" + coins;
            coinTextInGUI.text = coinText.text;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (!col.gameObject.CompareTag("coin")) return;
            
            coins += 1;
            SoundFXManager.Instance.playCoinFX.Play();
            coinText.text = "" + coins;
            coinTextInGUI.text = coinText.text;
            Destroy(col.gameObject);
        }

        public void RemoveCoins(int cost)
        {
            coins -= cost;
            coinText.text = coins.ToString();
            coinTextInGUI.text = coinText.text;
        }

    }
}