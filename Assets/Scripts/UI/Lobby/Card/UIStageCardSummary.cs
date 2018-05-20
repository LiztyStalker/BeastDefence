using UnityEngine;
using UnityEngine.UI;

public class UIStageCardSummary : MonoBehaviour, ISummary
{
    public delegate void UnitStageInformationDelegate(ISummary iSummary, int level);
    public event UnitStageInformationDelegate unitStageInfoEvent;

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Image m_icon;

    [SerializeField]
    Text m_levelText;

    ICard iCard;

    public Unit unit {
        get
        {
            if (iCard is UnitCard)
                return ((UnitCard)iCard).unit;

            Prep.LogWarning("", "유닛 카드가 아닙니다.", GetType());
            return null;
        }
    }

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => OnClicked());
    }

    public void setFilp()
    {
        m_icon.GetComponent<RectTransform>().localScale = new Vector2(-m_icon.GetComponent<RectTransform>().localScale.x, m_icon.GetComponent<RectTransform>().localScale.y);
    }

    public void setCard(ICard iCard)
    {
        this.iCard = iCard;

        if (iCard != null)
        {
            m_nameText.text = iCard.name;
            m_nameText.color = Color.white;
            m_icon.sprite = iCard.icon;
            m_icon.gameObject.SetActive(true);
            m_levelText.text = string.Format("Lv {0}", iCard.level);
            m_levelText.color = Color.white;
        }
        else
        {
            m_nameText.text = "";
            m_icon.sprite = null;
            m_icon.gameObject.SetActive(false);
            m_levelText.text = "";
        }
    }


    void OnClicked()
    {

        if (unitStageInfoEvent != null)
        {
            UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);
            unitStageInfoEvent(this, iCard.level);
        }
        else
        {
            Prep.LogWarning("", "이벤트가 등록되지 않았습니다.", GetType());
        }
    }

}
