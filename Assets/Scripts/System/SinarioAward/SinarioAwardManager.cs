using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;


public class SinarioAwardManager : SingletonClass<SinarioAwardManager>
{
    enum TYPE_SINARIOAWARD_DATA{Key, Category, Value}

//    readonly string xPath = "SinarioAward/Data";

    Dictionary<string, SinarioAward> m_sinarioAwardDic = new Dictionary<string, SinarioAward>();

    Dictionary<SinarioAward.TYPE_SINARIO_AWARD_CATEGORY, Sprite> m_sinarioSpriteDic = new Dictionary<SinarioAward.TYPE_SINARIO_AWARD_CATEGORY, Sprite>();
    


    public SinarioAwardManager()
    {
        initParse();
    }

    void initParse()
    {

        Sprite[] sinarioAwardSprites = Resources.LoadAll<Sprite>(Prep.awardCategoryImagePath);

        for (int i = 0; i < Enum.GetValues(typeof(SinarioAward.TYPE_SINARIO_AWARD_CATEGORY)).Length; i++)
        {
            if(sinarioAwardSprites.Length > i)
                m_sinarioSpriteDic.Add((SinarioAward.TYPE_SINARIO_AWARD_CATEGORY)i, sinarioAwardSprites[i]);
        }
        
        TextAsset textAsset = Resources.Load<TextAsset>(Prep.stageAwardDataPath);

        if (textAsset != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(Prep.getXmlDataPath(GetType()));


            foreach (XmlNode xmlNode in xmlNodeList)
            {
                try
                {
                    string key = xmlNode.SelectSingleNode(TYPE_SINARIOAWARD_DATA.Key.ToString()).InnerText;

                    Dictionary<SinarioAward.TYPE_SINARIO_AWARD_CATEGORY, string> sinarioDic = new Dictionary<SinarioAward.TYPE_SINARIO_AWARD_CATEGORY, string>();

                    //                Debug.LogWarning("childCnt : " + xmlNode.ChildNodes.Count);

                    for (int i = 1; i < xmlNode.ChildNodes.Count; i++)
                    {

                        string categoryKey = xmlNode.SelectSingleNode(string.Format("{0}{1}", TYPE_SINARIOAWARD_DATA.Category, ((i - 1) / 2))).InnerText;

                        if (categoryKey != "-")
                        {

                            //전용 카테고리 필요 - 

                            SinarioAward.TYPE_SINARIO_AWARD_CATEGORY typeCategory = (SinarioAward.TYPE_SINARIO_AWARD_CATEGORY)Enum.Parse(typeof(SinarioAward.TYPE_SINARIO_AWARD_CATEGORY), xmlNode.SelectSingleNode(string.Format("{0}{1}", TYPE_SINARIOAWARD_DATA.Category, ((i - 1) / 2))).InnerText);
                            i++;
                            string value = xmlNode.SelectSingleNode(string.Format("{0}{1}", TYPE_SINARIOAWARD_DATA.Value, ((i - 1) / 2))).InnerText;
                            sinarioDic.Add(typeCategory, value);

                        }
                    }

                    if (sinarioDic.Count > 0)
                    {
                        SinarioAward sinarioAward = new SinarioAward(key, sinarioDic);
                        m_sinarioAwardDic.Add(key, sinarioAward);
                    }
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
    }

    public SinarioAward getSinarioAward(string sinarioKey)
    {
        if (m_sinarioAwardDic.ContainsKey(sinarioKey))
        {
            return m_sinarioAwardDic[sinarioKey];
        }
        return null;
    }

    public Sprite getSinarioAwardSprite(SinarioAward.TYPE_SINARIO_AWARD_CATEGORY typeCategory)
    {
        if (m_sinarioSpriteDic.ContainsKey(typeCategory))
        {
            return m_sinarioSpriteDic[typeCategory];
        }
        return null;
    }
}

