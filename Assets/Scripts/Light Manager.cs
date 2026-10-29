using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;


public class LigthManager : MonoBehaviour
{
    [SerializeField] private HandleInteract interact;
    private Light2D[] lights;
    void Start()
    {
        lights = GetComponentsInChildren<Light2D>();
        ToggleLights(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(interact.getShardCount() == 1.0f)
        {
            ToggleLights(0, true);
            ToggleLights(1, true);
            ToggleLights(2, true);
        }
        if(interact.getShardCount() == 2.0f)
        {
            ToggleLights(3, true);
            ToggleLights(4, true);
        }
    }

    void ToggleLights(bool state)
    {
        foreach(Light2D light in lights)
        {
            if (light)
            {
                light.enabled = state;
            }
        }

    }

    void ToggleLights(int val, bool state)
    {   
        if(lights[val] != null)
        {
            lights[val].enabled = state;
        }
    }
}
