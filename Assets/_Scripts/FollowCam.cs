using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public float speed = 3.0f;
    public float zDistance = 10.0f;
    public float allowableOffset = 3.0f;

    // These define the *playable* area
    public Vector2 topLeft;
    public Vector2 bottomRight;

    private GameObject player;
    private Camera cam;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GetComponent<Camera>();

        transform.position = player.transform.position + Vector3.back * zDistance;
    }

    void LateUpdate()
    {
        Vector3 targetPos = player.transform.position + Vector3.back * zDistance;

        // Smooth follow only if we pass the allowable distance
        if (Vector3.Distance(transform.position, targetPos) > allowableOffset)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }

        // Now clamp to bounds using camera size
        ClampCameraToBounds();
    }

    void ClampCameraToBounds()
    {
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        Vector3 pos = transform.position;

        // Clamp LEFT and RIGHT
        pos.x = Mathf.Clamp(
            pos.x,
            topLeft.x + camWidth,
            bottomRight.x - camWidth
        );

        // Clamp TOP and BOTTOM
        pos.y = Mathf.Clamp(
            pos.y,
            bottomRight.y + camHeight,
            topLeft.y - camHeight
        );

        transform.position = pos;
    }
}
