using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBall : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float rollSpeed = 10f;

    [SerializeField] Vector3 respawnPoint;
    private Rigidbody rb;

    private SphereCollider spCollider;

    private bool isDragging = false;
    private Vector3 lastTouchPosition;
    private Vector3 firstTouchPosition;

    public SphereCollider SpCollider { get{ return spCollider;} }
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spCollider = GetComponent<SphereCollider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (isTouchingBall(touchPosition))
                    {
                        // WHY isDragging = true;
                        firstTouchPosition = touchPosition;
                        Debug.Log("Touched the ball");
                    }
                    break;

                case TouchPhase.Moved:

                    if(isTouchingBall(touchPosition)){

                        isDragging = true;
                        Debug.Log("I am draggin");
                        if (isDragging)
                        {
                            lastTouchPosition = touchPosition;
                        }

                    }
                    break;

                case TouchPhase.Ended:

                    lastTouchPosition = touchPosition;
                    Vector3 direction = (lastTouchPosition - firstTouchPosition).normalized;
                    Debug.Log($"This is my Direction Vector: {direction} ");
                    MoveBall(direction);
                    break;

                case TouchPhase.Canceled:
                    isDragging = false;
                    break;
            }
        }
    }

    // Not working properly
    private bool isTouchingBall(Vector3 touchPosition){
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit)) {
            if (hit.collider.gameObject == gameObject) {
                return true;
            }
        }
        return false;
    }

    private void MoveBall(Vector3 direction)
    {
        direction = direction.normalized;
        Debug.Log($"THis is my direction {direction}");

        float xVector = -direction.x;
        float zVector = -direction.z;

        rb.AddForce(xVector, (float) 0.0, zVector * rollSpeed , ForceMode.Impulse);
    }




}
