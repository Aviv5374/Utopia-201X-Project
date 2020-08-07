using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utopia201X.BordeGameScence.SwitchRoundsExstra;

namespace Utopia201X.BordeGameScence.CategoryManagers
{
    public class CategoryManager : MonoBehaviour
    {
        public static CategoryManager instance = null;
        private CategorySatisfyerManager categorySatisfyerManager;
        private CategoryLockerManager categoryLockerManager;

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

            categorySatisfyerManager = CategorySatisfyerManager.Instance;

            categoryLockerManager = CategoryLockerManager.Instance;
        }

        #region categorySatisfyerManager Methods

        public void SatisfyCategory(string releventPlayerName, string categoryName, bool hasHarbor, int satisfyerID)
        {
            categorySatisfyerManager.FinedAndSatisfyCategory(releventPlayerName, categoryName, hasHarbor, satisfyerID);
        }

        public void SatisfyerRemover(string releventPlayerName, string satisfyerName, int satisfyerID)
        {
            ActivitysManager.Instance.AddActivity(releventPlayerName);

            string typeOfCategorySatisfyer = "";

            if (!categorySatisfyerManager.ExtraSatisfyerFinderAndRemover(releventPlayerName, satisfyerName, satisfyerID, out typeOfCategorySatisfyer))
            {
                GameManager.instance.AskPlayerToSortSatisfaction(releventPlayerName, typeOfCategorySatisfyer, categorySatisfyerManager.ExtraSatisfyerGiver(releventPlayerName, typeOfCategorySatisfyer));
            }

            ActivitysManager.Instance.RemoveActivity(releventPlayerName);
        }

        public bool GetExtraSatisfyer(string releventPlayerName, string categoryName)
        {
            return categorySatisfyerManager.ExtraSatisfyerGiver(releventPlayerName, categoryName);
        }

        #endregion

        #region categoryLockerManager Methods

        public void LockAnLockCategory(string releventPlayerName, int rabelID)
        {
            ActivitysManager.Instance.AddActivity(releventPlayerName);

            categoryLockerManager.AnLockCategoryLocker(releventPlayerName, rabelID);
        }

        public void FreeLockCategory(string releventPlayerName, int releventRabelID)
        {
            ActivitysManager.Instance.AddActivity(releventPlayerName);

            categoryLockerManager.LockCategoryLiberator(releventPlayerName, releventRabelID);
        }

        #endregion

    }
}
