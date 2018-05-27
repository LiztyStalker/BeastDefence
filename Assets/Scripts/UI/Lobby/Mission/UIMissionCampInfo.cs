using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Defence.CommanderPackage;

public class UIMissionCampInfo : MonoBehaviour
{
    [SerializeField]
    Image m_commanderIcon;

    [SerializeField]
    Text m_commanderName;

    [SerializeField]
    Text m_commanderLevelText;

    [SerializeField]
    Text m_commanderForceText;

    [SerializeField]
    UISkillIcon[] m_uiSkillIcons;

    [SerializeField]
    UIStageCardSummary m_uiCard;
    
    [SerializeField]
    Transform m_soldierPanel;

    [SerializeField]
    Transform m_heroPanel;


    UIDataBoxManager m_uiDataBox;

    UIStageCardSummary[] uiUnitCardArray = null;

    UIUnitInfomation uiUnitInformation;


    void Awake()
    {
        m_uiDataBox = UIPanelManager.GetInstance.root.uiCommon.uiDataBox;

        for (int i = 0; i < m_uiSkillIcons.Length; i++)
        {
            //지휘관 스킬 삽입
            m_uiSkillIcons[i].dataBoxEvent += m_uiDataBox.setData;
        }

    }

    void initArray(bool isReverse = false)
    {

        uiUnitInformation = UIPanelManager.GetInstance.root.uiCommon.uiUnitInfomation;

        uiUnitCardArray = new UIStageCardSummary[Prep.maxUnitSlot + Prep.maxHeroSlot];

        //병사 및 영웅 삽입
        for (int i = 0; i < uiUnitCardArray.Length; i++)
        {
            uiUnitCardArray[i] = Instantiate(m_uiCard);

            //유닛
            if (i < Prep.maxUnitSlot)
            {
                uiUnitCardArray[i].transform.SetParent(m_soldierPanel);

            }
            else
            {
                //빈 셀 삽입하기
                //if (i == Prep.maxUnitSlot + Prep.maxHeroSlot)
                //{
                //    GameObject gameObj = new GameObject();
                //    gameObj.AddComponent<Image>();

                //    gameObj.transform.SetParent(m_heroPanel);
                //    gameObj.transform.localScale = Vector2.one;
                //    if (isReverse)
                //        gameObj.transform.SetAsFirstSibling();

                //}



                uiUnitCardArray[i].transform.SetParent(m_heroPanel);
            }

            uiUnitCardArray[i].transform.localScale = Vector2.one;
            uiUnitCardArray[i].name = uiUnitCardArray[i].name + i.ToString();

            if(i < Prep.maxUnitSlot + Prep.maxHeroSlot)
                uiUnitCardArray[i].unitStageInfoEvent += uiUnitInformation.setSummary;


            if (isReverse)
            {
                uiUnitCardArray[i].transform.SetAsFirstSibling();
                uiUnitCardArray[i].setFilp();
            }
        }
    }


    void setCommander(CommanderCard commanderCard)
    {
        if (commanderCard != null)
        {
            m_commanderIcon.sprite = commanderCard.icon;
            m_commanderLevelText.text = string.Format("{0}", commanderCard.level);
            m_commanderName.text = commanderCard.name;
            m_commanderForceText.text = string.Format("{0}", Prep.getForceToText(commanderCard.typeForce));
        }

        for (int i = 0; i < m_uiSkillIcons.Length ; i++)
        {
            //지휘관 스킬 삽입
            Skill skill = SkillManager.GetInstance.getSkill(commanderCard.skills[i]);
            if (skill != null)
            {
                m_uiSkillIcons[i].setSkill(skill, 1);
            }
//            uiUnitCardArray[i].setCard(null);
        }
    }



    public void setAllyCamp(Stage stage)
    {

        if (uiUnitCardArray == null) initArray(false);

        //지휘관 삽입
        CommanderCard commanderCard = Account.GetInstance.accUnit.getCommanderCard(Account.GetInstance.accUnit.nowCommander);
        setCommander(commanderCard);

        //병사 삽입
        List<string> unitList = Account.GetInstance.accUnit.getWaitUnitCards(Unit.TYPE_UNIT.Soldier, stage.typeForce);
        for (int i = 0; i < Prep.maxUnitSlot; i++)
        {
            if (unitList.Count > i)
            {
                UnitCard unitCard = Account.GetInstance.accUnit.getUnitCard(unitList[i]);
                uiUnitCardArray[i].setCard(unitCard);// setUnitCard(unitCard);

                //스테이지 1-2부터 열림
                uiUnitCardArray[i].GetComponent<Button>().interactable = Account.GetInstance.accSinario.isStage("Stage012");


            }
            else
            {
                uiUnitCardArray[i].setCard(null);
                uiUnitCardArray[i].GetComponent<Button>().interactable = false;
            }
        }


        //영웅 삽입
        List<string> heroList = Account.GetInstance.accUnit.getWaitUnitCards(Unit.TYPE_UNIT.Hero, stage.typeForce); ;
        for (int i = 0; i < Prep.maxHeroSlot; i++)
        {
            if (heroList.Count > i)
            {
                UnitCard unitCard = Account.GetInstance.accUnit.getUnitCard(heroList[i]);
                uiUnitCardArray[i + Prep.maxUnitSlot].setCard(unitCard);
                uiUnitCardArray[i + Prep.maxUnitSlot].GetComponent<Button>().interactable = true;
            }
            else
            {
                uiUnitCardArray[i + Prep.maxUnitSlot].setCard(null);
                uiUnitCardArray[i + Prep.maxUnitSlot].GetComponent<Button>().interactable = false;
            }

        }

        
        //스킬 삽입
        //List<string> heroList = Account.GetInstance.accountSkill;
        //for (int i = 0; i < unitList.Count; i++)
        //{
        //    UnitCard unitCard = Account.GetInstance.accountUnit.getHeroCard(heroList[i]);
        //    m_uiUnitCardArray[i + Prep.maxUnitSlot].setUnitCard(unitCard);
        //}


    }

    public void setEnemyCamp(Stage stage)
    {

        if (uiUnitCardArray == null) initArray(true);

        CommanderCard commanderCard = CommanderManager.GetInstance.getCommanderCard(stage.deck.commanderKey, stage.deck.commanderLevel);

        if (commanderCard == null) Debug.LogWarning("지휘관 카드를 찾지 못했습니다");

        setCommander(commanderCard);

        for(int i = 0; i < stage.deck.cardArray.Length; i++){
            uiUnitCardArray[i].setCard(stage.deck.cardArray[i]);

            if(stage.deck.cardArray[i] != null)
                uiUnitCardArray[i].GetComponent<Button>().interactable = true;
            else
                uiUnitCardArray[i].GetComponent<Button>().interactable = false;

//            Debug.Log("deck : " + mission.deck[i]);
        }
    }

    void OnDisable()
    {
        m_uiDataBox.close();
    }
}

