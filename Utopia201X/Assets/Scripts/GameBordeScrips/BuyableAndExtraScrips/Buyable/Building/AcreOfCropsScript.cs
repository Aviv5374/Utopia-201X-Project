using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utopia201X.BordeGameScence.CategoryManagers;

namespace Utopia201X.BordeGameScence.Buyable.Buildings
{
    public class AcreOfCropsScript : BuildingScript
    {
        private int numOfRoundsThisExist;
        private bool isAcreOfCropsConstruct;
        
        protected override void Awake()
        {
            base.Awake();
            this.isAcreOfCropsConstruct = false;
            this.numOfRoundsThisExist = Random.Range(0, 5);            
        }
        
        public override void Costruct(string newOwner)
        {
            base.Costruct(newOwner);
            if (!this.isAcreOfCropsConstruct)
            {
                this.isAcreOfCropsConstruct = true;
                CategoryManager.instance.SatisfyCategory(Owner, "food", false, BuyableObjectId);
            }
        }

        public bool IsTimeToBeDestroy()
        {
            if (RoundOfCreation - GameManager.instance.GetCurrentRound() == numOfRoundsThisExist)
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
            CategoryManager.instance.SatisfyerRemover(Owner, "AcreOfCrops", BuyableObjectId);
            base.PrepareToBeDestroy();
        }
        
    }
}