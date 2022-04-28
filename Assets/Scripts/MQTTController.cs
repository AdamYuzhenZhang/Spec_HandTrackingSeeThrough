using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MQTTController : MonoBehaviour
{
    public string nameController = "Bending Machine Controller";
    public string tagOfTheMQTTReceiver="";
    public MQTTReceiver _eventSender;
    public GameObject topPart;

    void Start()
    {
        //_eventSender=GameObject.FindGameObjectsWithTag(tagOfTheMQTTReceiver)[0].gameObject.GetComponent<MQTTReceiver>();
        _eventSender = _eventSender.gameObject.GetComponent<MQTTReceiver>();
        _eventSender.OnMessageArrived += OnMessageArrivedHandler;
    }

    private void OnMessageArrivedHandler(string newMsg)
    {
        Debug.Log("Event Fired. The message, from Object " +nameController+" is = " + newMsg);
        string[] messages = newMsg.Split();
        ParseAndMove(messages);
    }
    
    private void ParseAndMove(string[] messages)
    {
       float dist = float.Parse(messages[0]);
       dist /= 500;
       if (dist > 0.33f)
       {
           dist = 0.33f;
       }
       topPart.transform.position = new Vector3(0f, -0.3f+dist, 0.5f);
    }
}
