using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utopia201X.BordeGameScence.SwitchRoundsExstra
{
    public class ActivitysManager
    {
        private static ActivitysManager instance = null;
        //Dictionary<String= playerName, list<bool>> 
        private Dictionary<string, List<bool>> isActive;

        private ActivitysManager()
        {
            isActive = new Dictionary<string, List<bool>>();
            isActive.Add("Player1", new List<bool>());
            isActive.Add("Player2", new List<bool>());
        }

        public static ActivitysManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActivitysManager();
                }
                return instance;
            }

        }
        
        public void AddActivity(string releventPlayerName)
        {
            isActive[releventPlayerName].Add(true);
        }

        public void RemoveActivity(string releventPlayerName)
        {
            isActive[releventPlayerName].RemoveAt(isActive[releventPlayerName].Count-1);
        }

        public bool IsActiveEmpty()
        {
            if (isActive["Player1"].Count == 0 && isActive["Player2"].Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        

    }
}