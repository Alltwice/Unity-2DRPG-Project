using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rg;
    private Animator am;
    private float horizontalPut;
    private float verticalPut;
    private int faceDirection=1;
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        am = GetComponent<Animator>();
    }
    void Update()
    {
        horizontalPut = Input.GetAxis("Horizontal");
        if(horizontalPut>0&&transform.localScale.x==-1||horizontalPut<0&&transform.localScale.x==1)
        {
            faceDirection *= -1;
            transform.localScale = new Vector3(faceDirection, transform.localScale.y, transform.localScale.z);
        }
        am.SetFloat("horizontal", Mathf.Abs(horizontalPut));
        verticalPut = Input.GetAxis("Vertical");
        am.SetFloat("vertical",Mathf.Abs(verticalPut));
    }
    void FixedUpdate()
    {
        rg.linearVelocity = new Vector2(horizontalPut * speed, verticalPut * speed);
    }
}
