using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class DevelopManager : SingletonClass<DevelopManager>
{
    enum TYPE_DEVELOP_DATA{
        Key, 
        Name, 
        Group, 
        ValueGroup, 
        TechLevel, 
        Position, 
        MaxLevel, 
        BeginValue, 
        IncreaseValue, 
        BeginCost, 
        IncreaseCost, 
        ParentKey, 
        ParentLimit, 
        Contents
    }

    Dictionary<string, Develop> m_developDic = new Dictionary<string,Develop>();

    public IEnumerator developValues { get { return m_developDic.Values.GetEnumerator(); } }

    public DevelopManager()
    {
        initParse();
    }

    void initParse()
    {
        Sprite[] developIcons = Resources.LoadAll<Sprite>(Prep.developImagePath);

        TextAsset textAsset = Resources.Load<TextAsset>(Prep.developDataPath);

        if (textAsset != null)
        {
            

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes("Develop/Data");

            foreach (XmlNode xmlNode in xmlNodeList)
            {

                try
                {
                    string key = xmlNode.SelectSingleNode(TYPE_DEVELOP_DATA.Key.ToString()).InnerText;

                    if (key == "-")
                        continue;

                    string name = xmlNode.SelectSingleNode(TYPE_DEVELOP_DATA.Name.ToString()).InnerText;
                    Sprite icon = developIcons.Where(devIcon => devIcon.name == key).SingleOrDefault();
                    if (icon == null) Prep.LogWarning(key, "아이콘을 찾을 수 없음", GetType());

                    string typeGroupStr = xmlNode.SelectSingleNode(TYPE_DEVELOP_DATA.Group.ToString()).InnerText;
                    Develop.TYPE_DEVELOP_GROUP typeDevGroup = (Develop.TYPE_DEVELOP_GROUP)Enum.Parse(typeof(Develop.TYPE_DEVELOP_GROUP), typeGroupStr);

                    string typeValueStr = xmlNode.SelectSingleNode(TYPE_DEVELOP_DATA.ValueGroup.ToString()).InnerText;
                    Develop.TYPE_DEVELOP_VALUE_GROUP typeValueGroup = (Develop.TYPE_DEVELOP_VALUE_GROUP)Enum.Parse(typeof(Develop.TYPE_DEVELOP_VALUE_GROUP), typeValueStr);

                    int position;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_DEVELOP_DATA.Position.ToString()).InnerText, out position))
                        position = 0;

                    int techLevel = 0;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_DEVELOP_DATA.TechLevel.ToString()).InnerText, out techLevel))
                        techLevel = 0;

                    int maxLevel;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_DEVELOP_DATA.MaxLevel.ToString()).InnerText, out maxLevel))
                        maxLevel = 1;

                    float beginValue;
                    if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_DEVELOP_DATA.BeginValue.ToString()).InnerText, out beginValue))
                        beginValue = 1;

                    float increaseValue;
                    if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_DEVELOP_DATA.IncreaseValue.ToString()).InnerText, out increaseValue))
                        increaseValue = 1;

                    int beginCost;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_DEVELOP_DATA.BeginCost.ToString()).InnerText, out beginCost))
                        beginCost = 1;

                    int increaseCost;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_DEVELOP_DATA.IncreaseCost.ToString()).InnerText, out increaseCost))
                        increaseCost = 1;

                    string contents = xmlNode.SelectSingleNode(TYPE_DEVELOP_DATA.Contents.ToString()).InnerText;


                    Dictionary<string, int> parentDic = new Dictionary<string, int>();

                    for (int i = 0; i < 3; i++)
                    {
                        string parentKey = xmlNode.SelectSingleNode(string.Format("{0}{1}", TYPE_DEVELOP_DATA.ParentKey.ToString(), i + 1)).InnerText;

                        if (parentKey == "-")
                            continue;

                        int parentLimit = 0;
                        if (!int.TryParse(xmlNode.SelectSingleNode(string.Format("{0}{1}", TYPE_DEVELOP_DATA.ParentLimit.ToString(), i + 1)).InnerText, out parentLimit))
                        {
                            parentLimit = 0;
                        }

                        parentDic.Add(parentKey, parentLimit);

                    }

                    Develop develop = new Develop(
                                            key,
                                            name,
                                            icon,
                                            typeDevGroup,
                                            typeValueGroup,
                                            position,
                                            maxLevel,
                                            techLevel,
                                            beginValue,
                                            increaseValue,
                                            beginCost,
                                            increaseCost,
                                            contents,
                                            parentDic);

                    m_developDic.Add(key, develop);
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
            Prep.LogError(Prep.developDataPath, "를 찾을 수 없음", GetType());
        }
    }

    public Develop getDevelop(string key)
    {
        if (m_developDic.ContainsKey(key))
        {
            return m_developDic[key];
        }

        Prep.LogWarning(key, "를 찾을 수 없음", GetType());

        return null;
    }

    /// <summary>
    /// 종류 타입으로 가져오기
    /// </summary>
    /// <param name="typeDevGroup"></param>
    /// <returns></returns>
    public IEnumerator getDevelopEnumerator(Develop.TYPE_DEVELOP_GROUP typeDevGroup)
    {
        return m_developDic.Values.Where(dev => dev.typeDevGroup == typeDevGroup).GetEnumerator();
    }

    /// <summary>
    /// 종류의 최대 트리 계층 가져오기
    /// </summary>
    /// <param name="typeDevGroup"></param>
    /// <returns></returns>
    public int getDevelopMaxPosition(Develop.TYPE_DEVELOP_GROUP typeDevGroup)
    {
        return m_developDic.Values.Where(dev => dev.typeDevGroup == typeDevGroup).Max(dev => dev.position) + 1;
    }

    /// <summary>
    /// 값 타입으로 가져오기
    /// </summary>
    /// <param name="typeValueGroup"></param>
    /// <returns></returns>
    public IEnumerator getDevelopEnumerator(Develop.TYPE_DEVELOP_VALUE_GROUP typeValueGroup)
    {
        return m_developDic.Values.Where(dev => dev.typeValueGroup == typeValueGroup).GetEnumerator();
    }

    //해당 종류에 따라 개발 버튼 가져오기
}

