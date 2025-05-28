using UnityEngine;
using UnityEngine.Events;

public class SimpleEventHandler
{
    // 定义简单事件 (SO 文件维护比较麻烦)

    public static UnityAction PlayerLaunchEvent;
    public static void CallPlayerLaunchEvent()
    {
        PlayerLaunchEvent?.Invoke();
    }
}
