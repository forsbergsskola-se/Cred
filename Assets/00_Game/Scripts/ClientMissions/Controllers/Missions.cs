using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ClientMissions.Data;
using ClientMissions.Messages;
using Clothing.DressUp;
using Core;
using UnityEngine;
using UnityEngine.Events;
using Utilities;
using Utilities.Time;

namespace ClientMissions.Controllers {
    public class Missions : MonoBehaviour{
        
        const int MaxCurrentMissions = 3;
        [SerializeField] UnityEvent<bool> activateButtons = new UnityEvent<bool>();
        [SerializeField] ClientButton clientUiPrefab;
        [SerializeField] Transform contentParent;
        [SerializeField] int missionTimerInSec = 60;
        public List<SavableMissionData> savableMissionData = new List<SavableMissionData>();
        MissionData currentMission;
        readonly List<ClientButton> clientButtons = new List<ClientButton>();
        ISavedMission savedMission;
        Initializer initializer;
        Generator generator;
        static int firstStartTime = 1;
        TimeManager timeManager;
        
        List<MissionData> activeMissions = new List<MissionData>();

        IEnumerator Start(){
            initializer = GetComponent<Initializer>();
            savedMission = initializer.GetMissionHolder();
            generator = initializer.CreateMissionGenerator();
            EventBroker.Instance().SubscribeMessage<SelectMissionMessage>(SelectMission);
            yield return new WaitForSeconds(firstStartTime);//TODO: Change to wait for time manager to complete...
            timeManager = FindObjectOfType<TimeManager>();
            InstantiateMissionUI();
            CheckMissions();
            activateButtons?.Invoke(true);
            firstStartTime = 0;
        }

        void OnDestroy(){
            EventBroker.Instance().UnsubscribeMessage<SelectMissionMessage>(SelectMission);
        }

        public void RemoveMission(){
            if (currentMission == null){
                Debug.Log("CurrentMission == null");
                return;
            }
            savedMission.RemoveMission(currentMission.SavableMissionData);
            CheckMissions();
        }

        public void OnStartMission(){
            if (currentMission.ClientData == null){
                Debug.LogWarning("CurrentMission is null!");
                return;
            }
            EventBroker.Instance().SendMessage(new RemoveAllClothes());
            EventBroker.Instance().SendMessage(new ActiveMissionMessage(currentMission));
            EventBroker.Instance().SendMessage(new EventSceneLoad("DressupScene"));
        }
        public void CheckMissions(){
            savableMissionData = TimeCheck(savedMission.GetMissions());
            GenerateSavableMissions();
            activeMissions = InitializeMissions();
            SendMissionData(activeMissions);
        }

        List<MissionData> InitializeMissions(){
            return savableMissionData.Select(savedMissionData => initializer.GetSavedMission(savedMissionData)).ToList();
        }

        void GenerateSavableMissions(){
            if (savableMissionData.Count >= savedMission.MaxMissions) return;
            var missingMissions = savedMission.MaxMissions - savableMissionData.Count;
            for (var i = 0; i < missingMissions; i++){
                var newMission = generator.GenerateSavableMissionData();
                savedMission.AddMission(newMission);
                savableMissionData.Add(newMission);
            }
        }

        List<SavableMissionData> TimeCheck(List<SavableMissionData> savableMissionDatas){
            var dateTime = timeManager.timeHandler.GetTime();
            var unixTimestamp = TimeDateConverter.ToUnixTimestamp(dateTime);
            var tempList = savableMissionDatas.ToArray();
            foreach (var savableMission in tempList){
                if (unixTimestamp - savableMission.UnixUtcTimeStamp > missionTimerInSec){
                    savedMission.RemoveMission(savableMission);
                    savableMissionDatas.Remove(savableMission);
                    Debug.Log(unixTimestamp - savableMission.UnixUtcTimeStamp + " < " + missionTimerInSec);
                }
            }
            return savableMissionDatas;
        }

        void SelectMission(SelectMissionMessage selectMissionMessage){
            currentMission = selectMissionMessage.MissionData;
        }

        void InstantiateMissionUI(){
            for (var i = 0; i < MaxCurrentMissions; i++){
                clientButtons.Add(Instantiate(clientUiPrefab, contentParent));
            }
        }
        void SendMissionData(List<MissionData> missionData) {
            var dateTime = timeManager.timeHandler.GetTime();
            var currentUnixTime = TimeDateConverter.ToUnixTimestamp(dateTime);
            for (var i = 0; i < clientButtons.Count; i++){
                clientButtons[i].Setup(missionData[i], missionTimerInSec, currentUnixTime);
                
            }
        }
    }
}