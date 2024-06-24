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

    private Vector3 myTestVector = new Vector3(0,0,0);

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
            //Debug.Log("Touch position (screen): " + touchPosition);
            
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;
            //Debug.Log($"This is firsTouchPosition when done NOT correctly {firstTouchPosition}");
            

            // TO SOLVE, detects correctly the ball touch, however draggin works as well withou touching, perhaps reset firsttouchposition value 

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if(isTouchingBall(ray,out hit)) {
                        firstTouchPosition = touchPosition;
                        //Debug.Log($"This is firsTouchPosition when done correctly {firstTouchPosition}");
                    }else { 
                        }
                    break;

                case TouchPhase.Moved:
                    //Debug.Log("I am draggin");
                    break;

                case TouchPhase.Ended:
                        if(firstTouchPosition != myTestVector){
                            applyMoveVector(touchPosition);
                        }
                    
                    break;

                case TouchPhase.Canceled:
                    break;
            }
        }
    }

    // Not working properly
    

    private void MoveBall(Vector3 direction)
    {
        direction = direction.normalized;
        Debug.Log($"THis is my direction {direction}");

        float xVector = -direction.x;
        float zVector = -direction.z;

        rb.AddForce(xVector, (float) 0.0, zVector * rollSpeed , ForceMode.Impulse);
    }


    private bool isTouchingBall(Ray ray,out RaycastHit hit){
        if (Physics.Raycast(ray, out hit))
        {
            GameObject touchedObject = hit.collider.gameObject;
            if(touchedObject.tag == "Player") {
                return true;
            }  else {
                return false;
            }
            
        } else {
            return false;
        }
    }

    private void applyMoveVector(Vector3 touchPosition){
        lastTouchPosition = touchPosition;
        Vector3 direction = (lastTouchPosition - firstTouchPosition).normalized;
        Debug.Log($"This is my Direction Vector: {direction} ");
        MoveBall(direction);
    }



}
