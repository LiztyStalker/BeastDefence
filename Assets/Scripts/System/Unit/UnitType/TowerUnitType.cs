using System;
using Spine.Unity;

public class TowerUnitType : UnitType,  IUnitType
{


    public TowerUnitType()
        : base()
    {
        iUnitSearch = new UnderUnitSearch();
    }


    public override IActor skillAction(IActor iActor, IActor targetActor)
    {
        return null;
    }


    /// <summary>
    /// 공격 행동
    /// </summary>
    /// <param name="unitActor">사용자</param>
    /// <param name="targetActor">피격자</param>
    /// <param name="bulletActorManager">탄환행동자매니저</param>
    /// <returns></returns>
    public override IActor attackAction(IActor iActor, IActor targetActor)
    {


        BulletActorManager bulletActorManager = ActorManager.GetInstance.getActorManager(typeof(BulletActorManager)) as BulletActorManager;

        if (((UnitActor)iActor). typeTarget == Unit.TYPE_TARGETING.Melee)
        {

            if (targetActor != null)
            {
                if (targetActor.hitActor(iActor, iActor.attack))
                {
                    return null;
                }
            }
        }
        //투사체이면
        else
        {

            if (targetActor != null)
                bulletActorManager.createActor(iActor, targetActor);

            //적이 있는 위치로 투사체 발사
            //탄환 매니저 -
        }

        return targetActor;
    }

    public override IActor updateState(UnitActor unitActor, IActor targetActor)
    {
        iUnitState = iUnitState.updateState(unitActor, targetActor);

        return targetActor;
    }


}

