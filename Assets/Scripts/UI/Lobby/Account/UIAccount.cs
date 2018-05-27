using UnityEngine;
using UnityEngine.UI;

public class UIAccount : UIPanel
{
    [SerializeField]
    Image m_icon;

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Text m_levelText;

    [SerializeField]
    Slider m_expSlider;

    [SerializeField]
    Text m_expText;

    [SerializeField]
    Text m_playTimeText;

    [SerializeField]
    Text m_totalDaysText;

    [SerializeField]
    Text m_cmdText;

    [SerializeField]
    Text m_soldierText;

    [SerializeField]
    Text m_heroText;

    [SerializeField]
    Text m_achieveText;

    protected override void OnEnable()
    {
        base.OnEnable();
        setAccount();
    }

    void setAccount()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.OPEN);

        m_icon.sprite = null;
        m_nameText.text = Account.GetInstance.name;
        m_levelText.text = string.Format("{0}", Account.GetInstance.level);
        m_expSlider.value = Account.GetInstance.accData.expRate;
        m_expText.text = string.Format("{0}/{1}", Account.GetInstance.nowExp, Account.GetInstance.maxExp);
        m_playTimeText.text = string.Format("{0}", Account.GetInstance.accData.getPlayTime());
        m_totalDaysText.text = string.Format("{0}일", Account.GetInstance.accData.getTotalDays());
        m_cmdText.text = string.Format("{0}", Account.GetInstance.accUnit.getCommanderCount());
        m_soldierText.text = string.Format("{0}", Account.GetInstance.accUnit.getUnitCount(Unit.TYPE_UNIT.Soldier));
        m_heroText.text = string.Format("{0}", Account.GetInstance.accUnit.getUnitCount(Unit.TYPE_UNIT.Hero));
        m_achieveText.text = string.Format("{0}", Account.GetInstance.accAchieve.getAchieveSuccessCount());
    }

    public override void closePanel()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.CLOSE);
        base.closePanel();
    }
}

