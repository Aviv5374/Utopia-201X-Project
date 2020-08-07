using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utopia201X.BordeGameScence.FactoryScrips
{
    public class DemoBuilding : MonoBehaviour
    {              
        [SerializeField] private float speed = 1280f;
        internal string playerName;

        // Update is called once per frame
        void Update()
        {
            MousePositionTracker();

            if (Input.GetMouseButtonUp(0) || !GameManager.instance.IsUC4PreConditionRelevent(playerName))
            {
                Destroy(gameObject);                
            }
           
        }

        private void MousePositionTracker()
        {
            //Store start position to move from, based on objects current transform position.
            Vector2 start = transform.position;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;            
            transform.position = Vector2.MoveTowards(start, mousePosition, speed * Time.deltaTime);                      
        }
        
    }
}