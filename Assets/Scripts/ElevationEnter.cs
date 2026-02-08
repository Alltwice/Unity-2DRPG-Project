using UnityEngine;

public class ElevationEnter : MonoBehaviour
{
    public Collider2D[] mountainCollider;
    public Collider2D[] boundCollider;
    private SpriteRenderer sr;
    //碰到触发器就判定上山，关闭山体碰撞，启用山体边界，调高玩家图层
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            foreach (Collider2D mountain in mountainCollider)
            {
                mountain.enabled = false;
            }
            foreach (Collider2D bound in boundCollider)
            {
                bound.enabled = true;
            }
            sr = collision.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 15;
        }
    }
}
