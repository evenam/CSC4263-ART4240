using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableDuringRotation : MonoBehaviour {

    public Button buttonToBeDisabled;

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void DisableButton()
    {
        buttonToBeDisabled.interactable = false;
        StartCoroutine(WaitForRotate());
    }

    public IEnumerator WaitForRotate()
    {
        yield return new WaitForSeconds(1);
        buttonToBeDisabled.interactable = true;
    }
}
