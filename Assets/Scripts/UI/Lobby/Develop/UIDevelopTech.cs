using UnityEngine;
using UnityEngine.UI;

public class UIDevelopTech : MonoBehaviour
{

    [SerializeField]
    Text m_techText;

    public void setTech(int techLevel)
    {
        if (techLevel >= 0 && techLevel < Prep.techCount)
            m_techText.text = Prep.techLevelText[techLevel];
        else
            m_techText.text = techLevel.ToString();
    }
}

