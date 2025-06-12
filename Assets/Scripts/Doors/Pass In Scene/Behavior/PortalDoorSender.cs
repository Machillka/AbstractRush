using UnityEngine;

namespace Doors
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class PortalDoorSender : MonoBehaviour
    {
        [field: SerializeField]
        public Vector3 TargetPosition { get; set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                SimpleEventHandler.CallTranmitInSceneEvent(TargetPosition);
            }
        }
    }
}


