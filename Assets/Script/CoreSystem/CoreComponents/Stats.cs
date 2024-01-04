using UnityEngine;

namespace _Scripts.CoreSystem.CoreComponents
{
    
    public class Stats : CoreComponent
    {
        // Create to stat base on the stat system
        [field: SerializeField] public StatsSystem.Stat Health { get; private set; }
        
        // Poise is a stat to decide when a character will be stunned 
        // after taking a certain amount of damage
        [field: SerializeField] public StatsSystem.Stat Poise { get; private set; }

        [SerializeField] private float poiseRecoveryRate;
        
        protected override void Awake()
        {
            base.Awake();
        
            Health.Init();
            Poise.Init();
        }

        private void Update()
        {
            if(Poise.CurrentValue.Equals(Poise.MaxValue))
                return;
            
            // Increase the poise stat after the character enter Stun state
            Poise.Increase(poiseRecoveryRate * Time.deltaTime);
        }
    }
}
