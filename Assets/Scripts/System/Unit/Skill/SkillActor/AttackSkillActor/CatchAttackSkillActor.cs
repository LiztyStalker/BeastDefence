using UnityEngine;


class CatchAttackSkillActor : AttackSkillActor
{
    public override IActor skillAction(IActor iActor, IActor targetActor, Skill skill)
    {


        //적을 앞으로 가져와서 공격

        if (targetActor != null)
        {
            if (targetActor is UnitActor)
            {


                IActor[] targetActors = getTargetActors(iActor, skill.typeTeam, skill.size);
                int actorCnt = typeRangeCalculator(targetActors, skill.actorCnt);

                while (actorCnt-- > 0)
                {
                    if (targetActors[actorCnt] is UnitActor)
                    {
                        UnitActor targetUnitActor = targetActors[actorCnt] as UnitActor;

                        //성이 아니면 잡기 가능
                        if (targetUnitActor.typeUnit != Unit.TYPE_UNIT.Building)
                        {

                            //                    Vector2 movePos = iActor.transform.position;
                            Vector2 movePos = iActor.getPosition(iActor.layer);

                            if (iActor.typeController == UnitActor.TYPE_CONTROLLER.PLAYER)
                            {
                                //                        movePos.x = iActor.transform.position.x + 1f;
                                movePos.x = iActor.getPosition(iActor.layer).x + 1f;
                            }
                            else
                            {
                                //                        movePos.x = iActor.transform.position.x - 1f;
                                movePos.x = iActor.getPosition(iActor.layer).x - 1f;
                            }


                            targetUnitActor.setPosition(movePos);

                            if (targetUnitActor.hitActor(iActor, attack))
                                effectAction(name + "HitParticle", targetUnitActor.getPosition(targetUnitActor.layer), iActor.typeController, iActor.isFlip);
                            //effectAction(name + "HitParticle", targetActor.transform.position, iActor.typeController);
                            //이펙트 활성화

                        }
                    }
                }
            }
        }
        return null;
    }
}

