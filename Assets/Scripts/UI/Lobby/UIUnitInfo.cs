using System;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitInfo : MonoBehaviour
{


    public delegate void UnitRefleshDelegate(UnitCard unitCard);

    public event UnitRefleshDelegate unitRefleshEvent;

    UIDataBox m_uiDataBox;

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Text m_commanderForceText;
    
    [SerializeField]
    Image m_image;

    [SerializeField]
    Text m_levelText;

    [SerializeField]
    Text m_expText;

    [SerializeField]
    Text m_populationText;

    [SerializeField]
    Text m_forceText;

    [SerializeField]
    Text m_munitionsText;

    [SerializeField]
    Text m_healthText;

    [SerializeField]
    Text m_attackText;

    [SerializeField]
    Text m_attackSpeedText;

    [SerializeField]
    Text m_attackTypeText;

    [SerializeField]
    Text m_moveSpeedText;

    [SerializeField]
    Text m_productTimeText;

    [SerializeField]
    Text m_contentsText;

    //스킬 3개
    [SerializeField]
    UISkillIcon[] m_uiSkillIcons;

    [SerializeField]
    Text m_trainingText;

    [SerializeField]
    Button m_trainingButton;

    UnitCard m_unitCard;

    void Awake()
    {
        m_uiDataBox = UIPanelManager.GetInstance.root.uiCommon.uiDataBox;

        for (int i = 0; i < m_uiSkillIcons.Length; i++)
        {
            m_uiSkillIcons[i].dataBoxEvent += m_uiDataBox.setSkillData;
        }

        if(m_trainingButton != null)
            m_trainingButton.onClick.AddListener(() => OnTrainingClicked());
    }


    public void setUnitCard(UnitCard unitCard, TYPE_FORCE typeForce)
    {
//        m_unitCard = unitCard;
        m_commanderForceText.text = string.Format("{0}", Prep.getForceToText(typeForce));
        setUnitCard(unitCard);
    }

    public void setUnitCard(UnitCard unitCard)
    {
        m_unitCard = unitCard;

        if (unitCard != null)
        {
            m_levelText.text = string.Format("{0}", unitCard.level);
            m_expText.text = string.Format("{0}/{1}", unitCard.nowExperiance, unitCard.maxExperiance);


            m_nameText.text = unitCard.name;
            m_image.sprite = unitCard.icon;
            m_image.gameObject.SetActive(true);
            m_forceText.text = string.Format("{0}", Prep.getForceToText(unitCard.typeForce));
            m_levelText.text = string.Format("{0}", unitCard.level);
            m_expText.text = string.Format("{0}/{1}", unitCard.nowExperiance, unitCard.maxExperiance);
            m_populationText.text = string.Format("{0}", unitCard.population);
            m_munitionsText.text = string.Format("{0}", unitCard.munitions);
            m_healthText.text = string.Format("{0}", unitCard.health);
            m_attackText.text = string.Format("{0}", unitCard.attack);
            m_attackSpeedText.text = string.Format("{0}", unitCard.attackSpeed);
            m_attackTypeText.text = string.Format("{0}", unitCard.typeLine);
            m_moveSpeedText.text = string.Format("{0}", unitCard.moveSpeed);
            m_productTimeText.text = string.Format("{0}", unitCard.waitTime);

            //최대레벨에 도달했으면
            if (m_trainingButton != null)
            {
                if (unitCard.isMaxLevel())
                {
                    m_trainingText.text = "Max";
                    m_trainingButton.enabled = false;
                }
                else
                {
                    m_trainingText.text = string.Format("{0}", -unitCard.trainingCost);
                    m_trainingButton.enabled = true;
                }
            }


            m_contentsText.text = unitCard.contents;


            setSkill(unitCard.unit, unitCard.level);
        }
        else
        {
            m_levelText.text = string.Format("-");
            m_expText.text = string.Format("-/-");

            m_nameText.text = "";
            m_image.sprite = null;
            m_image.gameObject.SetActive(false);
            m_forceText.text = string.Format("-");
            m_levelText.text = string.Format("-");
            m_expText.text = string.Format("-/-");
            m_populationText.text = string.Format("-");
            m_munitionsText.text = string.Format("-");
            m_healthText.text = string.Format("-");
            m_attackText.text = string.Format("-");
            m_attackSpeedText.text = string.Format("-");
            m_attackTypeText.text = string.Format("-");
            m_moveSpeedText.text = string.Format("-");
            m_productTimeText.text = string.Format("-");
            m_contentsText.text = "-";

            if (m_trainingButton != null)
            {
                m_trainingText.text = "-";
                m_trainingButton.enabled = false;
            }

            setSkill(null, 0);

        }
    }

    public void setUnit(Unit unit)
    {
        if (unit != null)
        {
            m_nameText.text = unit.name;
            m_image.sprite = unit.icon;
            //        m_levelText.text = string.Format("{0}", unitCard.level);
            //        m_expText.text = string.Format("{0}/{1}", unitCard.nowExperiance, unitCard.maxExperiance);
            m_populationText.text = string.Format("{0}", unit.population);
            m_munitionsText.text = string.Format("{0}", unit.munitions);
            m_healthText.text = string.Format("{0}", unit.health);
            m_attackText.text = string.Format("{0}", unit.attack);
            m_attackSpeedText.text = string.Format("{0}", unit.attackSpeed);
            m_moveSpeedText.text = string.Format("{0}", unit.moveSpeed);
            m_productTimeText.text = string.Format("{0}", unit.waitTime);
            m_contentsText.text = unit.contents;

            setSkill(unit);
        }
    }


    void setSkill(Unit unit, int level = 1)
    {

        
        for (int i = 0; i < m_uiSkillIcons.Length; i++)
        {
            if(unit != null)
                m_uiSkillIcons[i].setSkill(unit.skillArray[i], level);
            else
                m_uiSkillIcons[i].setSkill(null, level);
        }
        
        
    }

    void OnTrainingClicked()
    {

        ///최대 레벨에 도달하지 않았으면
        if (!m_unitCard.isMaxLevel())
        {

            if (Account.GetInstance.accData.isValue(m_unitCard.trainingCost, Shop.TYPE_SHOP_CATEGORY.Gold))
            {
                //훈련시키겠습니까?
                UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg(string.Format("{0} 를 훈련시키겠습니까?", m_unitCard.name), TYPE_MSG_PANEL.WARNING, TYPE_MSG_BTN.OK_CANCEL, trainingEvent);
            }
            else
            {
                //골드가 부족합니다.
                UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("골드가 부족합니다.", TYPE_MSG_PANEL.ERROR, TYPE_MSG_BTN.OK);
            }
        }
        //
    }

    void trainingEvent()
    {
        //훈련 완료
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.LVUP);

        Account.GetInstance.accData.useValue(m_unitCard.trainingCost, Shop.TYPE_SHOP_CATEGORY.Gold);
        m_unitCard.addExperiance(m_unitCard.maxExperiance - m_unitCard.nowExperiance);

        //카드 업데이트
        setUnitCard(m_unitCard);
        unitRefleshEvent(m_unitCard);
    }
}

