using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

namespace TextDisplaying
{
    public class TextDisplayController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI maintext;
        [SerializeField] private GameObject panel;

        private List<TextDisplayPiece> _textLists;

        private void OnEnable()
        {
            SimpleEventHandler.TextDisplayEvent += OnTextPlay;
        }

        private void OnDisable()
        {
            SimpleEventHandler.TextDisplayEvent -= OnTextPlay;
        }

        /// <summary>
        /// 开始播放文字动画
        /// </summary>
        private void OnTextPlay(List<TextDisplayPiece> textLists)
        {
            _textLists = textLists;
            StartCoroutine(DisplayTexts());
        }

        /// <summary>
        /// 为单句话进行动画的放
        /// </summary>
        /// <param name="text">原始文本</param>
        /// <param name="duration"></param>
        /// <returns></returns>
        IEnumerator DisplayTexts()
        {
            // TODO: 看看是否需要添加一些文本特效之类的
            // TODO: 或许全部手动排版, 实现比较有张力的文字排版也挺好的, 然后 controller 只需要控制打开哪一个 panel 就好了

            panel.SetActive(true);
            foreach (var singleText in _textLists)
            {
                string originalTexts = singleText.originalTexts;
                float durationTime = singleText.durationTime;

                string textToDisplay = SliceText(originalTexts);
                maintext.text = textToDisplay;
                yield return new WaitForSeconds(durationTime);
                maintext.text = "";
            }
            panel.SetActive(false);
        }

        /// <summary>
        /// 传入原始文本, 转化成竖行排列的文本
        /// </summary>
        /// <param name="originText"></param>
        /// <returns>切好换行符的文本</returns>
        private string SliceText(string originText)
        {
            string slicedText = "";
            foreach (var letter in originText)
            {
                slicedText += letter + "\n";
            }
            return slicedText;
        }
    }
}


