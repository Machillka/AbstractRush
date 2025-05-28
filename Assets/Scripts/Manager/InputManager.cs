using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 控制游戏输入
/// 游戏输入相关设置
/// </summary>
public class InputManager : Singleton<InputManager>
{
    private InputSystemActions _inputController;
    private bool _isDisabledInput;
    private Vector3 playerPosition => GameObject.FindWithTag("Player").transform.position;

    public float MovementInput => _inputController.Player.Move.ReadValue<float>();
    public bool IsJumpPressed => _inputController.Player.Jump.IsPressed();

    public bool IsMouseLongPressing => _isMouseLongPressing;
    public Vector2 MousePositionOnScreen => Mouse.current.position.ReadValue();

    private bool _isMouseLongPressing = false;
    private bool _isAfterMouseLongPressedRelease = false;

    protected override void Awake()
    {
        base.Awake();
        _inputController = new InputSystemActions();
    }

    private void OnEnable()
    {
        _inputController.Enable();

        // 鼠标点击
        _inputController.Player.LookAt.started += ctx =>
        {
            // 清除所有长按相关状态
            _isMouseLongPressing = false;
            _isAfterMouseLongPressedRelease = false;
        };

        // 进入长按状态
        _inputController.Player.LookAt.performed += ctx =>
        {
            if (!_isDisabledInput)
            {
                _isMouseLongPressing = true;
                _isAfterMouseLongPressedRelease = false;
                SimpleEventHandler.CallLongPressingEvent();
                Debug.Log("Call!");
            }
        };

        _inputController.Player.LookAt.canceled += ctx =>
        {
            // 长按之后释放鼠标
            if (!_isDisabledInput && _isMouseLongPressing)
            {
                SimpleEventHandler.CallPlayerLaunchEvent();
            }

            // 取消长按状态
            _isMouseLongPressing = false;
        };
    }

    private void OnDisable()
    {
        _inputController.Disable();
    }

    public void DisableInput()
    {
        _isDisabledInput = true;
        _inputController.Disable();
    }

    public void EnableInput()
    {
        _isDisabledInput = false;
        _inputController.Enable();
    }
}
