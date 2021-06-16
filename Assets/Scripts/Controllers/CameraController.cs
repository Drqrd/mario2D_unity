using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow Target")]
    public GameObject target;

    [Header("Camera Bounds")]
    public GameObject leftBounds;
    public GameObject rightBounds;

    [Header("Free Movement Range")]
    public Vector2 followOffset = new Vector2(0f,0f);

    private Vector2 threshold;
    private Rect aspect;
    private float minSpeed = .5f;
    private Rigidbody rb;

    private void Start()
    {
        threshold = CalculateThreshold();
        rb = target.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        FollowPlayer();
    }

    Vector2 CalculateThreshold()
    {
        aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x -= followOffset.x;
        t.y -= followOffset.y;
        return t;
    }

    void FollowPlayer()
    {
        Vector2 follow = target.transform.position;
        float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
        float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * follow.y);

        Vector3 newPosition = transform.position;
        if (Mathf.Abs(xDifference) >= threshold.x) { newPosition.x = follow.x; }
        if (Mathf.Abs(yDifference) >= threshold.y) { newPosition.y = follow.y; }

        float cameraSpeed = rb.velocity.magnitude > minSpeed ? rb.velocity.magnitude : minSpeed;

        transform.position = Vector3.MoveTowards(transform.position, newPosition, cameraSpeed * Time.deltaTime);
    }

    // View boundary box
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = CalculateThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1f));
    }
}
