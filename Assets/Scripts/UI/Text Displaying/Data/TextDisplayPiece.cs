using UnityEngine;
using UnityEngine.AddressableAssets;
namespace TextDisplaying
{
    [System.Serializable]
    public class TextDisplayPiece
    {
        // 原始文本
        public string originalTexts;
        // 持续时间 默认 0.5f
        public float durationTime = 0.5f;
    }
}