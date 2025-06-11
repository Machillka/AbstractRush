using UnityEngine;
using UnityEngine.Events;
using TextDisplaying;
using System;
using System.Collections.Generic;

public class SimpleEventHandler
{
    // 定义简单事件 (SO 文件维护比较麻烦)

    public static UnityAction PlayerLaunchEvent;
    public static void CallPlayerLaunchEvent()
    {
        PlayerLaunchEvent?.Invoke();
    }

    public static event Action<List<TextDisplayPiece>> TextDisplayEvent;
    public static void CallTextDisplayEvent(List<TextDisplayPiece> texts)
    {
        TextDisplayEvent?.Invoke(texts);
    }
    // public static UnityAction OnLongPressingEvent;
    // public static void CallLongPressingEvent()
    // {
    //     OnLongPressingEvent?.Invoke();
    // }
}
