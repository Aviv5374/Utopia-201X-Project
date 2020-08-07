using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utopia201X.BordeGameScence.Buyable
{
    public class BuyableObjectScript : MonoBehaviour
    {
        private static Dictionary<string, int> buyableObjectIdCounter;
        private int buyableObjectId;
        private string owner;
        private int roundOfCreation;
        private bool needToBeDestroy;
        [SerializeField]private int cost;
        internal int damageFormRebelSoldier;       
        private bool isBuyableObjectConstruct;
        private Color originalColor;

        public int BuyableObjectId
        {
            get
            {
                return buyableObjectId;
            }

            private set
            {
                buyableObjectId = value;
            }
        }

        public string Owner
        {
            get
            {
                return owner;
            }

            protected set
            {
                owner = value;
            }
        }

        public int RoundOfCreation
        {
            get
            {
                return roundOfCreation;
            }

            protected set
            {
                roundOfCreation = value;
            }
        }

        internal bool NeedToBeDestroy 
        {
            get
            {
                return needToBeDestroy;
            }

            set
            {
                needToBeDestroy = value;
            }
        }

        public int Cost
        {
            get
            {
                return cost;
            }

            protected set
            {
                cost = value;
            }
        }

        public Color OriginalColor
        {
            get
            {
                return originalColor;
            }

            protected set
            {
                originalColor = value;
            }
        }

        protected virtual void Awake()
        {            
            if(buyableObjectIdCounter == null)
            {                
                buyableObjectIdCounter = new Dictionary<string, int>();
                buyableObjectIdCounter.Add("Player1", 0);
                buyableObjectIdCounter.Add("Player2", 0);                
            }

            this.BuyableObjectId = -1;
            this.Owner = "";
            this.RoundOfCreation = -1;
            this.NeedToBeDestroy = false;
            this.damageFormRebelSoldier = 0;
            this.isBuyableObjectConstruct = false;
        }

        protected void Start()
        {
            OriginalColor = gameObject.GetComponent<SpriteRenderer>().color;            
        }

        public virtual void Costruct(string newOwner)
        {            
            if (!this.isBuyableObjectConstruct)
            {
                this.isBuyableObjectConstruct = true;                
                this.BuyableObjectId = ++buyableObjectIdCounter[newOwner];
                this.Owner = newOwner;                
                this.RoundOfCreation = GameManager.instance.GetCurrentRound();
            }
        }
       
        public virtual void PrepareToBeDestroy()
        {            
            Destroy(gameObject); 
        }
        
    }
}