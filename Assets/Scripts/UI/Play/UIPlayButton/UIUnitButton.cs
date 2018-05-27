using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIUnitButton : UIPlayButton, IPointerDownHandler, IPointerUpHandler
{

    public delegate void CreateUnitDelegate(UnitCard unitCard, int index);
    public delegate bool CheckMunitionsDelegate(UnitCard unitCard);

    public event CreateUnitDelegate createUnitEvent;
    public event CheckMunitionsDelegate checkMunitionEvent;

    public event IsNotUsedDelegate isNotUsedEvent;
    public event RateCardTimeDelegate rateCardTimeEvent;
    public event MsgPanelDelegate msgPanelEvent;

//    UIController m_uiController;

    UnitCard m_unitCard;
    
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

    //사용중인 유닛 - 영웅
    [SerializeField]
    GameObject m_notUsedObject;

    Vector3 m_touchPos;

    //쿨타임 보여주기

    //public void setUIController(UIController uiController)
    //{
    //    m_uiController = uiController;
    //}
    public string key { 
        get { 
            if(m_unitCard != null) 
                return m_unitCard.key;
            return "";
        } 
    }

    public bool isEmpty()
    {
        return (m_unitCard == null);
    }

    void setEmptyUnit(int index)
    {
        m_unitCard = null;
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
    /// <param name="unitCard"></param>
    /// <param name="index"></param>
    public void setUnit(UnitCard unitCard, int index)
    {

        m_notUsedObject.SetActive(false);

        if (unitCard != null)
        {
            m_unitCard = unitCard;
            this.index = index;

            m_bgImage.sprite = unitCard.icon;
            m_timerImage.sprite = unitCard.icon;

            m_bgImage.gameObject.SetActive(true);
            m_timerImage.gameObject.SetActive(true);
                        
            m_dataObject.SetActive(true);


            m_cost.text = string.Format("{0}", m_unitCard.munitions);
            m_cost.color = Color.white;

            m_name.text = m_unitCard.name;
            m_name.color = Color.white;

            GetComponent<Button>().enabled = true;

        }
        else
        {
            setEmptyUnit(index);
        }
    }

    public void uiUpdate(float timer, bool isNotUsed = false)
    {


        if (m_isClicked)
        {
            m_infoTime += Prep.frameTime;
            if (m_infoTime > 1f)
            {
                uiDataBox.setData(m_unitCard, m_touchPos);
            }
        }
        else
        {
            m_infoTime = 0f;
        }


        //쿨타임 보여주기
        //영웅은 나와있는지 보여주기
        //나와있으면 사용 불가 창 띄우기
        //안 나와있으면 쿨타임 시작

        if (!isNotUsed)
        {
            m_notUsedObject.SetActive(false);
            if (m_unitCard != null)
            {
                float rate = rateCardTimeEvent(m_unitCard);
                m_timerImage.fillAmount = rate;
                m_timerText.text = string.Format("{0:f1}", m_unitCard.waitTime - timer);
                m_timerText.gameObject.SetActive(rate < 1f);
            }
        }
        else
        {
            m_notUsedObject.SetActive(true);
        }

    }

    //public void setCreateUnitDelegate(CreateUnitDelegate createUnitDel, CheckMunitionsDelegate checkMunitionsDel)
    //{
    //    createUnitEvent = createUnitDel;
    //    checkMunitionEvent = checkMunitionsDel;
    //}

    /// <summary>
    /// 유닛 생산 버튼 이벤트
    /// </summary>
    public void OnUnitCreateClicked()
    {

        //유닛이 들어있으면
        if (m_unitCard != null)
        {
            //정보를 보고 있지 않다면
            if (m_infoTime < 1f)
            {
                //사용불가가 아니면 - 영웅이면 영웅 인덱스에 없으면
                if (!isNotUsedEvent(key))
                {
                    //영웅이면
                    //현재 플레이어 영웅이 존재하면

                    //보급이 충분하면
                    if (checkMunitionEvent(m_unitCard))
                    {

                        //쿨타임이 완료되었으면 - UICtrler의 쿨타임이 완료되었으면
                        if (rateCardTimeEvent(m_unitCard) >= 1f)
                        {
                            //해당 유닛 생산하기
                            UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);
                            createUnitEvent(m_unitCard, index);
                        }
                        else
                        {
                            //쿨타임이 안되어 있습니다.
                            UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.ERROR);
                            msgPanelEvent("대기중입니다.");
                        }
                    }
                    else
                    {
                        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.ERROR);
                        msgPanelEvent("보급이 부족합니다");
                        //                    Debug.Log("보급이 부족합니다");
                    }
                }
                else
                {
                    //영웅이면
                    UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.ERROR);
                    msgPanelEvent("전장에서 활동중입니다.");
                    //다른 유닛이면 - 사용할 수 없습니다.
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_isClicked = true;
        m_touchPos = Input.mousePosition;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        m_touchPos = Vector3.zero;
        m_isClicked = false;
        m_infoTime = 0f;
    }

}

