using Clothing.Inventory;
using Currency.Coins;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Clothing.Upgrade.Donation {
    public class DonationValidityCheck : MonoBehaviour {
        DonationPopUpWarnings donationPopUpWarnings;
        Coin coin;
        public Image itemToDonateSlot;
        public Image upgradedItemSlot;
        
        public Button[] alternativesButtons;
        int upgradedOriginalStylePoints;
        CanvasGroup canvasGroup;
        int costOfDonation;
        int upgradedStylePoints;

        public Button confirmButton;

        CombinedWearables originalWearable;
        CombinedWearables upgradedWearable;
        
        void Awake() {
            EventBroker.Instance().SubscribeMessage<EventAddToUpgradeSlot>(DoesItemQualifyForDonation);
            EventBroker.Instance().SubscribeMessage<EventCoinsToSpend>(UpdateStylePoints);
            EventBroker.Instance().SubscribeMessage<EventTogglePopWindow>(OnClosePopUpWindow);
        }
        void OnClosePopUpWindow(EventTogglePopWindow obj) {
            if(!obj.popWindowIsActive)
                TryRemoveChildren();
        }
        
        void Start() {
            donationPopUpWarnings = GetComponent<DonationPopUpWarnings>();
            canvasGroup = GetComponent<CanvasGroup>();
            coin = FindObjectOfType<Coin>();
            foreach (var button in alternativesButtons) {
                button.interactable = false;
            }
        }

        void UpdateStylePoints(EventCoinsToSpend eventCoinsToSpend) {
            costOfDonation = eventCoinsToSpend.coins;
            upgradedStylePoints = upgradedWearable.stylePoints;
            upgradedWearable.stylePoints = eventCoinsToSpend.stylePoints + upgradedOriginalStylePoints;
            upgradedWearable.GetComponent<IconUpdate>().UpdateInformation();
        }

        public void DoesItemQualifyForDonation(EventAddToUpgradeSlot eventAddToUpgradeSlot) {
            if (!ValidateItem(eventAddToUpgradeSlot.combinedWearable)) {
                donationPopUpWarnings.ShowWarningPopUp(eventAddToUpgradeSlot.combinedWearable);
                return;
            }
            
            TryRemoveChildren();

            originalWearable = Instantiate(eventAddToUpgradeSlot.combinedWearable, itemToDonateSlot.transform, true);
            originalWearable.Amount = eventAddToUpgradeSlot.combinedWearable.Amount;
            originalWearable.stylePoints = eventAddToUpgradeSlot.combinedWearable.stylePoints;
            originalWearable.GetComponent<IconUpdate>().UpdateInformation();
            var scale = itemToDonateSlot.GetComponent<RectTransform>().localScale;
            originalWearable.transform.localPosition = Vector2.zero;
            originalWearable.GetComponent<RectTransform>().localScale = scale;
            Destroy(originalWearable.GetComponent<Button>());
            
            upgradedWearable = Instantiate(eventAddToUpgradeSlot.combinedWearable, upgradedItemSlot.transform, true);
            upgradedWearable.Amount = eventAddToUpgradeSlot.combinedWearable.Amount;
            upgradedWearable.stylePoints = eventAddToUpgradeSlot.combinedWearable.stylePoints;
            upgradedOriginalStylePoints = upgradedWearable.stylePoints;
            upgradedWearable.GetComponent<IconUpdate>().UpdateInformation();
            var scale2 = itemToDonateSlot.GetComponent<RectTransform>().localScale;
            upgradedWearable.transform.localPosition = Vector2.zero;
            upgradedWearable.GetComponent<RectTransform>().localScale = scale2;
            Destroy(upgradedItemSlot.GetComponent<Button>());

            EventBroker.Instance().SendMessage(new EventUpdateAlternativesButtons());
        }
        
        
        void TryRemoveChildren() {
            if (itemToDonateSlot.transform.childCount > 0) {
                Destroy(itemToDonateSlot.transform.GetChild(0).gameObject);
            }
            if (upgradedItemSlot.transform.childCount > 0) {
                Destroy(upgradedItemSlot.transform.GetChild(0).gameObject);
            }
        }


        public bool ValidateItem(CombinedWearables combinedWearables) {
            return combinedWearables.stylePoints < combinedWearables.rarity.MaxValue && 
                   combinedWearables.Amount > 1;
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.C)) {
                foreach (var test in FindObjectsOfType<CombinedWearables>()) {
                    test.Amount++;
                }
                EventBroker.Instance().SendMessage(new EventUpdateWearableHud());
            }

            if (Input.GetKeyDown(KeyCode.G)) {
                coin.Coins += 1000;
            }
        }

        void OnDestroy() {
            EventBroker.Instance().UnsubscribeMessage<EventAddToUpgradeSlot>(DoesItemQualifyForDonation);
            EventBroker.Instance().UnsubscribeMessage<EventCoinsToSpend>(UpdateStylePoints);
        }
        public void OnConfirm() {
            coin.Coins -= costOfDonation;
            GenerateNewItem();
            DeactivateWindow();
        }
        
        void GenerateNewItem() {
            var instance = Instantiate(upgradedWearable);
            instance.isPredefined = false;
            instance.GetComponent<IconUpdate>().UpdateImages();
            instance.GetComponent<IconUpdate>().UpdateInformation();

            instance.rewardMessage = "+" + (upgradedWearable.stylePoints - upgradedOriginalStylePoints);
            instance.showText = true;
            
            
            EventBroker.Instance().SendMessage(new EventUpdatePlayerInventory(originalWearable, -2));
            EventBroker.Instance().SendMessage(new EventUpdatePlayerInventory(upgradedWearable, 1));
            EventBroker.Instance().SendMessage(new EventShowReward(instance));
            EventBroker.Instance().SendMessage(new EventUpdateWearableHud());
            
            Destroy(instance.gameObject);
        }
        
        void DeactivateWindow() {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
            confirmButton.interactable = false;

            EventBroker.Instance().SendMessage(new EventTogglePopWindow(false));
            Debug.Log(gameObject.name);
            if (itemToDonateSlot.transform.childCount > 0) {
                Destroy(itemToDonateSlot.transform.GetChild(0).gameObject);
            }
            if (upgradedItemSlot.transform.childCount > 0) {
                Destroy(upgradedItemSlot.transform.GetChild(0).gameObject);
            }
        }
    }
}