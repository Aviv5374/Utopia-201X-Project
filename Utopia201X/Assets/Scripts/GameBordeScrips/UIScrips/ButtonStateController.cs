using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utopia201X.BordeGameScence.UI
{
    public class ButtonStateController : Button
    {         
        
        public void NormalState()
        {
            this.DoStateTransition(SelectionState.Normal, true);
        }

        public void HighlightedState()
        {
            this.DoStateTransition(SelectionState.Highlighted, true);
        }

        public void PressState()
        {
            this.DoStateTransition(SelectionState.Pressed, true);
        }
            
        //public void DisabledState()
        //{
        //    this.DoStateTransition(SelectionState.Disabled, true);
        //}

    }
}