using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 无参数的事件
/// </summary>
[CreateAssetMenu(fileName = "VoidEventSO", menuName = "Event/VoidEventSO")]
public class VoidEventSO : ScriptableObject
{
    [TextArea]
    public string description;

    public UnityAction OnEventRaisedActions;

    public void OnEventRaised(object sender)
    {
        OnEventRaisedActions?.Invoke();
    }
}
