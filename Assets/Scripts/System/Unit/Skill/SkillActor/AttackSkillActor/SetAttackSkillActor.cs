using UnityEngine;

public class SetAttackSkillActor : AttackSkillActor
{
    //탄환 키
    //    [SerializeField]
    //    string m_bulletKey;
        
    [SerializeField]
    TYPE_TEAM m_typeAlly;

    public string bulletKey { get { return name; } }


    //목표에 설치하기
    //아군, 적군

    public override IActor skillAction(IActor iActor, IActor targetActor, Skill skill)
    {

        BulletActorManager bulletActorManager = ActorManager.GetInstance.getActorManager(typeof(BulletActorManager)) as BulletActorManager;

        //유닛 찾기
//        IUnitSearch unitSearch = new RangeUnitSearch();
//        IActor getActor = unitSearch.searchUnitActor(iActor, targetActor, m_typeAlly);

        IActor[] targetActors = getTargetActors(iActor, skill.typeTeam, skill.size);
        int actCnt = typeRangeCalculator(targetActors, skill.actorCnt);

        while (actCnt-- > 0)
        {
            bulletActorManager.createActor(bulletKey, iActor, targetActors[actCnt], attack);
        }

        return null;


        
    }
}

