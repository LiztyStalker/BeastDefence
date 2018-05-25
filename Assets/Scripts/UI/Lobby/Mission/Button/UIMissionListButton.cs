using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Defence.CommanderPackage;

public class UIMissionListButton : MonoBehaviour
{

    public delegate void StageDelegate(Stage stage);

    public event StageDelegate stageEvent;

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Text m_typeText;

    [SerializeField]
    Image m_image;

    [SerializeField]
    Text m_stageText;

    [SerializeField]
    Text m_forceText;

    [SerializeField]
    Text m_levelText;

    [SerializeField]
    Transform m_awardTransform;

    [SerializeField]
    UIMissionAwardButton m_uiAwardBtn;

    List<UIMissionAwardButton> m_dataList = new List<UIMissionAwardButton>();

    Stage m_stage;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => OnStageClicked());
    }

    public void setStage(Stage stage)
    {
        m_stage = stage;
        if (m_stage != null)
        {
            m_nameText.text = stage.name;
            m_typeText.text = Prep.getTypeStageToText(stage.typeStage);
            m_image.sprite = stage.icon;
            m_stageText.text = stage.key;
            
            CommanderCard commanderCard = CommanderManager.GetInstance.getCommanderCard(stage.deck.commanderKey, stage.deck.commanderLevel);

            m_forceText.text = Prep.getForceToText(commanderCard.typeForce);
            m_levelText.text = string.Format("Lv {0}", commanderCard.level);

            SinarioAward sinarioAward = SinarioAwardManager.GetInstance.getSinarioAward(stage.key);

            foreach (SinarioAward.TYPE_SINARIO_AWARD_CATEGORY typeAccCategory in sinarioAward.sinarioAwardDic.Keys)
            {
                UIMissionAwardButton btn = Instantiate(m_uiAwardBtn);
                btn.setAward(typeAccCategory, sinarioAward.sinarioAwardDic[typeAccCategory], true);
                btn.transform.SetParent(m_awardTransform);
                btn.transform.localScale = Vector2.one;
                m_dataList.Add(btn);
            }

        }
    }

    public Stage getStage()
    {
        return m_stage;
    }

    void OnDisable()
    {
        for (int i = m_dataList.Count - 1; i >= 0; i--)
        {
            Destroy(m_dataList[i].gameObject);
        }

        m_dataList.Clear();
    }

    //아직 사용하지 않음
    void OnStageClicked()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);
        //바로 누르면 다음 미션으로 이동
        stageEvent(m_stage);
    }

}

