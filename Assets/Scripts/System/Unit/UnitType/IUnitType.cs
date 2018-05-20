using Spine.Unity;

public interface IUnitType
{

    IUnitState iUnitState { get; }
    IUnitSearch iUnitSearch { get; }

    void clear();

    //공격행동
    IActor attackAction(IActor iActor, IActor targetActor);

    //스킬행동
    IActor skillAction(IActor iActor, IActor targetActor);

    //애니메이션 시작 행동
    void startAction(UnitActor unitActor);

    //애니메이션 완료 행동
    void completeAction(UnitActor unitActor);
       

    //주기적 행동
    IActor updateState(UnitActor unitActor, IActor targetActor);


    void setDefaultState();
    void setDeadState();
}

