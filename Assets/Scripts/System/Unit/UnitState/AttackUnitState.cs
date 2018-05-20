using UnityEngine;
using Spine;

public class AttackUnitState : UnitState, IUnitState
{


    BulletActorManager bulletActorManager;

    public IUnitState updateState(UnitActor unitActor, IActor targetActor)
    {
        int index = unitActor.isSkillActive(this);

        //스킬이 발동되면
        if (index >= 0)
        {
            return new SkillUnitState(index);
        }

        //적이 없거나 공격불가 상태이면
        if (unitActor.isConstraint(new NotAttackStateControl(), unitActor) || targetActor == null)
        {
            //대기
            unitActor.transform.localScale = Vector2.one;
            return new IdleUnitState();
        }



        //방향
        float dir = targetActor.getPosition(unitActor.layer).x - unitActor.getPosition(unitActor.layer).x;

        float flip = (targetActor.typeController == UnitActor.TYPE_CONTROLLER.CPU) ? -1f : 1f;


        if (dir > 0)
            unitActor.transform.localScale = new Vector3(-1f * flip, 1f);
        else
            unitActor.transform.localScale = new Vector3(1f * flip, 1f);
        

        unitActor.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        //공격
        return this;

    }



    bool createEffect(string key, Vector2 pos, UnitActor.TYPE_CONTROLLER typeCtrler, bool isFlip)
    {
        EffectActorManager effectActorManager = ActorManager.GetInstance.getActorManager(typeof(EffectActorManager)) as EffectActorManager;
        if (effectActorManager != null)
        {
            effectActorManager.createActor(key, pos, typeCtrler, isFlip);
            return true;
        }
        return false;

    }




    
        



    public IActor attackAction(IActor iActor, IActor targetActor)
    {
        if(bulletActorManager == null)
            bulletActorManager = ActorManager.GetInstance.getActorManager(typeof(BulletActorManager)) as BulletActorManager;
        //        targetUnitActor = searchUnitActor(unitActor, targetUnitActor);

        
        IUnitSearch unitSearch = new RangeUnitSearch();
        IActor[] targetActors = unitSearch.searchUnitActors(iActor, TYPE_TEAM.Enemy, iActor.range);

        int count = ((UnitActor)iActor).typeRange;
        if (count == 0 || count > targetActors.Length)
            count = targetActors.Length;


        switch(iActor.typeTarget){
            case Unit.TYPE_TARGETING.Melee:
                    //이펙트 발생 - 근거리
                if(count != 0)
                    createEffect(((UnitActor)iActor).effectKey + "ActiveParticle", iActor.getPosition(iActor.layer), iActor.typeController, iActor.isFlip);

                while(count-- > 0){
                    
                    //피격되었으면
                    if (targetActors[count].hitActor(iActor, iActor.attack)){
                        createEffect(((UnitActor)iActor).effectKey + "HitParticle", targetActors[count].getPosition(iActor.layer), iActor.typeController, iActor.isFlip);

                        //사망했으면 - 다음 넘김
                        if (targetActor.isDead())
                            continue;
                    }
                }

                break;

            //투사체이면
            case Unit.TYPE_TARGETING.Range:
                //주변 적 가져온 후 탄환 발사
                while (count-- > 0)
                {
                    if (!targetActors[count].isDead())
                        //탄환 생성
                        bulletActorManager.createActor(iActor, targetActors[count]);
                }
                break;
        }

        return targetActor;
    }

    bool createEffect(UnitActor unitActor, string verb)
    {


        if (effectActorManager != null)
        {
            string effectStr = unitActor.key + verb;
            createEffect(effectStr, unitActor.getPosition(unitActor.layer), unitActor.typeController, unitActor.isFlip);
            return true;
        }
        return false;
    }

    public TrackEntry setAnimation(UnitActor unitActor, SetAnimationDelegate setAnimationDel)
    {
        return setAnimationDel("Attack", true);
    }

    public void completeAction(UnitActor unitActor)
    {
        createEffect(unitActor, "EndParticle");        
    }
    
    public void startAction(UnitActor unitActor)
    {
        createEffect(unitActor, "StartParticle");
    }

    public Skill.TYPE_SKILL_ACTIVE getTypeSkillActive()
    {
        return Skill.TYPE_SKILL_ACTIVE.Attack;
    }
}

