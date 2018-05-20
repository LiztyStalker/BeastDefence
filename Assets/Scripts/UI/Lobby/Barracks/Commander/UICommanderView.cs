using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Defence.CommanderPackage;

public class UICommanderView : MonoBehaviour
{

    public delegate void CommanderDelegate(CommanderCard commanderCard);
    public event CommanderDelegate commanderEvent;

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Image m_image;

    [SerializeField]
    Text m_forceText;

    [SerializeField]
    Text m_abilityText;

    [SerializeField]
    Text m_contentsText;

    [SerializeField]
    Text m_levelText;

    [SerializeField]
    UISkillIcon[] m_uiSkillIcons;

    [SerializeField]
    UICommanderInfo m_uiCommanderInfo;

    [SerializeField]
    Transform m_commanderListTransform;

    [SerializeField]
    UICommanderCardToggle m_uiCommanderCardToggle;

    [SerializeField]
    Button m_selectBtn;

    List<UICommanderCardToggle> m_cardList = new List<UICommanderCardToggle>();

    ToggleGroup m_toggleGroup;

    void Awake()
    {
        m_selectBtn.onClick.AddListener(() => OnChangeClicked());
        m_toggleGroup = m_commanderListTransform.GetComponent<ToggleGroup>();
    }


    IEnumerator getEnumerator(Stage stage)
    {
        if(stage == null)
            return Account.GetInstance.accUnit.getCommanderCards();
        return Account.GetInstance.accUnit.getCommanderCards(stage.typeForce);
    }

    public void initCards(Stage stage)
    {
        gameObject.SetActive(true);

        removeCards();

        setCommanderCard(stage);

        IEnumerator enumerator = getEnumerator(stage);

        if (enumerator != null)
        {
            while (enumerator.MoveNext())
            {
                //
                CommanderCard commanderCard = enumerator.Current as CommanderCard;


                //스테이지가 있을때 -
                if (stage != null)
                    //
                    if ((stage.typeForce & commanderCard.typeForce) != stage.typeForce)
                        continue;


                UICommanderCardToggle uiToggle = Instantiate(m_uiCommanderCardToggle);
                uiToggle.setCommander(commanderCard);
                uiToggle.GetComponent<Toggle>().onValueChanged.AddListener((isOn) => OnValueChanged(isOn));
                uiToggle.GetComponent<Toggle>().group = m_toggleGroup;
                uiToggle.transform.SetParent(m_commanderListTransform);
                uiToggle.transform.localScale = Vector2.one;

                m_cardList.Add(uiToggle);

                //이미 사용중인 토글은 가리기
                if (uiToggle.commanderCard.key == Account.GetInstance.accUnit.nowCommander)
                {
                    uiToggle.gameObject.SetActive(false);
                }
            }
        }
       
        setCommander(null);
        viewButton();
    }


    void setCommanderCard(Stage stage)
    {
        //세력이 다르면 세력 첫번째 지휘관으로 변경
        CommanderCard commanderCard = Account.GetInstance.accUnit.getCommanderCard(Account.GetInstance.accUnit.nowCommander);

        //스테이지가 있어야 함
        if (stage != null)
        {
            //세력이 다르면
            if ((stage.typeForce & commanderCard.typeForce) != stage.typeForce)
            {
                //해당 세력 첫번째 지휘관으로 변경
                Account.GetInstance.accUnit.nowCommander = Account.GetInstance.accUnit.getFirstCommanderCard(stage.typeForce).key;
                commanderCard = Account.GetInstance.accUnit.getCommanderCard(Account.GetInstance.accUnit.nowCommander);
            }

        }
        //지휘관 셋
        m_uiCommanderInfo.setInfo(commanderCard);
    }


    void removeCards()
    {
        for (int i = m_cardList.Count - 1; i >= 0; i--)
        {
            Destroy(m_cardList[i].gameObject);
        }
        m_cardList.Clear();
    }


    void OnValueChanged(bool isOn)
    {
        foreach (UICommanderCardToggle uiToggle in m_cardList)
        {
            if(uiToggle.gameObject.activeSelf && uiToggle.GetComponent<Toggle>().isOn)
            {
                UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);
                setCommander(uiToggle.commanderCard);
                break;
            }
        }
    }
    

    void setCommander(CommanderCard commanderCard)
    {
        if (commanderCard != null)
        {
            m_nameText.text = commanderCard.name;
            m_image.sprite = commanderCard.image;
            m_image.gameObject.SetActive(true);
            m_forceText.text = string.Format("{0}", commanderCard.typeForce);
            m_abilityText.text = string.Format("체력 : {0}\n지휘력 : {1}\n보급력 : {2}",
                                                commanderCard.health,
                                                commanderCard.leadership,
                                                commanderCard.munitions);
            m_contentsText.text = commanderCard.contents;
            m_levelText.text = string.Format("Lv{0} - {1}/{2}",
                                                commanderCard.level,
                                                commanderCard.nowExperiance,
                                                commanderCard.maxExperiance);

            for (int i = 0; i < m_uiSkillIcons.Length; i++)
            {
                Skill skill = SkillManager.GetInstance.getSkill(commanderCard.skills[i]);
                m_uiSkillIcons[i].setSkill(skill, commanderCard.level);
            }
        }
        else
        {
            m_nameText.text = "-";
            m_image.sprite = null;
            m_image.gameObject.SetActive(false);
            m_forceText.text = "-";
            m_abilityText.text = string.Format("체력 : -\n지휘력 : -\n보급력 : -");
            m_contentsText.text = "-";
            m_levelText.text = string.Format("Lv- - -/-");

            for (int i = 0; i < m_uiSkillIcons.Length; i++)
            {
                m_uiSkillIcons[i].setSkill(null, 0);
            }
        }

    }

    public void OnChangeClicked()
    {

        string prepCommanderKey = Account.GetInstance.accUnit.nowCommander;

        foreach (UICommanderCardToggle uiToggle in m_cardList)
        {
            //전 토글 보이기
            if (uiToggle.commanderCard.key == prepCommanderKey)
            {
                uiToggle.gameObject.SetActive(true);
                continue;
            }

            //선택한 토글 가리기
            if (uiToggle.GetComponent<Toggle>().isOn)
            {
                Account.GetInstance.accUnit.nowCommander = uiToggle.commanderCard.key;
                m_uiCommanderInfo.setInfo(uiToggle.commanderCard);
                uiToggle.gameObject.SetActive(false);
//                setCommander(uiToggle.commanderCard);
            }
        }

        UIPanelManager.GetInstance.root.uiCommon.btnSoundPlay.audioPlay(TYPE_BTN_SOUND.NONE);

        //선택된 정보 사라지기
        setCommander(null);


        viewButton();
        

    }

    /// <summary>
    /// 버튼 보이기
    /// </summary>
    void viewButton()
    {
        foreach (UICommanderCardToggle uiToggle in m_cardList)
        {
            if (uiToggle.gameObject.activeSelf)
            {
                uiToggle.GetComponent<Toggle>().isOn = true;
                setCommander(uiToggle.commanderCard);
                m_selectBtn.gameObject.SetActive(true);
                return;
            }
        }

        //지휘관이 1명만 있으면 선택버튼 안보임
        m_selectBtn.gameObject.SetActive(false);

    }

    void removeUnitCard()
    {
        for (int i = m_cardList.Count - 1; i >= 0; i--)
        {
            Destroy(m_cardList[i].gameObject);
        }

        m_cardList.Clear();

    }
}

