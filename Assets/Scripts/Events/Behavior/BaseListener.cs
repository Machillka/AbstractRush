using UnityEngine;
using UnityEngine.Events;

public class BaseListener<T> : MonoBehaviour
{
    public BaseEventSO<T> listenedEvent;                        // 定义监听的事件
    public UnityEvent<T> response;                              // 定义响应方法

    private void OnEnable()
    {
        if (listenedEvent != null)
        {
            listenedEvent.OnEventRaisedActions += OnEventRaised;
        }
    }

    private void OnDisable()
    {
        if (listenedEvent != null)
        {
            listenedEvent.OnEventRaisedActions -= OnEventRaised;
        }
    }

    private void OnEventRaised(T value)
    {
        response.Invoke(value);
    }
}
