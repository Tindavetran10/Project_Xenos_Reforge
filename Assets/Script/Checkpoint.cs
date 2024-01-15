using UnityEngine;
using UnityEngine.Serialization;

namespace Script
{
    public class Checkpoint : MonoBehaviour
    {
        private Animator _animator;
        [FormerlySerializedAs("checkPointID")] public string id;
        [FormerlySerializedAs("activated")] public bool activationStatus;

        
        [ContextMenu("Generate checkpoint ID")]
        private void GenerateID()
        {
            id = System.Guid.NewGuid().ToString();
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Player.PlayerStateMachine.Player>() != null)
            {
                ActivateCheckPoint();
            }
        }

        public void ActivateCheckPoint()
        {
            activationStatus = true;
        }
    }
}
