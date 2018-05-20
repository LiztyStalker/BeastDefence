using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Deck
{
    string m_commanderKey;
    int m_commanderLevel;

    int m_castleLv;
    int m_munitionsLv;
    int m_defenceLv;
    int m_defenceCnt;
    int m_soldierLv;
    int m_heroLv;
    ICard[] m_cardArray;// = new ICard[Prep.maxUnitSlot + Prep.maxHeroSlot + Prep.maxSkillSlot];

    public string commanderKey { get { return m_commanderKey; } }
    public int commanderLevel { get { return m_commanderLevel; } }
    public int castleLv { get { return m_castleLv; } }
    public int munitionsLv { get { return m_munitionsLv; } }
    public int defenceLv { get { return m_defenceLv; } }
    public int defenceCnt { get { return m_defenceCnt; } }
    public int soldierLv { get { return m_soldierLv; } }
    public int heroLv { get { return m_heroLv; } }
    public ICard[] cardArray { get { return m_cardArray; } }

    public Deck()
    {
        m_commanderLevel = 1;
        m_castleLv = 1;
        m_munitionsLv = 1;
        m_defenceLv = 1;
        m_soldierLv = 1;
        m_heroLv = 1;
    }

    public Deck(string commanderKey, 
        int commanderLevel, 
        int castleLv, 
        int munitionsLv, 
        int defenceLv, 
        int defenceCnt,
        int soldierLv, 
        int heroLv, 
        ICard[] cardArray)
    {
        m_commanderKey = commanderKey;
        m_commanderLevel = commanderLevel;
        m_castleLv = castleLv;
        m_munitionsLv = munitionsLv;
        m_defenceLv = defenceLv;
        m_defenceCnt = defenceCnt;
        m_soldierLv = soldierLv;
        m_heroLv = heroLv;
        m_cardArray = cardArray.ToArray<ICard>();
    }


    public string[] getUnitCardKeys()
    {
        return m_cardArray.Where(card => card is UnitCard).Select(card => card.key).ToArray<string>();
    }

}

