using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private ItemDataSO item;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            InventoryManager.Instance.AddItem(1, item);
            Destroy(gameObject);
        }
    }
}
