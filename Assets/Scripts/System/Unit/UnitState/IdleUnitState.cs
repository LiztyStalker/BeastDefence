using UnityEngine;
using Spine;

public class IdleUnitState : UnitState, IUnitState
{
    public IUnitState updateState(UnitActor unitActor, IActor targetActor)
    {

        //상태이상 여부
        //공격불가 - 공격되지 않음
        //멈춤 - idle에서 상태가 변화되지 않음
        //이동만 - move에서 상태가 변화되지 않음
        //스킬불가 - skill로 이동되지 않음


        //


        //중력이 없으면 순환
        if (unitActor.GetComponent<Rigidbody2D>().gravityScale <= 0f)
        {

//            if (unitActor.GetComponent<Rigidbody2D>().velocity != Vector2.zero)

            //무조건 이동이면
            if (unitActor.isConstraint(new OnlyMoveStateControl(), unitActor, AddConstraint.TYPE_BUFF_CONSTRAINT.Move))
            {
                Debug.LogWarning("OnlyMove");
                return new MoveUnitState();
            }
                        
            //이동만이 아니면
//            unitActor.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //적 발견시
            if (targetActor != null)
            {
                //공격 불가가 아니면
                return new AttackUnitState();

            }
            
            //적 미발견시
            if (unitActor.unitType is SoldierUnitType)
            {
                return new MoveUnitState();
            }
           

        }
        
        return this;
    }

    public TrackEntry setAnimation(UnitActor unitActor, SetAnimationDelegate setAnimationDel)
    {
        return setAnimationDel("Idle", true);
    }

    public void completeAction(UnitActor unitActor)
    {
    }


    public void startAction(UnitActor unitActor)
    {
        unitActor.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }


    public Skill.TYPE_SKILL_ACTIVE getTypeSkillActive()
    {
        return Skill.TYPE_SKILL_ACTIVE.Move;
    }
}

