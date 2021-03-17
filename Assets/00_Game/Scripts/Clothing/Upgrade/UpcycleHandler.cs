using System;
using UnityEngine;
using Utilities;

namespace Clothing.Upgrade {
    public class UpcycleHandler : MonoBehaviour {
        InventoryDataHandler inventoryDataHandler;

        void Start() {
            EventBroker.Instance().SubscribeMessage<MessageUpCycleClothes>(UpCycleCombine);
        }

        void OnDestroy() {
            EventBroker.Instance().UnsubscribeMessage<MessageUpCycleClothes>(UpCycleCombine);
        }

        void UpCycleCombine(MessageUpCycleClothes messageUpCycleClothes) {
            var item1 = messageUpCycleClothes.Wearable1;
            var item2 = messageUpCycleClothes.Wearable2;

            // Destroy(messageUpCycleClothes.Wearable1);
            // Destroy(messageUpCycleClothes.Wearable2);

            inventoryDataHandler.wearableDictionary.Remove(item1.ClothingType);
            inventoryDataHandler.wearableDictionary.Remove(item2.ClothingType);

            print("HasConfirmed " + item1 + item2);
        }

        void OnLoadWearableData() {
            throw new Exception("Not implemented yet!");
        }
    }
}