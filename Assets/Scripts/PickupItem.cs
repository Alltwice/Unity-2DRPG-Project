using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private ItemDataSO item;
    [SerializeField] private int pickupAmount = 1;
    [SerializeField] private float floatAmplitude = 0.15f;
    [SerializeField] private float floatFrequency = 2f;
    [SerializeField] private float magnetRange = 2.5f;
    [SerializeField] private float magnetSpeed = 6f;

    private Transform playerTransform;
    private Vector3 floatBasePosition;
    private float floatPhase;

    private void Start()
    {
        floatBasePosition = transform.position;
        floatPhase = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        if (playerTransform == null)
        {
            TryCachePlayer();
        }

        if (playerTransform != null)
        {
            float distance = Vector2.Distance(transform.position, playerTransform.position);
            if (distance <= magnetRange)
            {
                Vector2 nextPos = Vector2.MoveTowards(
                    transform.position,
                    playerTransform.position,
                    magnetSpeed * Time.deltaTime);
                transform.position = new Vector3(nextPos.x, nextPos.y, transform.position.z);
                return;
            }
        }

        float yOffset = Mathf.Sin(Time.time * floatFrequency + floatPhase) * floatAmplitude;
        transform.position = new Vector3(floatBasePosition.x, floatBasePosition.y + yOffset, floatBasePosition.z);
    }

    private void TryCachePlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (item == null)
        {
            Destroy(gameObject);
            return;
        }

        if (pickupAmount <= 0)
        {
            return;
        }

        bool added = InventoryManager.Instance.AddItem(pickupAmount, item);
        if (added)
        {
            Destroy(gameObject);
        }
    }
}
