using System.Collections;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    [SerializeField] private float fallDelay = 0.8f;
    [SerializeField] private float respawnDelay = 3f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ParticleSystem particlePrefab;

    private Vector3 startPosition;
    private bool isFalling;
    private bool isRespawning;

    void Start()
    {
        startPosition = transform.position;
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") &&
            !isFalling && !isRespawning)
        {
            isFalling = true;
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);

        if (particlePrefab != null)
        {
            Instantiate(particlePrefab, transform.position, Quaternion.identity);
        }

        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void Update()
    {
        if (isFalling && !isRespawning && transform.position.y < -6f)
        {
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        isRespawning = true;

        yield return new WaitForSeconds(respawnDelay);

        // Stop the platform's physics before repositioning it.
        rb.bodyType = RigidbodyType2D.Static;
        transform.position = startPosition;

        isFalling = false;
        isRespawning = false;
    }
}