using UnityEngine;
using UnityEngine.UI;

public abstract class UIMissionButton : MonoBehaviour
{


    public enum TYPE_ALARM { Main, Warning }

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Sprite[] m_alarmIcons;

    [SerializeField]
    Image m_icon;

    [SerializeField]
    Image m_alarmIcon;


    protected Text nameText { get { return m_nameText; } }
    protected Sprite[] alarmIcons { get { return m_alarmIcons; } }
    protected Image icon { get { return m_icon; } }
    protected Image alarmIcon { get { return m_alarmIcon; } }

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => OnClicked());
    }

    protected abstract void OnClicked();

}

