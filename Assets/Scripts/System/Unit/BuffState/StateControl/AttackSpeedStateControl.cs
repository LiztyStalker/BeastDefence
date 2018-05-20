

public class AttackSpeedStateControl : StateControl, IStateControl
{

    public AttackSpeedStateControl() : base()
    {
    }

    public AttackSpeedStateControl(float value, float increaseValue, TYPE_STATE_VALUE typeStateValue)
    {
        this.value = value;
        this.increaseValue = increaseValue;
        this.typeStateValue = typeStateValue;
    }
}

