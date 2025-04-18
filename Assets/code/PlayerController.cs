using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;

    [Header("Attack")]
    public float attackCooldown = 0.5f;
    private bool isAttacking = false;
    private int comboStep = 0;
    private float lastAttackTime = 0f;

    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;

    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthBar; // UI Health Bar
    public GameObject gameOverCanvas; // Game Over UI

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        // Initialize Health Bar
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        // Hide Game Over UI at start
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
    }

    void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement.normalized * speed;
    }

    void HandleMovement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (anim != null)
        {
            anim.SetBool("isMoving", movement.magnitude > 0);
        }

        if (movement.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (movement.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking && Time.time - lastAttackTime >= attackCooldown)
        {
            ComboAttack();
        }
    }

    void ComboAttack()
    {
        isAttacking = true;
        comboStep++;

        if (comboStep > 2)
            comboStep = 1;

        if (anim != null)
        {
            anim.SetTrigger("Attack" + comboStep);
        }

        lastAttackTime = Time.time;
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    void ResetAttack()
    {
        isAttacking = false;

        if (Time.time - lastAttackTime > attackCooldown)
        {
            comboStep = 0;
        }

        if (anim != null)
        {
            anim.SetTrigger("Idle");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (anim != null)
        {
            anim.SetTrigger("Hurt");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (anim != null)
        {
            anim.SetTrigger("Death");
        }

        GetComponent<PlayerController>().enabled = false;
        rb.linearVelocity = Vector2.zero;

        // Show Game Over UI
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        Time.timeScale = 0f; // Pause the game
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}