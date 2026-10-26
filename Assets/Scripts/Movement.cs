using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Movement : MonoBehaviour
{
    [Header("Movement Tweaks")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 20f;

    [Header("References")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private ParticleSystem ps;

    [SerializeField] private SpriteRenderer render;

    [SerializeField] private Animator animator;

    private Rigidbody2D rb;

    [SerializeField] private Volume volume;
    private Vignette vignette;

    private float horizontalInput;

    public MentalStability bar; 

    [SerializeField] private Vector3 spawnPoint = new Vector3(-6, 0, 0);

    private bool death;

    private bool isGrounded;

    private bool gameOver;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        volume.profile.TryGet(out vignette);
        vignette.intensity.value = 0.5f;
        Reset();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if(horizontalInput != 0)
        {   
            animator.SetBool("isRunning", true);

        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        
        if (transform.position.y < -5 && !death)
        {
            StartCoroutine(Death());
        }

        if(horizontalInput <= -1)
        {
            render.flipX = true;
        }
        if(horizontalInput >= 1)
        {
            render.flipX = false;
        }
    }

    void Reset()
    {
        vignette.intensity.value = 0.5f;
        transform.position = spawnPoint;
        
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
        }

        if (render != null)
        {
            render.enabled = true; 
        }

        death = false; 
    }

    void FixedUpdate()
    {
        if (death != true)
        {
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                0.2f,
                groundLayer
            );

            rb.linearVelocity = new Vector2(
                horizontalInput * moveSpeed,
                rb.linearVelocity.y
            );

            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    jumpForce
                );

                isGrounded = false;
            }
            ContinousDegrade();
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Danger") && !death)
        {

            StartCoroutine(Death());
        }

        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            var ob = collision.gameObject;
            transform.SetParent(ob.transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }
    }

    public void SetSpawn(Vector3 newPos)
    {
        spawnPoint = (newPos);
    }

    private IEnumerator Death()
    {
        death = true;

        Instantiate(ps, transform.position, Quaternion.identity);

        if (render != null)
        {
            render.enabled = false;
        }
        
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic; 
        }

        if (vignette == null) yield break;

        float startIntensity = vignette.intensity.value;
        float targetIntensity = startIntensity + 1.0f; 
        float duration = 2.0f; 
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / duration;

            vignette.intensity.value = Mathf.Lerp(startIntensity, targetIntensity, percentageComplete);
            
            yield return null; 
        }
        vignette.intensity.value = targetIntensity;
        if (gameOver)
        {
            
        }
        Reset();
        bar.increaseBar(-10.0f);
    }

    private void ContinousDegrade()
    {
        bar.increaseBar(-0.2f * Time.deltaTime);
    }
}
