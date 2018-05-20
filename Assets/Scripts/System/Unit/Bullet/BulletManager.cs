using System;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : SingletonClass<BulletManager>{
    //탄환 데이터


    enum TYPE_BULLET_DATA{Key, Name, TypePos, TypeShoot, TypeHit, MoveSpeed, Radius}

    Dictionary<string, Bullet> m_bulletDic = new Dictionary<string, Bullet>();

    Dictionary<string, Bullet> bulletDic { get { return m_bulletDic; } }

    public BulletManager()
    {
        initParse();
    }




    void initParse()
    {


        Sprite[] iconArray = Resources.LoadAll<Sprite>(Prep.bulletIconPath);

        TextAsset textAsset = Resources.Load<TextAsset>(Prep.bulletDataPath);

        if (textAsset != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes("Bullet/Data");

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                try
                {
                    string key = xmlNode.SelectSingleNode(TYPE_BULLET_DATA.Key.ToString()).InnerText;
                    string name = xmlNode.SelectSingleNode(TYPE_BULLET_DATA.Name.ToString()).InnerText;

                    Sprite icon = iconArray.Where(spr => spr.name == key).SingleOrDefault();

//                    string typeShootKey = xmlNode.SelectSingleNode(TYPE_BULLET_DATA.TypeShoot.ToString()).InnerText;
                    Bullet.TYPE_SHOOT typeShoot = (Bullet.TYPE_SHOOT)Enum.Parse(typeof(Bullet.TYPE_SHOOT), xmlNode.SelectSingleNode(TYPE_BULLET_DATA.TypeShoot.ToString()).InnerText);
                    //switch (typeShootKey)
                    //{
                    //    case "Direct":
                    //        typeShoot = Bullet.TYPE_SHOOT.DIRECT;
                    //        break;
                    //    case "Curved":
                    //        typeShoot = Bullet.TYPE_SHOOT.CURVED;
                    //        break;
                    //    case "Drop":
                    //        typeShoot = Bullet.TYPE_SHOOT.DROP;
                    //        break;
                    //    case "Set":
                    //        typeShoot = Bullet.TYPE_SHOOT.SET;
                    //        break;
                    //    default:
                    //        Prep.LogError(typeShootKey, "키를 찾을 수 없음", GetType());
                    //        break;
                    //}


//                    string typeHitKey = 
                    Bullet.TYPE_HIT typeHit = (Bullet.TYPE_HIT)Enum.Parse(typeof(Bullet.TYPE_HIT), xmlNode.SelectSingleNode(TYPE_BULLET_DATA.TypeHit.ToString()).InnerText);

                    Bullet.TYPE_START_POS typeShootPos = (Bullet.TYPE_START_POS)Enum.Parse(typeof(Bullet.TYPE_START_POS), xmlNode.SelectSingleNode(TYPE_BULLET_DATA.TypePos.ToString()).InnerText);
                    //switch (typeHitKey)
                    //{
                    //    case "Normal":
                    //        typeHit = Bullet.TYPE_HIT.NONE;
                    //        break;
                    //    case "Bomb":
                    //        typeHit = Bullet.TYPE_HIT.BOMB;
                    //        break;
                    //    case "Penetrate":
                    //        typeHit = Bullet.TYPE_HIT.PENETRATE;
                    //        break;
                    //    case "Splash":
                    //        typeHit = Bullet.TYPE_HIT.SPLASH;
                    //        break;
                    //    default:
                    //        Prep.LogError(typeHitKey, "키를 찾을 수 없음", GetType());
                    //        break;
                    //}

                    float moveSpeed;
                    if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_BULLET_DATA.MoveSpeed.ToString()).InnerText, out moveSpeed))
                    {
                        moveSpeed = 10f;
                    }
                    //                Debug.LogWarning("moveSpeed : " + key + " " + moveSpeed);

                    float radius;
                    if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_BULLET_DATA.Radius.ToString()).InnerText, out radius))
                    {
                        radius = 0f;
                    }
                    //                Debug.LogWarning("radius : " + key + " " + radius);

                    Bullet bullet = new Bullet(key, icon, typeShoot, typeHit, typeShootPos, moveSpeed, radius);

                    bulletDic.Add(key, bullet);
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

            Debug.Log("BulletCnt : " + bulletDic.Count);
        }
        else
        {
            Prep.LogError(Prep.bulletDataPath, "데이터를 찾을 수 없음", GetType());
        }

    }

    public Bullet getBullet(string key)
    {
        if (bulletDic.ContainsKey(key))
        {
            return bulletDic[key];
        }
        Prep.LogWarning(key, "를 찾을 수 없음", GetType());
        return null;
    }

}
