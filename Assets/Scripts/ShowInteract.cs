using UnityEngine;

public class ShowInteract : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool show;

    private Vector3 newPos;

    [SerializeField] HandleInteract interact;

    // Update is called once per frame
    void Update()
    {
        show = interact.getRange();

        if (show)
        {
            newPos = transform.position;
            newPos.z = 0;
        }
        else
        {
            newPos.z = -10;
        }

        transform.position = newPos;
    }
}
