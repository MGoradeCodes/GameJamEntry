using System.Collections;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform platform;
    [SerializeField] private Transform Point1;
    [SerializeField] private Transform Point2;

    [SerializeField] private float speed = 0.8f;
    int direction = 1;
    void Start()
    {
        
    }

    Vector2 currentTargetLocation()
    {
        if(direction == 1)
        {
            return Point1.position;
        }
        else
        {
            return Point2.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 target = currentTargetLocation();
        platform.position = Vector2.Lerp(platform.position, target, speed * Time.deltaTime);

        float distance = (target - (Vector2) platform.position).magnitude;
        if(distance <= 0.1f)
        {
            direction *= -1;
        }
    }
}
