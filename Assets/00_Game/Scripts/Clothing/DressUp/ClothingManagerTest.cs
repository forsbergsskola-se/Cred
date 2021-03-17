using System;
using UnityEngine;
using Utilities;

namespace Clothing {
    public class ClothingManagerTest : MonoBehaviour {
        
        [SerializeField] GameObject AlexTorso;
        [SerializeField] GameObject AlexPants;
        void Start() {
            EventBroker.Instance().SubscribeMessage<EventClothesChanged>(UpdateClothes);
            AlexTorso.GetComponent<SkinnedMeshRenderer>().material.mainTexture = FindObjectOfType<LastKnownClothes>().lastKnownShirt.Texture;
            AlexPants.GetComponent<SkinnedMeshRenderer>().material.mainTexture = FindObjectOfType<LastKnownClothes>().lastKnownPants.Texture;
        }

        void OnEnable() {

        }

        void UpdateClothes(EventClothesChanged eventClothesChanged) {
            switch (eventClothesChanged.Wearable.ClothingType.name) {
                case "Shirt":
                    AlexTorso.GetComponent<SkinnedMeshRenderer>().material.mainTexture = eventClothesChanged.Wearable.Texture;
                    FindObjectOfType<LastKnownClothes>().lastKnownShirt = eventClothesChanged.Wearable;
                    break;
                case "Pants":
                    AlexPants.GetComponent<SkinnedMeshRenderer>().material.mainTexture = eventClothesChanged.Wearable.Texture;
                    FindObjectOfType<LastKnownClothes>().lastKnownPants = eventClothesChanged.Wearable;
                    break;
            }
        }

        void OnDestroy() {
            EventBroker.Instance().UnsubscribeMessage<EventClothesChanged>(UpdateClothes);
        }
    }
}