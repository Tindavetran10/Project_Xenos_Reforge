using UnityEngine;

namespace Script.SaveSystem
{
    public class Checkpoint : MonoBehaviour
    {
        private Animator _animator;
        public string id;
        public bool activationStatus;

        
        [ContextMenu("Generate checkpoint ID")]
        private void GenerateID() => id = System.Guid.NewGuid().ToString();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Player.PlayerStateMachine.Player>() != null) 
                ActivateCheckPoint();
        }

        public void ActivateCheckPoint() => activationStatus = true;
    }
}
