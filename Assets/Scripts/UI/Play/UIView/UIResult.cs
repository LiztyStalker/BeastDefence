using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIResult : MonoBehaviour
{
    GameController gameCtrler;

    //최대 킬수
    //생산수
    //영웅킬수
    //영웅 생산수
    //최대 가한 데미지

    [SerializeField]
    UIResultAccountSummary m_uiAccountSummary;

    [SerializeField]
    UIResultAccountSummary m_uiCommanderSummary;

    //[SerializeField]
    //Text m_nameText;

    //[SerializeField]
    //Text m_levelText;

    //[SerializeField]
    //Text m_expText;

    //[SerializeField]
    //Slider m_expSlider;

    //[SerializeField]
    //Image m_image;

   
    [SerializeField]
    Text m_resultText;

    [SerializeField]
    Text m_timeText;

    [SerializeField]
    Text m_foodText;

    [SerializeField]
    UIMissionAwardButton m_uiMissionAwardBtn;

    [SerializeField]
    Transform m_missionAwardPanel;

    [SerializeField]
    UICardSelectInfo m_uiCardSelectInfo;

    [SerializeField]
    UICardSet m_uiCardSet;

    [SerializeField]
    Transform m_cardPanel;

    [SerializeField]
    UICardSelect m_uiCardSelect;

    [SerializeField]
    Button m_nextBtn;

    [SerializeField]
    Button m_retryBtn;

    int m_foodCount = 0;

    Dictionary<string, int> unitDic = new Dictionary<string, int>();

//    string[] testKeys = { "SpearMan", "SwordMan", "Archer", "CrossbowMan", "Engineer", "Scout", "Shielder", "Skirmisher" };

    public void setResult(GameController gameCtrler)
    {
        this.gameCtrler = gameCtrler;
        gameObject.SetActive(true);
        
        m_foodCount = Account.GetInstance.accUnit.getUnitTotalPopulation(Account.GetInstance.accSinario.nowStage.typeForce);
        m_foodText.text = string.Format("-{0}", m_foodCount);

    }


    void setResultCoroutine()
    {
        StartCoroutine(resultCoroutine());
    }
    

    IEnumerator resultCoroutine()
    {

        //버튼
        m_nextBtn.gameObject.SetActive(false);
        m_retryBtn.gameObject.SetActive(false);

        //플레이어 보이기
        m_uiAccountSummary.gameObject.SetActive(true);
        m_uiCommanderSummary.gameObject.SetActive(false);
        m_uiAccountSummary.setSummary(Account.GetInstance);

        //시간 보이기
        m_timeText.text = string.Format("{0:d2}:{1:d2}", gameCtrler.gameTime.Minutes, gameCtrler.gameTime.Seconds);
        
        yield return new WaitForSeconds(1f);

        Stage nowStage = Account.GetInstance.accSinario.nowStage;

        //테스트용 스테이지 가져오기
        if(nowStage == null) nowStage = WorldManager.GetInstance.getStage("StageTest");

        SinarioAward sinarioAward = SinarioAwardManager.GetInstance.getSinarioAward(nowStage.key);

        int addExp = 0;

        if (sinarioAward != null)
        {

            foreach (SinarioAward.TYPE_SINARIO_AWARD_CATEGORY typeCategory in sinarioAward.sinarioAwardDic.Keys)
            {
                UIMissionAwardButton uiMissionAwardBtn = Instantiate(m_uiMissionAwardBtn);
                
//                int value = sinarioAward.getValue(typeCategory);

                uiMissionAwardBtn.setAward(typeCategory, sinarioAward.getValue(typeCategory));
                uiMissionAwardBtn.transform.SetParent(m_missionAwardPanel);
                uiMissionAwardBtn.transform.localScale = Vector2.one;
                
                int count = 0;
                if (int.TryParse(sinarioAward.getValue(typeCategory), out count))
                {
                    if (typeCategory == SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.Exp)
                        addExp = count;

                    Account.GetInstance.accData.addValue(count, typeCategory);
                }
                else
                {
                    Prep.LogError(typeCategory.ToString(), "삽입 실패 int형이 아닙니다.", GetType());
                }

                //m_missionAwardPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                yield return new WaitForSeconds(0.5f);
            }

            m_uiAccountSummary.setSummary(Account.GetInstance);

            //선택형 카드 뒤집기
            if (sinarioAward.sinarioAwardDic.ContainsKey(SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.Card))
            {
                //string[] unitKeys = testKeys;
//                string[] unitKeys = Account.GetInstance.accSinario.nowStage.deck.getUnitCardKeys();//.Union(Account.GetInstance.accUnit.getWaitUnitCards(Unit.TYPE_UNIT.Soldier, nowStage.typeForce)).ToArray<string>();
                string[] unitKeys = Account.GetInstance.accUnit.getWaitUnitCards(Unit.TYPE_UNIT.Soldier, nowStage.typeForce).ToArray<string>();

                //Debug.LogWarning("unitKeys : " + unitKeys.Length);


                string value = sinarioAward.getValue(SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.Card);
                int count = 0;
                if (int.TryParse(value, out count))
                {
                    m_uiCardSelect.setCardSelect(unitKeys, count);
                    //카드열기 대기 - 카드 셀렉트에서 실행
                    yield return StartCoroutine(m_uiCardSelect.cardSelectCoroutine());
                }
                else
                {
                    //고정 카드 받기
                    Debug.Log("고정카드 받기");
                }
            }

            //획득한 카드 삽입하기
            unitDic = m_uiCardSelect.getUnitDic();
            

            //일반카드 획득
            //일반카드 획득창 보이기
            if (sinarioAward.sinarioAwardDic.ContainsKey(SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.NCard))
            {
                string unitKey = sinarioAward.sinarioAwardDic[SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.NCard];
                Unit unit = UnitManager.GetInstance.getUnit(unitKey);
                if (unit != null)
                {
//                    Account.GetInstance.accUnit.addUnit(unit);
                    //획득한 카드 삽입하기
                    if (unitDic.ContainsKey(unitKey))
                    {
                        unitDic[unitKey]++;
                    }
                    else
                    {
                        unitDic.Add(unitKey, 1);
                    }
                    m_uiCardSet.setUnit(unit);
                    m_uiCardSet.openPanel(null);
                    yield return StartCoroutine(m_uiCardSet.waitCoroutine());
                }
            }

//            yield return new WaitForSeconds(1f);

            //영웅카드 획득
            //영웅카드 획득창 보이기
            if (sinarioAward.sinarioAwardDic.ContainsKey(SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.HCard))
            {
                string unitKey = sinarioAward.sinarioAwardDic[SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.HCard];
                Unit unit = UnitManager.GetInstance.getUnit(unitKey);
                if (unit != null)
                {
//                    Account.GetInstance.accUnit.addUnit(unit);  
                    //획득한 카드 삽입하기
                    if (unitDic.ContainsKey(unitKey))
                    {
                        unitDic[unitKey]++;
                    }
                    else
                    {
                        unitDic.Add(unitKey, 1);
                    }
                    m_uiCardSet.setUnit(unit);
                    m_uiCardSet.openPanel(null);
                    yield return StartCoroutine(m_uiCardSet.waitCoroutine());
                }
            }

//            yield return new WaitForSeconds(0.5f);

            //지휘관 획득
            //지휘관 획득창 보이기
            if (sinarioAward.sinarioAwardDic.ContainsKey(SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.CCard))
            {
                string unitKey = sinarioAward.sinarioAwardDic[SinarioAward.TYPE_SINARIO_AWARD_CATEGORY.CCard];
                Commander cmd = Defence.CommanderPackage.CommanderManager.GetInstance.getCommander(unitKey);
                if (cmd != null)
                {
                    Account.GetInstance.accUnit.addCommanderCard(cmd);
                }
            }

        }

        //획득한 카드 보여주기
        foreach(string unitKey in unitDic.Keys){
            UICardSelectInfo uiCardSelectInfo = Instantiate(m_uiCardSelectInfo);
            uiCardSelectInfo.setCard(unitKey, unitDic[unitKey]);
            uiCardSelectInfo.transform.SetParent(m_cardPanel);
            uiCardSelectInfo.transform.localScale = Vector2.one;

            yield return new WaitForSeconds(0.5f);
        }


        //플레이어 경험치 추가
        //m_uiAccountSummary.setExp();
        


        //지휘관 보이기
        m_uiAccountSummary.gameObject.SetActive(false);
        m_uiCommanderSummary.gameObject.SetActive(true);


        CommanderCard cmdCard = Account.GetInstance.accUnit.getNowCommanderCard();
        m_uiCommanderSummary.setSummary(cmdCard);

        yield return new WaitForSeconds(0.5f);

        cmdCard.addExperiance(addExp);
        //지휘관 경험치 획득하기
        m_uiCommanderSummary.setSummary(cmdCard);


        //계정 - 플레이 시간 추가하기
        Account.GetInstance.accData.addPlayTime(gameCtrler.gameTime);
        
        
        //지휘관 경험치 추가
        //m_uiCommanderSummary.setExp();


        //클리어 확인 - 메인 퀘스트만
        if(nowStage.typeStage == Stage.TYPE_STAGE.Main)
            Account.GetInstance.accSinario.setStageClear(nowStage.key);
        //긴급 퀘스트는 사라지기



        //버튼 활성화
        //메인은 재시작이 활성화 되지 않음
        m_nextBtn.gameObject.SetActive(true);
        if (nowStage.typeStage != Stage.TYPE_STAGE.Main)
            m_retryBtn.gameObject.SetActive(true);
        
    }


    public void OnLobbyClicked()
    {
        Account.GetInstance.nextScene = Prep.sceneLobby;
        SceneManager.LoadScene(Prep.sceneLoad);
    }

    public void OnRetryClicked()
    {
        Account.GetInstance.nextScene = Prep.scenePlay;
        SceneManager.LoadScene(Prep.sceneLoad);
    }
}

