using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthUI : MonoBehaviour
{
    public Slider healthUI;
    public EnemyHealth enemyHealth;

    private void Awake()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
    }
    private void OnEnable()
    {
        enemyHealth.EnemyHealthChange += EnemyHealthChange;
    }
    private void OnDisable()
    {
        enemyHealth.EnemyHealthChange -= EnemyHealthChange;
    }
    public void EnemyHealthChange(int currentHealth,int maxHealth)
    {
        healthUI.maxValue = maxHealth;
        healthUI.value = currentHealth;
    }
}
