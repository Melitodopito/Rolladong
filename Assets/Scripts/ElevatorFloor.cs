using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ElevatorFloor : MonoBehaviour
{
    [SerializeField] float elevatorSpeed = (float)10.0;
    [SerializeField] float waitingTime;
    [SerializeField] float movingTime;
    Transform tr;

    [SerializeField] Vector3 downVector;

    
    private void Start()
    {
        tr = GetComponent<Transform>();
        StartCoroutine(ElevatorMoves());
    }

    private IEnumerator ElevatorMoves()
    {
        while (true)
        {
            yield return MoveElevator(-downVector, movingTime);
            yield return new WaitForSeconds(waitingTime);
            yield return MoveElevator(downVector, movingTime);
            yield return new WaitForSeconds(waitingTime);
            yield return MoveElevator(downVector, movingTime);
            yield return new WaitForSeconds(waitingTime);
            yield return MoveElevator(-downVector, movingTime);
            yield return new WaitForSeconds(waitingTime);
        }
    }

    private IEnumerator MoveElevator(Vector3 direction, float duration)
    {
        Vector3 startPosition = tr.position;
        Vector3 endPosition = startPosition + direction;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            tr.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        tr.position = endPosition;
    }


}
