using System;
using UnityEngine;


public class JumpAttackSkillActor : AttackSkillActor
{
    public override IActor skillAction(IActor iActor, IActor targetActor, Skill skill)
    {

        //적에게 붙은 다음 직접공격

        //UnityEngine.Debug.Log("skill");

//        RangeUnitSearch unitSearch = new RangeUnitSearch();
        IActor[] targetActors = getTargetActors(iActor, skill.typeTeam, skill.size, new MoveUnitState());
//        IActor[] targetActors = unitSearch.searchUnitActors(iActor, new MoveUnitState(), TYPE_ALLY.ENEMY);

        int actCnt = typeRangeCalculator(targetActors, skill.actorCnt);
        //if (typeRange == 0 || typeRange > targetActors.Length)
        //    typeRange = targetActors.Length;

        while (actCnt-- > 0)
        {
            
            //            Vector2 movePos = iActor.transform.position;
            Vector2 movePos = iActor.getPosition(iActor.layer);
            if (iActor.typeController == UnitActor.TYPE_CONTROLLER.PLAYER)
            {
                movePos.x = targetActors[actCnt].getPosition(iActor.layer).x - 1f;
                //                movePos.x = targetActor.transform.position.x - 1f;
            }
            else
            {
                movePos.x = targetActors[actCnt].getPosition(iActor.layer).x + 1f;
                //                movePos.x = targetActor.transform.position.x + 1f;
            }


//            Debug.Log("pos : " + movePos);

            //            iActor.transform.position = movePos;
            iActor.setPosition(movePos);

            if (targetActors[actCnt].hitActor(iActor, attack))
                effectAction(name + "HitParticle", targetActors[actCnt].getPosition(targetActors[actCnt].layer), iActor.typeController, iActor.isFlip);
            //                effectAction(name + "HitParticle", targetActor.transform.position, iActor.typeController);

        }

        return null;
    }
}

