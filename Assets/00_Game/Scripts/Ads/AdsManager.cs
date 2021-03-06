using UnityEngine;
using UnityEngine.Advertisements;
using Utilities;

namespace Ads {
    public class AdsManager : MonoBehaviour, IUnityAdsListener {
        string gameId = "4044681";
        string adID = "Rewarded_Android";
        bool doubleMyRewards = false;

        void Start() {
            if (Application.platform == RuntimePlatform.Android)
                gameId = "4044681";
            else if (Application.platform == RuntimePlatform.IPhonePlayer) {
                gameId = "4044680";
                adID = "Rewarded_iOS";
            }

            Advertisement.AddListener(this);
            Advertisement.Initialize(gameId, true);
        }

        public void ShowRewardedAd(bool doubleRewards) {
            doubleMyRewards = doubleRewards;
            Advertisement.Show(adID);
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
            if (showResult == ShowResult.Finished) {
                EventBroker.Instance().SendMessage(new EventAdWatched(doubleMyRewards)); //watch ad to earn coins
            } else if (showResult == ShowResult.Failed) {
                Debug.Log("[AdsManager_OnUnityAdsDidFinish]\nError while trying to watch ad");
            } else if (showResult == ShowResult.Skipped) {
                Debug.Log("[AdsManager_OnUnityAdsDidFinish]\nHa ha u tried to skip the ad I see.. no reward for u p*ssy");
            }
        }

        public void OnUnityAdsReady(string placementId) {
            // Make a button/text display that an ad is ready?
        }

        public void OnUnityAdsDidError(string message) {
            Debug.Log("[AdsManager_OnUnityAdsDidError]\nAd got an error");
        }

        public void OnUnityAdsDidStart(string placementId) {
            // Stuff to do when the ad starts
            Debug.Log("[AdsManager_OnUnityAdsDidStart]\nAd started");
        }
    }
}