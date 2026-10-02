using UnityEngine;

// 1. Player: движение по X/Y (WASD / стрелки), скорость в Inspector, без вращения от столкновений.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] float moveSpeed = 5f;
    [Tooltip("Половина размера арены. 0 = без ограничений.")]
    [SerializeField] float arenaHalfSize = 30f;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    Rigidbody2D rb;
    Vector2 input;

    void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true; // запрет физического вращения
    }

    void Update()
    {
        input = GameInput.Move();
        if (input.sqrMagnitude > 1f) input.Normalize();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = input * moveSpeed;

        if (arenaHalfSize > 0f)
        {
            Vector2 p = rb.position;
            Vector2 clamped = new Vector2(Mathf.Clamp(p.x, -arenaHalfSize, arenaHalfSize),
                                          Mathf.Clamp(p.y, -arenaHalfSize, arenaHalfSize));
            if (clamped != p) rb.position = clamped;
        }
    }

    void OnDisable()
    {
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }
}
