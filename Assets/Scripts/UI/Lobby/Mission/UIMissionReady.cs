using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMissionReady : UIPanel
{


    [SerializeField]
    UIMissionMap m_uiMap;

    [SerializeField]
    UIMissionCampInfo m_uiAllyCamp;

    [SerializeField]
    UIMissionCampInfo m_uiEnemyCamp;

    [SerializeField]
    Text m_foodText;


    int m_foodCount;

    //시나리오
    //적진영
    //아군진영
    //맵
    //지휘관
    //
    protected override void OnEnable()
    {
        base.OnEnable();
        setMissionReady(Account.GetInstance.accSinario.nowStage);
    }


    public void setMissionReady(Stage stage)
    {
        //시나리오 데이터 가져와야 함
        
        //아군진영 - Account
        m_uiAllyCamp.setAllyCamp(stage);
        
        //적진영 - 시나리오
        m_uiEnemyCamp.setEnemyCamp(stage);

        //맵 - 시나리오
        m_uiMap.setMap(MapManager.GetInstance.getMap(stage.mapKey));

        m_foodCount = Account.GetInstance.accUnit.getUnitTotalPopulation(stage.typeForce);
        //시나리오 - 
        m_foodText.text = string.Format("-{0}", m_foodCount);
    }



    public void OnStartClicked()
    {
        if (Account.GetInstance.accData.isValue(m_foodCount, Shop.TYPE_SHOP_CATEGORY.Food))
        {
            UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

            //출전
            Account.GetInstance.setEngage(Account.GetInstance.accSinario.nowStage.typeForce);

            //음식 소비
            Account.GetInstance.accData.useValue(m_foodCount, Shop.TYPE_SHOP_CATEGORY.Food);

            //씬 기억
            Account.GetInstance.nextScene = Prep.scenePlay;
            SceneManager.LoadScene(Prep.sceneLoad);
        }
        else
        {
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("식량이 부족합니다.", TYPE_MSG_PANEL.ERROR, TYPE_MSG_BTN.OK);
        }
    }
}

