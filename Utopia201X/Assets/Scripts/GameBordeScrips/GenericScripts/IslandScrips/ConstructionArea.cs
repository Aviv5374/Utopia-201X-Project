using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utopia201X.BordeGameScence.FactoryScrips.Handlers;

namespace Utopia201X.BordeGameScence.IslandScrips.ConstructionAreaScripts
{
    public class ConstructionArea : MonoBehaviour
    {
        private int id;
        private bool isSelected;
        private string releventBuyableName;
        private int releventBuyable_ID;
        internal string myIslandSide;
        private string myIslandOwner;        
        private Color spriteRendererDefulteColor;
        private Color spriteRendererHighlightedColor;
        private bool isSetupFinish;

        public int Id
        {
            get
            {
                return id;
            }

            private set
            {
                id = value;
            }
        }
        
        internal bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;
            }
        }
        
        internal string ReleventBuyableName
        {
            get
            {
                return releventBuyableName;
            }

            set
            {
                releventBuyableName = value;
            }
        }
        
        internal int ReleventBuyable_ID
        {
            get
            {
                return releventBuyable_ID;
            }

            set 
            {
                releventBuyable_ID = value;
            }
        }
                    
        // Use this for initialization
        void Start()
        {
            isSetupFinish = false;
            SetupId();
            IsSelected = false;
            ReleventBuyableName = "";
            ReleventBuyable_ID = -1;
            spriteRendererDefulteColor = gameObject.GetComponent<SpriteRenderer>().color;
            spriteRendererHighlightedColor = new Color(0.89f, 0.78f, 0.55f, 1f); // OR Color.yellow;
            StartCoroutine(SetupMyIslandOwner());
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0) && gameObject.GetComponent<SpriteRenderer>().color == spriteRendererHighlightedColor)
            {
                gameObject.GetComponent<SpriteRenderer>().color = spriteRendererDefulteColor;
            }
        }

        private void SetupId()
        {
            string constructionAreaName = gameObject.name;
            char[] nameCharArray = constructionAreaName.ToCharArray();

            if (char.GetNumericValue(nameCharArray[nameCharArray.Length - 1]) >= char.GetNumericValue('1') && char.GetNumericValue(nameCharArray[nameCharArray.Length - 1]) <= char.GetNumericValue('9'))
            {
                Id = (int)char.GetNumericValue(nameCharArray[nameCharArray.Length - 1]);
            }
            else
            {
                Id = ((int)char.GetNumericValue(nameCharArray[nameCharArray.Length - 3]) * 10) + (int)char.GetNumericValue(nameCharArray[nameCharArray.Length - 2]);
            }
        }

        private IEnumerator SetupMyIslandOwner()
        {
            yield return null;
            
            if (GameManager.instance.GetPlayerSide("Player1") == myIslandSide)
            {
                myIslandOwner = "Player1";                
            }
            else
            {
                myIslandOwner = "Player2";                
            }

            isSetupFinish = true;
        }

        private bool IsABuildingOnMe()
        {
            if (!IsSelected && ReleventBuyableName == "" && ReleventBuyable_ID <= -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void OnMouseEnter()
        {
            if (isSetupFinish && GameManager.instance.IsUC4PreConditionRelevent(myIslandOwner) && UC5_Handler.instance.IsManageDemoSideActive && !IsABuildingOnMe())
            {
                gameObject.GetComponent<SpriteRenderer>().color = spriteRendererHighlightedColor;
            }
        }

        private void OnMouseDown()
        {            
            if (isSetupFinish && GameManager.instance.IsUC4PreConditionRelevent(myIslandOwner) && UC5_Handler.instance.IsManageDemoSideActive && !IsABuildingOnMe())
            {
                gameObject.GetComponent<SpriteRenderer>().color = spriteRendererDefulteColor;                
                IsSelected = true;                
            }
        }    

        private void OnMouseExit()
        {
            if (isSetupFinish && GameManager.instance.IsUC4PreConditionRelevent(myIslandOwner) && UC5_Handler.instance.IsManageDemoSideActive && !IsABuildingOnMe())
            {
                gameObject.GetComponent<SpriteRenderer>().color = spriteRendererDefulteColor;
            }
        }
    
    }
}