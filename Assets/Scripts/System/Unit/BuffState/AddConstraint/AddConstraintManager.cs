using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class AddConstraintManager : SingletonClass<AddConstraintManager>
{
	enum TYPE_ADDCONSTRAINT_DATA{Key, AddConstraint, Class, Type, Range, Struct, Value}
    
    Dictionary<string, AddConstraint> m_constraintDic = new Dictionary<string, AddConstraint>();

    public AddConstraintManager()
    {
        initParse();
    }

    void initParse()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(Prep.addConstraintDataPath);

        if (textAsset != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes("AddConstraint/Data");

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                try
                {
                    string key = xmlNode.SelectSingleNode(TYPE_ADDCONSTRAINT_DATA.Key.ToString()).InnerText;



                    //                string typeBuffKey = xmlNode.SelectSingleNode(TYPE_ADDCONSTRAINT_DATA.AddConstraint.ToString()).InnerText;
                    AddConstraint.TYPE_BUFF_CONSTRAINT typeBuffConstraint = (AddConstraint.TYPE_BUFF_CONSTRAINT)Enum.Parse(typeof(AddConstraint.TYPE_BUFF_CONSTRAINT), xmlNode.SelectSingleNode(TYPE_ADDCONSTRAINT_DATA.AddConstraint.ToString()).InnerText);
                    //switch (typeBuffKey)
                    //{
                    //    case "Always":
                    //        typeBuffConstraint = AddConstraint.TYPE_BUFF_CONSTRAINT.ALWAYS;
                    //        break;
                    //    case "Attack":
                    //        typeBuffConstraint = AddConstraint.TYPE_BUFF_CONSTRAINT.ATTACK;
                    //        break;
                    //    case "Hit":
                    //        typeBuffConstraint = AddConstraint.TYPE_BUFF_CONSTRAINT.HIT;
                    //        break;
                    //    case "Move":
                    //        typeBuffConstraint = AddConstraint.TYPE_BUFF_CONSTRAINT.MOVE;
                    //        break;
                    //}

                    string className = xmlNode.SelectSingleNode(TYPE_ADDCONSTRAINT_DATA.Class.ToString()).InnerText;
                    string fieldName = xmlNode.SelectSingleNode(TYPE_ADDCONSTRAINT_DATA.Type.ToString()).InnerText;
                    string constraintRange = xmlNode.SelectSingleNode(TYPE_ADDCONSTRAINT_DATA.Range.ToString()).InnerText;
                    string structure = xmlNode.SelectSingleNode(TYPE_ADDCONSTRAINT_DATA.Struct.ToString()).InnerText;
                    string value = xmlNode.SelectSingleNode(TYPE_ADDCONSTRAINT_DATA.Value.ToString()).InnerText;

                    AddConstraint addConstraint = new AddConstraint(key, typeBuffConstraint, className, fieldName, constraintRange, structure, value);

                    m_constraintDic.Add(key, addConstraint);
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
            Prep.LogError(Prep.addConstraintDataPath, "를 찾을 수 없음", GetType());
        }
    }

    public AddConstraint getAddConstraint(string key)
    {
        if (m_constraintDic.ContainsKey(key))
        {
            return m_constraintDic[key];
        }

        Prep.LogWarning(key, "를 찾을 수 없음", GetType());
        return null;
    }
}
