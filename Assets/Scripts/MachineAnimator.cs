using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAnimator : MonoBehaviour
{
    public GameObject machineTop;

    public Vector3 Pos_Original;
    public Vector3 Pos_Down;
    public Vector3 Pos_Press;

    public void MoveUp()
    {
        StartCoroutine (MoveOverSeconds (machineTop, Pos_Original, 1f));
    }
    public void MoveDown()
    {
        StartCoroutine (MoveOverSeconds (machineTop, Pos_Down, 1f));
    }
    public void Press()
    {
        StartCoroutine (PressEnumerator (machineTop, Pos_Press, 0.3f));
    }
    
    IEnumerator MoveOverSeconds (GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }

    IEnumerator PressEnumerator(GameObject objectToMove, Vector3 end, float seconds)
    {
        StartCoroutine (MoveOverSeconds (machineTop, end, seconds));
        yield return new WaitForSeconds(seconds);
        StartCoroutine (MoveOverSeconds (machineTop, end, seconds));
    }
}
