using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class AchieveManager : SingletonClass<AchieveManager>
{
    enum TYPE_ACHIEVE_DATA{Key, Name, TypeCategory, TypeAchieve, AchieveValue, TypeAward, AwardValue, Contents}
    
//    readonly string xPath = "Achieve/Data";

    Dictionary<string, Achieve> m_achieveDic = new Dictionary<string, Achieve>();


    Dictionary<TYPE_ACCOUNT_CATEGORY, Sprite> defaultSpriteDic = new Dictionary<TYPE_ACCOUNT_CATEGORY, Sprite>();

    /// <summary>
    /// 기본 계정 리소스 초기화
    /// </summary>
    public void setDefaultSprite()
    {
        Sprite[] defaultSprites = Resources.LoadAll<Sprite>("Image/Default/TypeAccCategory");
        for (int i = 0; i < Enum.GetValues(typeof(TYPE_ACCOUNT_CATEGORY)).Length; i++)
        {
            if (defaultSprites.Length > i)
                defaultSpriteDic.Add((TYPE_ACCOUNT_CATEGORY)i, defaultSprites[i]);
            else
                defaultSpriteDic.Add((TYPE_ACCOUNT_CATEGORY)i, null);
        }
    }

    /// <summary>
    /// 계정 카테고리 별 기본 스프라이트 가져오기
    /// </summary>
    /// <param name="typeAccCategory"></param>
    /// <returns></returns>
    public Sprite getTypeCategorySprite(TYPE_ACCOUNT_CATEGORY typeAccCategory)
    {
        if (defaultSpriteDic.ContainsKey(typeAccCategory))
        {
            return defaultSpriteDic[typeAccCategory];
        }
        Debug.LogWarning(typeAccCategory + "못 찾음");
        return null;
    }

    //public IEnumerator values { get { return m_achieveDic.Values.GetEnumerator(); } }

    /// <summary>
    /// 해당 업적 리스트 가져오기
    /// 항목
    /// </summary>
    /// <param name="typeAchieveCategory"></param>
    /// <returns></returns>
    public IEnumerator getAchieveList(Achieve.TYPE_ACHIEVE_CATEGORY typeAchieveCategory)
    {
        return m_achieveDic.Values.Where(ach => ach.typeAchieveCategory == typeAchieveCategory).GetEnumerator();
    }

    /// <summary>
    /// 해당 업적 리스트 가져오기
    /// 데이터
    /// </summary>
    /// <param name="typeAchieve"></param>
    /// <returns></returns>
    public IEnumerator getAchieveList(Achieve.TYPE_ACHIEVE typeAchieve)
    {
        return m_achieveDic.Values.Where(ach => ach.typeAchieve == typeAchieve).GetEnumerator();
    }

    public AchieveManager()
    {
        initParse();
        setDefaultSprite();
    }

    void initParse()
    {
        Sprite[] achieveIcons = Resources.LoadAll<Sprite>(Prep.achieveImagePath);


        TextAsset textAsset = Resources.Load<TextAsset>(Prep.achieveDataPath);

        if (textAsset != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(Prep.getXmlDataPath(GetType()));

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                try
                {
                    string key = xmlNode.SelectSingleNode(TYPE_ACHIEVE_DATA.Key.ToString()).InnerText;
                    string name = xmlNode.SelectSingleNode(TYPE_ACHIEVE_DATA.Name.ToString()).InnerText;
                    string contents = xmlNode.SelectSingleNode(TYPE_ACHIEVE_DATA.Contents.ToString()).InnerText;

                    Sprite icon = achieveIcons.Where(data => data.name == key).SingleOrDefault();
                    if (icon == null) Prep.LogWarning(key, "아이콘을 찾을 수 없음", GetType());
                    
                    string typeCategory = xmlNode.SelectSingleNode(TYPE_ACHIEVE_DATA.TypeCategory.ToString()).InnerText;
                    Achieve.TYPE_ACHIEVE_CATEGORY typeAchieveCategory = (Achieve.TYPE_ACHIEVE_CATEGORY)Enum.Parse(typeof(Achieve.TYPE_ACHIEVE_CATEGORY), typeCategory);

                    string typeStr = xmlNode.SelectSingleNode(TYPE_ACHIEVE_DATA.TypeAchieve.ToString()).InnerText;
                    Achieve.TYPE_ACHIEVE typeAchieve = (Achieve.TYPE_ACHIEVE)Enum.Parse(typeof(Achieve.TYPE_ACHIEVE), typeStr);

                    int value = 0;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_ACHIEVE_DATA.AchieveValue.ToString()).InnerText, out value))
                        value = 0;

                    string typeAwardStr = xmlNode.SelectSingleNode(TYPE_ACHIEVE_DATA.TypeAward.ToString()).InnerText;
                    TYPE_ACCOUNT_CATEGORY typeAward = (TYPE_ACCOUNT_CATEGORY)Enum.Parse(typeof(TYPE_ACCOUNT_CATEGORY), typeAwardStr);

                    int awardValue = 0;
                    if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_ACHIEVE_DATA.AwardValue.ToString()).InnerText, out awardValue))
                        awardValue = 0;


                    Achieve achieve = new Achieve(key, name, icon, typeAchieveCategory, typeAchieve, value, typeAward, awardValue, contents);

                    m_achieveDic.Add(key, achieve);

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
            Prep.LogError(Prep.achieveDataPath, "를 찾을 수 없음", GetType());
        }
    }

    /// <summary>
    /// 업적 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Achieve getAchieve(string key)
    {
        if (m_achieveDic.ContainsKey(key))
        {
            return m_achieveDic[key];
        }
        Prep.LogWarning(key, "를 찾을 수 없음", GetType());

        return null;
    }

}

