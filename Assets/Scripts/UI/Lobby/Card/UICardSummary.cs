using UnityEngine;
using UnityEngine.UI;



public class UICardSummary : MonoBehaviour, ISummary
{
    public delegate void UnitInformationDelegate(ISummary iSummary, bool isEmployee);
    public event UnitInformationDelegate unitInformationEvent;
    //이름
    //아이콘
    //설명
    //비용

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Image m_image;

    [SerializeField]
    Text m_contents;

    [SerializeField]
    Text m_costText;

    [SerializeField]
    GameObject m_costPanel;

    Unit m_unit;

    public Unit unit { get { return m_unit; } }

    void Awake(){
        GetComponent<Button>().onClick.AddListener(() => OnClicked());
    }

    public void setUnit(Unit unit, bool isCost = true)
    {
        
        m_unit = unit;

        m_nameText.text = unit.name;
        m_image.sprite = unit.icon;
        m_contents.text = unit.contents;
        m_costText.text = unit.cost.ToString();
        m_costPanel.SetActive(isCost);
    }

    public void setCard(ICard iCard)
    {

        if (iCard != null)
        {
            if (iCard is UnitCard)
                m_unit = ((UnitCard)iCard).unit;

            m_nameText.text = m_unit.name;
            m_image.sprite = m_unit.icon;
            m_contents.text = m_unit.contents;
            //        m_costText.text = card.cost.ToString();
            m_costPanel.SetActive(false);
        }
    }


    public void OnClicked(){

        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

        unitInformationEvent(this, m_costPanel.activeSelf);
    }
    
}

