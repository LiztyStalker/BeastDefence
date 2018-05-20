using System;


public class CastleBuffSkillActor : BuffSkillActor
{
    public override IActor skillAction(IActor iActor, IActor targetActor, Skill skill)
    {
        BuffActor buffActor = getBuffActor();

        if (buffActor == null)
        {
            Prep.LogError(buffKey, "버프를 찾지 못했습니다", GetType());
            return null;
        }

        //타겟의 범위만큼 버프 걸기
        IUnitSearch unitSearch = new BoxUnitSearch();
        IActor[] unitActors = unitSearch.searchUnitActors(iActor, skill.typeTeam, 100f);


        if (iActor != null)
        {
            foreach (UnitActor actor in unitActors)
            {
                if (actor.typeUnit == Unit.TYPE_UNIT.Building)
                {
                    if (skill.typeTeam == TYPE_TEAM.Ally && actor.typeController == iActor.typeController)
                    {
                        actor.addBuff(buffActor, actor);
                        break;
                    }
                    else if (skill.typeTeam != TYPE_TEAM.Ally && actor.typeController != iActor.typeController)
                    {
                        actor.addBuff(buffActor, actor);
                        break;
                    }
                }
            }
        }

        return null;

    }
}

