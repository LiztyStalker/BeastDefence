using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitActor))]

public class UnitActorEditor : Editor
{
    UnitActor m_unitActor = null;

    void OnEnable()
    {
        m_unitActor = (UnitActor)target;
    }

    public override void OnInspectorGUI()
    {
        if (m_unitActor != null)
        {
            base.OnInspectorGUI();
       
            EditorGUILayout.LabelField("이름", string.Format("{0}", m_unitActor.name));
            EditorGUILayout.LabelField("체력", string.Format("{0}/{1}", m_unitActor.nowHealth, m_unitActor.maxHealth));
            EditorGUILayout.LabelField("공격력", string.Format("{0}", m_unitActor.attack));
            EditorGUILayout.LabelField("공속", string.Format("{0}", m_unitActor.attackSpeed));
            EditorGUILayout.LabelField("이속", string.Format("{0}", m_unitActor.moveSpeed));
            EditorGUILayout.LabelField("사정거리", string.Format("{0}", m_unitActor.range));
            EditorGUILayout.LabelField("피격지수", string.Format("{0}%", m_unitActor.defence * 100f));
            EditorGUILayout.LabelField("이동방식", string.Format("{0}", m_unitActor.typeMovement));
//            EditorGUILayout.LabelField("공격방식", string.Format("{0}", m_unitActor.typeRange));
            EditorGUILayout.LabelField("현재상태", string.Format("{0}", m_unitActor.unitType.iUnitState.ToString()));


        }

    }

    void OnDisable() {
        m_unitActor = null;
    }


}

