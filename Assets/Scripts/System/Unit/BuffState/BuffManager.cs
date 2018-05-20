using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BuffManager : SingletonClass<BuffManager>
{





    enum TYPE_BUFF_DATA { 
        Key, 
        Name,
        TypeUsed,
        Constraint, 
        Time, 
        IncTime, 
        Rate, 
        IncRate, 
        Overlap,
        StateControlKey0, 
        StateControlKey1,
        StateControlKey2 
    }




    //모든 버프 가져오기
    Dictionary<string, Buff> m_buffDic = new Dictionary<string, Buff>();



    public BuffManager()
    {
       
        initParse();
    }


   

    void initParse()
    {
         TextAsset textAsset = Resources.Load<TextAsset>(Prep.buffDataPath);

         if (textAsset != null)
         {
             XmlDocument xmlDoc = new XmlDocument();
             xmlDoc.LoadXml(textAsset.text);

             XmlNodeList xmlNodeList = xmlDoc.SelectNodes("Buff/Data");

             foreach (XmlNode xmlNode in xmlNodeList)
             {
                 try
                 {
                     string key = xmlNode.SelectSingleNode(TYPE_BUFF_DATA.Key.ToString()).InnerText;
                     string name = xmlNode.SelectSingleNode(TYPE_BUFF_DATA.Name.ToString()).InnerText;

                     //                Sprite icon = iconArray.Where(spr => spr.name == key).SingleOrDefault();

                     //string constraint = xmlNode.SelectSingleNode(TYPE_BUFF_DATA.Constraint.ToString()).InnerText;
                     //Buff.TYPE_BUFF_CONSTRAINT typeConstraint = Buff.TYPE_BUFF_CONSTRAINT.ALWAYS;
                     //switch (constraint)
                     //{
                     //    case "Hit":
                     //        typeConstraint = Buff.TYPE_BUFF_CONSTRAINT.HIT;
                     //        break;
                     //    case "Attack":
                     //        typeConstraint = Buff.TYPE_BUFF_CONSTRAINT.ATTACK;
                     //        break;
                     //    case "Always":
                     //        typeConstraint = Buff.TYPE_BUFF_CONSTRAINT.ALWAYS;
                     //        break;
                     //}
                     Buff.TYPE_USED typeUsed = (Buff.TYPE_USED)Enum.Parse(typeof(Buff.TYPE_USED), xmlNode.SelectSingleNode(TYPE_BUFF_DATA.TypeUsed.ToString()).InnerText);

                     string constraintKey = xmlNode.SelectSingleNode(TYPE_BUFF_DATA.Constraint.ToString()).InnerText;
                     AddConstraint addConstraint = AddConstraintManager.GetInstance.getAddConstraint(constraintKey);

                     float time;
                     if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_BUFF_DATA.Time.ToString()).InnerText, out time))
                     {
                         time = 0f;
                     }

                     float incTime;
                     if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_BUFF_DATA.IncTime.ToString()).InnerText, out incTime))
                     {
                         incTime = 0f;
                     }

                     float rate;
                     if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_BUFF_DATA.Rate.ToString()).InnerText, out rate))
                     {
                         rate = 0f;
                     }

                     float incRate;
                     if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_BUFF_DATA.IncRate.ToString()).InnerText, out incRate))
                     {
                         incRate = 0f;
                     }

                     int overlapCnt;
                     if (!int.TryParse(xmlNode.SelectSingleNode(TYPE_BUFF_DATA.Overlap.ToString()).InnerText, out overlapCnt))
                     {
                         overlapCnt = 0;
                     }


                     //                 string stateControlKeys = xmlNode.SelectSingleNode(TYPE_BUFF_DATA.StateControlKey0.ToString()).InnerText;

                     string[] stateControlKeyArray = new string[3];
                     stateControlKeyArray[0] = xmlNode.SelectSingleNode(TYPE_BUFF_DATA.StateControlKey0.ToString()).InnerText;
                     stateControlKeyArray[1] = xmlNode.SelectSingleNode(TYPE_BUFF_DATA.StateControlKey1.ToString()).InnerText;
                     stateControlKeyArray[2] = xmlNode.SelectSingleNode(TYPE_BUFF_DATA.StateControlKey2.ToString()).InnerText;

                     List<IStateControl> stateControlList = new List<IStateControl>();

                     foreach (string stateControlKey in stateControlKeyArray)
                     {
                         IStateControl stateControl = StateControlManager.GetInstance.getStateControl(stateControlKey.Trim());
                         if (stateControl != null)
                         {
                             UnityEngine.Debug.Log("stateControl : " + stateControl);
                             stateControlList.Add(stateControl);
                         }
                     }

                     IStateControl[] stateControlArray = stateControlList.ToArray<IStateControl>();

                     Buff buff = new Buff(key, name, time, incTime, rate, incRate, overlapCnt, typeUsed, addConstraint, stateControlArray, "");
                     m_buffDic.Add(key, buff);
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
        //버프 및 상태이상 초기화
    }

    /// <summary>
    /// 버프 가져오기
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Buff getBuff(string key)
    {
        if (m_buffDic.ContainsKey(key))
        {
            return m_buffDic[key];
        }
        return null;
    }
}

