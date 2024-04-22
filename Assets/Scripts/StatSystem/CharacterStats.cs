using System;
using System.Collections;
using Entity;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace StatSystem
{
    public class CharacterStats : MonoBehaviour
    {
        private EntityFX _fx;

        #region Stat System
        [Header("Major stats")]
        public Stat strength; 
        public Stat agility; 
        public Stat intelligence; 
        public Stat vitality;
 
        [Header("Offensive stats")]
        public Stat damage;
        public Stat critChance;
        public Stat critPower;
        
        [Header("Defensive stats")]
        public Stat maxHealth;
        public Stat armor;
        public Stat evasion;
        public Stat magicResistance;
        
        [Header("Poise stats")]
        public Stat maxPoiseResistance;
        public Stat poiseResetTime;
        public Stat lastPoiseReset;

        [Header("Magic stats")] 
        public Stat fireDamage;
        public Stat iceDamage;
        [FormerlySerializedAs("lightningDamage")] public Stat lightingDamage;
        #endregion
        
        public bool isIgnited;
        public bool isFrozen;
        public bool isShocked;
        
        [SerializeField] private float ailmentsDuration = 4;
        private float _ignitedTimer;
        private float _frozenTimer;
        private float _shockedTimer;
        
        private float _igniteDamageCooldown;
        private float _igniteDamageTimer;
        private int _igniteDamage;
        
        [SerializeField] private GameObject shockStrikePrefab;
        private int _shockDamage;
        
        // Used to change the health bar value
        public Action OnHealthChanged;
        public event Action<CharacterStats> OnDeath; 
        
        public bool IsDead { get; private set; }
        public bool IsStunned { get; set; }
        public bool IsAttacked { get; set; }

        public bool IsInvincible { get; private set; }
        
        [SerializeField] public int currentHealth;
        [SerializeField] public int currentPoise;
  
        protected virtual void Start()
        {
            critPower.SetDefaultValue(150);
            currentHealth = GetMaxHealthValue();
            currentPoise = GetMaxPoiseValue();

            _fx = GetComponentInParent<EntityFX>();
        }

        protected void Update()
        {
            _ignitedTimer -= Time.deltaTime;
            _frozenTimer -= Time.deltaTime;
            _shockedTimer -= Time.deltaTime;
            
            _igniteDamageTimer -= Time.deltaTime;
            
            if (_ignitedTimer < 0)
                isIgnited = false;
            
            if (_frozenTimer < 0)
                isFrozen = false;
            
            if (_shockedTimer < 0)
                isShocked = false;
            
            if(isIgnited) ApplyIgniteDamage();
            
            ResetPoiseValue();
        }

        private void ResetPoiseValue()
        {
            if (Time.time >= lastPoiseReset.GetValue() + poiseResetTime.GetValue())
            {
                lastPoiseReset.SetDefaultValue((int) Time.time);
                currentPoise = GetMaxPoiseValue();
            }
        }

        public virtual void IncreaseStatBy(int modifier, float duration, Stat statToModify) => 
            StartCoroutine(StatModifierCountdown(modifier, duration, statToModify));

        private static IEnumerator StatModifierCountdown(int modifier, float duration, Stat statToModify)
        {
            statToModify.AddModifier(modifier);
            yield return new WaitForSeconds(duration);
            statToModify.RemoveModifier(modifier);
        }
        
        public void DoDamage(CharacterStats targetStats)
        {
            if(targetStats.IsInvincible) return;
            if(TargetCanAvoidAttack(targetStats)) return;
            
            var totalDamage = damage.GetValue() + strength.GetValue();

            if (CanCrit()) totalDamage = CalculateCriticalDamage(totalDamage);
            
            totalDamage = CheckTargetArmor(targetStats, totalDamage);
            targetStats.TakeDamage(totalDamage);
            
            DoMagicalDamage(targetStats);
        }
        
        public void DoMagicalDamage(CharacterStats targetStats)
        {
            var fireDamageValue = fireDamage.GetValue();
            var iceDamageValue = iceDamage.GetValue();
            var lightningDamageValue = lightingDamage.GetValue();
            
            var totalMagicalDamage = fireDamageValue + iceDamageValue + lightningDamageValue + intelligence.GetValue();
            
            totalMagicalDamage = CheckTargetResistance(targetStats, totalMagicalDamage);
            targetStats.TakeDamage(totalMagicalDamage);
            
            if (Mathf.Max(fireDamageValue, iceDamageValue, lightningDamageValue) <= 0)
                return;
            
            AttemptToApplyAliments(targetStats, fireDamageValue, iceDamageValue, lightningDamageValue);
        }

        private static void AttemptToApplyAliments(CharacterStats targetStats, int fireDamage, int iceDamage, int lightningDamage)
        {
            var canApplyIgnite = fireDamage > iceDamage && fireDamage > lightningDamage;
            var canApplyChill = iceDamage > fireDamage && iceDamage > lightningDamage;
            var canApplyShock = lightningDamage > fireDamage && lightningDamage > iceDamage;

            var attempts = 0;
            while (!canApplyIgnite && !canApplyChill && !canApplyShock && attempts < 5)
            {
                var randValue = Random.value;
                if (randValue < .3f && fireDamage > 0)
                {
                    targetStats.ApplyAliments(true, false, false);
                    break;
                }

                if (randValue < .5f && iceDamage > 0)
                {
                    targetStats.ApplyAliments(false, true, false);
                    break;
                }

                if (lightningDamage > 0)
                {
                    targetStats.ApplyAliments(false, false, true);
                    break;
                }
                attempts++;
            }

            if (canApplyIgnite)
                targetStats.SetupIgniteDamage(Mathf.RoundToInt(fireDamage * .2f));

            if (canApplyShock)
                targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(lightningDamage * .1f));

            targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
        }

        private static int CheckTargetResistance(CharacterStats targetStats, int totalMagicalDamage)
        {
            totalMagicalDamage -= targetStats.magicResistance.GetValue() + targetStats.intelligence.GetValue() * 3;
            totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
            return totalMagicalDamage;
        }

        private void ApplyAliments(bool ignited, bool frozen, bool shocked)
        {
            var canApplyIgnite = !isIgnited && !isFrozen && !isShocked;
            var canApplyChill = !isIgnited && !isFrozen && !isShocked;
            var canApplyShock = !isIgnited && !isFrozen && !isShocked;
            
            if (ignited && canApplyIgnite)
            {
                isIgnited = true;
                _ignitedTimer = ailmentsDuration;

                _fx.IgniteFXFor(ailmentsDuration);
            }

            if (frozen && canApplyChill)
            {
                _frozenTimer = ailmentsDuration;
                isFrozen = true;
                
                _fx.FrozenFXFor(ailmentsDuration);
            }

            if (shocked && canApplyShock)
            {
                if (!isShocked) ApplyShock(true);
                else
                {
                    if (GetComponent<Player.PlayerStateMachine.Player>() != null)
                        return;

                    HitNearestTargetWithShockStrike();
                }
            }
        }

        private void ApplyIgniteDamage()
        {
            if(_igniteDamageTimer < 0)
            {
                DecreaseHealthBy(_igniteDamage);
                
                if(currentHealth <= 0 && !IsDead)
                    SetFlagDeath();
                
                _igniteDamageTimer = _igniteDamageCooldown;
            }
        }

        private void ApplyShock(bool shock)
        {
            if (isShocked)
                return;

            _shockedTimer = ailmentsDuration;
            isShocked = shock;

            _fx.ShockFXFor(ailmentsDuration);
        }

        private void SetupIgniteDamage(int damageValue) => _igniteDamage = damageValue;
        private void SetupShockStrikeDamage(int damageValue) => _shockDamage = damageValue;
        
        private void HitNearestTargetWithShockStrike()
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, 25);

            var closestDistance = Mathf.Infinity;
            Transform closestEnemy = null;

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy.EnemyStateMachine.Enemy>() != null 
                    && Vector2.Distance(transform.position, hit.transform.position) > 1)
                {
                    var distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        closestEnemy = hit.transform;
                    }
                }

                // delete if you don't want a shocked target to be hit by a shock strike
                if (closestEnemy == null)
                    closestEnemy = transform;
            }


            if (closestEnemy != null)
            {
                var newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
                //newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
            }
        }
        
        protected virtual void TakeDamage(int damageAmount)
        {
            DecreaseHealthBy(damageAmount);
            DecreasePoiseBy(damageAmount);
            
            if(!IsInvincible)
            {
                _fx.StartCoroutine("FlashFX");
                Attacked();
            }

            if (currentHealth <= 0 && !IsDead) SetFlagDeath();
            if (currentPoise <= 0)
            {
                StunCloseRange();
                StunLongRange();
            }
        }

        #region Stun and Attack Check
        // Function to Stun the entity that has close range attack
        protected virtual void StunCloseRange() => IsStunned = true;
        
        // Function to Stun the entity that has long range attack
        protected virtual void StunLongRange() => IsStunned = true;
        protected virtual void Attacked() => IsAttacked = true;
        #endregion

        #region Main Calculations for Health and Poise
        private int CalculateAdjustedAmount(int amount) => 
            IsInvincible ? 0 : Mathf.RoundToInt(amount * 1.1f);

        public virtual void IncreaseHealthBy(int healAmount)
        {
            currentHealth += healAmount;
            if(currentHealth > GetMaxHealthValue())
                currentHealth = GetMaxHealthValue();

            OnHealthChanged?.Invoke();
        }
        
        private void DecreaseHealthBy(int damageAmount)
        {
            var adjustedHealth = CalculateAdjustedAmount(damageAmount);
            
            if (adjustedHealth > 0)
            {
                currentHealth -= adjustedHealth;
                OnHealthChanged?.Invoke();
            }
        }
        
        private void DecreasePoiseBy(int poiseAmount)
        {
            var adjustedPoise = CalculateAdjustedAmount(poiseAmount);
            if (adjustedPoise > 0) currentPoise -= adjustedPoise;
        }
        #endregion
        
        #region Make an Entity Die
        protected virtual void SetFlagDeath()
        {
            IsDead = true;
            OnDeath?.Invoke(this);
            Debug.Log($"{gameObject.name} has died.");
        }

        public void KillEntity()
        {
            if(!IsDead)
                SetFlagDeath();
        }
        #endregion
        
        // No need to care if we finish the inventory system
        #region Stat Calculations
        private static int CheckTargetArmor(CharacterStats targetStats, int totalDamage)
        {
            if (targetStats.isFrozen)
                totalDamage -= Mathf.RoundToInt(targetStats.armor.GetValue() * .8f);
            else totalDamage -= targetStats.armor.GetValue();
            
            totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
            return totalDamage;
        }
        private bool TargetCanAvoidAttack(CharacterStats targetStats)
        {
            var totalEvasion = targetStats.evasion.GetValue() + targetStats.agility.GetValue();
            
            if (isShocked)
                totalEvasion += 20;
            
            return Random.Range(0, 100) < totalEvasion;
        }
        private bool CanCrit()
        {
            var totalCriticalChance = critChance.GetValue() + agility.GetValue();
            return Random.Range(0, 100) <= totalCriticalChance;
        }
        private int CalculateCriticalDamage(int damageAmount)
        {
            var totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
            var critDamage = damageAmount * totalCritPower;

            return Mathf.RoundToInt(critDamage);
        }
        
        public int GetMaxHealthValue() => 
            maxHealth.GetValue() + vitality.GetValue() * 5;

        private int GetMaxPoiseValue() => maxPoiseResistance.GetValue();

        public void MakeInvincible(bool invincible) => IsInvincible = invincible;
        
        public Stat GetStat(EnumList.StatType statType)
        {
            return statType switch
            {
                EnumList.StatType.Strength => strength,
                EnumList.StatType.Agility => agility,
                EnumList.StatType.Intelligence => intelligence,
                EnumList.StatType.Vitality => vitality,
                EnumList.StatType.Damage => damage,
                EnumList.StatType.CritChance => critChance,
                EnumList.StatType.CritPower => critPower,
                EnumList.StatType.Health => maxHealth,
                EnumList.StatType.Armor => armor,
                EnumList.StatType.Evasion => evasion,
                EnumList.StatType.MagicRes => magicResistance,
                EnumList.StatType.FireDamage => fireDamage,
                EnumList.StatType.IceDamage => iceDamage,
                EnumList.StatType.LightingDamage => lightingDamage,
                _ => null
            };
        }
        #endregion
    }
}
