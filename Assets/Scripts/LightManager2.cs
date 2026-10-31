using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class LightManager2 : MonoBehaviour
{
    [SerializeField] private HandleInteract interact;
    private Light2D[] lights;

    [SerializeField] private float minFlickerDelay = 0.05f;
    [SerializeField] private float maxFlickerDelay = 0.4f;

    private Coroutine chaosRoutine;
    private bool chaosActive;

    void Start()
    {
        lights = GetComponentsInChildren<Light2D>();
        ToggleLights(false);
    }


    void Update()
    {
        if (!CollapseState.instance.getState())
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
                ToggleLights(8, true);
                ToggleLights(9, true);
                ToggleLights(10, true);
                ToggleLights(11, true);
            }
        }

        if (CollapseState.instance.getState() == true)
        {
            StartChaos();
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
    private IEnumerator ChaosLights()
    {
        while (true)
        {
            foreach (Light2D light in lights)
            {
                if (light != null)
                {
                    light.enabled = Random.value > 0.5f;
                }
            }

            yield return new WaitForSeconds(
                Random.Range(minFlickerDelay, maxFlickerDelay)
            );
        }
    }

    void StartChaos()
    {
        if (chaosActive) return;

        chaosActive = true;
        chaosRoutine = StartCoroutine(ChaosLights());
    }
    public void StopChaos()
    {
        if (!chaosActive) return;

        chaosActive = false;

        if (chaosRoutine != null)
        {
            StopCoroutine(chaosRoutine);
            chaosRoutine = null;
        }

        ToggleLights(true);
    }
}
