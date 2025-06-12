using System;
using UnityEngine;

public class PlayerTransmitController : MonoBehaviour
{
    private void OnEnable()
    {
        SimpleEventHandler.TransmitInSceneEvent += OnTransmitInScene;
    }

    private void OnDisable()
    {
        SimpleEventHandler.TransmitInSceneEvent -= OnTransmitInScene;
    }

    private void OnTransmitInScene(Vector3 targetPosition)
    {
        // TODO: 或许可以添加某动画特效
        gameObject.transform.position = targetPosition;
    }
}
