using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIDataBox : MonoBehaviour, IPointerClickHandler
{
    protected Coroutine m_coroutine;

    protected void Awake()
    {
        close();
    }

    protected void Start()
    {
        if (m_coroutine != null)
            StopCoroutine(m_coroutine);
        m_coroutine = StartCoroutine(dataViewCoroutine());
    }

    protected IEnumerator dataViewCoroutine()
    {
        yield return new WaitForSeconds(3f);
        m_coroutine = null;
        gameObject.SetActive(false);
    }

    public virtual void close()
    {
        if (m_coroutine != null)
            StopCoroutine(m_coroutine);
        m_coroutine = null;
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        close();
    }
}

