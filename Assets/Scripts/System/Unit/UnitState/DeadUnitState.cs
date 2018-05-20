using UnityEngine;
using Spine;


public class DeadUnitState : UnitState, IUnitState
{
    public IUnitState updateState(UnitActor unitActor, IActor targetActor)
    {
//        if (unitActor.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
        return this;
    }

    public TrackEntry setAnimation(UnitActor unitActor, SetAnimationDelegate setAnimationDel)
    {
        return setAnimationDel("Dead", false);
    }

    public void completeAction(UnitActor unitActor)
    {
    }


    public void startAction(UnitActor unitActor)
    {
        //사망음 - 
        string deadKey = "Dead0" + Random.Range(0, 7);
        SoundManager.GetInstance.audioPlay(unitActor.soundPlay, deadKey);
        unitActor.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public Skill.TYPE_SKILL_ACTIVE getTypeSkillActive()
    {
        return Skill.TYPE_SKILL_ACTIVE.Move;
    }
}

