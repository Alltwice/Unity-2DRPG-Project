using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    public Slider playerHealth;
    private void OnEnable()
    {
        GameEvent.PlayerHealthChange += PlayerHealthChange;
    }
    private void OnDisable()
    {
        GameEvent.PlayerHealthChange -= PlayerHealthChange;
    }
    public void PlayerHealthChange(int currentHealth,int maxHealth)
    {
        playerHealth.maxValue = maxHealth;
        playerHealth.value = currentHealth;
    }
}
