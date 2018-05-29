using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Spine;
using Spine.Unity;


public class UnitManager : SingletonClass<UnitManager>
{

    enum TYPE_UNIT_DATA {Key, 
        Name, 
        IsActive,
        TypeUnit, 
        TypeForce,
        TypeMovement, 
        TypeTarget, 
        TypeRange,
        TypeLine,
        EffectKey,
        ProductTime,
        Population,
        Munitions,
        Health,
        IncHealth,
        Attack,
        IncAttack,
        AttackSpeed,
        IncAttackSpeed,
        MoveSpeed,
        IncMoveSpeed,
        Range,
        IncRange,
        Weight,
        Cost,
        Contents,
        Skill0,
        Skill1,
        Skill2
    };


    Dictionary<string, Unit> m_unitDic = new Dictionary<string, Unit>();

    Dictionary<string, Unit> unitDic { get { return m_unitDic; } }

    public IEnumerator units{get{return m_unitDic.Values.GetEnumerator();}}


    public UnitManager(){
        initParse();
    }

    void initParse()
    {
        GameObject[] skeletonObjectArray = Resources.LoadAll<GameObject>(Prep.unitSkeletonPath);
        Sprite[] iconArray = Resources.LoadAll<Sprite>(Prep.unitIconPath);

        TextAsset textAsset = Resources.Load<TextAsset>(Prep.unitDataPath);

        if (textAsset != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(Prep.getXmlDataPath(GetType()));

            foreach (XmlNode xmlNode in xmlNodeList)
            {

                try
                {

                    string key = xmlNode.SelectSingleNode(TYPE_UNIT_DATA.Key.ToString()).InnerText;
                    if (key == "-") continue;

                    bool isActive = false;

                    if (!bool.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.IsActive.ToString()).InnerText, out isActive))
                    {
                        Prep.LogError(key, "사용 불가", GetType());
                        continue;
                    }

                    if (!isActive)
                    {
                        Prep.LogError(key, "활성화 되지 않음", GetType());
                        continue;
                    }

                    string name = xmlNode.SelectSingleNode(TYPE_UNIT_DATA.Name.ToString()).InnerText;

                    string effectKey = xmlNode.SelectSingleNode(TYPE_UNIT_DATA.EffectKey.ToString()).InnerText;
                    
                    Sprite icon = iconArray.Where(spr => spr.name == key).SingleOrDefault();
                    if (icon == null)
                    {
                        Prep.LogError(key, "아이콘을 찾을 수 없음", GetType());
                        continue;
                    }

                    GameObject skeletonObj = skeletonObjectArray.Where(ske => ske.name == ("Skeleton@" + key)).SingleOrDefault();
                    SkeletonAnimation skeletonAnime = null;

                    if (skeletonObj != null)
                        skeletonAnime = skeletonObj.GetComponent<SkeletonAnimation>();
                    else
                    {
                        Prep.LogError(key, "스켈레톤을 찾을 수 없음", GetType());
                        continue;
                    }


                    //세력
                    TYPE_FORCE typeForce = (TYPE_FORCE)Enum.Parse(typeof(TYPE_FORCE), xmlNode.SelectSingleNode(TYPE_UNIT_DATA.TypeForce.ToString()).InnerText);

                    //유닛타입
                    Unit.TYPE_UNIT typeUnit = (Unit.TYPE_UNIT)Enum.Parse(typeof(Unit.TYPE_UNIT), xmlNode.SelectSingleNode(TYPE_UNIT_DATA.TypeUnit.ToString()).InnerText);

                    //이동방식
                    Unit.TYPE_MOVEMENT typeMovement = (Unit.TYPE_MOVEMENT)Enum.Parse(typeof(Unit.TYPE_MOVEMENT), xmlNode.SelectSingleNode(TYPE_UNIT_DATA.TypeMovement.ToString()).InnerText);

                    //타겟팅
                    Unit.TYPE_TARGETING typeTarget = (Unit.TYPE_TARGETING)Enum.Parse(typeof(Unit.TYPE_TARGETING), xmlNode.SelectSingleNode(TYPE_UNIT_DATA.TypeTarget.ToString()).InnerText);


                    //라인 공격 방식
                    Unit.TYPE_LINE typeLine = (Unit.TYPE_LINE)Enum.Parse(typeof(Unit.TYPE_LINE), xmlNode.SelectSingleNode(TYPE_UNIT_DATA.TypeLine.ToString()).InnerText);


                    //범위방식
//                    Unit.TYPE_RANGE typeRange = Unit.TYPE_RANGE.Single;
                    int typeRange = 1;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.TypeRange.ToString()).InnerText, out typeRange))
                    {
                        typeRange = 1;
                    }


                    //int i_type = 0;
                    //if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.TypeRange.ToString()).InnerText, out i_type))
                    //{
                    //    i_type = 0;
                    //}

                    //Unit.TYPE_RANGE typeRange = (Unit.TYPE_RANGE)i_type;

                    //if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.TypeLine.ToString()).InnerText, out i_type))
                    //{
                    //    i_type = 0;
                    //}

                    //Unit.TYPE_LINE typeLine = Unit.TYPE_LINE.NONE;


                    //if ((i_type & 0x4) == 0x4)
                    //    typeLine |= Unit.TYPE_LINE.AIR_SIDE;
                    //if ((i_type & 0x3) == 0x3)
                    //    typeLine |= Unit.TYPE_LINE.AIR_FORWARD;
                    //if ((i_type & 0x2) == 0x2)
                    //    typeLine |= Unit.TYPE_LINE.GND_SIDE;
                    //if ((i_type & 0x1) == 0x1)
                    //    typeLine |= Unit.TYPE_LINE.GND_FORWARD;


                    //                Debug.Log("typeLine : " + typeLine);

                    float productTime = 0f;
                    if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.ProductTime.ToString()).InnerText, out productTime))
                    {
                        productTime = 1f;
                    }


                    int population = 0;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.Population.ToString()).InnerText, out population))
                    {
                        population = 0;
                    }

                    int munitions = 0;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.Munitions.ToString()).InnerText, out munitions))
                    {
                        munitions = 0;
                    }

                    int health = 0;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.Health.ToString()).InnerText, out health))
                    {
                        health = 0;
                    }

                    int incHealth = 0;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.IncHealth.ToString()).InnerText, out incHealth))
                    {
                        incHealth = 0;
                    }

                    int attack = 0;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.Attack.ToString()).InnerText, out attack))
                    {
                        attack = 0;
                    }

                    int incAttack = 0;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.IncAttack.ToString()).InnerText, out incAttack))
                    {
                        incAttack = 0;
                    }

                    float attackSpeed = 0f;
                    if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.AttackSpeed.ToString()).InnerText, out attackSpeed))
                    {
                        attackSpeed = 0f;
                    }

                    float incAttackSpeed = 0f;
                    if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.IncAttackSpeed.ToString()).InnerText, out incAttackSpeed))
                    {
                        incAttackSpeed = 0f;
                    }

                    float moveSpeed = 0f;
                    if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.MoveSpeed.ToString()).InnerText, out moveSpeed))
                    {
                        moveSpeed = 0f;
                    }

                    float incMoveSpeed = 0f;
                    if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.IncMoveSpeed.ToString()).InnerText, out incMoveSpeed))
                    {
                        incMoveSpeed = 0f;
                    }

                    float range = 0f;
                    if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.Range.ToString()).InnerText, out range))
                    {
                        range = 0f;
                    }

                    float incRange = 0f;
                    if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.IncRange.ToString()).InnerText, out incRange))
                    {
                        incRange = 0f;
                    }

                    int weight = 0;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.Weight.ToString()).InnerText, out weight))
                    {
                        weight = 0;
                    }

                    int cost = 0;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_UNIT_DATA.Cost.ToString()).InnerText, out cost))
                    {
                        cost = 100;
                    }

                    string contents = xmlNode.SelectSingleNode(TYPE_UNIT_DATA.Contents.ToString()).InnerText;

                    Skill[] skillArray = new Skill[3];

                    for (int i = 0; i < skillArray.Length; i++)
                    {
                        string skillKey = xmlNode.SelectSingleNode(string.Format("{0}", (TYPE_UNIT_DATA.Skill0 + i))).InnerText;
                        Skill skill = SkillManager.GetInstance.getSkill(skillKey);
                        //                    Debug.Log("getSkill : " + skill + " " + key);
                        skillArray[i] = skill;
                    }
                    //skillArray[0] = "Guard";// xmlNode.SelectSingleNode(TYPE_UNIT_DATA.Skill0.ToString()).InnerText;
                    //skillArray[1] = xmlNode.SelectSingleNode(TYPE_UNIT_DATA.Skill1.ToString()).InnerText;
                    //skillArray[2] = xmlNode.SelectSingleNode(TYPE_UNIT_DATA.Skill2.ToString()).InnerText;




                    Unit unit = new Unit(
                        key,
                        name,
                        effectKey,
                        icon,
                        typeUnit,
                        typeForce,
                        typeMovement,
                        typeTarget,
                        typeRange,
                        typeLine,
                        skeletonAnime,
                        attack,
                        incAttack,
                        attackSpeed,
                        incAttackSpeed,
                        moveSpeed,
                        incMoveSpeed,
                        health,
                        incHealth,
                        range,
                        incRange,
                        weight,
                        cost,
                        munitions,
                        population,
                        productTime,
                        skillArray,
                        contents
                        );

                    m_unitDic.Add(key, unit);

                }
                catch (ArgumentException e)
                {
                    Prep.LogError(e.Message, "에 대한 오류가 발생하였습니다.", GetType());
                }
                catch (NullReferenceException e)
                {
                    Prep.LogError(e.Message, "을 찾을 수 없습니다.", GetType());
                }
            }



        }
        else
        {
            Prep.LogError(Prep.unitDataPath, "를 찾을 수 없음", GetType());
        }
    }


    public Unit getUnit(string key)
    {
        if (unitDic.ContainsKey(key))
        {
            return unitDic[key];
        }

        if(key != "-")
            Prep.LogWarning(key, "를 찾을 수 없음", GetType());

        return null;
    }

    /// <summary>
    /// 유닛 랜덤 가져오기
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public Unit getRandomUnit(int level)
    {
        int cnt = 100;
        Unit[] unitArray = unitDic.Values.ToArray<Unit>();

        while (cnt-- > 0)
        {
            int index = UnityEngine.Random.Range(0, unitDic.Count);

            if (unitArray[index].weight > 0)
            {
                return unitArray[index];
            }
        }

        return unitArray[0];

        
//        int totalWeight = unitDic.Values.Sum(unit => unit.weight);
        
        //while (enumerator.MoveNext())
        //{
        //}
    }

    /// <summary>
    /// 유닛 랜덤 가져오기
    /// 병사 한정
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public Unit getRandomUnit(int level, Unit.TYPE_UNIT typeUnit)
    {
        int cnt = 100;
        Unit[] unitArray = unitDic.Values.ToArray<Unit>();

        while (cnt-- > 0)
        {
            int index = UnityEngine.Random.Range(0, unitDic.Count);

            if(unitArray[index].typeUnit == typeUnit)
                if (unitArray[index].weight > 0)
                    return unitArray[index];
        }

        return unitArray[0];


        //        int totalWeight = unitDic.Values.Sum(unit => unit.weight);

        //while (enumerator.MoveNext())
        //{
        //}
    }

    /// <summary>
    /// 해당 유닛 키 중 한개 랜덤
    /// </summary>
    /// <param name="unitKeys"></param>
    /// <returns></returns>
    public Unit getRandomUnit(string[] unitKeys)
    {
        if (unitKeys != null)
        {
            if (unitKeys.Length > 0)
            {
                
                string unitKey = unitKeys[UnityEngine.Random.Range(0, unitKeys.Length)];
                Unit unit = getUnit(unitKey);

                if (unit != null)
                    return unit;
            }
        }

        return null;
    }


    /// <summary>
    /// 직렬화된 데이터를 변환하여 가져오기
    /// </summary>
    /// <param name="serial"></param>
    /// <returns></returns>
    public UnitCard getUnitCard(AccountUnitCardSerial serial)
    {
        if (getUnit(serial.key) != null)
        {
            return new UnitCard(serial);
        }
        return null;
    }

}

