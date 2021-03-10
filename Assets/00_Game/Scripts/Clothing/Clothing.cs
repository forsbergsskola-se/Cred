using Cred._00_Game.Scripts.Clothing;
using UnityEngine;
using UnityEngine.UI;

namespace Cred
{
    [CreateAssetMenu]
    public class Clothing : ScriptableObject {
        public int cost;
        public TempCoin temp;
        public bool affordable;
        /*Type of Club. Name added by designer in the Inspector*/
        public string nameOfClub;

        /*Categories of Clothing*/
        public bool categoryJeans => name.Contains("Jeans");
        public bool categoryHat => name.Contains("Hat");
        public bool categoryShirt => name.Contains("Shirt");

        /*TODO: More Categories*/

        /*Clothing styles*/
        public bool raverStyle => name.Contains("Raver");
        public bool hipsterStyle => name.Contains("Hipster");
        public bool influencerStyle => name.Contains("Influencer");

        public bool raverClub => nameOfClub.Contains("Hole");
        public bool hipsterClub => nameOfClub.Contains("Niche");
        public bool yardClub => nameOfClub.Contains("Yard");

        /*Deluxe Clothing*/
        public bool deluxeClothing => name.Contains("Deluxe");

        /*Standard Clothing*/
        public bool standardClothing => name.Contains("Standard");

        //public bool afford => ? temp.coin > cost;
     
    }
}