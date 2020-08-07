using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utopia201X.BordeGameScence.UI
{
    public class ErrorMassagesManagerScript : MonoBehaviour
    {
        public static ErrorMassagesManagerScript instance = null;
        [SerializeField] private GameObject errorMessagePanelLeft;        
        [SerializeField] private GameObject errorMessagePanelRight;        
        private Dictionary<string, Text[]> panelMessages;
        private string releventPanel;
        [SerializeField] private float defultWaitingTime = 3.5f;
        private float currentWaitingTime;
        private bool isTimingManagerActive;

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

        // Use this for initialization
        void Start()
        {
            panelMessages = new Dictionary<string, Text[]>();

            SetupPanelMessags("left");                    
            errorMessagePanelLeft.SetActive(false);

            SetupPanelMessags("right");                     
            errorMessagePanelRight.SetActive(false);

            releventPanel = ""; //GameManager.instance.GetPlayerSide("Player1"); OR string playerName ?????!!!!

            currentWaitingTime = defultWaitingTime;

            isTimingManagerActive = false;


        }

        private void SetupPanelMessags(string side)
        {
            if (side == "left")
            {
                panelMessages.Add(side, errorMessagePanelLeft.GetComponentsInChildren<Text>());
            }
            else
            {
                panelMessages.Add(side, errorMessagePanelRight.GetComponentsInChildren<Text>());
            }
            

            for (int i = 0; i < panelMessages[side].Length; i++)
            {
                panelMessages[side][i].text = "";
                panelMessages[side][i].gameObject.SetActive(false);               
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (releventPanel == "")
            {
                releventPanel = GameManager.instance.GetPlayerSide("Player1");//?????
            }
            //ToDo: Find  better logice for this
            if (isTimingManagerActive)
            {
                currentWaitingTime -= Time.deltaTime;
            }
            else
            {
                StartCoroutine(DeactiveErrorMessageInPanel(releventPanel, "?????"));
                currentWaitingTime = defultWaitingTime;
            }
        }
        
        //1
        public void ActiveErrorMessage (string playerName, int messageID, string message)
        {           
            switch (messageID)
            {
                case 1:
                    SetErrorMessageInPanel(ActivePanel(playerName), "The Buildings & Units Menu Is Closed");
                    break;
                case 2:
                    SetErrorMessageInPanel(ActivePanel(playerName), "Round Time Is Over");
                    break;
                case 3:
                    SetErrorMessageInPanel(ActivePanel(playerName), "Not Enough Gold");
                    break;
                case 4:
                    SetErrorMessageInPanel(ActivePanel(playerName), "The Button Is Locked");
                    break;
                case 5:
                    SetErrorMessageInPanel(ActivePanel(playerName), "Too Many Forts");
                    break;
                case 6:
                    SetErrorMessageInPanel(ActivePanel(playerName), "Not Found Selected Construction Area");
                    break;
                case 7:
                    SetErrorMessageInPanel(ActivePanel(playerName), "Invalid Construction Area");
                    break;
                case 8:
                    SetErrorMessageInPanel(ActivePanel(playerName), "There Is An Active Golden Citizen Group"); //???????
                    break;
                case 9:
                    SetErrorMessageInPanel(ActivePanel(playerName), "Yours Opponent Too Weak, Stop Trolling!!!");
                    break;
                case 10:
                    SetErrorMessageInPanel(ActivePanel(playerName), "Invasion Failed");
                    break;
                default://????
                    GameManager.instance.PlayErrorSound(false);
                    break;
            }

            //OR
            ///SetErrorMessageInPanel(ActivePanel(playerName), message);
        }

        //2
        private string ActivePanel(string playerName)
        {
            releventPanel = GameManager.instance.GetPlayerSide(playerName);//?????

            if (releventPanel == "left")
            {
                errorMessagePanelLeft.SetActive(true);
            }
            else
            {
                errorMessagePanelRight.SetActive(true);
            }

            return releventPanel;
        }

        //3
        private void SetErrorMessageInPanel(string releventActivePanel, string message)
        {            
            //... Set Message code ...
            for (int i = 0; i < panelMessages[releventActivePanel].Length; i++)
            {
                if (!panelMessages[releventActivePanel][i].IsActive())
                {
                    panelMessages[releventActivePanel][i].gameObject.SetActive(true);
                    panelMessages[releventActivePanel][i].text = message;
                    break;
                }
            }

            //every time I active or reactive a Coroutine it will restart it self.
            //Problem: additional unwanted waiting time.
            //ToDo: write a system that set Waiting Time before StartCoroutine(ManageMessageTiming())
            StartCoroutine(ManageMessageTiming(releventActivePanel, message));
            
            //... mybe more code ....
        }

        //4
        private IEnumerator ManageMessageTiming(string activePanel, string message)
        {
            isTimingManagerActive = true;

            yield return new WaitForSeconds(currentWaitingTime);            

            isTimingManagerActive = false;
        }

        //5
        private IEnumerator DeactiveErrorMessageInPanel(string activePanel, string message)
        {          
            for (int i = 0; i < panelMessages[activePanel].Length; i++)
            {
                if (panelMessages[activePanel][i].IsActive())
                {
                    panelMessages[activePanel][i].gameObject.SetActive(false);
                    yield return new WaitForSeconds(5.25f);
                }
                
            }

            yield return new WaitForSeconds(1.5f);
            if (CanThePanelBeShutDown(activePanel))//????????!!!!!!!!!
            {
                if (releventPanel == "left")
                {
                    errorMessagePanelLeft.SetActive(false);
                }
                else
                {
                    errorMessagePanelRight.SetActive(false);
                }
            }
        }

        //6??
        private bool CanThePanelBeShutDown(string activePanel)
        {
            for (int i = 0; i < panelMessages[activePanel].Length; i++)
            {
                if (panelMessages[activePanel][i].IsActive())
                {
                    return false;
                }
            }

            return true;
        }

    }
}