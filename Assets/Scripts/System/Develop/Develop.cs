using System;
using System.Collections.Generic;
using UnityEngine;


public class Develop
{

    public enum TYPE_DEVELOP_GROUP { 
        Castle, 
        Barracks, 
        Training, 
        Commander 
    }


    public enum TYPE_DEVELOP_VALUE_GROUP
    {
        CastleTechLv,
        CastleLv,
        MunitionsLv,
        DefenceLv,
        DefenceTechLv,
        AwardLv,
        SoldierLv,
        SoldierExp,
        SoldierWait,
        SoldierBarracks,
        SoldierBatch,
        SoldierMunitions,
        HeroLv,
        HeroExp,
        HeroWait,
        HeroBarracks,
        HeroBatch,
        HeroMunitions,
        BarracksTechLv,
        TrainingTechLv
    }

    //키
    string m_key;

    //이름
    string m_name;

    //아이콘
    Sprite m_icon;

    //개발 타입
    TYPE_DEVELOP_GROUP m_typeDevGroup;

    //값 타입
    TYPE_DEVELOP_VALUE_GROUP m_typeValueGroup;

    //트리 위치
    int m_position;

    //테크 레벨
    int m_techLevel;

    //최대 레벨
    int m_maxLevel;

    //내용
    string m_contents;

    //초기값
    float m_beginValue;

    //증가값
    float m_increaseValue;

    //초기비용
    int m_beginCost;

    //증가비용
    int m_increaseCost;
    
    //부모여부
    Dictionary<string, int> m_parentDic;

    public string name { get { return m_name; } }
    public string key { get { return m_key; } }
    public Sprite icon { get { return m_icon; } }
    public TYPE_DEVELOP_GROUP typeDevGroup { get { return m_typeDevGroup; } }
    public TYPE_DEVELOP_VALUE_GROUP typeValueGroup { get { return m_typeValueGroup; } }
    public int maxLevel { get { return m_maxLevel; } }
    public int techLevel { get { return m_techLevel; } }
    public int position { get { return m_position; } }
    public string contents { get { return m_contents; } }
    public Dictionary<string, int> parentDic { get { return m_parentDic; } }



    public Develop(
        string key, 
        string name, 
        Sprite icon, 
        TYPE_DEVELOP_GROUP typeDevelopGroup, 
        TYPE_DEVELOP_VALUE_GROUP typeValueGroup,
        int position,
        int maxLevel, 
        int techLevel,
        float beginValue, 
        float increaseValue, 
        int beginCost, 
        int increaseCost, 
        string contents, 
        Dictionary<string, int> parentDic
        )
    {
        m_key = key;
        m_name = name;
        m_icon = icon;
        m_typeDevGroup = typeDevelopGroup;
        m_typeValueGroup = typeValueGroup;
        m_position = position;
        m_maxLevel = maxLevel;
        m_techLevel = techLevel;
        m_beginValue = beginValue;
        m_increaseValue = increaseValue;
        m_beginCost = beginCost;
        m_increaseCost = increaseCost;
        m_contents = contents;
        m_parentDic = new Dictionary<string, int>(parentDic);
    }

    public float getValue(int level)
    {
        return m_beginValue + ((float)level * m_increaseValue);
    }

    public int getCost(int level)
    {
        return m_beginCost + (level * m_increaseCost);
    }

}

