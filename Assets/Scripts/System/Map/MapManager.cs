using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class MapManager : SingletonClass<MapManager>
{

    enum TYPE_MAP_DATA { Key, Name, Effect, Contents }

    Dictionary<string, Map> m_mapDic = new Dictionary<string, Map>();

//    readonly string xPath = "Map/Data";


    public MapManager()
    {
        initParse();
    }

    void initParse()
    {

        Sprite[] mapIcons = Resources.LoadAll<Sprite>(Prep.mapIconPath);
//        Sprite[] mapBackgrounds = Resources.LoadAll<Sprite>(Prep.mapBackgroundPath);
        Sprite[] mapImages = Resources.LoadAll<Sprite>(Prep.mapImagePath);
        GameObject[] mapPrefebs = Resources.LoadAll<GameObject>(Prep.mapPrefebsPath);
//        Sprite[] mapRoads = Resources.LoadAll<Sprite>(Prep.mapRoadPath);

        TextAsset textAsset = Resources.Load<TextAsset>(Prep.mapDataPath);

        if (textAsset != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(Prep.getXmlDataPath(GetType()));

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                try
                {
                    string key = xmlNode.SelectSingleNode(TYPE_MAP_DATA.Key.ToString()).InnerText;
                    string name = xmlNode.SelectSingleNode(TYPE_MAP_DATA.Name.ToString()).InnerText;
                    string contents = xmlNode.SelectSingleNode(TYPE_MAP_DATA.Contents.ToString()).InnerText;
                    string effect = xmlNode.SelectSingleNode(TYPE_MAP_DATA.Effect.ToString()).InnerText;

                    Sprite icon = mapIcons.Where(ic => ic.name == key).SingleOrDefault();
                    if(icon == null) Prep.LogWarning(key, " 아이콘을 찾을 수 없음", GetType());

                    Sprite image = mapImages.Where(ic => ic.name == key).SingleOrDefault();
                    if (image == null) Prep.LogWarning(key, " 이미지를 찾을 수 없음", GetType());

                    //Sprite road = mapRoads.Where(ic => ic.name == key).SingleOrDefault();
                    //if (road == null) Prep.LogWarning(key, " 도로를 찾을 수 없음", GetType());

                    //Sprite background = mapBackgrounds.Where(ic => ic.name == key).SingleOrDefault();
                    //if (background == null) Prep.LogWarning(key, " 백그라운드를 찾을 수 없음", GetType());

                    GameObject prefeb = mapPrefebs.Where(data => data.name == "Map@" + key).SingleOrDefault();
                    if (prefeb == null) throw new NullReferenceException(key + "Prefeb");

                    Map map = new Map(key, name, effect, contents, image, icon, prefeb);
                    m_mapDic.Add(key, map);

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
            Prep.LogError(Prep.stageDataPath, "를 찾을 수 없음", GetType());
        }
    }

    public Map getMap(string key)
    {
        if (m_mapDic.ContainsKey(key))
        {
            return m_mapDic[key];
        }
        Prep.LogError(key, "를 찾을 수 없음", GetType());
        return null;
    }
}

