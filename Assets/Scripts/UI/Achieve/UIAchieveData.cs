using UnityEngine;
using UnityEngine.UI;

public class UIAchieveData : MonoBehaviour
{

    public delegate void RefleshAchieveDataDelegate(Achieve achieve);

    public event RefleshAchieveDataDelegate refleshAchieveDataEvent;

    [SerializeField]
    Image m_image;

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Slider m_rateSlider;

    [SerializeField]
    Text m_rateText;

    [SerializeField]
    Image m_awardImage;

    [SerializeField]
    Text m_awardText;

    [SerializeField]
    GameObject m_successObject;

    [SerializeField]
    Button m_awardButton;



    Achieve m_achieve;

    public bool isSuccess { get { return Account.GetInstance.accAchieve.isAchieve(m_achieve.key); } }
    public float valueRate { get { return m_achieve.valueRate(); } }
    public string key { get { return m_achieve.key; } }

    void Awake()
    {
        m_awardButton.onClick.AddListener(() => OnClicked());
    }

    public void setAchieve(Achieve achieve)
    {
        m_achieve = achieve;

        m_image.sprite = achieve.icon;
        m_nameText.text = achieve.name;


        int nowValue = Account.GetInstance.accAchieve.getAchieveValue(achieve.typeAchieve);
        int maxValue = achieve.value;

        m_rateSlider.value = valueRate;
        m_rateText.text = string.Format("{0}/{1}({2:f2}%)", nowValue, maxValue, m_rateSlider.value * 100f);

        m_awardImage.sprite = AchieveManager.GetInstance.getTypeCategorySprite(m_achieve.typeAward);

        m_awardText.text = achieve.awardValue.ToString();




        if (isSuccess)
        {
            //보상 받음
            m_rateText.text = "-";
            m_successObject.SetActive(true);
            m_awardButton.gameObject.SetActive(false);
        }
        else
        {
            //보상 아직 안 받음
            m_successObject.SetActive(false);

            if (m_rateSlider.value >= 1f)
            {
                //보상 받는 버튼 활성화
                m_rateText.text = "터치하여 보상을 받으세요!";
                m_awardButton.gameObject.SetActive(true);
            }
            else
            {
                //버튼 비활성화
                m_awardButton.gameObject.SetActive(false);
            }
        }

    }


    //버튼을 누르면 보상
    void OnClicked()
    {
        //Debug.Log("보상 받음");
        //보상 애니메이션 실행

        if (!Account.GetInstance.accAchieve.isAchieve(m_achieve.key))
        {


            EffectActorManager effectActorManager = (EffectActorManager)ActorManager.GetInstance.getActorManager(typeof(EffectActorManager));

            if (effectActorManager != null)
            {
                effectActorManager.createActor("AchieveSuccessParticle", Camera.main.ScreenToWorldPoint(Input.mousePosition), UnitActor.TYPE_CONTROLLER.PLAYER, false);
            }
            else
            {
                Debug.LogWarning("못찾음");
            }



            Account.GetInstance.accAchieve.setAchieve(m_achieve.key);
            Account.GetInstance.accData.addValue(m_achieve.awardValue, m_achieve.typeAward);

            //새로고침
            refleshAchieveDataEvent(m_achieve);
        }
    }

}

