using System.Collections;
using AddressableLoadSystem;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Editor.UnitTests.PlayModeTests{
    public class MonoBehaviorAddressableTests
    {
       [Test]
        public void MonoBehaviorAddressableSimpleTest()
        {
            var gameObject = new GameObject("GameObject");
            var addressableHandler = gameObject.AddComponent<AddressableManager>();
            Assert.AreEqual(typeof(AddressableManager), addressableHandler.GetType());
        }
        [Test]
        public void MonoBehaviorAddressableTestWithResourceLoadPrefabInstantiate(){
            var gameObject = GameObject.Instantiate(Resources.Load("AddressableManager")as GameObject);
            var addressableHandler = gameObject.GetComponent<AddressableManager>();
            Assert.AreEqual(typeof(AddressableManager),addressableHandler.GetType());
        }
        [UnityTest]
        public IEnumerator MonoBehaviorLoadAddressableAssetsWithMultipleLabelReference(){
            var gameObject = GameObject.Instantiate(Resources.Load("AddressableManager") as GameObject);
            var addressableManager = gameObject.GetComponent<AddressableManager>();
            yield return new WaitForSeconds(0.5f);
            Debug.Log(addressableManager.ListCount);
            Assert.Less(0, addressableManager.ListCount);
            yield return null;
        }
    }
}
