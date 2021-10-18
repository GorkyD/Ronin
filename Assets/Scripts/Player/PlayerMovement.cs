using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 4.0f;
    [SerializeField] float jumpForce = 7.5f;

    private Animator animator;
    private Rigidbody2D body2d;
    private Sensor_Player groundSensor;
    private bool grounded = false;
    private bool combatIdle = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Player>();
    }

    private void Update()
    {
        if (!grounded && groundSensor.State()) 
        {
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }
        
        if(grounded && !groundSensor.State()) 
        {
            grounded = false;
            animator.SetBool("Grounded", grounded);
        }
        
        float inputX = Input.GetAxis("Horizontal");

        if (inputX > 0)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else if (inputX < 0)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        
        body2d.velocity = new Vector2(inputX * speed, body2d.velocity.y);
        
        animator.SetFloat("AirSpeed", body2d.velocity.y);
        
        if (Input.GetKeyDown(KeyCode.W) && grounded) 
        {
            animator.SetTrigger("Jump");
            grounded = false;
            animator.SetBool("Grounded", grounded);
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }
        
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            animator.SetInteger("AnimState", 2);
        }
        
        else if (combatIdle)
        {
            animator.SetInteger("AnimState", 1);
        }
        else
        {
            animator.SetInteger("AnimState", 0);
        }
    }
}
