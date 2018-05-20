using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IStateControl
{
    float value { get; }
    float increaseValue { get ; }

    //추가 연산 가져오기
    StateControl.TYPE_STATE_VALUE typeStateValue { get;}

    bool frameUpdate(UnitActor unitActor, int overlapCnt);
}

