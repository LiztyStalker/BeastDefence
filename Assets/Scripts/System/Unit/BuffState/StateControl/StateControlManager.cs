using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateControlManager : SingletonClass<StateControlManager>
{

    enum TYPE_STATE_CONTROL_DATA{Key, Name, Value, IncValue, TypeStateValue, TypeClass}

    //모든 상태이상 가져오기
    Dictionary<string, IStateControl> m_stateControlDic = new Dictionary<string, IStateControl>();


    public StateControlManager()
    {
        initParse();
    }

    void initParse()
    {
        //상태이상 초기화

//        GameObject[] skeletonObjectArray = Resources.LoadAll<GameObject>(Prep.unitSkeletonPath);
//        Sprite[] iconArray = Resources.LoadAll<Sprite>(Prep.unitIconPath);

        TextAsset textAsset = Resources.Load<TextAsset>(Prep.stateControlDataPath);

        if (textAsset != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(Prep.getXmlDataPath(GetType()));

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                try
                {
                    string key = xmlNode.SelectSingleNode(TYPE_STATE_CONTROL_DATA.Key.ToString()).InnerText;
                    string name = xmlNode.SelectSingleNode(TYPE_STATE_CONTROL_DATA.Name.ToString()).InnerText;

                    //                Sprite icon = iconArray.Where(spr => spr.name == key).SingleOrDefault();

                    float value;
                    if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_STATE_CONTROL_DATA.Value.ToString()).InnerText, out value))
                    {
                        value = 0f;
                    }

                    float incValue;
                    if (!float.TryParse(xmlNode.SelectSingleNode(TYPE_STATE_CONTROL_DATA.IncValue.ToString()).InnerText, out incValue))
                    {
                        incValue = 0f;
                    }


//                    string typeState = xmlNode.SelectSingleNode(TYPE_STATE_CONTROL_DATA.TypeStateValue.ToString()).InnerText;
                    StateControl.TYPE_STATE_VALUE typeStateValue = (StateControl.TYPE_STATE_VALUE)Enum.Parse(typeof(StateControl.TYPE_STATE_VALUE), xmlNode.SelectSingleNode(TYPE_STATE_CONTROL_DATA.TypeStateValue.ToString()).InnerText);

                    //switch (typeState)
                    //{
                    //    case "Static":
                    //        typeStateValue = StateControl.TYPE_STATE_VALUE.STATIC;
                    //        break;
                    //    case "Rate":
                    //        typeStateValue = StateControl.TYPE_STATE_VALUE.RATE;
                    //        break;
                    //    case "Value":
                    //        typeStateValue = StateControl.TYPE_STATE_VALUE.PLUS;
                    //        break;
                    //    case "Boolean":
                    //        typeStateValue = StateControl.TYPE_STATE_VALUE.BOOL;
                    //        break;
                    //    default:
                    //        Prep.LogWarning(typeState, "를 찾을 수 없음", GetType());
                    //        break;
                    //}




                    string typeClass = xmlNode.SelectSingleNode(TYPE_STATE_CONTROL_DATA.TypeClass.ToString()).InnerText;

                    IStateControl stateControl = null;

                    //리플렉션으로 제작해야 함 + StateControl
                    //typeClass + "StateControl"

                    switch (typeClass)
                    {
                        case "Attack":
                            stateControl = new AttackStateControl(value, incValue, typeStateValue);
                            break;
                        case "Defence":
                            stateControl = new DefenceStateControl(value, incValue, typeStateValue);
                            break;
                        case "AttackSpeed":
                            stateControl = new AttackSpeedStateControl(value, incValue, typeStateValue);
                            break;
                        case "MoveSpeed":
                            stateControl = new MoveSpeedStateControl(value, incValue, typeStateValue);
                            break;
                        case "Range":
                            stateControl = new RangeStateControl(value, incValue, typeStateValue);
                            break;
                        case "RecoveryHealth":
                            stateControl = new RecoveryHealthStateControl(value, incValue, typeStateValue);
                            break;
                        case "MaxHealth":
                            stateControl = new MaxHealthStateControl(value, incValue, typeStateValue);
                            break;
                        case "NotAttack":
                            stateControl = new NotAttackStateControl(value, incValue, typeStateValue);
                            break;
                        case "NotSkill":
                            stateControl = new NotSkillStateControl(value, incValue, typeStateValue);
                            break;
                        case "NotMove":
                            stateControl = new NotMoveStateControl(value, incValue, typeStateValue);
                            break;
                        case "Hide":
                            stateControl = new HideStateControl(value, incValue, typeStateValue);
                            break;
                        case "OnlyMove":
                            stateControl = new OnlyMoveStateControl(value, incValue, typeStateValue);
                            break;
                        case "Invisible":
                            stateControl = new InvisibleStateControl(value, incValue, typeStateValue);
                            break;

                        default:
                            Prep.LogWarning(typeClass, "를 찾을 수 없음", GetType());
                            break;
                    }

                    if (stateControl != null)
                        m_stateControlDic.Add(key, stateControl);
                    else
                        Prep.LogWarning(key, "상태이상을 찾을 수 없음", GetType());
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
            Prep.LogError(Prep.stateControlDataPath, "를 찾을 수 없음", GetType());
        }
    }

    public IStateControl getStateControl(string key)
    {
        if (m_stateControlDic.ContainsKey(key))
        {
            return m_stateControlDic[key];
        }

        Prep.LogError(key, "를 찾을 수 없음", GetType());
        
        return null;
    }
}

