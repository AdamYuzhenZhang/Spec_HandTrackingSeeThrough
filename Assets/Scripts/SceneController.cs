using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject sheet;
    private Transform sheetTransform;

    private void Start()
    {
        sheetTransform = sheet.transform;
    }
    
    public void ResetSheet()
    {
        sheet.transform.position = sheetTransform.position;
        sheet.transform.rotation = sheetTransform.rotation;
        sheet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        sheet.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
    
    public void RestartScene()
    {
        SceneManager.LoadScene("Bending2");
    }
    
    

}
