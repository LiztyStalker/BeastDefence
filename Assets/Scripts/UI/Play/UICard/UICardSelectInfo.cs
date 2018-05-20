using System;
using UnityEngine;
using UnityEngine.UI;

public class UICardSelectInfo : MonoBehaviour, ISummary
{
    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Image m_image;

    [SerializeField]
    Text m_levelText;

    [SerializeField]
    Text m_expText;

    [SerializeField]
    Slider m_expSlider;

    [SerializeField]
    Text m_countText;

    [SerializeField]
    GameObject m_newCardObject;

    public Unit unit
    {
        get;
        private set;
    }


    public void setCard(string unitKey, int count)
    {

        m_newCardObject.SetActive(false);

        //유닛 가져오기
        unit = UnitManager.GetInstance.getUnit(unitKey);
        
        //유닛 카드 유무 판별
        bool isUnitCard = Account.GetInstance.accUnit.isUnitCard(unit.key);

        //카드 삽입
        Account.GetInstance.accUnit.addUnit(unit);
        
        //유닛카드 가져오기
        UnitCard unitCard = Account.GetInstance.accUnit.getUnitCard(unit.key);

        //
        //카드가 있으면 가지고 있음
        if (unitCard != null)
        {
            if (count > 1)
            {
                m_countText.text = string.Format("x{0}", count);
                m_countText.gameObject.SetActive(true);
            }
            else
            {
                m_countText.gameObject.SetActive(false);
            }

            m_newCardObject.SetActive(!isUnitCard);
            m_nameText.text = unitCard.name;
            m_image.sprite = unitCard.icon;
            m_levelText.text = string.Format("Lv {0}", unitCard.level);
            m_expText.text = string.Format("{0}/{1}", unitCard.nowExperiance, unitCard.maxExperiance);
            m_expSlider.value = (float)unitCard.nowExperiance / (float)unitCard.maxExperiance;

            //계정 카드와 획득한 카드 합치기
            //
            //새카드이면 새카드 표식 보이기
        }




        //카드가 없으면 - 새카드 삽입 후 새카드 인증 - 경험치는 올라가지 않음
        //전 카드에서 후카드로 경험치 상승 보여주기 - 코루틴
        //새카드를 두장 받으면? - 하나 받고 하나 경험치 추가 (오른쪽 하단에 몇개 받았는지 숫자로 보여줌 - 1장은 안보여줌)
        //else
        //{
        //    m_newCardObject.SetActive(true);
        //    m_nameText.text = unit.name;
        //    m_image.sprite = unit.icon;
        //    m_levelText.text = string.Format("Lv {0}", unit.level);
        //    m_expText.text = string.Format("{0}/{1", unit.nowExperiance, unitCard.maxExperiance);
        //    m_expSlider.value = (float)unitCard.nowExperiance / (float)unitCard.maxExperiance;
        //}


        //카드 삽입

        //이벤트
        GetComponent<Button>().onClick.AddListener(() => OnClicked());

    }

    void OnClicked()
    {
        //카드 정보 보여주기
        UIPanelManager.GetInstance.root.uiCommon.uiUnitInfomation.setSummary(this, false);
    }

}

