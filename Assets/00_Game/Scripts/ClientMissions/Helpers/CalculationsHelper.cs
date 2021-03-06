using ClientMissions.Data;
using ClientMissions.Messages;
using UnityEngine;

namespace ClientMissions.Helpers{
    public struct CalculationsHelper{
        public static int NumberGenerator(int requirementAmountLeft){
            return requirementAmountLeft < 3 ? Random.Range(1, requirementAmountLeft + 1) :
                Random.Range(2, requirementAmountLeft + 1);
        }
        public static int GetLowestNumberFromThreeNumbers(int numberOne, int numberTwo, int numberThree){
            return Mathf.Min(Mathf.Min(numberOne,numberTwo), numberThree);
        }
        public static int CalculateReward(StylePointValues stylePointValues, int currentStylePoints, int maxReward, int minReward){
            var percentageValue = Mathf.InverseLerp(stylePointValues.MinStylePoints, stylePointValues.MaxStylePoints, currentStylePoints);
            var reward = Mathf.RoundToInt(Mathf.Lerp(minReward,maxReward, percentageValue));
            return reward;
        }
    }
}