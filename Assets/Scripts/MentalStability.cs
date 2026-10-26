using UnityEngine;
using UnityEngine.UI;

public class MentalStability : MonoBehaviour
{
    public Slider slider;

    [SerializeField] private Image fillImage;
    private float targetValue;

    void Start()
    {
        slider.value = 100f;
        targetValue = slider.value;
    }

    void Update()
    {
        if (slider.value != targetValue)
        {
            // 10f controls the speed of the interpolation (higher = faster)
            slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * 10f);
        }
        
        if(slider.value <= 75)
        {
            fillImage.color = (Color)new Color32(87, 126, 157, 255);
        }
        if(slider.value <= 50)
        {
            fillImage.color = Color.yellow;
        }
        if(slider.value <= 25)
        {
            fillImage.color = Color.red;
        }
    }

    public void increaseBar(float val)
    {
        targetValue += val;
    
        targetValue = Mathf.Clamp(targetValue, slider.minValue, slider.maxValue);
    }
}