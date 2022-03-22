using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestureSaver : MonoBehaviour
{
    public GameObject gestureDetector;
    private MyGestureDetector detectorScript;
    public Text debugState;
    public Text debugTime;

    private void Start()
    {
        detectorScript = gestureDetector.GetComponent<MyGestureDetector>();
        //StartCoroutine(recordCoroutine());
    }

    public void RecordHandGesture()
    {
        StartCoroutine(recordCoroutine());
    }

    IEnumerator recordCoroutine()
    {
        debugState.text = "About to record gestures";
        debugTime.text = "3";
        yield return new WaitForSeconds(1);
        debugTime.text = "2";
        yield return new WaitForSeconds(1);
        debugTime.text = "1";
        yield return new WaitForSeconds(1);
        debugTime.text = "0";
        debugState.text = "Recording Gesture Up";
        debugTime.text = "3";
        yield return new WaitForSeconds(1);
        debugTime.text = "2";
        yield return new WaitForSeconds(1);
        debugTime.text = "1";
        yield return new WaitForSeconds(1);
        debugTime.text = "0";
        saveGesture(MachineAction.action_up);
        debugState.text = "Recording Gesture Down";
        debugTime.text = "3";
        yield return new WaitForSeconds(1);
        debugTime.text = "2";
        yield return new WaitForSeconds(1);
        debugTime.text = "1";
        yield return new WaitForSeconds(1);
        debugTime.text = "0";
        saveGesture(MachineAction.action_down);
        debugState.text = "Recording Gesture Press";
        debugTime.text = "3";
        yield return new WaitForSeconds(1);
        debugTime.text = "2";
        yield return new WaitForSeconds(1);
        debugTime.text = "1";
        yield return new WaitForSeconds(1);
        debugTime.text = "0";
        saveGesture(MachineAction.action_press);
        debugState.text = "Recording Finished";

    }

    private void saveGesture(MachineAction machineAction)
    {
        detectorScript.Save(machineAction);
    }
}
