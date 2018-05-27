using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsHelper : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Advertisement.Show();
        ShowRewardedVideo();
	}

    void ShowRewardedVideo()
    {
        ShowOptions opts = new ShowOptions();
        opts.resultCallback = HandleShowResult;

        Advertisement.Show("rewardedVideo", opts);
    }


    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("비디오 완료");
        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("비디오 스킵");
        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("비디오 보기 실패");
        }
    }
	
}
