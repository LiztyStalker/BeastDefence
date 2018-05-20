using UnityEngine;

public class Tip
{
    string m_key;
//    string m_name;
    Sprite m_icon;
    string m_contents;

    public string key { get { return m_key; } }
//    public string name { get { return m_name; } }
    public string contents { get { return m_contents; } }
    public Sprite icon { get { return m_icon; } }

    public Tip(string key, Sprite icon, string contents)
    {
        m_key = key;
//        m_name = name;
        m_icon = icon;
        m_contents = contents;
    }
}

