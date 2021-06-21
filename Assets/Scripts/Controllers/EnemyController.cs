using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    float cameraPos;
    bool[] children;


    private void Start()
    {
       children = new bool[transform.childCount];
       for (int i = 0; i < children.Length; i++) { children[i] = true; }
    }
    private void Update()
    {
        cameraPos = Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width / Screen.height - Camera.main.rect.x;

        EnableEnemies();
    }

    private void EnableEnemies()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (children[i])
            {
                Transform child = transform.GetChild(i);
                if (child.localPosition.x < cameraPos && child.gameObject.activeSelf)
                {
                    if (child.name.Contains("Goomba")) { child.GetComponent<Goomba>().Enable(); children[i] = false; }
                }
            }
        }
    }
}
