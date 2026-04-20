using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthUI : MonoBehaviour
{
    public Slider healthUI;
    public EnemyHealth enemyHealth;
    [SerializeField] private Vector3 headOffset = new Vector3(0f, 0.2f, 0f);

    private RectTransform rectTransform;
    private Canvas parentCanvas;
    private Collider2D targetCollider;
    private Renderer targetRenderer;

    private void Awake()
    {
        rectTransform = transform as RectTransform;
        parentCanvas = GetComponentInParent<Canvas>();
        enemyHealth = GetComponentInParent<EnemyHealth>();
        CacheTargetBoundsComponent();
    }
    private void OnEnable()
    {
        if (enemyHealth != null)
            enemyHealth.EnemyHealthChange += EnemyHealthChange;
    }
    private void OnDisable()
    {
        if (enemyHealth != null)
            enemyHealth.EnemyHealthChange -= EnemyHealthChange;
    }
    private void LateUpdate()
    {
        UpdateFollowPosition();
    }
    public void EnemyHealthChange(int currentHealth,int maxHealth)
    {
        healthUI.maxValue = maxHealth;
        healthUI.value = currentHealth;
    }

    private void CacheTargetBoundsComponent()
    {
        if (enemyHealth == null)
            return;

        Transform targetRoot = enemyHealth.transform;
        targetCollider = targetRoot.GetComponentInChildren<Collider2D>();
        targetRenderer = targetRoot.GetComponentInChildren<Renderer>();
    }

    private Vector3 GetHeadWorldPosition()
    {
        Vector3 basePos = enemyHealth != null ? enemyHealth.transform.position : transform.position;

        if (targetCollider != null)
        {
            Bounds b = targetCollider.bounds;
            basePos = new Vector3(b.center.x, b.max.y, basePos.z);
        }
        else if (targetRenderer != null)
        {
            Bounds b = targetRenderer.bounds;
            basePos = new Vector3(b.center.x, b.max.y, basePos.z);
        }

        return basePos + headOffset;
    }

    private void UpdateFollowPosition()
    {
        if (enemyHealth == null)
            return;

        Vector3 targetWorld = GetHeadWorldPosition();

        if (parentCanvas == null || parentCanvas.renderMode == RenderMode.WorldSpace)
        {
            transform.position = targetWorld;
            return;
        }

        Camera uiCamera = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera;
        Camera worldCamera = Camera.main;
        if (worldCamera == null || rectTransform == null)
            return;

        Vector3 screenPoint = worldCamera.WorldToScreenPoint(targetWorld);
        if (screenPoint.z < 0f)
            return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            screenPoint,
            uiCamera,
            out Vector2 localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }
}
