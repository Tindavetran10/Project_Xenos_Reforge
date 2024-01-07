namespace Script.CoreSystem.CoreComponents
{
    public class Death : CoreComponent
    {
        /*[SerializeField] private GameObject[] deathParticles;

        private ParticleManager ParticleManager =>
            particleManager ? particleManager : core.GetCoreComponent(ref particleManager);
    
        private ParticleManager particleManager;*/
        
        private CharacterStats CharacterStats => _characterStats ? _characterStats : Core.GetCoreComponent(ref _characterStats);
        private CharacterStats _characterStats;

        private void Die()
        {
            /*foreach (var particle in deathParticles)
            {
                ParticleManager.StartParticles(particle);
            }*/
        
            Core.transform.parent.gameObject.SetActive(false);
        }

        private void OnEnable() => CharacterStats.Health.OnCurrentValueZero += Die;
        private void OnDisable() => CharacterStats.Health.OnCurrentValueZero -= Die;
    }
}