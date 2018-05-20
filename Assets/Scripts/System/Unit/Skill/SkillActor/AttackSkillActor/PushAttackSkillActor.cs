using UnityEngine;


public class PushAttackSkillActor : AttackSkillActor
{
    [SerializeField]
    float m_distance;

    public override IActor skillAction(IActor iActor, IActor targetActor, Skill skill)
    {
        IActor[] targetActors = getTargetActors(iActor, skill.typeTeam, skill.size);
        int actCnt = typeRangeCalculator(targetActors, skill.actorCnt);


        while (actCnt-- > 0)
        {

            //적을 밀어서 공격
            if (targetActors[actCnt] is UnitActor)
            {

                UnitActor targetUnitActor = targetActors[actCnt] as UnitActor;

                if (targetUnitActor.typeUnit != Unit.TYPE_UNIT.Building)
                {

                    Vector2 movePos = targetUnitActor.transform.position;
                    if (targetUnitActor.typeController == UnitActor.TYPE_CONTROLLER.PLAYER)
                    {
                        movePos.x = targetUnitActor.transform.position.x - m_distance;
                    }
                    else
                    {
                        movePos.x = targetUnitActor.transform.position.x + m_distance;
                    }

                    //성 뒤로 보내지 못하게 제작 필요
                    //애니메이션 필요

                    targetUnitActor.transform.position = movePos;

                    if (targetUnitActor.hitActor(iActor, attack))
                        effectAction(name + "HitParticle", targetUnitActor.getPosition(targetUnitActor.layer), iActor.typeController, iActor.isFlip);


                }
                
            }
            
        }



        return null;
    }
}

