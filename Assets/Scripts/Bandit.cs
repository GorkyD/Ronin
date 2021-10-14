using UnityEngine;
public class Bandit : MonoBehaviour 

{
    private Animator animator;
    private bool     isDead = false;
    
    private void Start () 
    {
        animator = GetComponent<Animator>();
    }

	private void Update () 
    {
 
        if (Input.GetKeyDown("e")) 
        {
            if (!isDead)
            {
                animator.SetTrigger("Death");
            }
            else
            {
                animator.SetTrigger("Recover");
            }

            isDead = !isDead;
        }
    }
}
