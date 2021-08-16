using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    PlayerController player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // When player collects coin, add to coin counter, play a sound and destroy this coin
    private void PlayerCollectedCoin()
    {
        Overlay.AddToCoins(1);
        AudioController.PlaySound("Coin");
        transform.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject == player.gameObject) { PlayerCollectedCoin(); }
    }
}
