using System.Collections.Generic;
using Clothing.Inventory;
using HUD.Clothing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utilities;

namespace Clothing.Upgrade.UpCycle {
    public class AssignCombinedWearableToUpCycle : MonoBehaviour, IPointerClickHandler {
        [FormerlySerializedAs("combineWearable")]
        CombinedWearables combinedWearable;
        public Text stylePointText;

        bool popUpWindowIsActive;

        void Start() {
            combinedWearable = GetComponent<CombinedWearables>();
        }

        void OnEnable() {
            EventBroker.Instance().SubscribeMessage<EventTogglePopWindow>(ValidateShouldDressUp);
        }

        void OnDisable() {
            EventBroker.Instance().UnsubscribeMessage<EventTogglePopWindow>(ValidateShouldDressUp);
        }

        public void Setup(CombinedWearables wearable, PopupWindowUpCycleDonate popupWindow) {
            this.combinedWearable = wearable;
            gameObject.SetActive(true);
            UpdateAmountStylePoint();
        }

        void UpdateAmountStylePoint() {
            stylePointText.text = combinedWearable.stylePoints.ToString();
        }

        void ValidateShouldDressUp(EventTogglePopWindow popUp) {
            popUpWindowIsActive = popUp.popWindowIsActive;
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (!popUpWindowIsActive) {
                if (GetComponent<CombinedWearables>().Amount == 0) {
                    EventBroker.Instance().SendMessage(new EventBuyNotOwnedClothes(combinedWearable));
                    return;
                }
                EventBroker.Instance().SendMessage(new EventClothesChanged(combinedWearable));
                UnityEngine.Analytics.Analytics.CustomEvent("Equip Cloths", new Dictionary<string, object> {
                    {"New Cloth", combinedWearable.ToString()}
                });
                return;
            }


            EventBroker.Instance().SendMessage(new EventAddToUpgradeSlot(combinedWearable));
            //EventBroker.Instance().SendMessage(new EventUpdateWearableHud());
        }
    }
}