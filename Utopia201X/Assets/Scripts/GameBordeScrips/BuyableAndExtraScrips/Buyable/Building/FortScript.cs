using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utopia201X.BordeGameScence.Buyable.Buildings
{
    public class FortScript : BuildingScript
    {
        private bool isFortConstruct;

        protected override void Awake()
        {
            base.Awake();
            this.isFortConstruct = false;            
        }
        
        public override void Costruct(string newOwner)
        {
            base.Costruct(newOwner);
            if (!this.isFortConstruct)
            {
                this.isFortConstruct = true;                
                GameManager.instance.AskToLockRabelInput(GameManager.instance.GetOppositPlayerName(Owner), 4, false);                
            }
        }

        public bool IsActive()
        {
            if (RoundOfCreation - GameManager.instance.GetCurrentRound() < 4)
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
            base.PrepareToBeDestroy();
        }
             
    }
}