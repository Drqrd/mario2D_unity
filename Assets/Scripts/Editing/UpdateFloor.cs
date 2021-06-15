using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpdateFloor : MonoBehaviour
{
    [Range(1,20)]
    public int x = 1, y = 1;
    public SpriteRenderer sprite;
    public BoxCollider bc;

    // Update when object is changed in editor
    #if UNITY_EDITOR
    void OnValidate() { UnityEditor.EditorApplication.delayCall += _OnValidate; }
    void _OnValidate()
    {
        UpdateObject();
    }
    #endif

    void UpdateObject()
    {
        bc.size = new Vector2(x, y);
        sprite.size = new Vector2(x, y);
    }
}
