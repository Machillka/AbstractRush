using System.Collections;
using UnityEngine;

public class TextEffectPool : BasePool<TextEffectPool>
{
    public void OnPlayerLaunch()
    {
        var obj = GetObjectFromPool();
        obj.transform.position = transform.position;

        // 延迟释放
        StartCoroutine(DelayRelease(obj, 0.5f));
    }

    private IEnumerator DelayRelease(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReleaseObjectToPool(obj);
    }
}
