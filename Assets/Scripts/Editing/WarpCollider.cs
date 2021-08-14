using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpCollider : MonoBehaviour
{
    PlayerController player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject == player.gameObject) { if (player.GetIsCrouching()) { StartCoroutine(player.Warp(transform)); } }
    }
}
