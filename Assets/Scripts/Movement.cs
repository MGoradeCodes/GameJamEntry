using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

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

    private CinemachineImpulseSource impulseSource;

    [SerializeField] private CinemachineImpulseSource destructSource;

    private Rigidbody2D rb;

    private Volume volume;
    private Vignette vignette;

    private float horizontalInput;

    private MentalStability bar; 

    private Vector3 spawnPoint ;

    private bool lowStabilityShake;
    private Coroutine shakeRoutine;

    private bool death;

    private bool isGrounded;

    private bool enterUnderground;

    private bool gameOver;
    int scene;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        scene = SceneManager.GetActiveScene().buildIndex;

        if(scene == 1)
        {
            spawnPoint = new Vector3(-22, 1, 0);
        }
        else if(scene == 2)
        {
            spawnPoint = new Vector3(-2, -2, 0);
        }

        Debug.Log("Scene index: " + scene);
        Debug.Log("Spawn point: " + spawnPoint);
        bar = Object.FindAnyObjectByType<MentalStability>();

        GameObject volumeObj = GameObject.Find("Global Volume");

        if (volumeObj != null)
        {
            volume = volumeObj.GetComponent<Volume>();

            if (volume != null && volume.profile != null)
            {
                volume.profile.TryGet(out vignette);
            }
        }
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
        
        if (transform.position.y < -20 && !death)
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
        if(impulseSource == null)
        {
            Debug.Log("Source is Null");

        }
        if(CameraShake.instance == null)
        {
            Debug.Log("Instance is Null");
        }
        Debug.Log(CameraShake.instance == null);
    }

    public void Reset()
    {
        if (vignette != null)
        {
            vignette.intensity.value = 0.5f;
        }
        
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
            Debug.Log(CameraShake.instance == null);
            CameraShake.instance.Shake(impulseSource);
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("underground area"))
        {
            enterUnderground = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("underground area"))
        {
            enterUnderground = false;
        }
    }

    public void SetSpawn(Vector3 newPos)
    {
        spawnPoint = new Vector3(newPos.x, newPos.y, 0f);
    }

    public bool undergroundStatus()
    {
        return enterUnderground;
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
        if (bar != null)
        {
            bar.increaseBar(-10.0f);
        }
        Reset();

    }

    private void ContinousDegrade()
    {
        if (bar != null)
        {
            bar.increaseBar(-0.4f * Time.deltaTime);

            if (bar.getBarValue() <= 50f && !lowStabilityShake)
            {
                CollapseState.instance.SetState(true);
                lowStabilityShake = true;
                shakeRoutine = StartCoroutine(ContinuousShake());
            }
            if(bar.getBarValue() > 50)
            {
                CollapseState.instance.SetState(false);
            }
        }
    }
    private IEnumerator ContinuousShake()
{
    while (true)
    {
        CameraShake.instance.Shake(impulseSource);

        yield return new WaitForSeconds(0.8f);
    }

}
}
