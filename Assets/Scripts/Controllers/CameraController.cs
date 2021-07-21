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
    public GameObject bottomBounds;

    [Header("Free Movement Range")]
    public Vector2 followOffset = new Vector2(0f,0f);

    private Vector2 threshold;
    private Rect aspect;
    private float minSpeed = .5f;
    private Rigidbody rb;

    private void Start()
    {
        SquareCamera();
        threshold = CalculateThreshold();
        rb = target.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        FollowPlayer();
        AdjustLeftBoundPosition();
    }

    void SquareCamera()
    {
        float width = Camera.main.rect.width / Camera.main.aspect;
        Camera.main.rect = new Rect((1f - width) / 2f, 0f, width, Camera.main.rect.height);

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

        float cameraSpeed = rb.velocity.x > minSpeed ? rb.velocity.x : minSpeed;

        if (target.transform.position.x > transform.position.x) { transform.position = Vector3.MoveTowards(transform.position, newPosition, cameraSpeed * Time.deltaTime); }
        
    }

    private void AdjustLeftBoundPosition()
    {
        float horizontalExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
        leftBounds.transform.position = new Vector3(transform.position.x - horizontalExtent / 2f - .5f, leftBounds.transform.position.y, 0f);
    }


    // View boundary box
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = CalculateThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1f));
    }
}
