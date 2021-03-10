﻿using UnityEngine;

namespace Cred.Scripts.SaveSystem {
    public class SaveHandler {

        string objectID;
        ISaveHandler backEndSaveSystem;

        public SaveHandler(string objectID) {
            this.objectID = objectID;
            backEndSaveSystem = new PlayerPrefsLocalSave(objectID);
            this.backEndSaveSystem.Authenticate();
        }

        public void Save(ISavable savable) {
            backEndSaveSystem.Save(objectID, savable.ToBeSaved());
        }

        public void Load(ISavable savable) {
            var tmp = backEndSaveSystem.Load(objectID);
            savable.OnLoad(tmp);
            Debug.Log("Object loaded from backEndSaveSystem: "+objectID);
        }
    }
}