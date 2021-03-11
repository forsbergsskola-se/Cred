﻿namespace Cred.Scripts.SaveSystem {
    public interface ISavable <T> {
        public T ToBeSaved();
        public void OnLoad(T value);
    }
}