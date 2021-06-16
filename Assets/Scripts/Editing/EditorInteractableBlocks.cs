using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UpdateInteractableBlock))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UpdateInteractableBlock update = (UpdateInteractableBlock)target;

        if (GUILayout.Button("Clear"))
        {
            update.DestroyChildren();
        }

        if (GUILayout.Button("Update"))
        {
            update.OnValidate();
        }
    }
}