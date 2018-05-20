using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Defence.TipManager;

public class UILoad : MonoBehaviour
{
	[SerializeField] Image m_imagePanel;
	[SerializeField] Text m_tipText;
	[SerializeField] Slider m_loadBar;
	[SerializeField] Text m_loadPercentText;
//	[SerializeField] Text m_loadContentsText;

	AsyncOperation loadSceneAsync = null;
//	TipClass m_tipData = null;
	
	
	void Start(){

        //팁 매니저에서 가져오기
        setTip(TipManager.GetInstance.getRandomTip());
        
        StartCoroutine(loadSceneCoroutine());
	}



    void setTip(Tip tip)
    {
        if (tip != null)
        {
            m_tipText.text = tip.contents;
            m_imagePanel.sprite = tip.icon;
        }
    }
	
	
	/// <summary>
	/// 게임 화면으로 이동
	/// </summary>
	/// <returns>The scene coroutine.</returns>
	IEnumerator loadSceneCoroutine(){
		
//		PlayerPrefs.DeleteKey ("isLoad");


        
        loadSceneAsync = SceneManager.LoadSceneAsync(Account.GetInstance.nextScene);

		while (!loadSceneAsync.isDone) {
//			Debug.Log("load Scene " + loadSceneAsync.progress);
			m_loadPercentText.text = string.Format("{0:f0}%", loadSceneAsync.progress * 100f);
			m_loadBar.value = loadSceneAsync.progress;



			yield return null;
		}

        //기존 데이터 로딩 - 로딩이 이미 되어있으면 - 먼저 로딩이 먼저 되었는지 확인
        //로딩이 안되어있으면 콜백으로 대기
        UnitManager.GetInstance.initInstance(); // 유닛 배치중
        EffectManager.GetInstance.initInstance(); //효과 생성중
        BulletManager.GetInstance.initInstance(); //탄환 수집중
        SkillManager.GetInstance.initInstance(); //스킬 훈련중
        WorldManager.GetInstance.initInstance(); //월드 생성중
        SoundManager.GetInstance.initInstance(); //사운드 생성중
        SinarioAwardManager.GetInstance.initInstance(); //보상 모으는중
        ShopManager.GetInstance.initInstance(); //상점 여는중
        TipManager.GetInstance.initInstance(); //힌트 생성중
        BuffManager.GetInstance.initInstance(); //버프 생성중
        AchieveManager.GetInstance.initInstance(); //업적 기록중
	}
}

