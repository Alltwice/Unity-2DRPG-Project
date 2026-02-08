using UnityEngine;

public class ElevationExit : MonoBehaviour
{
    public Collider2D[] mountainCollider;
    public Collider2D[] boundCollider;
    private SpriteRenderer sr;
    //碰到触发器就判定下山，打开山体碰撞，关闭山体边界，调低玩家图层
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            foreach (Collider2D mountain in mountainCollider)
            {
                mountain.enabled = true;
            }
            foreach (Collider2D bound in boundCollider)
            {
                bound.enabled = false;
            }
            sr = collision.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 5;
        }
    }
}
