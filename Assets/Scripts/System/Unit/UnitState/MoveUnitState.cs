using System;
using UnityEngine;
using Spine;


public class MoveUnitState : UnitState, IUnitState
{

    bool isMove = false;

    public IUnitState updateState(UnitActor unitActor, IActor targetActor)
    {
        //이동만시
        if(unitActor.isConstraint(new OnlyMoveStateControl(), unitActor, AddConstraint.TYPE_BUFF_CONSTRAINT.Move))
        {
            setMoveVelocity(unitActor);
            Debug.LogWarning("move");
            return this;
        }




        //시야 내 적 발견
        if (targetActor != null)
        {

            int index = unitActor.isSkillActive(this);
            //스킬이 발동되면
            if (index >= 0)
            {
                //스킬 등록
                Debug.Log("setSkill");
                return new SkillUnitState(index);
                //사용
            }

            //사정거리에 도달했으면 공격
//            Debug.Log("Pos : " + targetActor.getPosition(unitActor.layer));
//            Debug.Log("range : " + unitActor.range + " " + Vector2.Distance(targetActor.getPosition(unitActor.layer), unitActor.getPosition(unitActor.layer)));
            if (unitActor.range >= Vector2.Distance(targetActor.getPosition(unitActor.layer), unitActor.getPosition(unitActor.layer)))
            {
                return new AttackUnitState();
            }
           
        }

        //적 미발견시
        //이동불가시
        if (unitActor.isConstraint(new NotMoveStateControl(), unitActor))
        {
            return new IdleUnitState();
        }

        setMoveVelocity(unitActor);

        
        

        return this;
        //이동 지속
    }

    void setMoveVelocity(UnitActor unitActor)
    {
        if (!isMove)
        {
            if (unitActor.typeController == UnitActor.TYPE_CONTROLLER.PLAYER)
                unitActor.GetComponent<Rigidbody2D>().velocity = Vector2.right * unitActor.moveSpeed;
            else
                unitActor.GetComponent<Rigidbody2D>().velocity = Vector2.left * unitActor.moveSpeed;

            isMove = true;
        }
    }

    public TrackEntry setAnimation(UnitActor unitActor, SetAnimationDelegate setAnimationDel)
    {
        return setAnimationDel("Move", true);
    }

    public void completeAction(UnitActor unitActor)
    {
    }


    public void startAction(UnitActor unitActor)
    {
    }

    public Skill.TYPE_SKILL_ACTIVE getTypeSkillActive()
    {
        return Skill.TYPE_SKILL_ACTIVE.Move;
    }
}

