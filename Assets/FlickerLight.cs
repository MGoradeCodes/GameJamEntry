using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickerLight : MonoBehaviour
{
    private Light2D light2D;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float speed = 10f;
    private float seed;

    void Start()
    {
        light2D = GetComponentInChildren<Light2D>();
        seed = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (light2D != null)
        {
            float noise = Mathf.PerlinNoise(Time.time * speed + seed, 0f);
            light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        }
    }
}