using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utopia201X.BordeGameScence.SwitchRoundsExstra
{
    public class PlayerReportDocument
    {
        internal string playerName;

        private int goldEarnThisRound;
        private int citizensEarnThisRound;
        private int pointsEarnThisRound;

        internal bool rabelSign;
        internal int totalRebelsToProduce;
        internal int rabelCount;

        internal bool fortSign;

        internal bool goldenGroupSign;
        internal int goldenCount;

        public int GoldEarnThisRound
        {
            get
            {
                return goldEarnThisRound;
            }

            private set
            {
                goldEarnThisRound = value;
            }
        }

        public int CitizensEarnThisRound
        {
            get
            {
                return citizensEarnThisRound;
            }

            private set
            {               
                citizensEarnThisRound = value;
            }
        }

        public int PointsEarnThisRound
        {
            get
            {
                return pointsEarnThisRound;
            }

            private set
            {
                pointsEarnThisRound = value;
            }
        }
            
        public PlayerReportDocument()
        {
            GoldEarnThisRound = 0;
            CitizensEarnThisRound = 0;
            PointsEarnThisRound = 0;
            rabelSign = false;
            fortSign = false;
            goldenGroupSign = false;
        }

        public void AddGold(int newGold)
        {
            GoldEarnThisRound += newGold;
        }

        public void AddCitizens(int newCitizens)
        {
            CitizensEarnThisRound += newCitizens;            
        }

        public void AddPoins(int newPoins)
        {
            PointsEarnThisRound += newPoins;
        }

    }
}