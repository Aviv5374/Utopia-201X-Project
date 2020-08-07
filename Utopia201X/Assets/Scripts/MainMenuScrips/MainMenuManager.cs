using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Utopia201X.Eunms;

namespace Utopia201X.MainMenusScence.MainMenuManager
{
    public class MainMenuManager : MonoBehaviour
    {
        public static MainMenuManager instance = null;

        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private Button[] menusButtons;
        private BaseEventData eventData;
        private int indexCounter;
        private bool activeCoroutine;

        private int tempNumOfRounds;
        private int tempChosenTimePerRound;
        private GameType tempGameType;
        private Text numberOfRoundsText;
        private Text timeOfRoundText;

        public int TempNumOfRounds
        {
            get
            {
                return tempNumOfRounds;
            }

            private set
            {
                if (value <= 0)
                {
                    this.tempNumOfRounds = 50;
                }
                else if (value >= 51)
                {
                    this.tempNumOfRounds = 1;
                }
                else
                    this.tempNumOfRounds = value;
            }
        }

        public int TempChosenTimePerRound
        {
            get
            {
                return tempChosenTimePerRound;
            }

            private set
            {
                if (value <= 29)
                {
                    this.tempChosenTimePerRound = 120;
                }
                else if (value >= 121)
                {
                    this.tempChosenTimePerRound = 30;
                }
                else
                    this.tempChosenTimePerRound = value;
            }
        }

        public GameType TempGameType
        {
            get
            {
                return tempGameType;
            }

            private set
            {
                this.tempGameType = value;
            }
        }

        public int IndexCounter
        {
            get
            {
                return indexCounter;
            }

            private set
            {
                if (value >= menusButtons.Length)
                {
                    indexCounter = 0;
                }
                else if (value <= -1)
                {
                    indexCounter = menusButtons.Length - 1;
                }
                else
                {
                    indexCounter = value;
                }

            }
        }

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

            tempNumOfRounds = 10;
            tempChosenTimePerRound = 30;
            tempGameType = GameType.gameVsTime;
            //Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
            numberOfRoundsText = GameObject.Find("NumberOfRoundsText").GetComponent<Text>();
            //Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
            timeOfRoundText = GameObject.Find("TimeOfRoundText").GetComponent<Text>();

            IntoMenu();
        }

        private void Start()
        {
            indexCounter = -1;
            eventData = new BaseEventData(eventSystem);
        }

        private void Update()
        {
            //if (Input.GetButtonDown("Submit2"))
            //{
            //    menusButtons[8].onClick.Invoke();
            //}

            //if (Input.GetAxisRaw("Vertical") != 0  && activeCoroutine == false)
            //{
            //    StartCoroutine(SelectOnInput(Input.GetAxisRaw("Vertical")));
            //}

            //if (Input.GetAxisRaw("Horizontal") != 0)
            //{
            //    DeselectAllButtons();
            //}
        }

        private IEnumerator SelectOnInput(float inputDirection)
        {
            activeCoroutine = true;
            
            yield return new WaitForSeconds(0.025f);

            DeselectAllButtons();

            yield return new WaitForSeconds(0.095f);

            if (inputDirection < 0)
            {
                IndexCounter--;
            }
            else
            {
                IndexCounter++;
            }

            if (!menusButtons[IndexCounter].gameObject.activeSelf || !menusButtons[IndexCounter].IsInteractable())
            {
                
                if (inputDirection < 0)
                {
                    IndexCounter--;
                }
                else
                {
                    IndexCounter++;
                }
            }

            if (menusButtons[0].gameObject.activeSelf)
            {
                if (IndexCounter == 2)
                {
                    IndexCounter = 0;
                }
                else if (IndexCounter == menusButtons.Length - 1)
                {
                    IndexCounter = 1;
                }
            }

            eventSystem.SetSelectedGameObject(menusButtons[IndexCounter].gameObject);
            yield return null;

            activeCoroutine = false;

        }

        public void DeselectAllButtons()
        {
            for (int i = 0; i < menusButtons.Length; i++)
            {
                menusButtons[i].OnDeselect(eventData);
            }
        }

        private void IntoMenu()
        {
            //Set the text of levelText to the string of the current level number.
            numberOfRoundsText.text = " " + TempNumOfRounds;

            //Set the text of levelText to the string of the current level number.
            timeOfRoundText.text = " " + TempChosenTimePerRound;
        }

        public void SubInOne(string textName)
        {
            if (textName == numberOfRoundsText.name)
            {
                TempNumOfRounds = --TempNumOfRounds;
            }
            else if (textName == timeOfRoundText.name)
            {
                TempChosenTimePerRound = --TempChosenTimePerRound;
            }

            IntoMenu();
        }

        public void AddInOne(string textName)
        {
            if (textName == numberOfRoundsText.name)
            {
                TempNumOfRounds = ++TempNumOfRounds;
            }
            else if (textName == timeOfRoundText.name)
            {
                TempChosenTimePerRound = ++TempChosenTimePerRound;
            }

            IntoMenu();
        }

        //Set for the OnClick of GameVsAIBTN and GameVsTimeBTN
        public void SetTempGameType(string chosenGameType)
        {
            if (chosenGameType == "Time")
            {
                this.tempGameType = GameType.gameVsTime;
            }
            else if (chosenGameType == "AI")
            {
                this.tempGameType = GameType.gameVsAI;
            }

        }

        public void Submit(int gameBordSceneIndex)
        {
            DeselectAllButtons();
            SceneManager.LoadScene(gameBordSceneIndex);
        }

    }
}