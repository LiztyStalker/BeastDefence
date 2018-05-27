using UnityEngine;
using UnityEngine.SceneManagement;


public class UIPause : UIPanel
{

    public override void openPanel(UIPanel parent)
    {
        base.openPanel(parent);
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.OPEN);
        //시간 멈추기
        Time.timeScale = 0f;
        
    }

    public override void closePanel()
    {
        //시간 재생
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.CLOSE);
        base.closePanel();
        Time.timeScale = 1f;

    }



    public void OnResumeClicked()
    {
        closePanel();
    }

    public void OnHelpClicked()
    {

        //도움말 열기
    }

    public void OnOptionClicked()
    {
        ((UIPlayer)UIPanelManager.GetInstance.root).uiCommon.uiOption.openPanel(this);
    }

    public void OnExitClicked()
    {
        //정말로 퇴각하시겠습니까?
        ((UIPlayer)UIPanelManager.GetInstance.root).uiCommon.uiMsg.setMsg("정말로 퇴각하시겠습니까? \n사용된 식량은 돌려받지 못합니다.", TYPE_MSG_PANEL.WARNING, TYPE_MSG_BTN.OK_CANCEL, exitEvent);
    }


    void exitEvent()
    {
        Account.GetInstance.nextScene = Prep.sceneLobby;
        SceneManager.LoadScene(Prep.sceneLoad);
    }
}

