using Script.StatSystem;

namespace Script.Player.PlayerStats
{
    public class PlayerStats : CharacterStats
    {
        private PlayerStateMachine.Player _player;
        
        protected override void Start()
        {
            base.Start();
            _player = GetComponentInParent<PlayerStateMachine.Player>();
        }

        protected override void TakeDamage(int damageAmount)
        {
            base.TakeDamage(damageAmount);
            if(IsInvincible)
                return;
            
            _player.DamageImpact();
        }

        protected override void Die()
        {
            base.Die();
            _player.Die();
        }
    }
}