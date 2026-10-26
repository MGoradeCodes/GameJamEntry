using UnityEngine;

public class TrainEntry : MonoBehaviour
{
    [SerializeField] private Vector3 startPos;
    private Vector3 endPos = new Vector3(-10, -0.5f, 0);
    [SerializeField] private float speed = 1.0f; 
    
    private float elapsedTime = 0f;

    void Start()
    {
        transform.position = startPos;
    }

    void FixedUpdate()
    {
        StartEntry();
    }

    void StartEntry()
    {
        elapsedTime += speed * Time.fixedDeltaTime;
        
        // Clamp t so it doesn't exceed 1
        float t = Mathf.Clamp01(elapsedTime);

        transform.position = Vector3.Lerp(startPos, endPos, t);
    }
}