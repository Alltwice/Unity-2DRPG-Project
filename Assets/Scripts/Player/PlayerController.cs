using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rg;
    private Animator am;
    private float horizontalPut;
    private float verticalPut;
    private int faceDirection=1;
    private bool isBacking=false;
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        am = GetComponent<Animator>();
    }
    void Update()
    {
        if(isBacking==false)
        {
            horizontalPut = Input.GetAxis("Horizontal");
            if (horizontalPut > 0 && transform.localScale.x == -1 || horizontalPut < 0 && transform.localScale.x == 1)
            {
                faceDirection *= -1;
                transform.localScale = new Vector3(faceDirection, transform.localScale.y, transform.localScale.z);
            }
            am.SetFloat("horizontal", Mathf.Abs(horizontalPut));
            verticalPut = Input.GetAxis("Vertical");
            am.SetFloat("vertical", Mathf.Abs(verticalPut));
        }
    }
    void FixedUpdate()
    {
        if (isBacking == false)
        {
            rg.linearVelocity = new Vector2(horizontalPut * speed, verticalPut * speed);
        }
    }
    public void AttackBack(GameObject enemy, float backForce,float stunTime)
    {
        isBacking = true;
        Vector2 backDirection = (transform.position - enemy.transform.position).normalized;
        rg.linearVelocity = backDirection*backForce;
        StartCoroutine(AttackBackCounter(stunTime));
    }
    IEnumerator AttackBackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        isBacking = false;
    }
}
