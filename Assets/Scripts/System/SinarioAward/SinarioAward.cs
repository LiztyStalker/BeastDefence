using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SinarioAward
{

    public enum TYPE_SINARIO_AWARD_CATEGORY 
    { 
        Gold, 
        Exp, 
        Card, 
        Fruit, 
        Food, 
        NCard, 
        HCard, 
        CCard 
    }

    string m_key;

    Dictionary<TYPE_SINARIO_AWARD_CATEGORY, string> m_sinarioAwardDic = new Dictionary<TYPE_SINARIO_AWARD_CATEGORY, string>();
    //int m_card;
    //int m_gold;
    //int m_fruit;
    //int m_exp;
    //int m_food;

    public string key { get { return m_key; } }
    public Dictionary<TYPE_SINARIO_AWARD_CATEGORY, string> sinarioAwardDic { get { return m_sinarioAwardDic; } }

    public SinarioAward(string key, Dictionary<TYPE_SINARIO_AWARD_CATEGORY, string> sinarioAwardDic)
    {
        m_key = key;
        m_sinarioAwardDic = sinarioAwardDic;
    }

    public string getValue(TYPE_SINARIO_AWARD_CATEGORY typeCategory)
    {
        if (m_sinarioAwardDic.ContainsKey(typeCategory))
        {
            return m_sinarioAwardDic[typeCategory];
        }
        return string.Empty;
    }
}

