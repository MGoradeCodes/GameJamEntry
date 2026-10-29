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

    private MentalStability bar;

    [SerializeField] private Movement player;

    private TextMeshProUGUI ShardText;

    private Vector3 newPos;

    private bool inRange;
    private bool backpack;

    private static int shardCount = 0;

    private GameObject currentShard = null;
    private HandleBackpack currentBackpackScript = null;

    void Start()
    {
        bar = Object.FindAnyObjectByType<MentalStability>();
        
        GameObject shardTextObj = GameObject.Find("ShardText");
        if (shardTextObj != null)
        {
            ShardText = shardTextObj.GetComponent<TextMeshProUGUI>();
        }

        if (Circle != null) Circle.fillAmount = 0f;
        if (prompt != null) 
        {
            prompt.SetActive(false);
        }

        UpdateShardCountDisplay();
    }

    void Update()
    {
        if (currentShard != null || (inRange && backpack))
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
            UpdateShardCountDisplay(); 

            newPos = currentShard.transform.position;
            player.SetSpawn(newPos);
            
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

        if (backpack)
        {
            if (currentBackpackScript != null)
            {
                currentBackpackScript.TriggerBackpackAction();
            }
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

        if (collision.gameObject.CompareTag("Backpack"))
        {
            inRange = true;
            backpack = true;
            currentBackpackScript = collision.GetComponent<HandleBackpack>();
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
        if (collision.gameObject.CompareTag("Backpack"))
        {
            inRange = false;
            backpack = false;
            currentBackpackScript = null;
            if (prompt != null) 
            {
                prompt.SetActive(false);
            }
            ResetHolding();
        }
    }

    public void ShardCountIncrement(int val)
    {
        shardCount += val;
        if (bar != null)
        {
            bar.increaseBar(15.0f);
        }
        UpdateShardCountDisplay();
    }

    public int getShardCount()
    {
        return shardCount;
    }

    private void UpdateShardCountDisplay()
    {
        if (ShardText != null)
        {
            ShardText.text = "Shard Count : " + shardCount;
        }
    }
}
