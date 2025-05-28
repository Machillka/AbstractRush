using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// [CreateAssetMenu(fileName = "BaseEventSO", menuName = "Event/BaseEventSO")]
public class BaseEventSO<T> : ScriptableObject
{
    [TextArea]
    public string description;

    public UnityAction<T> OnEventRaisedActions;                        // event 发生时的委托调用

    /// <summary>
    /// 执行事件
    /// </summary>
    /// <param name="value">执行此类型的事件所需要的参数</param>
    /// <param name="sender">告诉监听者是谁</param>
    public void OnEventRaised(T value, object sender)
    {
        OnEventRaisedActions?.Invoke(value);
    }
}
