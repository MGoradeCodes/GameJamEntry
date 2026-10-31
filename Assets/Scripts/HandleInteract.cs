using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public static int shardCount = 0;

    private GameObject currentShard = null;
    private HandleBackpack currentBackpackScript = null;
    private TicketManager currentTicketScript = null;

    private bool ticket;

    public static HandleInteract instance;

    private GameObject shardTextObj;

    public static bool isStart = true;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        isStart = (SceneManager.GetActiveScene().buildIndex == 0);
    }

    void Start()
    {
        bar = Object.FindAnyObjectByType<MentalStability>();
        
        shardTextObj = GameObject.Find("ShardCount");
        if (shardTextObj != null)
        {
            ShardText = shardTextObj.GetComponent<TextMeshProUGUI>();
        }

        if (Circle != null) Circle.fillAmount = 0f;
        if (prompt != null) 
        {
            prompt.SetActive(false);
        }

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex != 0)
        {
            isStart = false;
        }
        else
        {
            bar.gameObject.SetActive(true);
        }

        UpdateShardCountDisplay();
    }

    void Update()
    {
        Debug.Log("is Start? : " + isStart);
        if (currentShard != null || (inRange && (backpack || ticket)))
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

        if (shardTextObj != null)
        {
            if (isStart || NextLevel.isEnd)
            {
                shardTextObj.SetActive(false);
            }
            else
            {
                shardTextObj.SetActive(true);
            }
        }
    }

    void OnInteractionComplete()
    {
        if (currentShard != null && !backpack && !ticket)
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
            else
            {
                Debug.LogError("HandleInteract: Backpack detected, but HandleBackpack component is missing on the collider target!");
            }
        }

        if (ticket)
        {
            if (currentTicketScript != null)
            {
                currentTicketScript.TriggerTicket();
            }
            else
            {
                Debug.LogError("HandleInteract: Ticket detected, but TicketManager component is missing on the collider target!");
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
        
        if (collision.gameObject.CompareTag("Ticket"))
        {
            inRange = true;
            ticket = true;
            currentTicketScript = collision.GetComponent<TicketManager>();
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
        if (collision.gameObject.CompareTag("Ticket"))
        {
            inRange = false;
            ticket = false;
            currentTicketScript = null;
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
        if (shardTextObj != null && ShardText != null)
        {
            ShardText.text = "Shard Count : " + shardCount;
        }
    }
}