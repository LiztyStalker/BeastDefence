using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using UnityEngine;
using Defence.CharacterPackage;

public class ContentsManager : SingletonClass<ContentsManager>
{
    enum TYPE_CONTENTS_DATA { Key, ParentKey, StageKey, TypeEvent, TypePos, Character, TypeFace, Contents }

    Dictionary<string, List<Contents> > m_contentsDic = new Dictionary<string, List<Contents> > ();
    

    public ContentsManager()
    {
        initParse();
    }

    void initParse()
    {

        Sprite[] sprites = Resources.LoadAll<Sprite>(Prep.contentsImagePath);


        TextAsset textAsset = Resources.Load<TextAsset>(Prep.contentsDataPath);

        if (textAsset != null)
        {
            


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(Prep.getXmlDataPath(GetType()));

            foreach (XmlNode xmlNode in xmlNodeList)
            {

                try
                {
                    string key = xmlNode.SelectSingleNode(TYPE_CONTENTS_DATA.Key.ToString()).InnerText;

                    if (key == "-")
                        continue;

                    string parentKey = xmlNode.SelectSingleNode(TYPE_CONTENTS_DATA.ParentKey.ToString()).InnerText;
                    if (parentKey == "-") parentKey = "";

                    string stageKey = xmlNode.SelectSingleNode(TYPE_CONTENTS_DATA.StageKey.ToString()).InnerText;

                    //string eventClass = xmlNode.SelectSingleNode(TYPE_CONTENTS_DATA.Event.ToString()).InnerText;

                    Contents.TYPE_CONTENTS_EVENT typeEvent =
                        (Contents.TYPE_CONTENTS_EVENT)Enum.Parse(
                        typeof(Contents.TYPE_CONTENTS_EVENT),
                        xmlNode.SelectSingleNode(TYPE_CONTENTS_DATA.TypeEvent.ToString()).InnerText
                        );

                    Contents.TYPE_CONTENTS_POS typePos = 
                        (Contents.TYPE_CONTENTS_POS)Enum.Parse(
                        typeof(Contents.TYPE_CONTENTS_POS), 
                        xmlNode.SelectSingleNode(TYPE_CONTENTS_DATA.TypePos.ToString()).InnerText
                        );

                    string character = xmlNode.SelectSingleNode(TYPE_CONTENTS_DATA.Character.ToString()).InnerText;

//                    string typeFace = xmlNode.SelectSingleNode(TYPE_CONTENTS_DATA.TypeFace.ToString()).InnerText;
                    Character.TYPE_FACE typeFace = (Character.TYPE_FACE)Enum.Parse(typeof(Character.TYPE_FACE), xmlNode.SelectSingleNode(TYPE_CONTENTS_DATA.TypeFace.ToString()).InnerText);


                    string contentsStr = xmlNode.SelectSingleNode(TYPE_CONTENTS_DATA.Contents.ToString()).InnerText;

                    Sprite image = sprites.Where(spr => spr.name == key).SingleOrDefault();

                    Contents contents = new Contents(
                                            key,
                                            parentKey,
                                            stageKey,
                                            character,
                                            contentsStr,
                                            typeFace,
                                            image,
                                            typeEvent,
                                            typePos
                                            );

                    if(!m_contentsDic.ContainsKey(contents.stageKey))
                        m_contentsDic.Add(contents.stageKey, new List<Contents>());

                    m_contentsDic[contents.stageKey].Add(contents);
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
            Prep.LogError(Prep.contentsDataPath, "를 찾을 수 없음", GetType());
        }

    }

    /// <summary>
    /// 대사 가져오기
    /// </summary>
    /// <param name="stageKey"></param>
    /// <returns></returns>
    public List<Contents> getContents(string stageKey, Contents.TYPE_CONTENTS_EVENT typeEvent)
    {
        if (m_contentsDic.ContainsKey(stageKey))
        {
            List<Contents> contentsList = m_contentsDic[stageKey].Where(cont => cont.typeContentsEvent == typeEvent).ToList<Contents>();
            List<Contents> tmpContentsList = new List<Contents>();

            foreach(Contents contents in contentsList)
            {
                //부모키가 등록되어 있거나 부모키가 없으면 등록
                if(contents.parentKey == "" ||
                    Account.GetInstance.accSinario.isContents(contents.parentKey))
                {
                    tmpContentsList.Add(contents);
                }
            }
            return tmpContentsList;
        }

        Prep.LogError(stageKey, "를 찾을 수 없음", GetType());
        return null;
    }

    ///// <summary>
    ///// 캐릭터 얼굴 가져오기
    ///// </summary>
    ///// <param name="key"></param>
    ///// <param name="typeFace"></param>
    ///// <returns></returns>
    //public Sprite getCharacterFace(string key, Contents.TYPE_FACE typeFace)
    //{
    //    if (m_characterFaceDic.ContainsKey(key))
    //    {
    //        return m_characterFaceDic[key][(int)typeFace];
    //    }
    //    return null;
    //}
}

