using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Utopia201X.MainMenusScence
{
    public class ButtonExtraBehaviourScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Button releventButton;        
        
        public void OnPointerEnter(PointerEventData eventData)
        {            
            StartCoroutine(LongPressManaging());
        }

        public void OnPointerExit(PointerEventData eventData)
        {            
            StopAllCoroutines();
        }

        private IEnumerator LongPressManaging()
        {            
            yield return new WaitForSeconds(1.378f);

            while (!Input.GetMouseButtonDown(0) && Input.GetMouseButton(0))
            {                
                releventButton.onClick.Invoke();
                yield return new WaitForSeconds(0.25f);
            }

            StartCoroutine(LongPressManaging());
        }

       
    }
}
