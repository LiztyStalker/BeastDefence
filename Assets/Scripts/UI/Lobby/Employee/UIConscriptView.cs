using System.Collections.Generic;
using UnityEngine;

public class UIConscriptView : UIPanel
{
    [SerializeField]
    UICardSummary m_uiUniCard;

    [SerializeField]
    Transform m_conscriptViewPanel;

    UIUnitInfomation m_uiUnitInformation;

    Vector2 gapPos = new Vector2(3.5f, 2.1f);


    List<UICardSummary> m_unitCardList = new List<UICardSummary>();
    List<Unit> m_unitList = new List<Unit>();

    public void setConscript(int count, Unit.TYPE_UNIT typeUnit)
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.OPEN);

        openPanel(null);


        m_uiUnitInformation = UIPanelManager.GetInstance.root.uiCommon.uiUnitInfomation;


        //1개
        //5개
        //10개
        //영웅

        for (int i = 0; i < count; i++)
        {
            m_unitList.Add(UnitManager.GetInstance.getRandomUnit(0, typeUnit));
            Account.GetInstance.accUnit.addUnit(m_unitList[i]);
        }

        //코루틴 작업 필요

        if (count == 1)
        {
            UICardSummary uiUnitCard = Instantiate(m_uiUniCard) as UICardSummary;
            uiUnitCard.setUnit(m_unitList[0], false);
            uiUnitCard.unitInformationEvent += m_uiUnitInformation.setSummary;
            uiUnitCard.transform.position = Vector2.zero;
            uiUnitCard.transform.SetParent(m_conscriptViewPanel);
            uiUnitCard.transform.localScale = Vector2.one;
            m_unitCardList.Add(uiUnitCard);

        }
        else if (count == 5)
        {
            for (int i = 0; i < count; i++)
            {
                UICardSummary uiUnitCard = Instantiate(m_uiUniCard) as UICardSummary;

                uiUnitCard.setUnit(m_unitList[i], false);
                uiUnitCard.unitInformationEvent += m_uiUnitInformation.setSummary;
                uiUnitCard.transform.position = new Vector2((float)(i - 2) * gapPos.x, 0f);
                uiUnitCard.transform.SetParent(m_conscriptViewPanel);
                uiUnitCard.transform.localScale = Vector2.one;
                m_unitCardList.Add(uiUnitCard);
            }
        }
        else if (count == 10)
        {
            for (int i = 0; i < count; i++)
            {
                UICardSummary uiUnitCard = Instantiate(m_uiUniCard) as UICardSummary;
                uiUnitCard.setUnit(m_unitList[i], false);
                uiUnitCard.unitInformationEvent += m_uiUnitInformation.setSummary;
                uiUnitCard.transform.position = new Vector2((float)((i % 5) - 2) * gapPos.x, (gapPos.y * ((i / 5 >= 1) ? -1f : 1f)) + 0.75f);
                uiUnitCard.transform.SetParent(m_conscriptViewPanel);
                uiUnitCard.transform.localScale = Vector2.one;
                m_unitCardList.Add(uiUnitCard);
            }

        }
    }


    protected override void OnDisable()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.CLOSE);

        for (int i = m_unitCardList.Count - 1; i >= 0; i--)
        {
            Destroy(m_unitCardList[i].gameObject);
        }

        m_unitCardList.Clear();
        m_unitList.Clear();

        base.OnDisable();
    }
}

