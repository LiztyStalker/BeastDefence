using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BulletActor))]

public class BulletActorEditor : Editor
{
    BulletActor m_bulletActor = null;

    void OnEnable()
    {
        m_bulletActor = (BulletActor)target;
    }

    public override void OnInspectorGUI()
    {
        if (m_bulletActor != null)
        {
            base.OnInspectorGUI();

            EditorGUILayout.LabelField("이름", string.Format("{0}", m_bulletActor.name));
            EditorGUILayout.LabelField("공격력", string.Format("{0}", m_bulletActor.attack));
            EditorGUILayout.LabelField("이동속도", string.Format("{0}", m_bulletActor.moveSpeed));
            EditorGUILayout.LabelField("피격범위", string.Format("{0}", m_bulletActor.range));
            EditorGUILayout.LabelField("발사방식", string.Format("{0}", m_bulletActor.typeShoot));
            EditorGUILayout.LabelField("피격방식", string.Format("{0}", m_bulletActor.typeHit));
        }

    }

    void OnDisable()
    {
        m_bulletActor = null;
    }


}

