using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
//    [SerializeField]
//    Text m_gameText;

    [SerializeField]
    GameObject m_gameReady;

    [SerializeField]
    GameObject m_gameStart;

    [SerializeField]
    GameObject m_gameVictory;

    [SerializeField]
    GameObject m_gameDefeat;


    public void gameReady()
    {


//        m_gameReady.SetActive(true);

        //m_gameText.text = "준비";

        //if (m_gameText.isActiveAndEnabled)
        //    m_gameText.gameObject.SetActive(false);

        StartCoroutine(startCoroutine(m_gameReady, 1f));
    }


    public void gameStart()
    {
//        m_gameText.text = "시작!";

//        m_gameStart.SetActive(true);

        StartCoroutine(startCoroutine(m_gameStart.gameObject, 1f));
    }






    public void gameEnd(bool isVictory)
    {
        if(isVictory)
            StartCoroutine(startCoroutine(m_gameVictory, 3f));
        //            m_gameText.text = "승리";
        else
            StartCoroutine(startCoroutine(m_gameDefeat, 3f));
        //            m_gameText.text = "패배";

//        StartCoroutine(startCoroutine(m_gameText.gameObject, 3f));

    }

    IEnumerator startCoroutine(GameObject gameobject, float time)
    {

        gameobject.SetActive(true);
        yield return new WaitForSeconds(time);
        gameobject.SetActive(false);

    }
}

