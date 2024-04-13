using StatSystem;

namespace Player.PlayerStats
{
    public class PlayerStats : CharacterStats
    {
        private Player.PlayerStateMachine.Player _player;
        
        protected override void Start()
        {
            base.Start();
            _player = GetComponentInParent<Player.PlayerStateMachine.Player>();
        }
        
        protected override void SetFlagDeath()
        {
            base.SetFlagDeath();
            _player.Die();
        }
    }
}