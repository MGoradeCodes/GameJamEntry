using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandleInteract : MonoBehaviour
{
    [SerializeField] private Image Circle;
    [SerializeField] private float holdDuration = 1f;
    
    private float holdingTimer = 0f;

    [SerializeField] private ParticleSystem ps;
    private bool isHolding = false;

    [SerializeField] private GameObject prompt;

    [SerializeField] private MentalStability bar;

    [SerializeField] private Movement player;

    [SerializeField] private TextMeshProUGUI ShardText;

    private Vector3 newPos;

    private bool inRange;

    private int shardCount = 0;

    // Store the shard the player is currently touching
    private GameObject currentShard = null;

    void Start()
    {
        if (Circle != null) Circle.fillAmount = 0f;
        if (prompt != null) 
        {
            prompt.SetActive(false); 
        }
    }

    void Update()
    {
        // Only allow holding "E" if the player is actually standing next to a shard
        if (currentShard != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isHolding = true;
            }

            if (isHolding && Input.GetKey(KeyCode.E))
            {
                holdingTimer += Time.deltaTime;
                
                if (Circle != null)
                {
                    Circle.fillAmount = holdingTimer / holdDuration;
                }
                
                if (holdingTimer >= holdDuration)
                {
                    OnInteractionComplete();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            ResetHolding();
        }
    }

    void OnInteractionComplete()
    {
        if (currentShard != null)
        {
            shardCount++;
            ShardText.text = "Shard Count : " + shardCount;
            newPos = currentShard.transform.position;
            player.SetSpawn(newPos);
            
            bar.increaseBar(15.0f);
            
            if (ps != null)
            {
                Instantiate(ps, currentShard.transform.position, Quaternion.identity);
            }
            
            Destroy(currentShard);
            if (prompt != null) 
            {
                prompt.SetActive(false);
            }
            
            currentShard = null; 
        }
        
        ResetHolding();
    }

    void ResetHolding()
    {
        holdingTimer = 0f;
        isHolding = false;
        if (Circle != null) Circle.fillAmount = 0f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Memory Shard"))
        {
            inRange = true;
            currentShard = collision.gameObject;
            if (prompt != null) 
            {
                prompt.SetActive(true);
            }
        }
    }

    public bool getRange()
    {
        return inRange;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Memory Shard"))
        {
            inRange = false;
            currentShard = null;
            if (prompt != null) 
            {
                prompt.SetActive(false);
            }
            ResetHolding();
        }
    }
}
