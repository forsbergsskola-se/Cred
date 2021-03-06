using System.Collections.Generic;
using ClientMissions.Controllers;
using ClientMissions.Data;
using ClientMissions.Messages;
using TMPro;
using UnityEngine;
using Utilities;

namespace ClientMissions.Hud {
    public class RequirementsUI : MonoBehaviour {
        [SerializeField] TMP_Text requirementHeader;
        [SerializeField] TMP_Text stylePoints;
        [SerializeField] TMP_Text requirementTMPTextPrefab;
        [SerializeField] Transform requirementTextParent;
        Dictionary<string, TMP_Text> requirements = new Dictionary<string, TMP_Text>();
        [SerializeField] List<GameObject> clientGameObjects = new List<GameObject>();
        [SerializeField] MeshRenderer clientSkin;
        string requiredStylepoints;


        void Awake() {
            EventBroker.Instance().SubscribeMessage<SendActiveMissionMessage>(OnGetMissionData);
        }

        void OnDestroy() {
            EventBroker.Instance().UnsubscribeMessage<RequirementUIMessage>(UpdateRequirementUI);
            EventBroker.Instance().UnsubscribeMessage<CurrentStylePointsMessage>(UpdateStylePointsUI);
            EventBroker.Instance().UnsubscribeMessage<SendActiveMissionMessage>(OnGetMissionData);
        }

        void OnGetMissionData(SendActiveMissionMessage sendActiveMissionMessage) {
            var missionData = sendActiveMissionMessage.MissionData;
            if (missionData.ClientData == null) {
                Debug.LogWarning("RequirementUI missionData is null");
                return;
            }

            requiredStylepoints = $"/{missionData.StylePointValues.MinStylePoints.ToString()}";
            requirementHeader.text = $"{missionData.ClientData.name}s requirements:";
            stylePoints.text = $"0{requiredStylepoints}";
            foreach (var client in clientGameObjects) {
                client.SetActive(client.name == missionData.ClientData.name);
            }

            clientSkin.material = missionData.ClientData.Skin;
            foreach (var requirement in missionData.Requirements) {
                var temp = Instantiate(requirementTMPTextPrefab, requirementTextParent);
                temp.text = requirement.ToString();
                requirements.Add(requirement.ToString(), temp);
            }

            EventBroker.Instance().SubscribeMessage<RequirementUIMessage>(UpdateRequirementUI);
            EventBroker.Instance().SubscribeMessage<CurrentStylePointsMessage>(UpdateStylePointsUI);
            EventBroker.Instance().UnsubscribeMessage<SendActiveMissionMessage>(OnGetMissionData);
        }

        void UpdateStylePointsUI(CurrentStylePointsMessage currentStylePoints) {
            stylePoints.text = $"{currentStylePoints.CurrentStylePoints}{requiredStylepoints}";
        }

        void UpdateRequirementUI(RequirementUIMessage requirementUIMessage) {
            if (requirements.ContainsKey(requirementUIMessage.RequirementName)) {
                requirements[requirementUIMessage.RequirementName].fontStyle = requirementUIMessage.IsCompleted
                    ? FontStyles.Strikethrough
                    : FontStyles.Normal;
            }
        }
    }
}