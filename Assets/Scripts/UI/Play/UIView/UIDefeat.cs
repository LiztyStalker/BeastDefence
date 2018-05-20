using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIDefeat : MonoBehaviour
{
    [SerializeField]
    Text m_foodText;

    int m_foodCount = 0;

    public void setDefeat()
    {
        gameObject.SetActive(true);

        //병사 인구만큼 푸드 소모
        //

        m_foodCount = Account.GetInstance.accUnit.getUnitTotalPopulation(Account.GetInstance.accSinario.nowStage.typeForce);
        m_foodText.text = string.Format("-{0}", m_foodCount);
    }

    public void OnLobbyClicked()
    {
        //로비로
        Account.GetInstance.nextScene = Prep.sceneLobby;
        SceneManager.LoadScene(Prep.sceneLoad);
    }

    public void OnRetryClicked()
    {
        //재시작
        //푸드 소모
        if (Account.GetInstance.accData.isValue(m_foodCount, Shop.TYPE_SHOP_CATEGORY.Food))
        {
            Account.GetInstance.accData.useValue(m_foodCount, Shop.TYPE_SHOP_CATEGORY.Food);
        }
        else
        {
            //음식이 부족합니다.
        }
        
        Account.GetInstance.nextScene = Prep.scenePlay;
        SceneManager.LoadScene(Prep.sceneLoad);
    }
}

