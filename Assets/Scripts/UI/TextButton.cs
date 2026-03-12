using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public TextMeshProUGUI buttonText;
    public float haverScale = 1.1f;
    public float animationSpeed = 5f;
    Vector2 initialScale;
    Vector2 targetScale;
    private void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        initialScale = transform.localScale;
        targetScale = initialScale;
    }
    private void Update()
    {
        transform.localScale = Vector2.Lerp(transform.localScale,targetScale,Time.unscaledDeltaTime * animationSpeed);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = initialScale * haverScale;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        targetScale = initialScale * haverScale * 0.95f;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        targetScale = initialScale;
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        targetScale = initialScale * haverScale;
    }
}
