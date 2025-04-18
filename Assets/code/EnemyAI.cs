using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection & Movement")]
    public float detectRange = 5f; // Phạm vi phát hiện Player
    public float moveSpeed = 3f;   // Tốc độ di chuyển của quái
    public float attackRange = 1.5f; // Phạm vi tấn công Player

    [Header("Combat")]
    public int damage = 10; // Sát thương quái gây ra
    public float attackCooldown = 1f; // Thời gian hồi chiêu tấn công

    private Transform player; // Vị trí của Player
    private Rigidbody2D rb;   // Thành phần vật lý của quái
    private Animator anim;    // Điều khiển hoạt ảnh
    private bool isAttacking = false; // Kiểm soát trạng thái tấn công
    private float lastAttackTime = 0f; // Thời gian tấn công gần nhất
    private bool facingRight = true; // Theo dõi hướng của quái

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogWarning("Player không tìm thấy trong Scene. Đảm bảo Player có tag 'Player'.");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectRange) // Phát hiện Player trong phạm vi
        {
            if (distance > attackRange) // Theo đuổi Player
            {
                ChasePlayer();
            }
            else if (!isAttacking) // Tấn công Player
            {
                AttackPlayer();
            }
        }
        else // Player ngoài phạm vi phát hiện
        {
            StopMovement();
        }
    }

    void ChasePlayer()
    {
        anim.SetBool("isRunning", true); // Kích hoạt hoạt ảnh chạy
        Vector2 direction = (player.position - transform.position).normalized; // Tính hướng tới Player
        rb.linearVelocity = direction * moveSpeed; // Di chuyển quái tới Player

        // Lật hướng quái nếu cần
        Flip(direction.x);
    }

    void AttackPlayer()
    {
        StopMovement(); // Dừng di chuyển khi tấn công

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            anim.SetTrigger("isAttacking"); // Bật hoạt ảnh tấn công
            lastAttackTime = Time.time; // Cập nhật thời gian tấn công

            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage); // Gây sát thương cho Player
                Debug.Log($"Quái tấn công! Player mất {damage} máu.");
            }
        }
    }

    void StopMovement()
    {
        rb.linearVelocity = Vector2.zero; // Dừng mọi chuyển động
        anim.SetBool("isRunning", false); // Tắt hoạt ảnh chạy
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    // Hàm Flip để quái quay qua/quay lại
    void Flip(float moveDirectionX)
    {
        if (moveDirectionX > 0 && !facingRight)
        {
            facingRight = true;
            transform.localScale = new Vector3(1, 1, 1); // Đặt hướng quái sang phải
        }
        else if (moveDirectionX < 0 && facingRight)
        {
            facingRight = false;
            transform.localScale = new Vector3(-1, 1, 1); // Đặt hướng quái sang trái
        }
    }
}