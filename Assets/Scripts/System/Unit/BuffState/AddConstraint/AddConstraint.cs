using System;
using System.Reflection;

public class AddConstraint
{



    //발동 조건
    public enum TYPE_BUFF_CONSTRAINT { None, Attack, Hit, Move }
    //0 - 항상
    //1 - 공격시
    //2 - 피격시
    //3 - 이동시

    string m_key;
    TYPE_BUFF_CONSTRAINT m_typeBuffConstraint;
    string m_className;
    string m_fieldName;
    string m_constraintRange;
    string m_structure;
    string m_value;

    public AddConstraint(
        string key,
        TYPE_BUFF_CONSTRAINT typeBuffConstraint,
        string className,
        string fieldName,
        string constraintRange,
        string structure,
        string value
        )
    {
        m_key = key;
        m_typeBuffConstraint = typeBuffConstraint;
        m_className = className;
        m_fieldName = fieldName;
        m_constraintRange = constraintRange;
        m_structure = structure;
        m_value = value;
    }

    public bool isConstraint(IActor iActor, TYPE_BUFF_CONSTRAINT typeBuffConstraint)
    {

        //IActor = 탄환, 유닛, 프로젝터, CPU, 


        //조건이 없으면

        //
        //항상 - 무조건 조건 여부 판단
        //또는 해당 조건에 맞아야 함
        if (m_typeBuffConstraint == TYPE_BUFF_CONSTRAINT.None || m_typeBuffConstraint == typeBuffConstraint)
        {
            if (m_className == "-")
            {
                return true;
            }
            else
            {
                //클래스명이 같으면
                try
                {

                    Type classType = iActor.GetType();

                    PropertyInfo property = classType.GetProperty(m_fieldName);

                    object propertyValue = property.GetValue(iActor, property.GetIndexParameters());

//                    Prep.LogWarning(m_value, (m_value == propertyValue.ToString()).ToString(), GetType());

                    switch (m_constraintRange)
                    {
                        case "==":
                            return (propertyValue.ToString() == m_value);
                        case "!=":
                            return (propertyValue.ToString() != m_value);
                        case ">=":
                            return (propertyValue.ToString().CompareTo(m_value) > 0);
                        case "<=":
                            return (propertyValue.ToString().CompareTo(m_value) < 0);
                    }
                }
                catch (NullReferenceException e)
                {
                    UnityEngine.Debug.LogError("AddConstraint : " + e.Message);

                    return false;
                }

            }
        }
        return false;





        //    try
        //    {
        //        //클래스명 가져오기
        //        Type classType = iActor.GetType();

        //        UnityEngine.Debug.LogError("classType : " + classType.Name);

        //        //멤버 가져오기 - 유닛에 대한 멤버 가져오기
        //        PropertyInfo property = classType.GetProperty(m_fieldName);

        //        UnityEngine.Debug.LogError("property : " + m_fieldName);

        //        UnityEngine.Debug.LogError("iActor : " + iActor.GetType());

        //        object propertyValue = property.GetValue(iActor, property.GetIndexParameters());

        //        //값 가져오기
        //        //UnityEngine.Debug.LogError("field : " + property.GetValue(iActor, property.GetIndexParameters()));
        //        //            UnityEngine.Debug.LogError("field : " + field.GetValue((Bullet)iActor));





        //        //멤버타입 가져오기 - 형 가져오기
        //        //Type valueType = Type.GetType(m_structure);

        //        //멤버타입 기준으로 형 변환하기
        //        //(valueType)m_value;

        //        //형변환된 값으로 조건 따지기

        //        //UnityEngine.Debug.LogError("valueType : " + valueType.Name);





        //        //조건범위 가져오기 - 기본 값과 데이터값 비교
        //        switch (m_constraintRange)
        //        {
        //            case "==":
        //                //값 조건여부
        //                UnityEngine.Debug.LogError("propertyEqule : " + (propertyValue.ToString() == m_value));
        //                if ((propertyValue.ToString() == m_value))
        //                    return true;

        //                break;
        //            case "!=":
        //                //값 조건여부
        //                if ((propertyValue.ToString() != m_value))
        //                    return true;
        //                break;
        //            case ">=":
        //                //값 조건여부
        //                break;
        //            case "<=":
        //                //값 조건여부
        //                break;
        //        }
        //    }
        //    catch (NullReferenceException e)
        //    {
        //        UnityEngine.Debug.LogError("AddConstraint : " + e.Message);

        //        return false;
        //    }
        //}
        //return false;
    }

}

