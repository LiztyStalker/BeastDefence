using UnityEngine;

public class Bullet {

    /// <summary>
    /// 발사 타입
    /// DIRECT : 등속운동
    /// CURVED : 포물선운동
    /// DROP : 낙하운동 - 포물선 Y축이 위쪽
    /// </summary>
    public enum TYPE_SHOOT { Direct, Curved, Drop, Set}

    public enum TYPE_HIT { Normal, Bomb, Penetrate, Splash}

    public enum TYPE_START_POS { Myself, Enemy, Castle }

    //키
    string m_key;

    //이펙트키
    string m_effectKey;

    //이미지
    Sprite m_image;

    //이동속도
    float m_moveSpeed;

    //탄환 공격 타입
    TYPE_SHOOT m_typeShoot;

    //탄환 피격 타입
    TYPE_HIT m_typeHit;

    //탄환 발사 위치
    TYPE_START_POS m_typeShootPos;

    //범위
    float m_radius;

    //사용유닛
    //


    public string key { get { return m_key; } }
    public string effectKey { get { return m_effectKey; } set { m_effectKey = value; } }
    public Sprite image { get { return m_image; } }
    public float moveSpeed { get { return m_moveSpeed; } }
    public TYPE_SHOOT typeShoot { get { return m_typeShoot; } }
    public TYPE_HIT typeHit { get { return m_typeHit; } }
    public TYPE_START_POS typeShootPos { get { return m_typeShootPos; } }
    public float radius { get { return m_radius; } }

    public Bullet(string key, Sprite image, TYPE_SHOOT typeShoot, TYPE_HIT typeHit, TYPE_START_POS typeShootPos, float moveSpeed, float radius)
    {
        m_key = key;
        m_image = image;
        m_typeShoot = typeShoot;
        m_typeShootPos = typeShootPos;
        m_typeHit = typeHit;
        m_moveSpeed = moveSpeed;
        m_radius = radius;
    }

}
