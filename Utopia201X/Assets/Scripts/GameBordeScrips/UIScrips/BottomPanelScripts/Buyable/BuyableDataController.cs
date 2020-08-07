using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Utopia201X.BordeGameScence.UI.Bottom.Buyable
{
    public class BuyableDataController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private BuyableDataPanelController releventBuyableDataPanel;
        [SerializeField] private string[] buyabaleData = new string[5];
       
        public void OnPointerEnter(PointerEventData eventData)
        {                        
            releventBuyableDataPanel.Activate(buyabaleData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {            
            releventBuyableDataPanel.Deactivate();
        }

        public void OnSelect(BaseEventData eventData)
        {
            releventBuyableDataPanel.Activate(buyabaleData);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            releventBuyableDataPanel.Deactivate();
        }
    }
}