using UnityEngine;
using UnityEngine.UI;

public class UIMissionStageButton : UIMissionButton
{
    //아이콘
    //해당 미션
    //이벤트 미션 등
    //클릭시 정보 보여주기




    public delegate void StageInfoDelegate(Stage stage);
    public event StageInfoDelegate stageInfoEvent;

    //큰맵 버튼 - 누르면 작은맵으로
    //작은맵 버튼 - 누르면 정보 보여주기

    Stage m_stage;

    public void setBtn(Stage stage)
    {
        m_stage = stage;
//        transform.position = stage.coordinate;
        nameText.text = stage.name;
        icon.sprite = stage.icon;
    }

    protected override void OnClicked()
    {
        //
        //미션 내용 보이기
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

        stageInfoEvent(m_stage);
    }


}

