using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _speed = 4.0f;
    [SerializeField] float _jumpForce = 7.5f;

    private Animator _animator;
    private Rigidbody2D _body2d;
    private Sensor_Player _groundSensor;
    private bool _grounded = false;
    private bool _combatIdle = false;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _body2d = GetComponent<Rigidbody2D>();
        _groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Player>();
    }

    private void Update()
    {
        if (!_grounded && _groundSensor.State()) 
        {
            _grounded = true;
            _animator.SetBool("Grounded", _grounded);
        }
        
        if(_grounded && !_groundSensor.State()) 
        {
            _grounded = false;
            _animator.SetBool("Grounded", _grounded);
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
        
        _body2d.velocity = new Vector2(inputX * _speed, _body2d.velocity.y);
        
        _animator.SetFloat("AirSpeed", _body2d.velocity.y);
        
        if (Input.GetKeyDown("space") && _grounded) 
        {
            _animator.SetTrigger("Jump");
            _grounded = false;
            _animator.SetBool("Grounded", _grounded);
            _body2d.velocity = new Vector2(_body2d.velocity.x, _jumpForce);
            _groundSensor.Disable(0.2f);
        }
        
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            _animator.SetInteger("AnimState", 2);
        }
        
        else if (_combatIdle)
        {
            _animator.SetInteger("AnimState", 1);
        }
        else
        {
            _animator.SetInteger("AnimState", 0);
        }
        
        //BattleMode
        if (Input.GetKeyDown("f"))
        {
            _combatIdle = !_combatIdle;
        }
    }
}
