namespace _Scripts.CoreSystem.CoreComponents
{
    public class Death : CoreComponent
    {
        /*[SerializeField] private GameObject[] deathParticles;

        private ParticleManager ParticleManager =>
            particleManager ? particleManager : core.GetCoreComponent(ref particleManager);
    
        private ParticleManager particleManager;*/
        
        private Stats Stats => _stats ? _stats : Core.GetCoreComponent(ref _stats);
        private Stats _stats;
    
        public void Die()
        {
            /*foreach (var particle in deathParticles)
            {
                ParticleManager.StartParticles(particle);
            }*/
        
            Core.transform.parent.gameObject.SetActive(false);
        }

        private void OnEnable() => Stats.Health.OnCurrentValueZero += Die;
        private void OnDisable() => Stats.Health.OnCurrentValueZero -= Die;
    }
}