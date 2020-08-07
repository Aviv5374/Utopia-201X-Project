using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utopia201X.BordeGameScence.CategoryManagers;

namespace Utopia201X.BordeGameScence.Buyable.Units
{
    public class RebelSoldiersScript : BuyableObjectScript
    {
        private bool isRebelSoldiersConstruct;

        protected override void Awake()
        {
            base.Awake();
            this.isRebelSoldiersConstruct = false;
        }        

        public override void Costruct(string newOwner)
        {
            newOwner = GameManager.instance.GetOppositPlayerName(newOwner);
            base.Costruct(newOwner);
            if (!this.isRebelSoldiersConstruct)
            {
                this.isRebelSoldiersConstruct = true;
                CategoryManager.instance.LockAnLockCategory(Owner, BuyableObjectId);
            }
        }

        public bool IsTimeToBeDestroy()
        {
            if (RoundOfCreation - GameManager.instance.GetCurrentRound() == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void PrepareToBeDestroy()
        {
            CategoryManager.instance.FreeLockCategory(Owner, BuyableObjectId);
            base.PrepareToBeDestroy();
        }
    }
}
