using UnityEngine;
public class Bandit : MonoBehaviour 

{
    private Animator _animator;
    private bool     _isDead = false;
    
    private void Start () 
    {
        _animator = GetComponent<Animator>();
    }

	private void Update () 
    {
 
        if (Input.GetKeyDown("e")) 
        {
            if (!_isDead)
            {
                _animator.SetTrigger("Death");
            }
            else
            {
                _animator.SetTrigger("Recover");
            }

            _isDead = !_isDead;
        }
    }
}
