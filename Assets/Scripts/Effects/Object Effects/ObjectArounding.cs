using System.Collections;
using UnityEngine;

/// <summary>
/// 使得有此组件的物体可以在一定范围内慢速随机游走
/// </summary>
public class ObjectArounding : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveRadius;                // 每次可以走的最大范围
    public float maxDuration;               // 完成每一步的最长时间
    public float rotateRange;

    private Vector3 _startPosition, _targetPosition;
    private Quaternion _startRotation, _targetRotation;
    private float _timeCounter;

    private void Start()
    {
        StartCoroutine(Movement());
    }

    // private IEnumerator Rotation()
    // {
    //     while (true)
    //     {

    //     }
    // }

    private IEnumerator Movement()
    {
        while (true)
        {
            // 在球形范围内生成随机点
            Vector3 positionOffset = Random.insideUnitSphere * moveRadius;
            _targetPosition = transform.position + positionOffset;

            Vector3 rotateOffset = new Vector3(
                Random.Range(-rotateRange, rotateRange),
                Random.Range(-rotateRange, rotateRange),
                Random.Range(-rotateRange, rotateRange)
            );

            _targetRotation = Quaternion.Euler(rotateOffset) * transform.rotation;

            float duration = Random.Range(Settings.minAroundingTime, maxDuration);

            _timeCounter = 0f;
            _startPosition = transform.position;
            _startRotation = transform.rotation;
            while (_timeCounter < duration)
            {
                _timeCounter += Time.deltaTime;
                transform.position = Vector3.Lerp(_startPosition, _targetPosition, _timeCounter / duration);
                transform.rotation = Quaternion.Slerp(_startRotation, _targetRotation, _timeCounter / duration);
                yield return null;
            }
        }
    }
}
