using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public float bobHeight = 10f;
    public float bobSpeed = 3f;
    public float hoverScale = 1.15f;
    public float scaleSpeed = 15f;

    private Vector3 startPos;
    private Vector3 targetScale;
    private Vector3 baseScale;

    private NextLevel level;

    void Start()
    {
        HandleInteract.isStart = true;
        startPos = transform.localPosition;
        baseScale = transform.localScale;
        targetScale = baseScale;
        level = GameObject.FindAnyObjectByType<NextLevel>();
    
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = baseScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = baseScale;
    }

    public void OnPointerClickHandler(PointerEventData eventData)
    {
        StartGame();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartGame();
    }

    void StartGame()
    {

        level.TriggerNextLevel();
        HandleInteract.isStart = false;
        NextLevel.isEnd = false;
    }
}