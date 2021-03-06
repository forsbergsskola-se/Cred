 using UnityEngine;

namespace Core {
    public class PersistantObject : MonoBehaviour {
        public GameObject persistantObject;
        static bool hasBeenInstantiate;

        void Start() {
            if (hasBeenInstantiate) return;

            InstantiateObject();
            hasBeenInstantiate = true;
        }

        void InstantiateObject() {
            var instance = Instantiate(persistantObject);

            DontDestroyOnLoad(instance);
        }
    }
}