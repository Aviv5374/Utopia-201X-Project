using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utopia201X.BordeGameScence.Managers
{
    public class EnvironmentObjectManager : MonoBehaviour
    {
        public static EnvironmentObjectManager instance = null;
        //Dictionary<string = name,List<EnvironmentObject>>
        private Dictionary<string, List<GameObject>> currentEnvironmentObject;

        /*
        private EnvironmentObjectManager()
        {
            currentEnvironmentObject = new Dictionary<string, List<EnvironmentObject>>();
            currentEnvironmentObject.Add("SchoolsOfFish", new List<SchoolsOfFish>());....
        }
        */

        /*
   public static EnvironmentObjectManager Instance
   {
       get
       {
           if(instance == null){
               instance = new EnvironmentObjectManager();
           }
           return instance;
       }

   }
   */

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

        private void IntoEnvironmentObject() { }

        private bool EnvironmentChecker() { return true; }

        
    }
}
