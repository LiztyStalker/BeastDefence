using System;
using UnityEngine;

public class UIMissionAreaButton : UIMissionButton
{


    public delegate void AreaInfoDelegate(Area area);
    public event AreaInfoDelegate areaInfoEvent;

    //큰맵 버튼 - 누르면 작은맵으로
    //작은맵 버튼 - 누르면 정보 보여주기

    Area m_area;

    public Area area { get { return m_area; } }

    public bool isCompare(Area area)
    {
        return m_area == area;
    }

    public void setBtn(Area area, Vector2 pos)
    {
        if (area != null)
        {
            m_area = area;
            transform.position = area.pos + pos;
            nameText.text = area.name;
            icon.sprite = area.icon;
        }

        //현재 최근 미션이 있는지 여부 필요
        //현재 서브 미션이 있는지 여부 필요
        alarmIcon.sprite = null;
        alarmIcon.gameObject.SetActive(false);
    }

    /// <summary>
    /// 알람 아이콘 보이기
    /// </summary>
    /// <param name="typeAlarm"></param>
    public void setAlarm(UIMissionButton.TYPE_ALARM typeAlarm)
    {
        alarmIcon.sprite = alarmIcons[(int)typeAlarm];
        alarmIcon.gameObject.SetActive(true);
    }

    protected override void OnClicked()
    {
        //하위 스테이지 보이기
        //areaInfoEvent(m_area);

        //미션 정보 보이기
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);
        areaInfoEvent(m_area);
    }

}

