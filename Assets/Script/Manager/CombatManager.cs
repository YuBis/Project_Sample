using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

public class CombatManager : BaseManager<CombatManager>
{
    public void ProcessApplySkill(GameCharacter thrower, GameCharacter target, SkillInstanceData skillInstance)
    {
        if (thrower == null || target == null || skillInstance == null)
            return;

        if (thrower.CHARACTER_STATE == CharacterState.DIE ||
            target.CHARACTER_STATE == CharacterState.DIE)
            return;

        var hitCount = skillInstance.GetStatValue(StatType.HIT_COUNT);
        if (hitCount == 0)
            hitCount = 1;

        var atk = thrower.GetStatValue(StatType.DAMAGE);
        var skillDamagePer = skillInstance.GetStatValue(StatType.DAMAGE_PER);

        var damage = atk * skillDamagePer * 0.01;

        for (int i = 0; i < hitCount; i++)
        {
            target.DecreaseHP(damage);

            Universe.LogDebug(target.name + " Received " + damage + " Damage! Left HP : " + target.CURRENT_HP);

            if (target.CURRENT_HP > 0)
                target.AI.AddNextAI(AIStateType.HIT);
            else
            {
                target.AI.AddNextAI(AIStateType.DIE);
                // Todo : add exp to executor and drop rewards. << this must proceed on characterManager.
            }
        }

        var knockBackRange = skillInstance.GetStatValue(StatType.KNOCK_BACK);
        if (knockBackRange != 0)
            target.Knockback(thrower, (float)knockBackRange);
    }
}