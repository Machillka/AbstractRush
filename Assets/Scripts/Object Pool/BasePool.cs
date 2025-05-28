using UnityEngine;
using UnityEngine.Pool;

public class BasePool<T> : MonoBehaviour
{
    public GameObject objPrefab;
    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(objPrefab, transform),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );

        PreFillPoll();
    }

    /// <summary>
    /// 初始化对象池
    /// </summary>
    /// <param name="count"></param>
    private void PreFillPoll(int count = 10)
    {
        var preFillArray = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            preFillArray[i] = _pool.Get();
        }

        foreach (var obj in preFillArray)
        {
            _pool.Release(obj);
        }
    }

    // WORKFLOW: GetObj -> 实现特定效果 -> ReleaseObj

    public GameObject GetObjectFromPool()
    {
        return _pool.Get();
    }

    public void ReleaseObjectToPool(GameObject obj)
    {
        _pool.Release(obj);
    }
}
