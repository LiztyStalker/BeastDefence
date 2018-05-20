using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using UnityEngine;



public class WorldManager : SingletonClass<WorldManager>
{
    enum TYPE_WORLD_DATA { Key, Name, X, Y}
    enum TYPE_AREA_DATA { Key, WorldKey, Name, X, Y}
    enum TYPE_STAGE_DATA{Key, ParentKey, Name, AreaKey, TypeStage, MapKey, MapSize, TypeForce, TypeMode, Requester, Contents}

    readonly string xWorldPath = "World/Data";
    readonly string xAreaPath = "Area/Data";
    readonly string xStagePath = "Stage/Data";

    Dictionary<string, World> m_worldDic = new Dictionary<string, World>();
    Dictionary<string, Area> m_areaDic = new Dictionary<string, Area>();
    Dictionary<string, Stage> m_stageDic = new Dictionary<string, Stage>();


    public IEnumerator worldValues { get { return m_worldDic.Values.GetEnumerator(); } }
    public IEnumerator areaValues { get { return m_areaDic.Values.GetEnumerator(); } }
    public IEnumerator stageValues { get { return m_stageDic.Values.GetEnumerator(); } }

    public WorldManager()
    {
        initWorldParse();
        initAreaParse();
        initStageParse();
    }


    void initWorldParse()
    {
        Sprite[] images = Resources.LoadAll<Sprite>(Prep.worldImagePath);
        Sprite[] icons = Resources.LoadAll<Sprite>(Prep.worldIconPath);

        TextAsset textAsset = Resources.Load<TextAsset>(Prep.worldDataPath);

        if (textAsset != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xWorldPath);

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                string key = xmlNode.SelectSingleNode(TYPE_WORLD_DATA.Key.ToString()).InnerText;
                string name = xmlNode.SelectSingleNode(TYPE_WORLD_DATA.Name.ToString()).InnerText;

                Sprite icon = icons.Where(tIcon => tIcon.name == key).SingleOrDefault();
                if (icon == null) Prep.LogWarning(key, "아이콘을 찾을 수 없음", GetType());

                Sprite image = images.Where(tIcon => tIcon.name == key).SingleOrDefault();
                if (image == null) Prep.LogWarning(key, "아이콘을 찾을 수 없음", GetType());

                Vector2 pos = Vector2.zero;
                if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_WORLD_DATA.X.ToString()).InnerText, out pos.x))
                    pos.x = 0f;

                if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_WORLD_DATA.Y.ToString()).InnerText, out pos.y))
                    pos.y = 0f;


                World world = new World(key, name, pos, icon, image);
                m_worldDic.Add(key, world);

            }
        }
        else
        {
            Prep.LogError(Prep.worldDataPath, "를 찾을 수 없음", GetType());
        }
    }

    void initAreaParse()
    {
        Sprite[] stageIcons = Resources.LoadAll<Sprite>(Prep.areaImagePath);

        TextAsset textAsset = Resources.Load<TextAsset>(Prep.areaDataPath);

        if (textAsset != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xAreaPath);

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                string key = xmlNode.SelectSingleNode(TYPE_AREA_DATA.Key.ToString()).InnerText;
                string name = xmlNode.SelectSingleNode(TYPE_AREA_DATA.Name.ToString()).InnerText;
                string worldKey = xmlNode.SelectSingleNode(TYPE_AREA_DATA.WorldKey.ToString()).InnerText;

                Sprite icon = stageIcons.Where(tIcon => tIcon.name == key).SingleOrDefault();
                if (icon == null) Prep.LogWarning(key, "아이콘을 찾을 수 없음", GetType());

                Vector2 pos = Vector2.zero;
                if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_AREA_DATA.X.ToString()).InnerText, out pos.x))
                    pos.x = 0f;

                if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_AREA_DATA.Y.ToString()).InnerText, out pos.y))
                    pos.y = 0f;


                Area area = new Area(key, name, worldKey, pos, icon);
                m_areaDic.Add(key, area);

            }
        }
        else
        {
            Prep.LogError(Prep.areaDataPath, "를 찾을 수 없음", GetType());
        }
    }

    void initStageParse()
    {
        Sprite[] stageIcons = Resources.LoadAll<Sprite>(Prep.stageImagePath);

        TextAsset textAsset = Resources.Load<TextAsset>(Prep.stageDataPath);

        if (textAsset != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xStagePath);

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                try
                {
                    string key = xmlNode.SelectSingleNode(TYPE_STAGE_DATA.Key.ToString()).InnerText;
                    string parentKey = xmlNode.SelectSingleNode(TYPE_STAGE_DATA.ParentKey.ToString()).InnerText;
                    string name = xmlNode.SelectSingleNode(TYPE_STAGE_DATA.Name.ToString()).InnerText;
                    string areaKey = xmlNode.SelectSingleNode(TYPE_STAGE_DATA.AreaKey.ToString()).InnerText;

                    Sprite icon = stageIcons.Where(tIcon => tIcon.name == key).SingleOrDefault();
                    if (icon == null) Prep.LogWarning(key, "아이콘을 찾을 수 없음", GetType());

                    //Vector2 pos = Vector2.zero;
                    //if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_STAGE_DATA.X.ToString()).InnerText, out pos.x))
                    //    pos.x = 0f;

                    //if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_STAGE_DATA.Y.ToString()).InnerText, out pos.y))
                    //    pos.y = 0f;

                    string mapKey = xmlNode.SelectSingleNode(TYPE_STAGE_DATA.MapKey.ToString()).InnerText;

                    TYPE_MAP_SIZE typeMapSize = (TYPE_MAP_SIZE)Enum.Parse(typeof(TYPE_MAP_SIZE), xmlNode.SelectSingleNode(TYPE_STAGE_DATA.MapSize.ToString()).InnerText);

                    TYPE_FORCE typeForce = (TYPE_FORCE)Enum.Parse(typeof(TYPE_FORCE), xmlNode.SelectSingleNode(TYPE_STAGE_DATA.TypeForce.ToString()).InnerText);

                    Stage.TYPE_MODE typeMode = (Stage.TYPE_MODE)Enum.Parse(typeof(Stage.TYPE_MODE), xmlNode.SelectSingleNode(TYPE_STAGE_DATA.TypeMode.ToString()).InnerText);

                    Stage.TYPE_STAGE typeStage = (Stage.TYPE_STAGE)Enum.Parse(typeof(Stage.TYPE_STAGE), xmlNode.SelectSingleNode(TYPE_STAGE_DATA.TypeStage.ToString()).InnerText);

                    string requester = xmlNode.SelectSingleNode(TYPE_STAGE_DATA.Requester.ToString()).InnerText;
                    string contents = xmlNode.SelectSingleNode(TYPE_STAGE_DATA.Contents.ToString()).InnerText;

                    Deck deck = DeckManager.GetInstance.getDeck(key);

                    Stage stage = new Stage(key, parentKey, name, areaKey, mapKey, typeMapSize, typeForce, typeMode, typeStage, icon, requester, contents, deck);
                    m_stageDic.Add(key, stage);

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

    
    /// <summary>
    /// 스테이지 가져오기
    /// </summary>
    /// <param name="stageKey"></param>
    /// <returns></returns>
    public Stage getStage(string stageKey)
    {
        if (m_stageDic.ContainsKey(stageKey))
        {
            return m_stageDic[stageKey];
        }
        Prep.LogWarning(stageKey, "를 찾을 수 없음", GetType());
        return null;
    }

    /// <summary>
    /// 스테이지 개수 가져오기
    /// </summary>
    /// <param name="stageKey"></param>
    /// <returns></returns>
    public int getAreaInStageCount(string areaKey)
    {
        return m_stageDic.Values.Where(stage => stage.areaKey == areaKey).Count();
    }

    /// <summary>
    /// 지역에 맞는 스테이지 가져오기
    /// </summary>
    /// <param name="area"></param>
    /// <returns></returns>
    public IEnumerator getStageEnumerator(string areaKey){
        Stage[] stageArray = m_stageDic.Values.Where(stage => stage.areaKey == areaKey).ToArray<Stage>();
        if (stageArray == null)
            return null;
        return stageArray.GetEnumerator();
    }

    /// <summary>
    /// 지역 가져오기
    /// </summary>
    /// <param name="areaKey"></param>
    /// <returns></returns>
    public Area getArea(string areaKey)
    {
        if (m_areaDic.ContainsKey(areaKey))
        {
            return m_areaDic[areaKey];
        }
        Prep.LogWarning(areaKey, "를 찾을 수 없음", GetType());
        return null;
    }


    /// <summary>
    /// 월드에 맞는 지역 가져오기
    /// 
    /// </summary>
    /// <param name="area"></param>
    /// <returns></returns>
    public IEnumerator getAreaEnumerator(string worldKey)
    {
        Area[] areaArray = m_areaDic.Values.Where(area => area.worldKey == worldKey).ToArray<Area>();

        if (areaArray == null)
            return null;
        return areaArray.GetEnumerator();
    }

    /// <summary>
    /// 월드 가져오기
    /// </summary>
    /// <param name="worldKey"></param>
    /// <returns></returns>
    public World getWorld(string worldKey)
    {
        if (m_worldDic.ContainsKey(worldKey))
        {
            return m_worldDic[worldKey];
        }
        Prep.LogWarning(worldKey, "를 찾을 수 없음", GetType());
        return null;
    }

    /// <summary>
    /// 다음 스테이지 키 가져오기
    /// </summary>
    /// <param name="nowStageKey">클리어한 스테이지 키</param>
    /// <returns></returns>
    public string getNextStageKey(string nowStageKey)
    {
        IEnumerator enumerator = stageValues;
        while (enumerator.MoveNext())
        {
            Stage enumStage = enumerator.Current as Stage;

            Debug.Log("enumStage : " + enumStage.key);

            //조건키가 클리어키가 같으면 반환 - 단, 메인키 여야함
            if(enumStage.parentKey == nowStageKey)
                if(enumStage.typeStage == Stage.TYPE_STAGE.Main)
                    return enumStage.key;

        }
        //찾지 못했으면
        return "-";
    }

    public string getFirstStageKey()
    {
        return m_stageDic.Values.ToArray<Stage>()[0].key;
    }
}

