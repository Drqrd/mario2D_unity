using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathM
{
    public static Vector2 Abs(Vector2 v)
    {
        v = new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
        return v;
    }

    public static Vector3 Abs(Vector3 v)
    {
        v = new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        return v;
    }
}
