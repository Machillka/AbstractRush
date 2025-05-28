using UnityEngine;
using UnityEngine.Events;

public class VoidListener : MonoBehaviour
{
    public VoidEventSO listenedEvent;                        // 定义监听的事件
    public UnityEvent response;                              // 定义响应方法

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

    private void OnEventRaised()
    {
        response.Invoke();
    }
}
