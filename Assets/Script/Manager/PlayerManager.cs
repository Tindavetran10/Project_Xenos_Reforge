using UnityEngine;

namespace Script.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;
        public Player.PlayerStateMachine.Player player;

        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);
            else Instance = this;
        }
    }
}