using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utopia201X.BordeGameScence.UI.Top
{
    public class TopPanelManager
    {
        private static TopPanelManager instance = null;
        //Dictionry<string = section, Text>
        private Dictionary<string, Text> leftPlayerPanelTextDictionary;
        //Dictionry<string = section, Text>
        private Dictionary<string, Text> timerPanelTextDictionary;
        //Dictionry<string = section, Text>
        private Dictionary<string, Text> rightPlayerPanelTextDictionary;

        private TopPanelManager()
        {
            //set timer text 
            timerPanelTextDictionary = new Dictionary<string, Text>();
            timerPanelTextDictionary.Add("currentTimeOfRoundText", GameObject.Find("CurrentTimeOfRoundText").GetComponent<Text>());
            timerPanelTextDictionary.Add("currentRoundText", GameObject.Find("CurrentRoundText").GetComponent<Text>());

            //set left player text
            leftPlayerPanelTextDictionary = new Dictionary<string, Text>();
            leftPlayerPanelTextDictionary.Add("currentPointsLeftText", GameObject.Find("CurrentTotalPointsLeftText").GetComponent<Text>());
            leftPlayerPanelTextDictionary.Add("currentCitizensNumLeftText", GameObject.Find("CurrentCitizensNumLeftText").GetComponent<Text>());
            leftPlayerPanelTextDictionary.Add("currentGoldLeftText", GameObject.Find("CurrentGoldLeftText").GetComponent<Text>());
            leftPlayerPanelTextDictionary.Add("ownerOfLeftIslandText", GameObject.Find("OwnerOfLeftIslandText").GetComponent<Text>());

            //set right player text
            rightPlayerPanelTextDictionary = new Dictionary<string, Text>();
            rightPlayerPanelTextDictionary.Add("currentPointsRightText", GameObject.Find("CurrentTotalPointsRightText").GetComponent<Text>());
            rightPlayerPanelTextDictionary.Add("currentCitizensNumRightText", GameObject.Find("CurrentCitizensNumRightText").GetComponent<Text>());
            rightPlayerPanelTextDictionary.Add("currentGoldRightText", GameObject.Find("CurrentGoldRightText").GetComponent<Text>());
            rightPlayerPanelTextDictionary.Add("ownerOfRightIslandText", GameObject.Find("OwnerOfRightIslandText").GetComponent<Text>());
        }

        public static TopPanelManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new TopPanelManager();
                return instance;
            }

        }

        #region Forst Panel Setup Methods
       
        public void SetupPlayerPanel(int points, int citizen, int gold, string ownerName, string side, string gameType)
        {            
            switch (ownerName)
            {
                case "Player1":
                    switch (side)
                    {
                        case "left":
                            leftPlayerPanelTextDictionary["ownerOfLeftIslandText"].text = "Player1"; //OR  "You";
                            break;
                        case "right":
                            rightPlayerPanelTextDictionary["ownerOfRightIslandText"].text = "Player1"; //OR  "You";
                            break;
                        default:
                            GameManager.instance.PlayErrorSound(false);
                            break;
                    }
                    break;
                case "Player2":
                    switch (side)
                    {
                        case "left":
                            leftPlayerPanelTextDictionary["ownerOfLeftIslandText"].text = Player2TextSeter(gameType);
                            break;
                        case "right":
                            rightPlayerPanelTextDictionary["ownerOfRightIslandText"].text = Player2TextSeter(gameType);
                            break;
                        default:
                            GameManager.instance.PlayErrorSound(false);
                            break;
                    }
                    break;
                default:
                    GameManager.instance.PlayErrorSound(false);
                    break;
            }

            GameManager.instance.SetupScorePanelPlayersTitels(leftPlayerPanelTextDictionary["ownerOfLeftIslandText"].text, rightPlayerPanelTextDictionary["ownerOfRightIslandText"].text);

            UpdatePlayerPanel(points, citizen, gold, side);
        }

        private string Player2TextSeter(string gameType)
        {
            if (gameType == "gameVsTime")
            {
                return "None";
            }
            else if (gameType == "gameVsAI")
            {
                return "Player2"; //OR "Computer";
            }
            else
            {
                return "Error!";
            }
        }

        #endregion

        #region Panel Updates Methods

        public void SetTimerPanel(int time, int round)
        {
            timerPanelTextDictionary["currentTimeOfRoundText"].text = "" + time;
            timerPanelTextDictionary["currentRoundText"].text = "" + round;
        }

        public void UpdatePlayerPanel(int points, int citizen, int gold, string side)
        {
            switch (side)
            {
                case "left":
                    leftPlayerPanelTextDictionary["currentPointsLeftText"].text = "" + points;
                    leftPlayerPanelTextDictionary["currentCitizensNumLeftText"].text = "" + citizen;
                    leftPlayerPanelTextDictionary["currentGoldLeftText"].text = "" + gold;
                    break;
                case "right":
                    rightPlayerPanelTextDictionary["currentPointsRightText"].text = "" + points;
                    rightPlayerPanelTextDictionary["currentCitizensNumRightText"].text = "" + citizen;
                    rightPlayerPanelTextDictionary["currentGoldRightText"].text = "" + gold;
                    break;
                default:
                    GameManager.instance.PlayErrorSound(false);
                    Debug.Log("ERROR form SetPlayerPanelBySide");
                    break;
            }

        }

        #endregion
                              

    }
}