using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utopia201X.BordeGameScence.Managers
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager instance = null;
        private int numOfRounds;
        private int defaultTimePerRound;
        private int currentTimeOfRounnd;
        private bool isTheLastSecond;
        private bool isTimerSet;

        public int NumOfRounds
        {
            get
            {
                return numOfRounds;
            }            
        }
      
        public int CurrentTimeOfRounnd
        {
            get
            {
                return currentTimeOfRounnd;
            }
            
        }

        public bool IsTheLastSecond
        {
            get
            {
                return isTheLastSecond;
            }

            private set
            {
                isTheLastSecond = value;
            }
        }

        private void Awake()
        {
            //Check if there is already an instance of SoundManager
            if (instance == null)
                //if not, set it to this.
                instance = this;
            //If instance already exists:
            else if (instance != this)
                //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
                Destroy(gameObject);

            //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
            DontDestroyOnLoad(gameObject);
            
            IsTheLastSecond = false;
            isTimerSet = false;
        }

        internal void SetTimer(int chosenNumOfRounds, int chosenTimePerRound)
        {
            if (!isTimerSet)
            {
                isTimerSet = true;
                numOfRounds = chosenNumOfRounds;
                defaultTimePerRound = chosenTimePerRound;
                currentTimeOfRounnd = chosenTimePerRound;
            }
        }

        private void CheckIsTheLastSecond()
        {
            if (currentTimeOfRounnd <= 1)
            {
                IsTheLastSecond = true;
            }
            else
            {
                IsTheLastSecond = false;
            }
        }

        public void StartTimer()
        {
            CheckIsTheLastSecond();
            InvokeRepeating("TimerLifeCycle", 0.5f, 1);
        }

        private void TimerLifeCycle()
        {
            currentTimeOfRounnd--;
            GameManager.instance.UpdateUITimer(currentTimeOfRounnd, numOfRounds);
            CheckIsTheLastSecond();
            CheckTimer();
        }

        private void CheckTimer()
        {
            if (currentTimeOfRounnd <= 0)
            {
                StopTimer();
                StartCoroutine(GameManager.instance.SwitchBetweenRoundsSetUp());                
            }
        }

        public void StopTimer()
        {
            CancelInvoke("TimerLifeCycle");
        }

        public void ResetTimer()
        {            
            currentTimeOfRounnd = defaultTimePerRound;
            numOfRounds--;
            GameManager.instance.UpdateUITimer(currentTimeOfRounnd, numOfRounds);
        }

    }
}