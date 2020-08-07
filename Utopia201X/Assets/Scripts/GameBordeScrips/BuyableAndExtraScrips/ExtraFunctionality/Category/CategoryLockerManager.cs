using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utopia201X.BordeGameScence.SwitchRoundsExstra;

namespace Utopia201X.BordeGameScence.CategoryManagers
{
    public class CategoryLockerManager
    {
        private class LockCategory
        {
            private int citizenGroupIndex;
            private string categorynName;            
            private int rabelID;

            public LockCategory(int citizenGroupIndex, string categorynName, int rabelID)
            {
                this.CategorynName = categorynName;
                this.CitizenGroupIndex = citizenGroupIndex;
                this.RabelID = rabelID;
            }
                                   
            public int CitizenGroupIndex
            {
                get
                {
                    return citizenGroupIndex;
                }

                private set
                {
                    citizenGroupIndex = value;
                }
            }

            public string CategorynName
            {
                get
                {
                    return categorynName;
                }

                private set
                {
                    categorynName = value;
                }
            }

            public int RabelID
            {
                get
                {
                    return rabelID;
                }

                private set
                {
                    rabelID = value;
                }
            }
        }

        private static CategoryLockerManager instance = null;
        //Dictionary<srting playerName, list<LockCategory>>
        private Dictionary<string, List<LockCategory>> lockCategorys;

        private CategoryLockerManager()
        {
            lockCategorys = new Dictionary<string, List<LockCategory>>();
            lockCategorys.Add("Player1", new List<LockCategory>());
            lockCategorys.Add("Player2", new List<LockCategory>());
        }

        public static CategoryLockerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CategoryLockerManager();
                }
                return instance;
            }
        }

        public void AnLockCategoryLocker(string releventPlayerName, int rabelID)
        {
            int localCitizensGroupIndex = -1;
            string localCategoryName = "";
            int localCategoryIndex = -1;

            //lock the relevent category
           localCitizensGroupIndex = GameManager.instance.AskPlayerToLockRandomCategory(releventPlayerName, localCitizensGroupIndex, ref localCategoryName, ref localCategoryIndex);

            //add the Lock Category with the parameters from is reference in anLockCategorys list
            lockCategorys[releventPlayerName].Add(new LockCategory(localCitizensGroupIndex, localCategoryName, rabelID));
           
            ActivitysManager.Instance.RemoveActivity(releventPlayerName);
        }

        public void LockCategoryLiberator(string releventPlayerName, int releventRabelID)
        {
            for (int i = 0; i < lockCategorys[releventPlayerName].Count; i++)
            {
                if (lockCategorys[releventPlayerName][i].RabelID == releventRabelID)
                {
                    //an lock the relevent category
                    GameManager.instance.AskPlayerToFreeCategory(releventPlayerName, lockCategorys[releventPlayerName][i].CategorynName, lockCategorys[releventPlayerName][i].CitizenGroupIndex);
                   
                    //Remove At current index is reference in lockCategorys list
                    lockCategorys[releventPlayerName].RemoveAt(i);

                    ActivitysManager.Instance.RemoveActivity(releventPlayerName);

                    return;
                }
            }
            
            ActivitysManager.Instance.RemoveActivity(releventPlayerName);
        }

    }
}