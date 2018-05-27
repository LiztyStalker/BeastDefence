using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Defence.CharacterPackage;
using Defence.CommanderPackage;

public class UIMissionInfo : MonoBehaviour {

//    public delegate Stage PrevStageDelegate(Stage nowStage);
//    public delegate Stage NextStageDelegate(Stage nowStage);

//    public event PrevStageDelegate prevStageEvent;
//    public event NextStageDelegate nextStageEvent;

    [SerializeField]
    Text m_contentsText;

    [SerializeField]
    Image m_requesterIcon;

    [SerializeField]
    Text m_requesterNameText;

    [SerializeField]
    Transform m_awardPanel;

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Text m_levelText;

    [SerializeField]
    Text m_forceText;

    [SerializeField]
    Image m_commanderImage;

    [SerializeField]
    Transform m_deckPanel;

    [SerializeField]
//    UICardSummary m_uiCardSummary;
    UIStageCardSummary m_uiStageCardSummary;

    [SerializeField]
    UIMissionAwardButton m_uiMissionAwardBtn;

    [SerializeField]
    UIUnitInfomation m_cardInfo;

    List<UIStageCardSummary> cardSummaryList = new List<UIStageCardSummary>();
    List<UIMissionAwardButton> stageAwardBtnList = new List<UIMissionAwardButton>();

    UIUnitInfomation uiUnitInformation;

    Stage m_stage;

    public Stage getStage()
    {
        return m_stage;
    }

    public void setStage(Stage stage)
    {
        //openPanel(null);
        clear();

        gameObject.SetActive(true);

        uiUnitInformation = UIPanelManager.GetInstance.root.uiCommon.uiUnitInfomation;


        m_stage = stage;

        //m_nameText.text = stage.name;
        m_contentsText.text = stage.contents;

        Character character = CharacterManager.GetInstance.getCharacter(stage.requesterKey);

        if (character != null)
        {
            m_requesterIcon.sprite = character.icon;
            m_requesterNameText.text = character.name;
        }
        else
        {
            m_requesterIcon.sprite = null;
            m_requesterNameText.text = "-";
        }



        //보상 보이기

        SinarioAward sinarioAward = SinarioAwardManager.GetInstance.getSinarioAward(stage.key);

        foreach (SinarioAward.TYPE_SINARIO_AWARD_CATEGORY typeCategory in sinarioAward.sinarioAwardDic.Keys)
        {
            UIMissionAwardButton uiMissionAwardBtn = Instantiate(m_uiMissionAwardBtn);
            uiMissionAwardBtn.setAward(typeCategory, sinarioAward.getValue(typeCategory));
            uiMissionAwardBtn.transform.SetParent(m_awardPanel);
            uiMissionAwardBtn.transform.localScale = Vector2.one;

            stageAwardBtnList.Add(uiMissionAwardBtn);

        }

        //유닛 덱 보이기

        foreach (ICard iCard in stage.deck.cardArray)
        {
//            Debug.LogWarning("ICard : " + iCard);

            if (iCard != null)
            {
                UIStageCardSummary uiCard = Instantiate(m_uiStageCardSummary);
                uiCard.setCard(iCard);
                uiCard.transform.SetParent(m_deckPanel);
                uiCard.transform.localScale = Vector2.one;
                uiCard.unitStageInfoEvent += uiUnitInformation.setSummary;
                cardSummaryList.Add(uiCard);
            }
        }

        //적 지휘관 카드 제작
        CommanderCard commanderCard = CommanderManager.GetInstance.getCommanderCard(stage.deck.commanderKey, stage.deck.commanderLevel);

        if (commanderCard != null)
        {
            m_nameText.text = commanderCard.name;
            m_levelText.text = string.Format("Lv {0}", commanderCard.level);
            m_forceText.text = string.Format("{0}", Prep.getForceToText(commanderCard.typeForce));
            m_commanderImage.sprite = commanderCard.icon;
        }

        StartCoroutine(UIPanelManager.GetInstance.root.uiCommon.uiContents.contentsCoroutine(Contents.TYPE_CONTENTS_EVENT.StageInfo));

    }

    void clear()
    {
        for (int i = cardSummaryList.Count - 1; i >= 0; i--)
        {
            Destroy(cardSummaryList[i].gameObject);
        }

        cardSummaryList.Clear();


        for (int i = stageAwardBtnList.Count - 1; i >= 0; i--)
        {
            Destroy(stageAwardBtnList[i].gameObject);
        }

        stageAwardBtnList.Clear();
    }

   
}
