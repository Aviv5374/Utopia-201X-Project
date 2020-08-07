using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utopia201X.BordeGameScence.UI.Bottom.Buyable
{
    public class RebelSoldiersBTNScript : MonoBehaviour
    {                
        private int roundOfUnteractable;      
        private int stayUnteracrableFor;        
        private bool activeGoldenSine;
        private bool lastInrteracrableState;

        private bool Inrteracrable
        {
            get
            {
                return gameObject.GetComponent<Button>().interactable;
            }

            set
            {
                gameObject.GetComponent<Button>().interactable = value;
            }
        }
        
        internal bool LastInrteracrableState
        {
            get
            {
                return lastInrteracrableState;
            }

            set
            {
                lastInrteracrableState = value;
            }
        }

        private void Awake()
        {
            roundOfUnteractable = -1;
            stayUnteracrableFor = -1;
            activeGoldenSine = false;
            LastInrteracrableState = gameObject.GetComponent<Button>().IsInteractable();
        }       

        public void CheckInput(bool withErrorSound)
        {            
            if (!IsRabelInputLock() && withErrorSound)
            {
                GameManager.instance.PlayErrorSound(true);                
            }
        }
                           
        public void LockRabelInput(int stayUnrteracrableFor, bool activeGoldenSine)
        {            
            //the path for a new Active Golden Citizen Group to update the activeGoldenSine
            if (stayUnrteracrableFor == 1 && activeGoldenSine)
            {                
                this.activeGoldenSine = activeGoldenSine;

                //return when a Fort alraedy Lock Rabel Input
                if (this.stayUnteracrableFor == 4)
                {                                      
                    return;
                }
            }                                    
            //The usual path. Forts and Active Golden Citizen Group pass from here
            this.roundOfUnteractable = GameManager.instance.GetCurrentRound();
            this.stayUnteracrableFor = stayUnrteracrableFor;
            LastInrteracrableState = false;
            Inrteracrable = false;
        }

        public void ResetRabelInput(int resetWith, bool deactiveGoldenSine)
        {
            //if enter the Rabel Input is Lock probably by a Fort
            if (!IsRabelInputLock())
            {
                //To update the activeGoldenSine when a Fort is LockRabelInput
                if (this.stayUnteracrableFor == 4 && deactiveGoldenSine)
                {
                    this.activeGoldenSine = false;
                }
                //if there aren't any Active Fort to replace with, replace current Fort with an Active Golden Citizen Group
                else if (resetWith >= 4 && this.activeGoldenSine)
                {
                    this.roundOfUnteractable = GameManager.instance.GetCurrentRound();
                    this.stayUnteracrableFor = 1;
                }
                //Update roundOfUnteractable and check if Rabel Input still lock
                else
                {
                    roundOfUnteractable += resetWith;
                    IsRabelInputLock();
                }
            }
        }
        
        private bool IsRabelInputLock()
        {                        
            if (roundOfUnteractable > -1 && roundOfUnteractable - GameManager.instance.GetCurrentRound() >= stayUnteracrableFor || LastInrteracrableState)
            {
                roundOfUnteractable = -1;
                stayUnteracrableFor = -1;
                activeGoldenSine = false;
                LastInrteracrableState = true;
                Inrteracrable = true;
            }
            return Inrteracrable;                        
        }

    }
}