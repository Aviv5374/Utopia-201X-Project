using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Utopia201X
{
    public class TestScript : MonoBehaviour
    {
        private static TestScript instance = null;

        private float defultWaitingTime = 2.5f;
        private float currentWaitingTime;
        private bool isCoroutinesText1Active = false;
        private int coroutinesText1Counter = 0;

        public TextMeshProUGUI textMesh;
        private int counter;

        private Button testButton;

        public Transform buyableTransform;

        public static TestScript Instance
        {
            get
            {
                return instance;
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

            currentWaitingTime = defultWaitingTime;
            
        }

        // Use this for initialization
        void Start()
        {
            //StartCoroutine(TextProCounterTest());
            //GameObject gO = (GameObject)Instantiate(Resources.Load("PopUpCanvas2"), buyableTransform);
            //Debug.Assert(gO == null);
        }

        // Update is called once per frame
        void Update()
        {
             
        }

        public void StartCoroutinesText1()
        {
            StopAllCoroutines();
            //Debug.Log("Before CoroutinesText1 4 Time");
            //StartCoroutine("CoroutinesText1");
            //Debug.Log("After CoroutinesText1 4 Time");
        }

        public IEnumerator CoroutinesText1()
        {
            int localCoroutinesText1ID = ++coroutinesText1Counter;
            int counter = 0;
            isCoroutinesText1Active = true;
            Debug.Log("CoroutinesText1 id " + localCoroutinesText1ID +" is now Started.");
            //Debug.Log("The Current Waiting Time is: " + currentWaitingTime.ToString());                        
            //yield return new WaitForSeconds(currentWaitingTime);
            
            if (true)
            {
                while (counter < 10)
                {
                    counter++;
                    Debug.Log("counter of CoroutinesText1 id " + localCoroutinesText1ID + " is: " + counter.ToString());
                    yield return null;
                }
            }
            StartCoroutine("CoroutinesText1");
            isCoroutinesText1Active = false;
            Debug.Log("CoroutinesText1 id " + localCoroutinesText1ID + " is now finished.");
        }

        public IEnumerator CoroutinesText2()
        {
            Debug.Log("CoroutinesText2 is now Started.");
            //yield return new WaitForSeconds(currentWaitingTime);
            int counter = 0;            
            while (counter < 15)
            {
                counter++;
                Debug.Log("counter of CoroutinesText2: " + counter.ToString());
                yield return null;
            }

            Debug.Log("CoroutinesText2 is now finished.");
        }

        private IEnumerator TextProCounterTest()
        {
            counter++;
            textMesh.text = counter.ToString();
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(TextProCounterTest());
        }
    }
}