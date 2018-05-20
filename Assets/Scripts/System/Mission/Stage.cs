//using System.Collections.Generic;
using UnityEngine;

public class Stage
{

    public enum TYPE_MODE {
        Annihilation, //섬멸
        Assault, //공격
        Defence //방어
    }

    public enum TYPE_STAGE
    {
        Main,
        Normal,
        Warning,
        Infinite
    }

    string m_key;
    string m_parentKey;
    string m_name;
    string m_areaKey;

    string m_mapKey;
    TYPE_MAP_SIZE m_typeMapSize;
    TYPE_FORCE m_typeForce;
    TYPE_MODE m_typeMode;
    TYPE_STAGE m_typeStage;

    string m_requesterKey;
    string m_contents;

    Sprite m_icon;
//    Vector2 m_coordinate;
    Deck m_deck;


    public string key { get { return m_key; } }
    public string parentKey { get { return m_parentKey; } }
    public string name { get { return m_name; } }
    public string areaKey { get { return m_areaKey; } }

    public Sprite icon { get { return m_icon; } }
 //   public Vector2 coordinate { get { return m_coordinate; } }

    public string mapKey { get { return m_mapKey; } }
    public TYPE_MAP_SIZE typeMapSize { get { return m_typeMapSize; } }
    public TYPE_FORCE typeForce { get { return m_typeForce; } }
    public TYPE_MODE typeMode { get { return m_typeMode; } }
    public TYPE_STAGE typeStage { get { return m_typeStage; } }

    public Deck deck { get { return m_deck; } }

    public string requesterKey { get { return m_requesterKey; } }
    public string contents { get { return m_contents; } }


    public Stage()
    {
        m_deck = new Deck();
        m_typeMapSize = TYPE_MAP_SIZE.Small;
    }

    public Stage(
        string key, 
        string parentKey,
        string name, 
        string areaKey, 
        string mapKey, 
        TYPE_MAP_SIZE typeMapSize, 
        TYPE_FORCE typeForce, 
        TYPE_MODE typeMode,
        TYPE_STAGE typeStage,
        Sprite icon, 
//        Vector2 pos, 
        string requesterKey, 
        string contents, 
        Deck deck)
    {
        m_key = key;
        m_parentKey = parentKey;
        m_name = name;
        m_areaKey = areaKey;
        m_icon = icon;
//        m_coordinate = pos;
        m_requesterKey = requesterKey;
        m_contents = contents;
        m_deck = deck;
        m_mapKey = mapKey;
        m_typeMapSize = typeMapSize;
        m_typeForce = typeForce;
        m_typeMode = typeMode;
        m_typeStage = typeStage;

    }

}

