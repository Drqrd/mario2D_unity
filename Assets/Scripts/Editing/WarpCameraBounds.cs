using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpCameraBounds : MonoBehaviour
{
 
    // Some copy pasta code from camera controller so that the camera warp position can be easily seen in editor
    Vector2 CalculateThreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x -= Camera.main.GetComponent<CameraController>().followOffset.x;
        t.y -= Camera.main.GetComponent<CameraController>().followOffset.y;
        return t;
    }

    // View boundary box
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = CalculateThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1f));
    }
}
