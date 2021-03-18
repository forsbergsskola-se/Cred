using Clothing;
using Clothing.Upgrade;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities;

namespace HUD.Clothing {
    public class InventoryButtonScript : MonoBehaviour, IPointerClickHandler {
        public Wearable _wearable;
        PopupWindowUpCycleDonate _popupWindow;
        public bool upcyclingClothingChosen;
        public Text stylePointText;
        public Text amountText;

        public void Setup(Wearable wearable, PopupWindowUpCycleDonate popupWindow) {
            _wearable = wearable;
            gameObject.SetActive(true);
            GetComponent<Image>().sprite = wearable.Sprite;
            UpdateAmountStylePoint(wearable);
            print(wearable.StylePoints + " " + wearable.Rarity.name);
            _popupWindow = popupWindow;
        }

        public void UpdateAmountStylePoint(Wearable wearable) {
            stylePointText.text = wearable.StylePoints.ToString();
            amountText.text = wearable.Amount.ToString();
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (!_popupWindow.popupActive) {
                EventBroker.Instance().SendMessage(new EventClothesChanged(_wearable));
                Debug.Log(_wearable.Sprite.name);
            }
            else {
                if (_popupWindow.isUpCycleWindow) {
                    EventBroker.Instance().SendMessage(new EventAddUpCycleClothes(_wearable));
                    upcyclingClothingChosen = true;
                }

                if (_popupWindow.isDonateWindow) {
                    Debug.Log("Donate is Active");
                    EventBroker.Instance().SendMessage(new MessageDonateClothes(_wearable));
                }
            }
        }
    }
}