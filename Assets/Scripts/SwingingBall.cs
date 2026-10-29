using Unity.VisualScripting;
using UnityEngine;

public class SwingingBall : MonoBehaviour
{   
    [SerializeField] private float speed = 1.5f;
    private float angle;
    // Update is called once per frame
    void Update()
    {
        angle = Mathf.Sin(Time.time * speed) * 60f;
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}
