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

    public float MovementInput => _inputController.Player.Move.ReadValue<float>();
    public bool JumpPressed => _inputController.Player.Jump.IsPressed();

    protected override void Awake()
    {
        base.Awake();
        _inputController = new InputSystemActions();
    }

    private void OnEnable()
    {
        _inputController.Enable();
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
