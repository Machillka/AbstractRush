using System.Collections;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float acceleration = 10f;                        // 加速度
    public float deceleration = 10f;                        // 减速度 (类似模拟摩擦力)
    public float gravityScale = 1f;

    [Header("Launch Settings")]
    public float launchForce = 10f;
    public float launchCoolDownDuration = 0.5f;
    private int _maxLaunchCount = 1;
    private Vector2 _launchDirection;

    [Header("Physics Settings")]
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    [Header("Effects")]
    [SerializeField] private TextEffectPool launchEffectPool;

    [Header("Components")]
    private Rigidbody2D _rb;
    [SerializeField] private Transform groundCheckPoint;

    [Header("State Settings")]
    private bool _isGrounded = false;
    // private bool _isMoving = false;
    private bool _canJump = false;
    // private bool _isJumping = false;
    private bool _canLaunch = false;
    private bool _isLaunchingCoolDown = false;
    private int _launchCounter = 0;
    private bool _isLeftGrounding = false;                   // 是否通过跳跃和发射离开地面

    [Header("Input Settings")]
    private float _moveInput;                               // 接受 InputManager 输入
    private bool _isJumpPressed;                            // 接受 InputManager 跳跃按键输入
    private bool _isLaunchPressed;
    private bool _isLongPressing;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _launchCounter = 0;
    }

    private void OnEnable()
    {
        SimpleEventHandler.PlayerLaunchEvent += OnPlayerLaunch;
    }

    private void OnDisable()
    {
        SimpleEventHandler.PlayerLaunchEvent -= OnPlayerLaunch;
    }

    private void Update()
    {
        _moveInput = InputManager.Instance.MovementInput;
        _isJumpPressed = InputManager.Instance.IsJumpPressed;
        _isLongPressing = InputManager.Instance.IsMouseLongPressing;
    }

    private void FixedUpdate()
    {
        // WORKFLOW: 物理检测 -> 是否可以进行操作 -> 执行操作 -> ( 取消状态 )
        GroundCheck();                                     // 运动前进行物理检测
        ResetCounter();

        // 操作检测
        JumpCheck();
        LaunchCheck();

        MoveExecute();
        JumpExecute();
        LaunchExecute();
    }

    #region Events

    private void OnPlayerLaunch()
    {
        _isLaunchPressed = true;
    }


    #endregion


    #region Check

    private void ResetCounter()
    {
        // 跳跃后 回到地面
        if (_isLeftGrounding && _isGrounded)
        {
            _launchCounter = 0;
            _isLeftGrounding = false;
        }
    }

    /// <summary>
    /// 物理检测
    /// 检测是否在地面
    /// </summary>
    private void GroundCheck()
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
        // TODO: 实现检测跳跃
        _canJump = _isGrounded && _isJumpPressed;
    }

    /// <summary>
    ///
    /// </summary>
    private void LaunchCheck()
    {
        if (_isLaunchPressed && !_isLaunchingCoolDown && _launchCounter < _maxLaunchCount)
        {
            _canLaunch = true;
        }
        else
        {
            _canLaunch = false;
        }
        _isLaunchPressed = false;
    }

    #endregion

    private void JumpExecute()
    {
        if (!_canJump)
            return;
        // 直接给垂直速度
        _rb.linearVelocityY = jumpForce / gravityScale;
        _isLeftGrounding = true;
        _canJump = false;
    }

    /// <summary>
    /// 实现人物移动
    /// </summary>
    private void MoveExecute()
    {
        // 发射的一小段时间内取消移动
        if (_isLaunchingCoolDown)
            return;

        float targetSpeed = _moveInput * moveSpeed;
        float deltaSpeed = targetSpeed - _rb.linearVelocityX;
        float accelerationRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;
        float movemntSpeed = deltaSpeed * accelerationRate * Time.fixedDeltaTime;

        _rb.linearVelocityX += movemntSpeed;
    }

    private void LaunchExecute()
    {
        if (!_canLaunch)
            return;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePositionOnScreen);
        _launchDirection = (mouseWorldPosition - transform.position).normalized;
        _rb.linearVelocity = Vector2.zero;                                  // 重置速度
        _rb.AddForce(_launchDirection * launchForce, ForceMode2D.Impulse);

        _launchCounter++;
        _isLeftGrounding = true;
        // 释放特效
        launchEffectPool.OnPlayerLaunch();
        // 开始发射计算
        StartCoroutine(LaunchingCoolDown());
    }

    private IEnumerator LaunchingCoolDown()
    {
        _isLaunchingCoolDown = true;
        yield return new WaitForSeconds(launchCoolDownDuration);
        _isLaunchingCoolDown = false;
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
