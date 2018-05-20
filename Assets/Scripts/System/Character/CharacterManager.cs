using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;


namespace Defence.CharacterPackage
{
    public class CharacterManager : SingletonClass<CharacterManager>
    {

        enum TYPE_CHARACTER_DATA { Key, Name}

        Dictionary<string, Character> m_dataDic = new Dictionary<string, Character>();

//        Dictionary<string, List<Sprite>> m_characterFaceDic = new Dictionary<string, List<Sprite>>();



        public CharacterManager()
        {
            initParse();
        }

        void initParse()
        {

            //DirectoryInfo dInfo = new DirectoryInfo(Prep.characterFacePath);
            //foreach (DirectoryInfo dData in dInfo.GetDirectories())
            //{
            //    Sprite[] faceSprites = Resources.LoadAll<Sprite>(Prep.characterFacePath + "/" + dData.Name);
            //    m_characterFaceDic.Add(dData.Name, new List<Sprite>());
            //    foreach (Sprite spr in faceSprites)
            //    {
            //        m_characterFaceDic[dData.Name].Add(spr);
            //    }
            //}

            Sprite[] icons = Resources.LoadAll<Sprite>(Prep.characterIconPath);

            TextAsset textAsset = Resources.Load<TextAsset>(Prep.characterDataPath);

            if (textAsset != null)
            {


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(textAsset.text);
                
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes(Prep.getXmlDataPath(GetType()));

                foreach (XmlNode xmlNode in xmlNodeList)
                {

                    try
                    {
                        string key = xmlNode.SelectSingleNode(TYPE_CHARACTER_DATA.Key.ToString()).InnerText;

                        if (key == "-")
                            continue;

                        string name = xmlNode.SelectSingleNode(TYPE_CHARACTER_DATA.Name.ToString()).InnerText;
                        

//                        Sprite icon = icons.Where(ic => ic.name == key).SingleOrDefault();
//                        if(icon == null) Prep.LogError(key, "아이콘을 찾을 수 없음", GetType());

                        Dictionary<Character.TYPE_FACE, Sprite> faceDic = new Dictionary<Character.TYPE_FACE, Sprite>();

                        for (int i = 0; i < Enum.GetValues(typeof(Character.TYPE_FACE)).Length; i++)
                        {
                            string faceKey = key.Replace("Char", i.ToString());
                            Sprite face = icons.Where(ic => ic.name == faceKey).SingleOrDefault();

                            if (face == null)
                            {
                                Prep.LogWarning(faceKey, "캐릭터의 얼굴이 없습니다.", GetType());
                                faceDic.Add((Character.TYPE_FACE)i, null);
                            }
                            else
                                faceDic.Add((Character.TYPE_FACE)i, face);
                        }

                        Sprite icon = faceDic[Character.TYPE_FACE.Normal];
                        
                        Character data = new Character(
                                                key,
                                                icon,
                                                name,
                                                faceDic
                                                );

                        Debug.Log("key : " + key);

                        m_dataDic.Add(key, data);
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
                Prep.LogError(Prep.characterDataPath, "를 찾을 수 없음", GetType());
            }
        }

        //지휘관 가져오기
        public Character getCharacter(string key)
        {
            if (m_dataDic.ContainsKey(key))
            {
                return m_dataDic[key];
            }

            Prep.LogError(key, "를 찾을 수 없습니다", GetType());
            return null;
        }

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


