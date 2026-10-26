using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float followSpeed = 6.0f;
    [SerializeField] private Transform player;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;


    void LateUpdate()
    {
        // Clamp the target X position
        float targetX = Mathf.Clamp(player.position.x, minX, maxX);
        Vector3 newPos;

        // Keep current Y and Z
        Vector3 targetPosition = new Vector3(
            targetX,
            transform.position.y,
            transform.position.z
        );

        if(transform.position.x > 74)
        {
            newPos = (transform.position);
            newPos.y = 4;
            transform.position = Vector3.Lerp(transform.position, newPos, followSpeed*Time.deltaTime);
        }
        else
        {
            newPos = transform.position;
            newPos.y = 0;
            transform.position = Vector3.Lerp(transform.position, newPos, followSpeed*Time.deltaTime);
        }
        
         

        // Smoothly move towards the target
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }

}
