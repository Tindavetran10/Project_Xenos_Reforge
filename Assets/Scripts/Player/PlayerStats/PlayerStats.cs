using InventorySystem_and_Items;
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
            
            GetComponentInParent<PlayerItemDrop>()?.GenerateDrop();
        }

        protected override void DecreaseHealthBy(int damageAmount)
        {
            base.DecreaseHealthBy(damageAmount);
            
            if (IsDead) return;
            
            var currentArmor = InventoryManager.instance.GetEquipment(EnumList.EquipmentType.Armor);

            if (currentArmor != null) 
                currentArmor.ExecuteItemEffect(_player.playerTransform);
        }
    }
}