using UnityEngine;

public class RangeBuffSkillActor : BuffSkillActor
{
    [SerializeField]
    float m_radius;

    public override IActor skillAction(IActor iActor, IActor targetActor, Skill skill)
    {
        BuffActor buffActor = getBuffActor();

        if (buffActor == null)
        {
            Prep.LogError(buffKey, "버프를 찾지 못했습니다", GetType());
            return null;
        }

        //타겟의 범위만큼 버프 걸기
//        IUnitSearch unitSearch = new BoxUnitSearch();
//        IActor[] unitActors = unitSearch.searchUnitActors(iActor, typeAlly, m_radius);

        IActor[] targetActors = getTargetActors(iActor, skill.typeTeam, skill.size);
        int actCnt = typeRangeCalculator(targetActors, skill.actorCnt);


//        Debug.LogWarning("범위 버프 : " + unitActors.Length);

        if (iActor != null)
        {
            foreach (UnitActor actor in targetActors)
            {
                if(actor.typeUnit != Unit.TYPE_UNIT.Building)
                    actor.addBuff(buffActor, actor);
            }
        }
        
        return null;
    }


}

