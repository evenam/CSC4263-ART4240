using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableDuringRotation : MonoBehaviour {

    // Both left and right arrows to be disbaled while poly is rotating
    public Button buttonToBeDisabled1;
    public Button buttonToBeDisabled2;

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void DisableButton()
    {
        buttonToBeDisabled1.interactable = false;
        buttonToBeDisabled2.interactable = false;
        StartCoroutine(WaitForRotate());
    }

    public IEnumerator WaitForRotate()
    {
        yield return new WaitForSeconds(1);
        buttonToBeDisabled1.interactable = true;
        buttonToBeDisabled2.interactable = true;
    }
}
