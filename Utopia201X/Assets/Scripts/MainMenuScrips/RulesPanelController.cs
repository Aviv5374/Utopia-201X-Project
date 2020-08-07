using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utopia201X.MainMenusScence
{
    public class RulesPanelController : MonoBehaviour
    {
        public void ToggleRulesPanel(GameObject panel)
        {
            panel.SetActive(!panel.activeSelf);
        }

        public void ToggleRulesPanelBTN(GameObject button)
        {
            button.SetActive(!button.activeSelf);
        }
    }
}