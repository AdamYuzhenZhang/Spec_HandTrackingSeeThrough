using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[System.Serializable]
public struct MyGesture
{
    public string name;
    public List<Vector3> fingerDatas;
    public UnityEvent onRecognized;
}

public enum MachineAction
{
    action_up,
    action_down,
    action_press
}

public class MyGestureDetector : MonoBehaviour
{
    // How much accurate the recognize should be
    [Header("Threshold value")]
    public float threshold = 0.1f;
    
    public OVRSkeleton skeleton;
    public List<MyGesture> gestures;
    private List<OVRBone> fingerbones = null;

    // Other boolean to check if are working correctly
    private bool hasStarted = false;
    private bool hasRecognize = false;
    private bool done = false;

    // Add an event if you want to make happen when a gesture is not identified
    [Header("Not Recognized Event")]
    public UnityEvent notRecognize;
    
    // three events
    public UnityEvent event_up;
    public UnityEvent event_down;
    public UnityEvent event_press;

    void Start()
    {
        // When the Oculus hand had his time to initialize hand, with a simple coroutine i start a delay of
        // a function to initialize the script
        StartCoroutine(DelayRoutine(2.5f, Initialize));
    }

    // Coroutine used for delay some function
    public IEnumerator DelayRoutine(float delay, Action actionToDo)
    {
        yield return new WaitForSeconds(delay);
        actionToDo.Invoke();
    }

    public void Initialize()
    {
        SetSkeleton();
        hasStarted = true;
    }
    public void SetSkeleton()
    {
        // Populate the private list of fingerbones from the current hand we put in the skeleton
        fingerbones = new List<OVRBone>(skeleton.Bones);
    }

    void Update()
    {
        //if the initialization was successful
        if (hasStarted.Equals(true))
        {
            // start to Recognize every gesture we make
            MyGesture currentGesture = Recognize();

            // we will associate the recognize to a boolean to see if the Gesture
            // we are going to make is one of the gesture we already saved
            hasRecognize = !currentGesture.Equals(new MyGesture());

            // and if the gesture is recognized
            if (hasRecognize) 
            {
                // we change another boolean to avoid a loop of event
                done = true;

                // after that i will invoke what put in the Event if is present
                currentGesture.onRecognized?.Invoke();
            }
            // if the gesture we done is no more recognized
            else
            {
                // and we just activated the boolean from earlier
                if (done)
                {
                    Debug.Log("Not Recognized");
                    // we set to false the boolean again, so this will not loop
                    done = false;

                    // and finally we will invoke an event when we end to make the previous gesture
                    notRecognize?.Invoke();
                }
            }
        }
    }

    public void Save(MachineAction machineAction)
    {
        // We create a new Gesture struct
        MyGesture g = new MyGesture();

        // givin to it a default name
        g.name = machineAction.ToString();

        // we create also a new list of Vector 3
        List<Vector3> data = new List<Vector3>();

        // with a foreach we go through every bone we set at the start
        // in the list of fingerbones
        foreach (var bone in fingerbones)
        {
            // and we will going to populate the list of Vector3 we done before with all the trasform found in the fingerbones
            // the fingers positions are in base at the hand Root
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }

        // after the foreach we are going to associate the list of Vector3 to the one we create from the struct "g"
        g.fingerDatas = data;
        
        // add events
        switch (machineAction)
        {
            case MachineAction.action_up:
                g.onRecognized = event_up;
                break;
            case MachineAction.action_down:
                g.onRecognized = event_down;
                break;
            case MachineAction.action_press:
                g.onRecognized = event_press;
                break;
        }

        // and in the end we will going to add this new gesture in our list of gestures
        gestures.Add(g);
    }

    MyGesture Recognize()
    {
        // in the Update if we initialized correctly, we create a new Gesture
        MyGesture currentGesture = new MyGesture();

        // we set a new float of a positive infinity
        float currentMin = Mathf.Infinity;

        // we start a foreach loop inside our list of gesture
        foreach (var gesture in gestures)
        {
            // initialize a new float about the distance
            float sumDistance = 0;

            // and a new bool to check if discart a gesture or not
            bool isDiscarded = false;

            // then with a for loop we check inside the list of bones we initalized at the start with "SetSkeleton"
            for (int i = 0; i < fingerbones.Count; i++)
            {
                // then we create a Vector3 that is exactly the transform from global position to local position of the current hand
                // we are making the gesture
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerbones[i].Transform.position);

                // with a new float we calculate the distance between the current gesture we are making with all the gesture we saved
                float distance = Vector3.Distance(currentData, gesture.fingerDatas[i]);

                // if the distance is bigger respect the threshold
                if (distance > threshold)
                {
                    // then we discart it because or is another gesture or we made bad the gesture we wanted to do
                    isDiscarded = true;
                    break;
                }

                // if the distance is correct we will add it to the first float we have created
                sumDistance += distance;
            }

            // if the gesture we made is not discarted and the distance of the gesture i minor then then Mathf.inifinty
            if (!isDiscarded && sumDistance < currentMin)
            {
                // then we set current min to the distance we have
                currentMin = sumDistance;

                // and we associate the correct gesture we have just done to the variable we created
                currentGesture = gesture;
            }
        }

        // so in the end we can return from the function the exact gesture we want to do
        return currentGesture;
    }
}
