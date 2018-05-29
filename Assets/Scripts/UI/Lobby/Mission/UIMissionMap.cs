using UnityEngine;
using UnityEngine.UI;

public class UIMissionMap : MonoBehaviour
{
    [SerializeField]
    Image m_map;

    [SerializeField]
    Text m_mapNameText;

    [SerializeField]
    Text m_targetText;

    [SerializeField]
    Text m_contentsText;


    public void setMap(Map map)
    {
        if (map != null)
        {
            m_map.sprite = map.icon;
            m_mapNameText.text = map.name;
            m_targetText.text = map.contents; //임무 목표
            m_contentsText.text = map.contents;
        }
    }
}

