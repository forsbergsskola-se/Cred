using System;
using System.Collections;
using Clothing.Inventory;
using Clothing.Upgrade.Donation;
using Clothing.Upgrade.UpCycle;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
namespace HUD.MysteryBox {
    public class RewardDisplay : MonoBehaviour {
        [SerializeField] float sizeToDisplayReward = 4f;
        [SerializeField] Text rewardText;
        DonationValidityCheck donationValidityCheck;
        public string rewardMessage;

        void OnEnable() {
            EventBroker.Instance().SubscribeMessage<EventShowReward>(ShowReward);
            rewardText.text = "";
        }

        void OnDisable() {
            EventBroker.Instance().UnsubscribeMessage<EventShowReward>(ShowReward);
        }

        void ShowReward(EventShowReward eventShowReward) {
            var reward = eventShowReward.Reward;
            var instance = Instantiate(reward.gameObject, this.transform);
            if (eventShowReward.textAmount > 0) {
                rewardText.text = rewardMessage + eventShowReward.textAmount;
            }
            else {
                rewardText.text = rewardMessage;
            }

            for (var i = 1; i < instance.transform.childCount; i++) {
                instance.transform.GetChild(i).gameObject.SetActive(false);
            }

            Resize(instance, sizeToDisplayReward);
            instance.GetComponent<AssignCombinedWearableToUpCycle>().enabled = false;
            StartCoroutine(DestroyRewardOnClick(instance));
        }

        IEnumerator DestroyRewardOnClick(GameObject go) {
            while (!Input.GetKeyDown(KeyCode.Mouse0)) {
                yield return null;
            }
            rewardText.text = "";
            donationValidityCheck = FindObjectOfType<DonationValidityCheck>();
            donationValidityCheck.stylePointsBackground.gameObject.SetActive(false);
            EventBroker.Instance().SendMessage(new EventUpdateWearableHud());
            Destroy(go);
        }

        void Resize(GameObject go, float newScale) {
            var newSize = new Vector2(newScale, newScale);
            go.GetComponent<RectTransform>().localScale = newSize;
        }
    }
}