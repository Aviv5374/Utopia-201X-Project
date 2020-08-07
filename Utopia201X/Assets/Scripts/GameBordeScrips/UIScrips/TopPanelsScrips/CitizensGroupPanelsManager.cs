using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Utopia201X.BordeGameScence.UI.Top
{
    public class CitizensGroupPanelsManager : MonoBehaviour
    {
        private static CitizensGroupPanelsManager instance = null;

        [SerializeField] private CitizensGroupPanelController[] citizensGroupsViewLeftPanels = new CitizensGroupPanelController[5];
        [SerializeField] private TextMeshProUGUI[] leftExtraSatisfyerTexts = new TextMeshProUGUI[3];

        [SerializeField] private CitizensGroupPanelController[] citizensGroupsViewRightPanels = new CitizensGroupPanelController[5];
        [SerializeField] private TextMeshProUGUI[] rightExtraSatisfyerTexts = new TextMeshProUGUI[3];

        public static CitizensGroupPanelsManager Instance
        {
            get
            {
                return instance;
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

        // Use this for initialization
        void Start()
        {
            StartCoroutine(SetupPanelsOwners());
        }
        
        #region Citizens Groups Panels Methods

        private IEnumerator SetupPanelsOwners()
        {
            yield return null;

            for (int i = 0; i < citizensGroupsViewLeftPanels.Length; i++)
            {
                if (GameManager.instance.GetPlayerSide("Player1") == "left")
                {
                    citizensGroupsViewLeftPanels[i].PanelOwner = "Player1";
                    citizensGroupsViewRightPanels[i].PanelOwner = "Player2";
                }
                else
                {
                    citizensGroupsViewLeftPanels[i].PanelOwner = "Player2";
                    citizensGroupsViewRightPanels[i].PanelOwner = "Player1";
                }

            }
        }

        public void UpdateWorkStatus(string side, int groupPanelIndex, int workStatusNumber)
        {
            if (side == "left")
            {
                citizensGroupsViewLeftPanels[groupPanelIndex].WorkStatusChanger(workStatusNumber);
            }
            else
            {
                citizensGroupsViewRightPanels[groupPanelIndex].WorkStatusChanger(workStatusNumber);
            }
        }

        public void ActiveCitizensGroupsPanel(string side)
        {
            if (side == "left")
            {
                FindAndActiveGroupPanel(citizensGroupsViewLeftPanels);
            }
            else
            {
                FindAndActiveGroupPanel(citizensGroupsViewRightPanels);
            }
        }

        private void FindAndActiveGroupPanel(CitizensGroupPanelController[] citizensGroupsPanels)
        {
            for (int i = 0; i < citizensGroupsPanels.Length; i++)
            {
                if (!citizensGroupsPanels[i].gameObject.activeSelf)
                {
                    citizensGroupsPanels[i].gameObject.SetActive(true);
                    return;
                }
            }
        }

        #endregion

        #region Extras Panels Methods

        public void UpdateExtraPanel(string side, string categoryName, int updateTo)
        {
            if (side == "left")
            {
                UpdateExtraText(leftExtraSatisfyerTexts, categoryName, updateTo);
            }
            else
            {
                UpdateExtraText(rightExtraSatisfyerTexts, categoryName, updateTo);
            }
        }

        private void UpdateExtraText(TextMeshProUGUI[] extraSatisfyerTexts, string categoryName, int updateTo)
        {
            switch (categoryName)
            {
                case "food":
                    extraSatisfyerTexts[0].text = updateTo.ToString();
                    break;
                case "house":
                    extraSatisfyerTexts[1].text = updateTo.ToString();
                    break;
                case "education":
                    extraSatisfyerTexts[2].text = updateTo.ToString();
                    break;
                default:
                    GameManager.instance.PlayErrorSound(false);
                    break; ;
            }
        }

        #endregion

    }
}