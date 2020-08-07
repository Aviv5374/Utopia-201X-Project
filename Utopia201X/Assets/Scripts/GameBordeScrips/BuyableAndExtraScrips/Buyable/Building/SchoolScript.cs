using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utopia201X.BordeGameScence.CategoryManagers;

namespace Utopia201X.BordeGameScence.Buyable.Buildings
{
    public class SchoolScript : BuildingScript
    {
        private bool isSchoolConstruct;

        protected override void Awake()
        {
            base.Awake();
            this.isSchoolConstruct = false;
        }
        
        public override void Costruct(string newOwner)
        {
            base.Costruct(newOwner);
            if (!this.isSchoolConstruct)
            {
                this.isSchoolConstruct = true;
                CategoryManager.instance.SatisfyCategory(Owner, "education", false, BuyableObjectId);
            }
        }

        public override void PrepareToBeDestroy()
        {
            CategoryManager.instance.SatisfyerRemover(Owner, "School", BuyableObjectId);
            base.PrepareToBeDestroy();
        }
    }
}
