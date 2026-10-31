using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MentalStability : MonoBehaviour
{
    public GameObject stabilityBarUI; 
    public Slider slider;
    [SerializeField] private Image fillImage;

    [SerializeField] private NextLevel level;
    private float targetValue;

    void Start()
    {
        if (slider != null)
        {
            slider.value = 100f;
            targetValue = slider.value;
        }
    }

    void Update()
    {
        if (NextLevel.isEnd || HandleInteract.isStart)
        {
            if (stabilityBarUI != null) stabilityBarUI.SetActive(false);
        }
        else
        {
            if (stabilityBarUI != null && level.getBuildIdx() != 0) stabilityBarUI.SetActive(true);
        }

        if (slider != null)
        {
            if (slider.value != targetValue)
            {
                slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * 8f);
            }

            if (slider.value <= 25) fillImage.color = Color.red;
            else if (slider.value <= 50) fillImage.color = Color.yellow;
            else if (slider.value <= 75) fillImage.color = new Color32(87, 126, 157, 255);
        }
    }

    public void increaseBar(float val)
    {
        if (slider != null)
        {
            targetValue += val;
            targetValue = Mathf.Clamp(targetValue, slider.minValue, slider.maxValue);
        }
        if (targetValue <= 0 && !NextLevel.isEnd)
        {
            NextLevel.isEnd = true;
            level.LoadEndScreen();
        }
    }

    public float getBarValue()
    {
        return slider.value;
    }
}