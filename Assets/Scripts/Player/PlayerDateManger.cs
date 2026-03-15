using UnityEngine;

public class PlayerDateManger : MonoBehaviour
{
    public static PlayerDateManger instance { get; private set; }

    [Header("基础数据")]
    public float moveSpeed;
    public int damage;
    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
