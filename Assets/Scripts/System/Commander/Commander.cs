using System;
using UnityEngine;


/// <summary>
/// 소속
/// </summary>
[Flags]
public enum TYPE_FORCE
{
    //소속 없음
    /// <summary>
    /// 무소속
    /// </summary>
    None = 1 << 0,

    //각 군단
    /// <summary>
    /// 반란군
    /// </summary>
    Rebel = 1 << 1,
    /// <summary>
    /// 용병단
    /// </summary>
    FreeCompany = 1 << 2,
    /// <summary>
    /// 기사단
    /// </summary>
    Order = 1 << 3,

    /// <summary>
    /// 반란군 용병단 연합
    /// </summary>
    Union_Rebel_Free = Rebel | FreeCompany,

    /// <summary>
    /// 기사단 용병단 연합
    /// </summary>
    Union_Order_Free = Order | FreeCompany,

    /// <summary>
    /// 전체
    /// </summary>
    All = Rebel | FreeCompany | Order

}

public class Commander
{

    string m_key;
    string m_name;
    Sprite m_icon;
    Sprite m_image;
    TYPE_FORCE m_typeForce;
    int m_health;
    int m_leadership;
    int m_munitions;
    string[] m_skills;
    string m_contents;



    public string key { get { return m_key; } }
    public string name { get { return m_name; } }
    public Sprite icon { get { return m_icon; } }
    public Sprite image { get { return m_image; } }
    public TYPE_FORCE typeForce { get { return m_typeForce; } }
    public int health { get { return m_health; } }
    public int leadership { get { return m_leadership; } }
    public int munitions { get { return m_munitions; } }
    public string[] skills { get { return m_skills; } }
    public string contents { get { return m_contents; } }


    public Commander(
        string key,
        string name,
        Sprite icon,
        Sprite image,
        TYPE_FORCE typeForce,
        int health,
        int leadership,
        int munitions,
        string[] skills,
        string contents
        )
    {
        m_key = key;
        m_name = name;
        m_icon = icon;
        m_image = image;
        m_typeForce = typeForce;
        m_health = health;
        m_leadership = leadership;
        m_munitions = munitions;
        m_skills = (string[])skills.Clone();
        m_contents = contents;
    }
}

