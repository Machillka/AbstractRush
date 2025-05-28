using System.Collections;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Camere")]
    private Camera mainCamera;
    private float _originOrthographicSize;
    private float zoomInOrthSize;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 6f;
    public float acceleration = 10f;                        // 加速度
    public float deceleration = 10f;                        // 减速度 (类似模拟摩擦力)
    public float gravityScale = 1f;

    [Header("Launch Settings")]
    public float launchForce = 10f;
    public float launchCoolDownDuration = 0.5f;
    private int _maxLaunchCount = 1;
    private Vector2 _launchDirection;
    [SerializeField]private GameObject _shootArrow;

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
    private Vector3 _mouseOnWorldPosition;
    private Vector3 _mouseOnScreenPosition;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _launchCounter = 0;
        mainCamera = Camera.main;

        if (mainCamera != null)
        {
            _originOrthographicSize = mainCamera.orthographicSize;
            zoomInOrthSize = _originOrthographicSize * Settings.cameraScale;
        }
    }

    private void OnEnable()
    {
        SimpleEventHandler.PlayerLaunchEvent += OnPlayerLaunch;
        // SimpleEventHandler.OnLongPressingEvent += OnLongPressing;
    }

    private void OnDisable()
    {
        SimpleEventHandler.PlayerLaunchEvent -= OnPlayerLaunch;
        // SimpleEventHandler.OnLongPressingEvent -= OnLongPressing;
    }

    private void Update()
    {
        _moveInput = InputManager.Instance.MovementInput;
        _isJumpPressed = InputManager.Instance.IsJumpPressed;
        _isLongPressing = InputManager.Instance.IsMouseLongPressing;
        _mouseOnScreenPosition = InputManager.Instance.MousePositionOnScreen;

        if (_isLongPressing)
        {
            DrawShootArrow();
        }
        else
        {
            _shootArrow.gameObject.SetActive(false);
        }

        // Debug.Log($"IS LP : {_isLongPressing}");
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

    /// <summary>
    /// 定义鼠标长按的时候需要干的事情
    /// 实现时间缓慢流动
    /// 实现镜头拉近
    /// 绘制箭头
    /// </summary>
    // private void OnLongPressing()
    // {
    //     Debug.Log("On LP");
    //     StartCoroutine(CameraZoneIn(0.45f));
    // }

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

        _mouseOnWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(_mouseOnScreenPosition.x, _mouseOnScreenPosition.y, -mainCamera.transform.position.z));

        _launchDirection = (_mouseOnWorldPosition - transform.position).normalized;
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


    /// <summary>
    /// 绘制瞄准发射方向的箭头
    /// </summary>
    private void DrawShootArrow()
    {
        _mouseOnWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(_mouseOnScreenPosition.x, _mouseOnScreenPosition.y, -mainCamera.transform.position.z));

        Vector2 dir = _mouseOnWorldPosition - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        _shootArrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _shootArrow.gameObject.SetActive(true);
    }

    /// <summary>
    /// 实现长按逻辑
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    // private IEnumerator CameraZoneIn(float duration)
    // {
    //     // TODO: 考虑是否添加镜头缩放 ——— 发现实际效果好像不是很妙
    //     // Time.timeScale = Settings.timeScale;
    //     while (InputManager.Instance.IsMouseLongPressing)
    //     {
    //         Debug.Log("In LP");
    //         DrawShootArrow();
    //         yield return null;
    //     }

    //     // 结束长按
    //     _shootArrow.gameObject.SetActive(false);

    //     // Time.timeScale = 1f;
    // }

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
