using System;
using System.Collections.Generic;
using InventorySystem_and_Items.Effects;
using Manager;
using Player.PlayerStats;
using StatSystem;
using UnityEngine;

namespace InventorySystem_and_Items.Data
{
    
    
    [CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
    public class ItemDataEquipment : ItemData
    {
        public EnumList.EquipmentType equipmentType;
        
        #region Stat System for Equipment

        public float itemCoolDown;
        public ItemEffect[] itemEffects;
        
        [Header("Major stats")]
        public int strength; // 1 point increase damage and critical power by 1%
        public int agility; // 1 point increase evasion and critical chance by 1%
        public int intelligence; // 1 point increase magic damage and 1 magic resistance by 3%
        public int vitality; // 1 point increase health by 5%
 
        [Header("Offensive stats")]
        public int damage;
        public int critChance;
        public int critPower;
        
        [Header("Defensive stats")]
        public int maxHealth;
        public int armor;
        public int evasion;
        public int maxPoiseResistance;
        public int poiseResetTime;
        public int lastPoiseReset;
        #endregion

        #region Crafting System
        [Header("Craft Requirements")]
        public List<InventoryItem> craftingMaterials;
        #endregion
        
        private int _descriptionLength;
        
        public void ExecuteItemEffect(Transform enemyPosition)
        {
            foreach (var itemEffect in itemEffects) 
                itemEffect.ExecuteEffect(enemyPosition);
        }
        
        private void ModifyPlayerStats(Action<Stat, int> action)
        {
            var playerStats = PlayerManager.GetInstance().player.GetComponentInChildren<PlayerStats>();

            action(playerStats.strength, strength);
            action(playerStats.agility, agility);
            action(playerStats.intelligence, intelligence);
            action(playerStats.vitality, vitality);
    
            action(playerStats.damage, damage);
            action(playerStats.critChance, critChance);
            action(playerStats.critPower, critPower);
    
            action(playerStats.maxHealth, maxHealth);
            action(playerStats.armor, armor);
            action(playerStats.evasion, evasion);
            action(playerStats.maxPoiseResistance, maxPoiseResistance);
            action(playerStats.poiseResetTime, poiseResetTime);
            action(playerStats.lastPoiseReset, lastPoiseReset);
        }

        public void AddModifiers() => ModifyPlayerStats((stat, modifier) => stat.AddModifier(modifier));

        public void RemoveModifiers() => ModifyPlayerStats((stat, modifier) => stat.RemoveModifier(modifier));
        
        public string GetDescription()
        {
            Sb.Length = 0;
            _descriptionLength = 0;

            AddItemDescription(strength, "Strength");
            AddItemDescription(agility, "Agility");
            AddItemDescription(intelligence, "Intelligence");
            AddItemDescription(vitality, "Vitality");

            AddItemDescription(damage, "Damage");
            AddItemDescription(critChance, "Crit.Chance");
            AddItemDescription(critPower, "Crit.Power");

            AddItemDescription(maxHealth, "Health");
            AddItemDescription(evasion, "Evasion");
            AddItemDescription(armor, "Armor");
            
            if (_descriptionLength < 5)
            {
                for (var i = 0; i < 5 - _descriptionLength; i++)
                {
                    Sb.AppendLine();
                    Sb.Append("");
                }
            }
            
            return Sb.ToString();
        }
        
        private void AddItemDescription(int value, string itemNameText)
        {
            if (value != 0)
            {
                if (Sb.Length > 0)
                    Sb.AppendLine();

                if (value > 0)
                    Sb.Append("+ " + value + " " + itemNameText);

                _descriptionLength++;
            }       
        }
    }
}