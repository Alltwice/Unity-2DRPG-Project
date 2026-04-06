using UnityEngine;
using UnityEngine.InputSystem; // 必须引用这个命名空间

public class MouseFollower : MonoBehaviour
{
    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private Vector2 offset = new Vector2(20, -20);
    private RectTransform rectTransform;

    void Awake() => rectTransform = GetComponent<RectTransform>();

    void Update()
    {
        if (parentCanvas == null) return;
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            mousePos,
            parentCanvas.worldCamera,
            out localPoint
        );

        rectTransform.anchoredPosition = localPoint + offset;
    }
}