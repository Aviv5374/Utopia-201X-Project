using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utopia201X.BordeGameScence.UI
{
    public class ArrowBehaviourScript : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(gameObject, 4.5f);
        }
                        
    }
}