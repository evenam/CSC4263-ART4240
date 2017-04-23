using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateSongSelect : MonoBehaviour
{
    public bool canRotate;

    void Start()
    {
        canRotate = true;
    }

    void Update ()
    {
        if (canRotate)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                canRotate = false;
                rotateLeft();
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                canRotate = false;
                rotateRight();
            }
        }
    }

    public void rotateLeft()
    {
        if (canRotate)
        {
            canRotate = false;
            StartCoroutine(RotateCoroutine(Vector3.up * 72, 1));
        }
    }

    public void rotateRight()
    {
        if (canRotate)
        {
            canRotate = false;
            StartCoroutine(RotateCoroutine(Vector3.up * -72, 1));
        }
    }

    public IEnumerator RotateCoroutine(Vector3 angles, float time)
    {
        Quaternion fAngle = transform.rotation;
        Quaternion tAngle = Quaternion.Euler(transform.eulerAngles + angles);
        for(float t = 0F; t < 1F; t += Time.deltaTime/time)
        {
            transform.rotation = Quaternion.Lerp(fAngle, tAngle, t);
            yield return null;
        }
        canRotate = true;
    }
}
