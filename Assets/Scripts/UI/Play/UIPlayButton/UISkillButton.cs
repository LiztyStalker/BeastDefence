using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillButton : UIPlayButton , IPointerDownHandler, IPointerUpHandler
{
    public delegate void SetProjectorDelegate(SkillCard skillCard);
    public delegate void CloseProjectorDelegate(SkillCard skillCard, int index, bool isUsed);
    

    public event SetProjectorDelegate setProjectorEvent;
    public event CloseProjectorDelegate closeProjectorEvent;

    public event IsNotUsedDelegate isNotUsedEvent;
    public event RateCardTimeDelegate rateCardTimeEvent;
    public event MsgPanelDelegate msgPanelEvent;



    SkillCard m_skillCard;

//    Skill m_skill;

//    int m_index;
    bool m_isView = false;

    [SerializeField]
    Text m_name;

    [SerializeField]
    Text m_cost;

    //백그라운드 이미지 - 살짝 어두움
    
    [SerializeField]
    Image m_bgImage;

    //타이머이미지 - 
    [SerializeField]
    Image m_timerImage;

    [SerializeField]
    Text m_timerText;

    //데이터 트랜스폼
    [SerializeField]
    GameObject m_dataObject;

    [SerializeField]
    GameObject m_notUsedObject;

    //쿨타임 보여주기

    //public void setUIController(UIController uiController)
    //{
    //    m_uiController = uiController;
    //}

    public string key { 
        get { 
            if(m_skillCard != null)
                return m_skillCard.key;
            return "";
        } 
    }

    public bool isEmpty()
    {
        return (m_skillCard == null);
    }
    
    void setEmptySkill(int index)
    {
//        m_skill = null;
        m_skillCard = null;
        this.index = index;

        m_bgImage.sprite = null;
        m_timerImage.sprite = null;

        m_bgImage.gameObject.SetActive(false);
        m_timerImage.gameObject.SetActive(false);

        m_dataObject.SetActive(false);
        GetComponent<Button>().enabled = false;

    }

    /// <summary>
    /// 유닛을 설정합니다
    /// unit = null이면 빈 블록으로 지정합니다.
    /// </summary>
    /// <param name="skillCard"></param>
    /// <param name="index"></param>
    public void setSkill(SkillCard skillCard, int index)
    {

        m_notUsedObject.SetActive(false);

//        Debug.Log("skill : " + skill);
        if (skillCard != null)
        {
            m_skillCard = skillCard;
            this.index = index;

            m_bgImage.sprite = skillCard.icon;
            m_timerImage.sprite = skillCard.icon;

            m_bgImage.gameObject.SetActive(true);
            m_timerImage.gameObject.SetActive(true);

            m_dataObject.SetActive(true);

//            m_cost.text = string.Format("{0}", m_skill.munitions);
            m_name.text = m_skillCard.name;

            GetComponent<Button>().enabled = true;

        }
        else
        {
            setEmptySkill(index);
        }
    }



    public void uiUpdate(float timer, bool isNotUsed = false)
    {
        //쿨타임 보여주기
        //영웅은 나와있는지 보여주기
        //나와있으면 사용 불가 창 띄우기
        //안 나와있으면 쿨타임 시작

        if (!isNotUsed)
        {
            m_notUsedObject.SetActive(false);
            if (m_skillCard != null)
            {
                float rate = rateCardTimeEvent(m_skillCard);
                m_timerImage.fillAmount = rate;
                m_timerText.text = string.Format("{0:f1}", m_skillCard.waitTime - timer);
                m_timerText.gameObject.SetActive(rate < 1f);
            }
        }
        else
        {
            m_notUsedObject.SetActive(true);
        }

    }

    //public void setCreateUnitDelegate(CreateSkillDelegate createSkillDel)
    //{
    //    m_createSkillDel = createSkillDel;
    //}

    public void OnPointerDown(PointerEventData eventData)
    {
        //스킬프로젝터에 보임
        Debug.Log("스킬프로젝터 열기");
        //프로젝터 열기3
        if (!isNotUsedEvent(m_skillCard.key))
        {
            if (rateCardTimeEvent(m_skillCard) >= 1f)
            {
                setProjectorEvent(m_skillCard);
                m_isView = true;
            }
            else
            {
                msgPanelEvent("대기중입니다.");
            }
        }
        else
        {
            msgPanelEvent("사용할 수 없습니다.");
        }

    }



    public void OnPointerUp(PointerEventData eventData)
    {
        //스킬 사용자가 열렸으면
        if(m_isView){
            Debug.Log("스킬 사용");
            closeProjectorEvent(m_skillCard, index, true);
            m_isView = false;
        }

        //터치가 스킬 버튼 위이면 취소 - 바로 실행되는 스킬 제외
        //아니면 스킬 사용
    }
}

