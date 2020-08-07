using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utopia201X.BordeGameScence.UI.Bottom
{
    public class GameMenuPanelManager : MonoBehaviour
    {        
        public static GameMenuPanelManager instance = null;

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
        
        public void Pause()
        {            
            Time.timeScale = 0f;
        }

        public void Resume()
        {            
            Time.timeScale = 1f;           
        }
    }
}