using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utopia201X.BordeGameScence.CategoryManagers;

namespace Utopia201X.BordeGameScence.Buyable.Buildings
{
    public class HousingProjectScript : BuildingScript
    {
        private bool isHousingProjectConstruct;

        protected override void Awake()
        {
            base.Awake();
            this.isHousingProjectConstruct = false;
        }        

        public override void Costruct(string newOwner)
        {
            base.Costruct(newOwner);
            if (!this.isHousingProjectConstruct)
            {
                this.isHousingProjectConstruct = true;
                CategoryManager.instance.SatisfyCategory(Owner, "house", false, BuyableObjectId);
            }
        }

        public override void PrepareToBeDestroy()
        {
            CategoryManager.instance.SatisfyerRemover(Owner, "HousingProject", BuyableObjectId);
            base.PrepareToBeDestroy();
        }
    }
}