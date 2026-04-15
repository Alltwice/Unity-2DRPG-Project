using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    public Slider playerHealth;
    private void OnEnable()
    {
        GameEvent.PlayerHealthChange += PlayerHealthChange;
        GameEvent.PlayerHealthSyncOnly += PlayerHealthChange;
    }
    private void OnDisable()
    {
        GameEvent.PlayerHealthChange -= PlayerHealthChange;
        GameEvent.PlayerHealthSyncOnly -= PlayerHealthChange;
    }
    public void PlayerHealthChange(int currentHealth,int maxHealth)
    {
        playerHealth.maxValue = maxHealth;
        playerHealth.value = currentHealth;
    }
}
