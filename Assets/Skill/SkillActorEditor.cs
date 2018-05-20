#if UNITYEDITOR
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(SkillActor))]
public class SkillActorEditor : Editor
{
    //SkillActor m_skillAction = null;

    void OnEnable()
    {
        //m_skillAction = (SkillActor)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        
    }
}
#endif
