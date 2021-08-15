using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    float cameraX;
    float cameraY;
    bool[] children;


    private void Start()
    {
       children = new bool[transform.childCount];
       for (int i = 0; i < children.Length; i++) { children[i] = true; }
    }
    private void Update()
    {
        // Position of camera, enemies are activated when at camera bound
        cameraX = Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width / Screen.height - Camera.main.rect.x;
        cameraY = Camera.main.transform.position.y;

        EnableEnemies();
    }

    private void EnableEnemies()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (children[i])
            {
                Transform child = transform.GetChild(i);
                // if within x bound and is in same level   
                if (child.localPosition.x < cameraX && child.localPosition.y < cameraY)
                {
                    child.GetComponent<EnemyInterface>().Enable();
                    children[i] = false;
                }
            }
        }
    }
}
