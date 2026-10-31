using System.Collections;
using UnityEngine;

public class TicketManager : MonoBehaviour
{
    [SerializeField] private Transform shard;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private HandleInteract interact;
    [SerializeField] private float height = 1.3f;
    [SerializeField] private float duration = 0.8f;

    [SerializeField] private Movement player;

    private Vector3 newPos;
    private bool isRising = false; 

    void Start()
    {
        if (shard != null)
        {
            newPos = shard.position;
            newPos.z = -10;
            shard.position = newPos;
        }
    }

    public void TriggerTicket()
    {
        if (!isRising)
        {
            isRising = true;

            if (shard != null)
            {
                newPos.z = 0;
                shard.position = newPos;
                StartCoroutine(RiseUp());
            }
        }
    }

    private IEnumerator RiseUp()
    {
        if (shard == null) yield break;

        Vector3 startPosition = shard.position;
        Vector3 targetPosition = startPosition + new Vector3(0, height, 0);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / duration;
            float smoothPercentage = Mathf.SmoothStep(0f, 1f, percentageComplete);

            if (shard == null) yield break;

            shard.position = Vector3.Lerp(startPosition, targetPosition, smoothPercentage);
            
            yield return null; 
        }

        if (shard == null) yield break;
        shard.position = targetPosition;
        
        Debug.Log("Object finished rising!");
        
        yield return new WaitForSeconds(0.5f);

        if (shard == null) yield break;
        
        Instantiate(ps, shard.position, Quaternion.identity);
        Destroy(shard.gameObject);
        
        if (interact != null)
        {
            interact.ShardCountIncrement(1);
        }

        yield return new WaitForSeconds(0.6f);
        
        if (player != null)
        {
            player.SetSpawn(new Vector3(88, 3, 0));
            player.Reset();
        }
        Destroy(gameObject);
    }
}