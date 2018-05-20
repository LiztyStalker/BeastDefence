using UnityEngine;


public class World
{
    //키
    //세계이름
    //아이콘

    string m_key;
    string m_name;
    Sprite m_icon;
    Sprite m_image;
    Vector2 m_pos;

    public string key { get { return m_key; } }
    public string name { get { return m_name; } }
    public Sprite icon { get { return m_icon; } }
    public Sprite image { get { return m_image; } }
    public Vector2 pos { get { return m_pos; } }

    public World(string key, string name, Vector2 pos, Sprite icon, Sprite image)
    {
        m_key = key;
        m_name = name;
        m_icon = icon;
        m_image = image;
        m_pos = pos;
    }
}

