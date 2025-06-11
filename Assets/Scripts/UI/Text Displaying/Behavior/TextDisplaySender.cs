using System.Collections.Generic;
using UnityEngine;

namespace TextDisplaying
{
    /// <summary>
    /// 发送者, 挂载在可以发送的物体上
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class TextDisplaySender : MonoBehaviour
    {
        public List<TextDisplayPiece> texts;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 在区域内触发
            if (collision.CompareTag("Player"))
            {
                Debug.Log("Start Displaying");
                SimpleEventHandler.CallTextDisplayEvent(texts);

                // 发送事件之后直接销毁, 防止重复触发
                Destroy(gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            Collider2D coll = GetComponent<Collider2D>();
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(coll.bounds.center, coll.bounds.size);
        }
    }
}


