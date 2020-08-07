using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utopia201X.BordeGameScence.FactoryScrips.Handlers
{
    public class UC5_Handler : MonoBehaviour
    {
        public static UC5_Handler instance = null;
        [SerializeField] private GameObject[] buildingDemoPrefabs = new GameObject[12];
        private bool canConfirmerBeActivet = false;
        private bool isManageDemoSideActive = false;

        internal bool CanConfirmerBeActivet
        {
            get
            {
                return canConfirmerBeActivet;
            }
            set
            {
                canConfirmerBeActivet = value;
            }
        }

        public bool IsManageDemoSideActive
        {
            get
            {
                return isManageDemoSideActive;
            }
            private set
            {
                isManageDemoSideActive = value;
            }
        }

        private void Awake()
        {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
        }

        public IEnumerator ManageDemoPrefabsSide(string playerName)
        {            
            IsManageDemoSideActive = true;
            GameObject relevantDemoBuilding = null;
            string relevantPlayerSide = GameManager.instance.GetPlayerSide(playerName);
            int localIndex = BuyableFactory.instance.RelevantIndex;

            if (relevantPlayerSide != "")
            {
                if (relevantPlayerSide == "right")
                {
                    localIndex += 6;
                }
                relevantDemoBuilding = Instantiate(buildingDemoPrefabs[localIndex], transform);
                relevantDemoBuilding.GetComponent<DemoBuilding>().playerName = playerName;
            }
            else
            {
                yield return new WaitForSeconds(0.7f);
            }

            while (relevantDemoBuilding != null) { yield return null; }

            CanConfirmerBeActivet = true;
            IsManageDemoSideActive = false;
        }

        public bool ConstructionAreaConfirmer(string playerName, string buildingName, ref Transform placement)
        {            
            CanConfirmerBeActivet = false;
            return GameManager.instance.AskToFindLegalCoustructionArea(playerName, buildingName, ref placement);
        }


    }
}