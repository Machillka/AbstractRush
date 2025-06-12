using UnityEngine;
using UnityEngine.Events;
using TextDisplaying;
using System;
using System.Collections.Generic;

public class SimpleEventHandler
{
    // 定义简单事件 (SO 文件维护比较麻烦)

    #region Player Movement
    public static UnityAction PlayerLaunchEvent;
    public static void CallPlayerLaunchEvent()
    {
        PlayerLaunchEvent?.Invoke();
    }
    #endregion

    #region Effects

    public static event Action<List<TextDisplayPiece>> TextDisplayEvent;
    public static void CallTextDisplayEvent(List<TextDisplayPiece> texts)
    {
        TextDisplayEvent?.Invoke(texts);
    }
    #endregion

    #region Doors

    public static event Action<Vector3> TransmitInSceneEvent;
    public static void CallTranmitInSceneEvent(Vector3 targetPosition)
    {
        TransmitInSceneEvent?.Invoke(targetPosition);
    }

    #endregion
}
