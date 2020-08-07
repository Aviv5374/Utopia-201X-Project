using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpTextScript : MonoBehaviour {

    [SerializeField] private GameObject canves;

	// Use this for initialization
	void Start ()
    {
        //gameObject.GetComponent<Text>().rectTransform.position = canves.GetComponentInParent<Transform>().position;		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DestroyText()
    {        
        Destroy(canves);
    }
}
