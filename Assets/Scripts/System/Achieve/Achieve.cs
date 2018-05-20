using UnityEngine;

public class Achieve
{

    public enum TYPE_ACHIEVE_CATEGORY{
        Normal, 
        Unit, 
        Pay, 
        Account, 
        Develop
    }

    public enum TYPE_ACHIEVE{
        GamePlay,
        MainStageCnt,
        AccLevel,
        TotalUsedGold,
        TotalUsedFruit,
        TotalUsedFood,
        CmdLevel,
        SoldierLevel,
        HeroLevel,
        GetUnit,
        GetSoldier,
        GetHero,
        TotalUsedUnit,
        TotalUsedSoldier,
        TotalUsedHero,
        TotalGold,
        TotalFruit,
        TotalFood,
        LoginDay,
        PlayTime,
        DevCount
    }


    string m_key;
    string m_name;
    Sprite m_icon;
    TYPE_ACHIEVE m_typeAchieve;
    int m_value;

    TYPE_ACHIEVE_CATEGORY m_typeAchieveCategory;
    TYPE_ACCOUNT_CATEGORY m_typeAward;
    int m_awardValue;
    string m_contents;

    public string key { get { return m_key; } }
    public string name { get { return m_name; } }
    public Sprite icon { get { return m_icon; } }
    public TYPE_ACHIEVE typeAchieve { get { return m_typeAchieve; } }
    public int value { get { return m_value; } }
    public TYPE_ACHIEVE_CATEGORY typeAchieveCategory { get { return m_typeAchieveCategory; } }
    public TYPE_ACCOUNT_CATEGORY typeAward { get { return m_typeAward; } }
    public int awardValue { get { return m_awardValue; } }
    public string contents { get { return m_contents; } }

    public Achieve(
        string key,
        string name,
        Sprite icon,
        TYPE_ACHIEVE_CATEGORY typeAchieveCategory,
        TYPE_ACHIEVE typeAchieve,
        int value,
        TYPE_ACCOUNT_CATEGORY typeAward,
        int awardValue,
        string contents
        )
    {
        m_key = key;
        m_name = name;
        m_icon = icon;
        m_typeAchieveCategory = typeAchieveCategory;
        m_typeAchieve = typeAchieve;
        m_value = value;
        m_typeAward = typeAward;
        m_awardValue = awardValue;
        m_contents = contents;
    }

    /// <summary>
    /// 보상 받음
    /// </summary>
    /// <returns></returns>
    public bool isSuccess()
    {
        return Account.GetInstance.accAchieve.isAchieve(key);
    }

    /// <summary>
    /// 달성 비율 가져오기
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public float valueRate()
    {
        return Mathf.Clamp01((float)Account.GetInstance.accAchieve.getAchieveValue(typeAchieve) / (float)value);
    }
}

