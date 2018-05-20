using UnityEngine;
using UnityEngine.UI;

public class UICommanderCardToggle : MonoBehaviour
{

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Text m_levelText;

    [SerializeField]
    Text m_forceText;

    [SerializeField]
    Image m_icon;

    CommanderCard m_commanderCard;

    public CommanderCard commanderCard { get { return m_commanderCard; } }
    
    public void setCommander(CommanderCard commanderCard)
    {
        if (commanderCard != null)
        {
            m_commanderCard = commanderCard;

            m_nameText.text = commanderCard.name;
            m_nameText.color = Color.white;
            m_levelText.text = string.Format("Lv {0}", commanderCard.level);
            m_levelText.color = Color.white;
            m_icon.sprite = commanderCard.icon;
            m_forceText.text = string.Format("{0}", commanderCard.typeForce);
            m_forceText.color = Color.white;
        }
    }

    //토글 누르면 정보 보여줌
    



}

