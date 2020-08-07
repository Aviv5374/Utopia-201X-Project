using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Utopia201X.BordeGameScence.UI.Top
{
    public class CitizensGroupPanelController : MonoBehaviour
    {
        [SerializeField] private int panelNumber;
        private string panelOwner;
        private bool isOwnerSet;

        [SerializeField] private Image[] lightsImages = new Image[3];
        [SerializeField] private Sprite greenSprite;
        [SerializeField] private Sprite orangeSprite;
        [SerializeField] private Sprite redSprite;
        [SerializeField] private Color goldenColor;
        private Color defaultColor;
        private Image panelImage;

        [SerializeField] private TextMeshProUGUI workStatus;
    
        internal string PanelOwner
        {
            get
            {
                return panelOwner;
            }

            set
            {
                if (!isOwnerSet)
                {
                    panelOwner = value;
                    isOwnerSet = true;
                }

            }
        }

        // Use this for initialization
        void Start()
        {
            panelImage = GetComponent<Image>();
            defaultColor = panelImage.color;
            InvokeRepeating("CheckMyGroupStatus", 0.4f, 0.25f);
        }
        
        #region lights Images Management Methods

        private void CheckMyGroupStatus()
        {
            if (GameManager.instance.IsPlayerHasAnActiveGoldenCitizenGroup(PanelOwner, panelNumber))
            {
                panelImage.color = goldenColor;

                for (int i = 0; i < lightsImages.Length; i++)
                {
                    lightsImages[i].overrideSprite = greenSprite;
                }
            }
            else
            {
                panelImage.color = defaultColor;
                
                int[] CatgorysStatus = GameManager.instance.AskForCitizensGroupCatgorysStatus(PanelOwner, panelNumber);
                for (int i = 0; i < lightsImages.Length; i++)
                {
                    ChangeLightColor(i, CatgorysStatus[i]);
                }
            }
        }

        private void ChangeLightColor(int index, int spriteNumber)
        {
            switch (spriteNumber)
            {
                case 1:
                    lightsImages[index].overrideSprite = greenSprite;
                    break;
                case 2:
                    lightsImages[index].overrideSprite = redSprite;
                    break;
                case 3:
                    lightsImages[index].overrideSprite = null;
                    break;
                default:
                    GameManager.instance.PlayErrorSound(false);
                    break;
            }
        }

        #endregion

        public void WorkStatusChanger(int workStatusNumber)
        {
            switch (workStatusNumber)
            {
                case 1:                    
                    workStatus.text = "Factory Owner";
                    break;
                case 2:
                    workStatus.text = "Worker";
                    break;
                case 3:
                    workStatus.text = "Unemployed";
                    break;
                default:
                    GameManager.instance.PlayErrorSound(false);
                    break;
            }
        }

    }
}