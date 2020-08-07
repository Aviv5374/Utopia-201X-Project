using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Utopia201X.BordeGameScence.FactoryScrips;


namespace Utopia201X.BordeGameScence.UI.Bottom.Buyable
{
    public class BuyableMenuPanelController : MonoBehaviour
    {
        [SerializeField] private ButtonStateController[] buyabelMenuPanelBnts = new ButtonStateController[9];
        [SerializeField] private EventSystem eventSystem;        
        private BaseEventData eventData;
        private int indexCounter;
        private bool activeCoroutine;

        private string panelOwner;
       //private string ownerVertical;???
       //private string ownerHorizontal;???
      
        private string[] hotkeysName = { "FortHotKey", "FactoryHotKey", "AcreOfCropsHotKey", "SchoclHotKey", "HospitalHotKey", "HousingProjectHotKey", "RebelSoldiersHotKey", "PTBoatHotKey", "FishingBoatHotKey" };
        private Dictionary<string, float> hotKeyTimingControllers;

        private bool UC4_Active;
        private BuyableFactory buyableFactory;
      
        public int IndexCounter
        {
            get
            {
                return indexCounter;
            }

            private set
            {
                if (value >= buyabelMenuPanelBnts.Length)
                {
                    indexCounter = 0;
                }
                else if (value <= -1)
                {
                    indexCounter = buyabelMenuPanelBnts.Length - 1;
                }
                else
                {
                    indexCounter = value;
                }

            }
        }

        internal string PanelOwner
        {
            get
            {
                return panelOwner;
            }

            set
            {
                panelOwner = value;
            }
        }

        // Use this for initialization
        void Start()
        {            
            eventData = new BaseEventData(eventSystem);

            //set hotKeyTimingControllers
            hotKeyTimingControllers = new Dictionary<string, float>();
            for (int i = 0; i < hotkeysName.Length; i++)
            {
                hotKeyTimingControllers.Add(hotkeysName[i], -1f);
            }

            UC4_Active = false;
            //set buyableFactory
            buyableFactory = BuyableFactory.instance;
        }

        private void OnEnable()
        {            
            RabelInputCheck(false);
        }

        // Update is called once per frame
        void Update()
        {           
            //ControllerAxisMoveCheck();

            HotkeysClickCheck();
            
            if (UC4_Active && !buyableFactory.IsFactoryActive)
            {
                UC4_Active = false;
                ManageInputIntractability(true);
            }
            
        }

        #region Select & Deselect Management Methods

        private void ControllerAxisMoveCheck()
        {
            if (Input.GetAxisRaw("Vertical") != 0 && activeCoroutine == false)
            {
                StartCoroutine(SelectOnInput(Input.GetAxisRaw("Vertical")));
            }

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                DeselectAllButtons();
            }
        }

        public void DeselectAllButtons()
        {
            for (int i = 0; i < buyabelMenuPanelBnts.Length; i++)
            {
                buyabelMenuPanelBnts[i].OnDeselect(eventData);
            }
        }

        private IEnumerator SelectOnInput(float inputDirection)
        {
            activeCoroutine = true;

            yield return new WaitForSeconds(0.025f);

            DeselectAllButtons();

            yield return new WaitForSeconds(0.095f);           

            eventSystem.SetSelectedGameObject(buyabelMenuPanelBnts[IndexCounter].gameObject);
            yield return null;

            activeCoroutine = false;

        }

        #endregion

        #region HotKey Management Methods

        private void HotkeysClickCheck()
        {
            ManageReleaseHotkeys();
            ManageHotkeyTiming();
            ManagePressHotkeys();
        }

        private void ManageReleaseHotkeys()
        {
            for (int index = 0; index < hotkeysName.Length; index++)
            {
                if (buyabelMenuPanelBnts[index].IsInteractable() && Input.GetButtonUp(hotkeysName[index]))
                {
                    buyabelMenuPanelBnts[index].NormalState();
                }
            }
        }

        private void ManageHotkeyTiming()
        {
            for (int index = 0; index < hotkeysName.Length; index++)
            {
                if (hotKeyTimingControllers[hotkeysName[index]] >= 0f)
                {
                    hotKeyTimingControllers[hotkeysName[index]] += Time.deltaTime;
                }

                if (hotKeyTimingControllers[hotkeysName[index]] >= 0.125f)
                {
                    hotKeyTimingControllers[hotkeysName[index]] = -1f;
                }
            }
        }

        private void ManagePressHotkeys()
        {
            for (int index = 0; index < hotkeysName.Length; index++)
            {
                if (IsLegalHotKeyPress(index) && hotKeyTimingControllers[hotkeysName[index]] <= -1f)
                {
                    if (hotkeysName[index] == "RebelSoldiersHotKey" && !RabelInputCheck(true) || !buyabelMenuPanelBnts[index].IsInteractable())
                    {
                        if (hotkeysName[index] == "PTBoatHotKey" || hotkeysName[index] == "FishingBoatHotKey")
                        {                            
                            GameManager.instance.PlayErrorSound(true);                            
                        }
                        return;
                    }
                    
                    hotKeyTimingControllers[hotkeysName[index]] = 0f;
                    buyabelMenuPanelBnts[index].PressState();
                    buyabelMenuPanelBnts[index].onClick.Invoke();
                }
            }
        }

        //Confirm that only the higthest value hotkey press in a frame will counte as legal
        private bool IsLegalHotKeyPress(int index)
        {
            if (hotkeysName[index] == "FishingBoatHotKey" && Input.GetButtonDown(hotkeysName[index]))
            {
                return true;
            }
            else if (Input.GetButtonDown(hotkeysName[index]) && !Input.GetButton(hotkeysName[index + 1]))
            {
                for (int j = index + 2; j < hotkeysName.Length; j++)
                {
                    if (Input.GetButtonDown(hotkeysName[j]))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region On Click Methods

        public void ManageInputIntractability(bool wantedState)
        {
            for (int index = 0; index < buyabelMenuPanelBnts.Length; index++)
            {
                if (index == 6)
                {
                    if (wantedState)
                    {
                        buyabelMenuPanelBnts[index].interactable = RabelInputCheck(false);
                        continue;
                    }
                    else
                    {
                        buyabelMenuPanelBnts[index].GetComponent<RebelSoldiersBTNScript>().LastInrteracrableState = buyabelMenuPanelBnts[index].IsInteractable();
                    }
                }

                buyabelMenuPanelBnts[index].interactable = wantedState;
            }

            //DELETE when UC8-12 are relevent:
            buyabelMenuPanelBnts[7].interactable = false;
            buyabelMenuPanelBnts[8].interactable = false;
        }

        //UC4
        public void BuyBuyableObject(int buyableHotkeyNumber)
        {
            UC4_Active = true;
            //Check if current time and buyableHotkeyNumber is valid
            if (!GameManager.instance.IsItTheLastSecond() && buyableHotkeyNumber >= 1 && buyableHotkeyNumber <= 9)
            {
                buyableFactory.PrepareBuilder(buyableHotkeyNumber, PanelOwner);
            }            
            else
            {                
                GameManager.instance.PlayErrorSound(true);
            }
        }

        #endregion

        #region RabelInput Management Methods

        private bool RabelInputCheck(bool withErrorSound)
        {
            buyabelMenuPanelBnts[6].GetComponent<RebelSoldiersBTNScript>().CheckInput(withErrorSound);
            return buyabelMenuPanelBnts[6].IsInteractable();
        }

        public void LockRabelInput(int stayUnteracrableFor, bool activeGoldenSine)
        {
            buyabelMenuPanelBnts[6].GetComponent<RebelSoldiersBTNScript>().LockRabelInput(stayUnteracrableFor, activeGoldenSine);
        }

        public void ResetRabelInput(int resetWith, bool deactiveGoldenSine)
        {
            buyabelMenuPanelBnts[6].GetComponent<RebelSoldiersBTNScript>().ResetRabelInput(resetWith, deactiveGoldenSine);
        }

        #endregion
    }
}