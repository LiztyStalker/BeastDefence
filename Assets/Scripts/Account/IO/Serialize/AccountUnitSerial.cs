using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class AccountUnitSerial
{
    public string lastTime;
//    public int totalHeroEngage;
//    public int totalSoldierEngage;
    public List<AccountCommanderCardSerial> commanderCardList = new List<AccountCommanderCardSerial>();
    public List<List<string>> unitWaitList = new List<List<string>>();
    public List<AccountUnitCardSerial> unitCardList = new List<AccountUnitCardSerial>();
}

