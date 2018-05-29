using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIName : MonoBehaviour, IEnumerator
{
    [SerializeField]
    Button m_okButton;

    [SerializeField]
    InputField m_inputField;

    bool isClicked = false;


    public object Current
    {
        get {
            if(!gameObject.activeSelf)
                gameObject.SetActive(true);
            return null; 
        }
    }

    public bool MoveNext()
    {
        return !isClicked;
    }

    public void Reset(){}

    void Awake()
    {
        m_okButton.onClick.AddListener(() => OnClicked());
    }


    void OnClicked()
    {

        if(m_inputField.text.Length < 2)
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("계정이름이 짧습니다. 2칸 이상 8칸 이하로 적어주세요.", TYPE_MSG_PANEL.WARNING, TYPE_MSG_BTN.OK);
        else if(m_inputField.text.Length >= 8)
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg("계정이름이 깁니다. 2칸 이상 8칸 이하로 적어주세요.", TYPE_MSG_PANEL.WARNING, TYPE_MSG_BTN.OK);
        else
            UIPanelManager.GetInstance.root.uiCommon.uiMsg.setMsg(string.Format("계정이름을 '{0}' 로 사용하시겠습니까?", m_inputField.text), TYPE_MSG_PANEL.INFO, TYPE_MSG_BTN.OK_CANCEL, clicked);
    }

    void clicked()
    {
        Account.GetInstance.accData.setName(m_inputField.text);
        isClicked = true;
        //튜토리얼 시작
        gameObject.SetActive(false);
    }

   
    

}

