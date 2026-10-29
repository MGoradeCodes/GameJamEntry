using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager2 : MonoBehaviour
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
        if(interact.getShardCount() == 3.0f)
        {
            ToggleLights(0, true);
            ToggleLights(1, true);
            ToggleLights(2, true);
            ToggleLights(3, true);
        }
        if(interact.getShardCount() == 4.0f)
        {
            ToggleLights(4, true);
            ToggleLights(5, true);
            ToggleLights(6, true);
            ToggleLights(7, true);
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
