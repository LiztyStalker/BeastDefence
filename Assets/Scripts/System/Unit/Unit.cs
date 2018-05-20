using System;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;


public class Unit : IUnit{

    [Flags]
    public enum TYPE_LINE{
        NONE = 0x0,

        GND_FORWARD = 0x1,
        GND_SIDE = 0x2,
        GND_ALL = GND_FORWARD | GND_SIDE,

        AIR_FORWARD = 0x4,
        AIR_SIDE = 0x8,
        AIR_ALL = AIR_FORWARD | AIR_SIDE,

        ALL = GND_ALL | AIR_ALL,

        ALL_FORWARD = GND_FORWARD | AIR_FORWARD
    }

    public enum TYPE_UNIT{Soldier, Hero, Defence, Building, Skill}

    //이동 형식
    public enum TYPE_MOVEMENT { Gnd, Air }

    //타겟팅 형식
    public enum TYPE_TARGETING{Melee, Range}

//    public enum TYPE_RANGE{Single, Multiple}


    //키
    string m_key;

    //이름
    string m_name;

    //효과 키
    string m_effectKey;

    //아이콘
    Sprite m_icon;

    //병사타입
    TYPE_UNIT m_typeUnit;

    //세력
    TYPE_FORCE m_typeForce;

    //이동타입
    TYPE_MOVEMENT m_typeMovement;

    //공격타입
    TYPE_TARGETING m_typeTargeting;

    //범위타입 -
    //0 - 전체 
    //1 - 1명
    //n - n명
    public int m_typeRange = 1;

    //라인타입
    TYPE_LINE m_typeLine;

    //갑옷

    //스켈레톤
    SkeletonAnimation m_skeletonAnimation;

    //공격력
    int m_attack;

    //공격력 증가량
    int m_increaseAttack;

    //공격속도
    float m_attackSpeed;

    //공격속도 증가량
    float m_increaseAttackSpeed;

    //이동속도
    float m_moveSpeed;

    //이동속도 증가량
    float m_increaseMoveSpeed;

    //체력
    int m_health;

    //체력 증가량
    int m_increaseHealth;

    //사정거리
    float m_range;

    //사정거리 증가량
    float m_increaseRange;

    //군수품
    int m_munitions;

    //인구
    int m_population;
    
    //생산 쿨타임
    float m_productTime;

    //가중치
    int m_weight;

    //비용
    int m_cost;


    //스킬리스트    
    Skill[] m_skillArray;

    //설명
    string m_contents;

    public string key { get { return m_key; } }
    public string name { get { return m_name; } }
    public string effectKey { get { return m_effectKey; } }

    public SkeletonAnimation skeletonAnimation { get { return m_skeletonAnimation; } }

    public TYPE_UNIT typeUnit { get { return m_typeUnit; } }
    public TYPE_FORCE typeForce { get { return m_typeForce; } }
    public TYPE_MOVEMENT typeMovement { get { return m_typeMovement; } }
    public TYPE_TARGETING typeTargeting { get { return m_typeTargeting; } }
    public int typeRange { get { return m_typeRange; } }
//    public TYPE_RANGE typeRange { get { return m_typeRange; } }
    public TYPE_LINE typeLine { get { return m_typeLine; } }

    public int attack { get { return m_attack; } }
    public int increaseAttack { get { return m_increaseAttack; } }

    public float attackSpeed { get { return m_attackSpeed; } }
    public float increaseAttackSpeed { get { return m_increaseAttackSpeed; } }

    public float moveSpeed { get { return m_moveSpeed; } }
    public float increaseMoveSpeed { get { return m_increaseMoveSpeed; } }

    public int health { get { return m_health; } }
    public int increaseHealth { get { return m_increaseHealth; } }

    public float range { get { return m_range; } }
    public float increaseRange { get { return m_increaseRange; } }

    public Sprite icon { get { return m_icon; } }

    public int munitions { get { return m_munitions; } }
    public int population { get { return m_population; } }

    public int weight { get { return m_weight; } }

    public int cost { get { return m_cost; } }

    public float waitTime { get { return m_productTime; } }

    public Skill[] skillArray { get { return m_skillArray; } }

    public string contents { get { return m_contents; } }

    public Unit(
        string key,
        string name,
        string effectKey,
        Sprite icon,
        TYPE_UNIT typeUnit,
        TYPE_FORCE typeForce,
        TYPE_MOVEMENT typeMovement,
        TYPE_TARGETING typeTargeting,
        int typeRange,
        TYPE_LINE typeLine,
        SkeletonAnimation skeletonAnimation,
        int attack,
        int increaseAttack,
        float attackSpeed,
        float increaseAttackSpeed,
        float moveSpeed,
        float increaseMoveSpeed,
        int health,
        int increaseHealth,
        float range,
        float increaseRange,
        int weight,
        int cost,
        int munisions,
        int population,
        float productTime,
        Skill[] skillArray,
        string contents
        )
    {

        m_key = key;
        m_name = name;
        m_effectKey = effectKey;
        m_icon = icon;
        m_typeForce = typeForce;
        m_typeMovement = typeMovement;
        m_typeUnit = typeUnit;
        m_typeTargeting = typeTargeting;
        m_typeRange = typeRange;
        m_typeLine = typeLine;
        m_skeletonAnimation = skeletonAnimation;
        m_attack = attack;
        m_increaseAttack = increaseAttack;
        m_attackSpeed = attackSpeed;
        m_increaseAttackSpeed = increaseAttackSpeed;
        m_health = health;
        m_increaseHealth = increaseHealth;
        m_moveSpeed = moveSpeed;
        m_increaseMoveSpeed = increaseMoveSpeed;
        m_range = range;
        m_increaseRange = increaseRange;
        m_weight = weight;
        m_cost = cost;
        m_munitions = munisions;
        m_population = population;
        m_productTime = productTime;
        m_skillArray = (Skill[])skillArray.Clone();
        m_contents = contents;
    }

    /// <summary>
    /// 유닛 탐색 알고리즘 가져오기
    /// </summary>
    /// <returns></returns>
    //public IUnitSearch getUnitSearch()
    //{
    //    return new RangeUnitSearch();

    //    switch (typeRange)
    //    {
    //        case TYPE_RANGE.SINGLE:
    //            return new DirectUnitSearch();
    //        case TYPE_RANGE.MULTIPLE:
    //            return new RangeUnitSearch();
    //        default:
    //            Debug.LogWarning(string.Format("{0} 를 지정하지 않았음", typeRange));
    //            break;
    //    }
    //    return null;
    //}
}
