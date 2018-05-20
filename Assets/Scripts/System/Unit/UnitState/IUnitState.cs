using System;
using Spine;

public delegate TrackEntry SetAnimationDelegate(string name, bool isLoop);

public interface IUnitState
{

    IUnitState updateState(UnitActor unitActor, IActor targetActor);
    TrackEntry setAnimation(UnitActor unitActor, SetAnimationDelegate setAnimationDel);
    void completeAction(UnitActor unitActor);
    void startAction(UnitActor unitActor);
    Skill.TYPE_SKILL_ACTIVE getTypeSkillActive();
}

