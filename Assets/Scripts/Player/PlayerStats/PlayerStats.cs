using StatSystem;

namespace Player.PlayerStats
{
    public class PlayerStats : CharacterStats
    {
        private global::Player.PlayerStateMachine.Player _player;
        
        protected override void Start()
        {
            base.Start();
            _player = GetComponentInParent<global::Player.PlayerStateMachine.Player>();
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