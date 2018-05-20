using System;
using UnityEngine;
using UnityEngine.UI;

public class UIResource : MonoBehaviour
{

    [SerializeField]
    Text m_nameText;

    [SerializeField]
    Text m_levelText;

    [SerializeField]
    Slider m_expSlider;

    [SerializeField]
    Text m_expText;
    
    [SerializeField]
    Text m_foodText;

    [SerializeField]
    Slider m_foodSlider;

    [SerializeField]
    Text m_foodTimeText;

    [SerializeField]
    Text m_goldText;

    [SerializeField]
    Text m_fruitText;



    public void FixedUpdate()
    {
        m_nameText.text = Account.GetInstance.name;
        m_levelText.text = string.Format("Lv {0}", Account.GetInstance.level);
        m_expText.text = string.Format("{0}/{1}", Account.GetInstance.nowExp, Account.GetInstance.maxExp);
        m_expSlider.value = (float)Account.GetInstance.nowExp / (float)Account.GetInstance.maxExp;

        m_foodText.text = string.Format("{0}/{1}", Account.GetInstance.nowFood, Account.GetInstance.maxFood);
        m_foodTimeText.text = Account.GetInstance.accData.nowTime();
        m_foodSlider.value = Account.GetInstance.accData.nowTimeRate();
        m_goldText.text = Account.GetInstance.gold.ToString();
        m_fruitText.text = Account.GetInstance.fruit.ToString();

    }
}

