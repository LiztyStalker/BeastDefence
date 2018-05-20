using UnityEngine;

public class PowerAttackSkillActor : AttackSkillActor
{
    const float forceOffset = 10f;

    [SerializeField]
    float m_force;

    public override IActor skillAction(IActor iActor, IActor targetActor, Skill skill)
    {

        //RangeUnitSearch unitSearch = new RangeUnitSearch();
        IActor[] targetActors = getTargetActors(iActor, skill.typeTeam, skill.size, new MoveUnitState());

        int actCnt = typeRangeCalculator(targetActors, skill.actorCnt);


        while (actCnt-- > 0)
        {

            //적을 공중으로 띄움

            if (targetActors[actCnt] is UnitActor)
            {

                UnitActor targetUnitActor = targetActors[actCnt] as UnitActor;

                //성이 아니면 띄우기 가능
                if (targetUnitActor.typeUnit != Unit.TYPE_UNIT.Building)
                {
                    targetUnitActor.unitType.setDefaultState();

                    if (targetUnitActor.typeController == UnitActor.TYPE_CONTROLLER.PLAYER)
                        targetUnitActor.GetComponent<Rigidbody2D>().velocity = new Vector2(-1f, 1f) * m_force;
                    else
                        targetUnitActor.GetComponent<Rigidbody2D>().velocity = new Vector2(1f, 1f) * m_force;

                    targetUnitActor.GetComponent<Rigidbody2D>().gravityScale = 1f;

                    if (targetUnitActor.hitActor(iActor, attack))
                        effectAction(name + "HitParticle", targetUnitActor.getPosition(targetUnitActor.layer), iActor.typeController, iActor.isFlip);
                    //대기 상태로 전환
                }
            }

            
        }

        return null;
    }
}

