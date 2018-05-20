using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class DirectAttackSkillActor : AttackSkillActor
{
    //직접 공격
    public override IActor skillAction(IActor iActor, IActor targetActor, Skill skill)
    {


        IActor[] targetActors = getTargetActors(iActor, skill.typeTeam, skill.size);
        int actCnt = typeRangeCalculator(targetActors, skill.actorCnt);


        while (actCnt-- > 0)
        {
            if (targetActors[actCnt].hitActor(iActor, attack))
                //                effectAction(name + "HitParticle", targetActor.transform.position, iActor.typeController);
                effectAction(name + "HitParticle", targetActors[actCnt].getPosition(targetActors[actCnt].layer), iActor.typeController, iActor.isFlip);
        }
        return null;
    }

}

