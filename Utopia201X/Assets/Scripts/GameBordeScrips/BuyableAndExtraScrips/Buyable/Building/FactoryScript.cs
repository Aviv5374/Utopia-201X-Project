using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utopia201X.BordeGameScence.Buyable.Buildings
{
    public class FactoryScript : BuildingScript
    {
        private bool isFactoryConstruct;

        protected override void Awake()
        {
            base.Awake();
            this.isFactoryConstruct = false;
        }       

        public override void Costruct(string newOwner)
        {
            base.Costruct(newOwner);
            if (!this.isFactoryConstruct)
            {
                this.isFactoryConstruct = true;
                GameManager.instance.SortOwnershipOfFactorys(Owner, 1);
            }
        }
                
        public override void PrepareToBeDestroy()
        {
            GameManager.instance.SortOwnershipOfFactorys(Owner);
            base.PrepareToBeDestroy();
        }

    }
}