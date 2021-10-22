using UnityEngine;

public class EnemyCombatSystem : MonoBehaviour
{
    #region SerializeField Variables

    [SerializeField] private float attackDistance;
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float timer;
    [SerializeField] private int attackDamage = 25;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;

    #endregion

    #region Private Variables

    private Animator anim;
    private float distance;
    private bool attackMode;
    private bool cooling;
    private float intTimer;

    #endregion

    #region Public Variables

    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject hotZone;
    public GameObject triggerArea;

    #endregion

    private void Start()
    {
        SelectTarget();
        intTimer = timer;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!attackMode)
        {
            Move();
        }

        if (!InsideOfLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            SelectTarget();
        }

        if (inRange)
        {
            EnemyLogic();
        }
    }


    private void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            StopAttack();
        }
        else if (distance <= attackDistance && cooling == false)
        {
            Attack();
        }

        if (cooling)
        {
            CoolDown();
            anim.SetBool("Attack", false);
        }
    }

    private void Move()
    {
        anim.SetBool("Walk", true);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void Attack()
    {

        timer = intTimer;
        attackMode = true;

        anim.SetBool("Walk", false);
        anim.SetBool("Attack", true);
    }

    private void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Attack", false);
    }

    private void TriggerCooling()
    {
        cooling = true;
    }

    private void AttackMechanic()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D player in hitEnemies)
        {
            player.GetComponent<Player>().TakeDamage(attackDamage);
        }
    }

    private void CoolDown()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    private bool InsideOfLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector3.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector3.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        Flip();
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180;
        }
        else
        {
            rotation.y = 0;
        }

        transform.eulerAngles = rotation;
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}


