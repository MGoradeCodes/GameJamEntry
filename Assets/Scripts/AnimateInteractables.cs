using UnityEngine;

public class AnimateInteractables : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private double height;
    private float speed = 1.3f;

    [SerializeField] private float offsetY;

    [SerializeField] private float divideFactor;
    private float angle;
    private float theta;
    private Vector3 newPos;
    void Start()
    {
        height = 0;
    }
    void FixedUpdate()
    {
        angle += speed*Time.deltaTime;
        theta = angle;
        height = Mathf.Sin(theta) / divideFactor;
        newPos = transform.position;
        newPos.y = (float)height + offsetY;
        transform.position = newPos;

    }   
}
