using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class RecoveryHealthStateControl : StateControl, IStateControl
{
    public RecoveryHealthStateControl()
        : base()
    {
    }

    public RecoveryHealthStateControl(float value, float increaseValue, TYPE_STATE_VALUE typeStateValue)
    {
        this.value = value;
        this.increaseValue = increaseValue;
        this.typeStateValue = typeStateValue;
    }


    public override bool frameUpdate(UnitActor unitActor, int overlapCnt)
    {

        base.frameUpdate(unitActor, overlapCnt);

        //1프레임당 값
        float frameValue = value * Prep.frameTime * overlapCnt;


        int valueRate = -(int)((float)(unitActor.maxHealth) * frameValue);

        UnityEngine.Debug.LogError("unitActor : " + unitActor.key + " " + valueRate);
        

        if (typeStateValue == TYPE_STATE_VALUE.Rate)
        {
            if (valueRate > 0f)
                //체력 까임
                unitActor.hitUnit(valueRate);
            else
                //체력 회복
                unitActor.addHealth(valueRate);
        }
        else
        {
            if (valueRate > 0f)
                //체력 까임
                unitActor.hitUnit((int)frameValue);
            else
                //체력 회복
                unitActor.addHealth((int)frameValue);
        }


        return true;
    }

}

