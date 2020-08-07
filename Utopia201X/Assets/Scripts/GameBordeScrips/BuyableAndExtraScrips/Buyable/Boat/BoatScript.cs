using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utopia201X.BordeGameScence.Buyable.Boats
{
    public class BoatScript : BuyableObjectScript
    {
        private static Dictionary<string, int> boatIdCounter;
        private int boatId;
        private bool isBoatConstruct;

        public int BoatId
        {
            get
            {
                return boatId;
            }

            private set
            {
                boatId = value;
            }
        }

        protected override void Awake()
        {
            base.Awake();            
            if (boatIdCounter == null)
            {
                boatIdCounter = new Dictionary<string, int>();
                boatIdCounter.Add("Player1", 0);
                boatIdCounter.Add("Player2", 0);                
            }

            this.BoatId = -1;
            this.isBoatConstruct = false;
        }
        
        public override void Costruct(string newOwner)
        {
            base.Costruct(newOwner);
            if (!this.isBoatConstruct)
            {
                this.isBoatConstruct = true;
                this.BoatId = ++boatIdCounter[Owner];
            }
        }

        public void GetControlOnBoat()
        {

        }

        public void MoveBoat()
        {

        }
       
        public override void PrepareToBeDestroy()
        {
            base.PrepareToBeDestroy();
        }
    }
}