using UnityEngine;
using UnityEngine.UI;

public class UIDevelopButton : MonoBehaviour
{


    public delegate void DevelopViewInfoDelegate(UIDevelopButton devBtn, int level);
    public event DevelopViewInfoDelegate developViewInfoEvent;
    

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Image m_image;

    [SerializeField]
    Text m_costText;

    [SerializeField]
    Text m_levelText;

    [SerializeField]
    Transform m_linkTransform;

    [SerializeField]
    LineRenderer[] m_lineRenderers;

    [SerializeField]
    Transform m_notUseTransform;

    [SerializeField]
    Text m_notUseText;

    Develop m_develop;
    int m_level;

    public Develop develop { get { return m_develop; } }
    public Transform linkTransform { get { return m_linkTransform; } }

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => OnDevelopClicked());
    }

    /// <summary>
    /// 개발 설정
    /// </summary>
    /// <param name="develop">개발</param>
    /// <param name="level">개발 현재 레벨</param>
    public void setDevelop(Develop develop, int nowLevel)
    {
        m_develop = develop;
        m_level = nowLevel;

        m_nameText.text = develop.name;
        m_image.sprite = develop.icon;
        m_levelText.text = string.Format("Lv {0}", nowLevel);
        m_costText.text = develop.getCost(nowLevel).ToString();
        m_notUseTransform.gameObject.SetActive(false);

        //테크레벨과 부모가 개발되어야 활성화
        //부모가 되어있지 않으면 정보 보여주어야 함




        //테크레벨 가져오기,
        //부모 레벨 가져오기
        //

        m_notUseText.text = "";


        Debug.Log("level : " + m_develop.typeValueGroup + " " + Account.GetInstance.accDevelop.getDevelopValue(m_develop.typeValueGroup).ToString() + " " + m_develop.techLevel.ToString());

        if (nowLevel >= develop.maxLevel)
        {
            //마스터됨
            GetComponent<Button>().interactable = false;
            m_notUseTransform.gameObject.SetActive(true);
            m_notUseText.text = develop.name + "\n개발 완료";

        }

        //현재 보이는 종류의 연구된 테크가 같거나 높으면
        else if ((int)Account.GetInstance.accDevelop.getDevelopTechLevel(m_develop.typeDevGroup) >= m_develop.techLevel)
        //else if ((Account.GetInstance.accData.level / 10) + 1 > develop.techLevel)
        {

            //지정된 부모 레벨보다 낮으면
            foreach (string parentKey in develop.parentDic.Keys)
            {
                Develop parentDev = DevelopManager.GetInstance.getDevelop(parentKey);

                //플레이어가 개발
                int parentLv = Account.GetInstance.accDevelop.getDevelopLevel(parentKey);

                //
                if (develop.parentDic[parentKey] > parentLv)
                {
                    //부모레벨 조건 체크
                    GetComponent<Button>().interactable = false;
                    m_notUseTransform.gameObject.SetActive(true);
                    m_notUseText.text += parentDev.name + " Lv" + develop.parentDic[parentKey] + " 필요\n";
                }
            }

            if (m_notUseText.text == "")
            {
                GetComponent<Button>().interactable = true;
                m_notUseTransform.gameObject.SetActive(false);
                m_notUseText.text = "";
            }

        }
        else
        {
            GetComponent<Button>().interactable = false;
            m_notUseTransform.gameObject.SetActive(true);
            m_notUseText.text = string.Format("{0} 기술 레벨 필요",  m_develop.techLevel + 1);
        }

    }

    //라인 그리기
    public void setLine(int index, Vector2 pos)
    {

//        Debug.Log("pos : " + transform.position + " " + pos);
        m_lineRenderers[index].positionCount = 2;
//        m_lineRenderers[index].useWorldSpace = false;
        m_lineRenderers[index].SetPosition(0, (Vector2)m_lineRenderers[index].transform.position);
        m_lineRenderers[index].SetPosition(1, pos);
    }

    //라인 업데이트
    




    void OnDevelopClicked()
    {
        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

        //부모도 업데이트 되어야 함
        developViewInfoEvent(this, m_level);
    }


}

