using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utopia201X.BordeGameScence.CategoryManagers;

namespace Utopia201X.BordeGameScence.Buyable.Boats
{
    public class FishingBoatScript : BoatScript
    {
        private bool isFishing;
        private bool isFishingBoatConstruct;

        protected override void Awake()
        {
            base.Awake();
            this.isFishing = false;
            this.isFishingBoatConstruct = false;
        }
            
        public override void Costruct(string newOwner)
        {
            base.Costruct(newOwner);
            if (!this.isFishingBoatConstruct)
            {
                this.isFishingBoatConstruct = true;
                CategoryManager.instance.SatisfyCategory(Owner, "food", true, BuyableObjectId);
            }
        }
       
        private void OnTriggerEnter2D(Collider2D collision)//?????????
        {
            //if(collision == fish)
            ///isFishing = true;
            //InvokeRepeating("AddDefaultGoidValue", 0.5f, 1);
        }

        private void OnTriggerExit2D(Collider2D collision)//???????????
        {
            //if(isFishing && IsInvoking("AddDefaultGoidValue"))
            //isFishing = false;
            //CancelInvoke("AddDefaultGoidValue");
        }

        public override void PrepareToBeDestroy()
        {
            CategoryManager.instance.SatisfyerRemover(Owner, "FishingBoat", BuyableObjectId);
            base.PrepareToBeDestroy();
        }
    }
}