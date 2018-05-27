using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIGameMsg : MonoBehaviour
{
    const float appearTime = 2f;

    [SerializeField]
    GameObject m_warningObject;

    [SerializeField]
    GameObject m_infoObject;

    [SerializeField]
    GameObject m_allyHeroObject;

    [SerializeField]
    GameObject m_enemyHeroObject;

    [SerializeField]
    Text m_msgText;

    Coroutine coroutine = null;

    void Awake()
    {
        gameFinish();
    }
        


    public void gameFinish(){
        if (coroutine != null) StopCoroutine(coroutine);
        m_warningObject.SetActive(false);
        m_infoObject.SetActive(false);
        m_allyHeroObject.SetActive(false);
        m_enemyHeroObject.SetActive(false);
    }
    /// <summary>
    /// 성 체력 경고창 출력
    /// </summary>
    /// <param name="rate">성 체력 비율</param>
    public void setCastleWarning(float rate)
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.WARNING);
        m_warningObject.SetActive(rate <= Prep.castleWarningRate);
    }

    /// <summary>
    /// 메시지 출력
    /// </summary>
    /// <param name="msg"></param>
    public void setMsg(string msg)
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.INFOR);
        m_msgText.text = msg;
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(startCoroutine(m_infoObject, 1f));

    }

    /// <summary>
    /// 영웅 등장 출력
    /// </summary>
    public void setHeroAppear(UIController uiCtrler)
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.WARNING);
        if (coroutine != null)
            StopCoroutine(coroutine);

        if (uiCtrler is UIPlayer)
        {
            //아군
            coroutine = StartCoroutine(startCoroutine(m_allyHeroObject, appearTime));
        }
        else if (uiCtrler is UICPU)
        {
            //적군
            coroutine = StartCoroutine(startCoroutine(m_enemyHeroObject, appearTime));
        }
        else
        {
            Prep.LogError(uiCtrler.name, "컨트롤러가 정해지지 않았음", GetType());
        }

    }

    IEnumerator startCoroutine(GameObject gameobject, float time)
    {

        gameobject.SetActive(true);
        yield return new WaitForSeconds(time);
        gameobject.SetActive(false);
        coroutine = null;

    }

}

