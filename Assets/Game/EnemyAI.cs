using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Параметры")]
    public float speed = 2f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public float damage = 10f;
    public int health = 3;
    private static int deathCounter = 0;

    [Header("Анимации")]
    public Animator animator;
    public int attackVariants = 12;
    public int deathVariants = 4;

    private GameObject player;
    private float lastAttackTime;

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        if (!animator) animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (health <= 0 || player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        // всегда бежит, если жив
        animator.SetBool("IsRunning", true);

        if (distance > attackRange)
        {
            // поворот и движение к игроку
            Vector3 dir = (player.transform.position - transform.position).normalized;
            dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * 5f);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            // остановка и атака
            animator.SetBool("IsRunning", false);

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                int atk = Random.Range(0, attackVariants);
                animator.SetInteger("AttackIndex", atk);
                animator.SetTrigger("Attack");

                if (player.TryGetComponent<PlayerHealth>(out var hp))
                    hp.TakeDamage(damage);

                lastAttackTime = Time.time;
            }
        }
    }

    public void TakeHit()
    {
        if (health <= 0) return;

        health--;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("IsRunning", false);
        FindObjectOfType<KillstreakManager>()?.OnEnemyKilled();
        FindObjectOfType<PlayerController_NewInput>()?.AddScore(1);


        int death = deathCounter;
        deathCounter = (deathCounter + 1) % deathVariants; // сброс после 3

        animator.SetInteger("DieIndex", death);
        animator.SetTrigger("Die");
        foreach (Transform child in transform)
        {
            ParticleSystem ps = child.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(ps.gameObject);
            }
        }
        Destroy(gameObject, 5f);
    }


}
