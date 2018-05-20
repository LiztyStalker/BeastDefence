using UnityEngine;
using System.Collections;

public class ThrowAttackSkillActor : AttackSkillActor
{
    //탄환 키
//    [SerializeField]
//    string m_bulletKey;

    [SerializeField, Range(1, 100)]
    int m_count = 1;

    [SerializeField, Range(0.01f, 1f)]
    float m_gapTime = 1f;

    public string bulletKey { get { return name; } }

    public override Unit.TYPE_TARGETING typeTarget
    {
        get
        {
            return Unit.TYPE_TARGETING.Range;
        }
    }

    //목표로 던지기

    public override IActor skillAction(IActor iActor, IActor targetActor, Skill skill)
    {

        BulletActorManager bulletActorManager = ActorManager.GetInstance.getActorManager(typeof(BulletActorManager)) as BulletActorManager;



        //탄 나누기
        //피격되어야 할 적 - 탄환 수
        //피격되 적이 많으면 1기씩 탄환 발사 - 나머지는 버림
        //

        if (targetActor != null)
            bulletActorManager.createActor(bulletKey, iActor, targetActor, attack, m_count, m_gapTime);
        else
        {

            //int pos = Prep.getRandomLineIndex();
            //Vector2 randPos = Prep.getRandomLinePosition(iActor.transform.position, 1f, pos);
            //int layer = Prep.getLayer(pos, Unit.TYPE_MOVEMENT.GROUND);

            //지정된 해당 라인 안으로 랜덤으로 떨어짐
            //            bulletActorManager.createActor(bulletKey, iActor, iActor.transform.position, 1f, attack, m_count, m_gapTime);
            bulletActorManager.createActor(bulletKey, iActor, iActor.getPosition(iActor.layer), 1f, attack, m_count, m_gapTime);

        }
        
//        StartCoroutine(createSkillAction(iActor, targetActor, m_count));
        
        return null;
    }

    
}

