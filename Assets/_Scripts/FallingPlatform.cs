using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 1.0f;
    public float respawnDelay = 2.0f;
    public float fallGravityScale = 5.0f;

    private Vector3 originalPosition;
    private Rigidbody2D rb;
    private Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        originalPosition = transform.position;

        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            StartCoroutine(FallRoutine());
        }
    }

    IEnumerator ShakePlatform()
    {
        Vector3 original = transform.position;
        float timer = 0;

        while (timer < fallDelay)
        {
            transform.position = original + (Vector3)Random.insideUnitCircle * 0.05f;
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = original;
    }

    IEnumerator FallRoutine()
    {
        yield return StartCoroutine(ShakePlatform());

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = fallGravityScale;

        yield return new WaitForSeconds(0.2f);
        col.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        transform.position = originalPosition;

        col.enabled = true;
    }
}
