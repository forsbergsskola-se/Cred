using System.Collections.Generic;
using Clothing;
using Clothing.Inventory;
using Clothing.Upgrade;
using Clothing.Upgrade.UpCycle;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace HUD.Clothing {
    //TODO: If you press anything outside of the InventoryButtons, could the InvCanvas act like you pressed the InventoryButton?

    public class InventoryCanvas : MonoBehaviour {
        [SerializeField] GameObject scrollView;
        [SerializeField] GameObject buttonHolder;
        [SerializeField] Text closeButtonText;
        public AssignCombinedWearableToUpCycle inventoryContentPrefab;
        [SerializeField] Transform contentParent;

        [SerializeField] PopupWindowUpCycleDonate popupWindonwUpcycleDonate;

        readonly List<AssignCombinedWearableToUpCycle> inventoryContent = new List<AssignCombinedWearableToUpCycle>();
        InventoryDataHandler inventoryDataHandler;

        public int InventoryContentCount => inventoryContent.Count;

        void Start() {
            inventoryDataHandler = GetComponent<InventoryDataHandler>();
            EventBroker.Instance().SubscribeMessage<MessageUpCycleClothes>(UpCycleWearable);
        }

        public void ToggleButton(ClothingType clothingType) {
            if (!inventoryDataHandler.wearableDictionary.ContainsKey(clothingType)) {
                Debug.LogWarning($"No item was found in {clothingType.name}");
                return;
            }

            closeButtonText.text = clothingType.name;
            buttonHolder.SetActive(false);
            scrollView.SetActive(true);
            ContentPooling(clothingType);
            AddToInventory(clothingType);
        }

        public void CloseScrollview() {
            buttonHolder.SetActive(true);
            scrollView.SetActive(false);
        }

        void AddToInventory(ClothingType clothingType) {
            for (int i = 0; i < inventoryDataHandler.wearableDictionary[clothingType].Count; i++) {
                inventoryContent[i].Setup(inventoryDataHandler.wearableDictionary[clothingType][i], popupWindonwUpcycleDonate);
            }
        }

        void ContentPooling(ClothingType clothingType) {
            var clothingTypeCount = inventoryDataHandler.wearableDictionary[clothingType].Count;

            if (inventoryContent.Count < clothingTypeCount) {
                for (int i = inventoryContent.Count; i < clothingTypeCount; i++) {
                    inventoryContent.Add(Instantiate(inventoryContentPrefab, contentParent));
                }
            }
            else {
                for (int i = clothingTypeCount; i < inventoryContent.Count; i++) {
                    inventoryContent[i].gameObject.SetActive(false);
                }
            }
        }

        void UpCycleWearable(MessageUpCycleClothes messageUpCycleClothes) {
            for (int i = 0; i < inventoryContent.Count; i++) {
            }
        }
    }
}