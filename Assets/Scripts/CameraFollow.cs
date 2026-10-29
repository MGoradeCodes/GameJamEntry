using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float followSpeed = 6.0f;
    [SerializeField] private Transform player;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    private GameObject levelobj;

    private NextLevel level;

    [SerializeField] private Movement playerMovement;

    void Start()
    {
        levelobj = GameObject.Find("LevelLoader");
        level = levelobj.GetComponent<NextLevel>();
    }

    void LateUpdate()
    {
        if (player == null || playerMovement == null) return;
        float targetX = Mathf.Clamp(player.position.x, minX, maxX);
  
        float targetY; 

        if (player.position.y < -10f)
        {
            targetY = transform.position.y;
        }
        else
        {
            targetY = player.position.y;

            if (level.getBuildIdx() < 1)
            {
                if (transform.position.x > 74)
                {
                    targetY = 4f;
                }
                else if (transform.position.x < 74 && !playerMovement.undergroundStatus())
                {
                    targetY = 0f;
                }
            }

        }

        Vector3 targetPosition = new Vector3(
            targetX,
            targetY,
            transform.position.z
        );
         
    
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }
}
