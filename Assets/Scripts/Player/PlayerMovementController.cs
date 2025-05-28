using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float acceleration = 10f;                        // 加速度
    public float deceleration = 10f;                        // 减速度 (类似模拟摩擦力)
    public float gravityScale = 1f;

    [Header("Physics Settings")]
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;


    [Header("Components")]
    private Rigidbody2D _rb;
    [SerializeField] private Transform groundCheckPoint;

    [Header("State Settings")]
    private bool _isGrounded = false;
    private bool _isMoving = false;
    private bool _canJump = false;

    [Header("Input Settings")]
    private float _moveInput;                               // 接受 InputManager 输入 
    private bool _isJumpPressed;                            // 接受 InputManager 跳跃按键输入

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _moveInput = InputManager.Instance.MovementInput;
        _isJumpPressed = InputManager.Instance.JumpPressed;
    }

    private void FixedUpdate()
    {
        // WORKFLOW: 物理检测 -> 是否可以进行操作 -> 执行操作
        PhysicsCheck();                                     // 运动前进行物理检测

        // 操作检测
        JumpCheck();

        MoveExecute();
        JumpExecute();
    }

    /// <summary>
    /// 物理检测
    /// 检测是否在地面
    /// </summary>
    private void PhysicsCheck()
    {
        // 是否在地面
        _isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }

    /// <summary>
    /// 进行是否可以执行跳跃操作的检测
    /// </summary>
    private void JumpCheck()
    {
        // TODO: 实现土狼跳
        _canJump = _isGrounded && _isJumpPressed;
    }

    private void JumpExecute()
    {
        if (_canJump)
        {
            // 直接给垂直速度
            _rb.linearVelocityY = jumpForce / gravityScale;
        }
    }

    // TODO: 人物发射
    private void LaunchExecute()
    {

    }

    /// <summary>
    /// 实现人物移动
    /// </summary>
    private void MoveExecute()
    {
        float targetSpeed = _moveInput * moveSpeed;
        float deltaSpeed = targetSpeed - _rb.linearVelocityX;
        float accelerationRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;
        float movemntSpeed = deltaSpeed * accelerationRate * Time.fixedDeltaTime;

        // 计算新速度; Y 轴保持不变
        // _rb.linearVelocity = new Vector2(_rb.linearVelocityX + movemntSpeed, _rb.linearVelocityY);
        _rb.linearVelocityX += movemntSpeed;

        // TODO: 人物朝向
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            if (!_isGrounded)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}
