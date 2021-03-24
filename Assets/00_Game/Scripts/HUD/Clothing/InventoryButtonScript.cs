using Clothing;
using Clothing.Upgrade;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utilities;

namespace HUD.Clothing {
    public class InventoryButtonScript : MonoBehaviour, IPointerClickHandler {
        [FormerlySerializedAs("combineWearable")]
        public CombinedWearables combinedWearable;
        PopupWindowUpCycleDonate _popupWindow;
        public bool upcyclingClothingChosen;
        public Text stylePointText;
        public Text amountText;

        bool popUpWindowIsActive;
        void Start() {
            combinedWearable = GetComponent<CombinedWearables>();
            
            EventBroker.Instance().SendMessage(new EventUpdatePlayerInventory(combinedWearable, -1));
            
            //what wearable in "combinedWearable" in slot1 (orange)
            //what wearable in "combinedWearable" in slot2 (black)
            
            //generate new combined wearable (orange + black)
            
            //send message new item created
            //send message subtract from slot1
            //send message subtract from slot2
            EventBroker.Instance().SubscribeMessage<EventTogglePopWindow>(ValidateShouldDressUp);
        }

        public void Setup(CombinedWearables wearable, PopupWindowUpCycleDonate popupWindow) {
            this.combinedWearable = wearable;
            gameObject.SetActive(true);
            //GetComponent<Image>().sprite = wearable.Sprite;
            UpdateAmountStylePoint();
            //print(wearable.stylePoints + " " + wearable.Rarity.name);
            _popupWindow = popupWindow;
            LockedUpcycledWearables();
        }

        public void UpdateAmountStylePoint() {
            stylePointText.text = combinedWearable.stylePoints.ToString();
            //TODO amount is not in wearable
            //amountText.text = combinedWearable.stylePoints.ToString();
        }

        void ValidateShouldDressUp(EventTogglePopWindow popUp) {
            popUpWindowIsActive = popUp.popWindowIsActive;
        }

        public void LockedUpcycledWearables() {
            // if (!wearable.HasUnlockedUpCycledWearable() && wearable.isUpCycledWearable)
            // {
            //     gameObject.SetActive(false);
            // }
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (!popUpWindowIsActive) {
                EventBroker.Instance().SendMessage(new EventClothesChanged(combinedWearable));
                return;
            }
            
            EventBroker.Instance().SendMessage(new EventAddToUpgradeSlot(combinedWearable, this));
            
            // if (!combinedWearable.Unlocked()) {
            //     Debug.Log("Not unlocked.");
            //     return;
            // }
            // if (!_popupWindow.popupActive) {
            //     EventBroker.Instance().SendMessage(new EventClothesChanged(combinedWearable));
            //     Debug.Log(combinedWearable.Sprite.name);
            // }
            // else {
            //     if (_popupWindow.isUpCycleWindow && combinedWearable.Amount > 0) {
            //         EventBroker.Instance().SendMessage(new EventAddUpCycleClothes(combinedWearable, this));
            //         upcyclingClothingChosen = true;
            //     }
            //
            //     if (_popupWindow.isDonateWindow) {
            //         Debug.Log("Donate is Active");
            //         EventBroker.Instance().SendMessage(new MessageDonateClothes(combinedWearable));
            //     }
            //}
        }
    }
}