using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LigthManager : MonoBehaviour
{
    [SerializeField] private HandleInteract interact;

    [SerializeField] private float minFlickerDelay = 0.05f;
    [SerializeField] private float maxFlickerDelay = 0.4f;

    private Coroutine chaosRoutine;
    private bool chaosActive;

    private Light2D[] lights;
    void Start()
    {
        lights = GetComponentsInChildren<Light2D>();
        ToggleLights(false);
    }

    
    void Update()
    {
        if (CollapseState.instance.getState() !=true)
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
