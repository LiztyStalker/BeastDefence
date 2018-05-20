using UnityEngine;


public class Area
{
    //지역키
    string m_key;

    //지역명
    string m_name;

    //위치
    Vector2 m_pos;
    
    //아이콘
    Sprite m_icon;

    //월드키
    string m_worldKey;

    public string key { get { return m_key; } }
    public string name { get { return m_name; } }
    public Vector2 pos { get { return m_pos; } }
    public Sprite icon { get { return m_icon; } }
    public string worldKey { get { return m_worldKey; } }

    public Area(string key, string name, string worldKey, Vector2 pos, Sprite icon)
    {
        m_key = key;
        m_name = name;
        m_worldKey = worldKey;
        m_pos = pos;
        m_icon = icon;
    }
}

