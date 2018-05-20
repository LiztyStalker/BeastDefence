using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class NotAttackStateControl : StateControl, IStateControl
{

    public NotAttackStateControl() : base()
    {
    }

    public NotAttackStateControl(float value, float increaseValue, TYPE_STATE_VALUE typeStateValue)
    {
        this.value = value;
        this.increaseValue = increaseValue;
        this.typeStateValue = typeStateValue;
    }
}


