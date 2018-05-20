using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class InvisibleStateControl : StateControl, IStateControl
{

    public InvisibleStateControl() : base()
    {
    }

    public InvisibleStateControl(float value, float increaseValue, TYPE_STATE_VALUE typeStateValue)
    {
        this.value = value;
        this.increaseValue = increaseValue;
        this.typeStateValue = typeStateValue;
    }
}


