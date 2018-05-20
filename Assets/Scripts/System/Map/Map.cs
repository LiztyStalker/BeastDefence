using System;
using UnityEngine;

public enum TYPE_MAP_SIZE { Small, Medium, Large }

public class Map
{
    string m_key;
    string m_name;

    Sprite m_icon;
    Sprite m_image;
//    Sprite m_background;
//    Sprite m_road;

    GameObject m_prefebs;

    string m_contents;
    string m_effect;

    public string key { get { return m_key; } }
    public string name { get { return m_name; } }
    public string contents { get { return m_contents; } }
    public string effect { get { return m_effect; } }
//    public Sprite background { get { return m_background; } }
    public Sprite icon { get { return m_icon; } }
//    public Sprite road { get { return m_road; } }
    public Sprite image { get { return m_image; } }
    public GameObject prefebs { get { return m_prefebs; } }



    public Map(
        string key, 
        string name, 
        string effect, 
        string contents, 
        Sprite image, 
        Sprite icon,
        GameObject prefebs)
    {
        m_key = key;
        m_name = name;
        m_contents = contents;
        m_effect = effect;
//        m_background = background;
        m_icon = icon;
//        m_road = road;
        m_image = image;
        m_contents = contents;
        m_prefebs = prefebs;
    }


}

