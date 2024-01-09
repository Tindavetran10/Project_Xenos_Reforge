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

        public override void TakeDamage(int damageAmount)
        {
            base.TakeDamage(damageAmount);
            _player.DamageEffect();
        }

        protected override void Die()
        {
            base.Die();
            _player.Die();
        }
    }
}