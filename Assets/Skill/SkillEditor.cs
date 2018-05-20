#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Skill))]
public class SkillEditor : Editor
{
    Skill m_skill = null;

    Skill.TYPE_SKILL_ACTIVE typeSkillPlay;

    void OnEnable()
    {
//        m_skill = (Skill)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.TextField("Key", m_skill.key);
        EditorGUILayout.TextField("Name", m_skill.name);

        //발동타입

        //행동타입
        typeSkillPlay = (Skill.TYPE_SKILL_ACTIVE)EditorGUILayout.EnumPopup("SkillActive", m_skill.typeSkillPlay);

        //스킬발동 확률
        EditorGUILayout.FloatField("SkillRate", m_skill.skillRate);

        //쿨타임
        EditorGUILayout.FloatField("CoolTime", m_skill.coolTime);

        //스킬 액터
    }
}

#endif

