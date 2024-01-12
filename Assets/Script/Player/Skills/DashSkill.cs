using UnityEngine;

namespace Script.Player.Skills
{
    public class DashSkill : Skill
    {
        public override void UseSkill()
        {
            base.UseSkill();
            Debug.Log("Create clone behind");
        }
    }
}