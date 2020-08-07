using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Utopia201X.BordeGameScence.UI.Bottom.Buyable
{
    public class BuyableDataPanelController : MonoBehaviour
    {               
        [SerializeField] private TextMeshProUGUI[] buyableDataTexts = new TextMeshProUGUI[5];
        private string[] defaultDataStrings = { "Name : ", "Cost : ", "Info : ", "HotKey : ", "How To Build : " };
       
        // Use this for initialization
        void Start()
        {
            Deactivate();
        }
        
        public void Activate(string[] stingToAdd)
        {
            gameObject.SetActive(true);
            SetupPanel(stingToAdd);
        }

        private void SetupPanel(string[] stingToAdd)
        {
            for (int i = 0; i < buyableDataTexts.Length; i++)
            {
                buyableDataTexts[i].text += stingToAdd[i];
            }
        }

        private void RestartPanel()
        {
            for (int i = 0; i < buyableDataTexts.Length; i++)
            {
                buyableDataTexts[i].text = defaultDataStrings[i];
            }
        }

        public void Deactivate()
        {
            RestartPanel();
            gameObject.SetActive(false);
        }
    }
}
