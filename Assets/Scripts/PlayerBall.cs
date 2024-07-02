using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBall : MonoBehaviour
{
    [SerializeField] float rollSpeed = 10f;
    [SerializeField] private GameManager gameManager;
    private Transform tr;


    // Colliders an Rigid body
    private SphereCollider spCollider;
    public SphereCollider SpCollider { get{ return spCollider;} }  
    private Collision col;
    private Rigidbody rb;

    // Vectors
    [SerializeField] Vector3 respawnPoint;

    public Vector3 RespawnPoint
        {
            get { return respawnPoint; }
            set { respawnPoint = value; }
        }
    private Vector3 lastTouchPosition;
    private Vector3 firstTouchPosition;
    private Vector3 myTestVector = new Vector3(0,0,0);
    
    //Bools
    private bool ballTouchedWall = false;
    private bool ballInHole = false;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spCollider = GetComponent<SphereCollider>();
        tr = GetComponent<Transform>();
        
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
    


    // NEXT MAKE COUNTING POINTS AND TIMER
    private void MoveBall(Vector3 direction)
    {
        direction = direction.normalized;
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
        //Debug.Log($"This is my Direction Vector: {direction} ");
        MoveBall(direction);
    }

    

    private void OnTriggerEnter(UnityEngine.Collider other){
        if(other.CompareTag("Hole")) {
            ballInHole = true;
        }
    }

    private void OnCollisionStay(Collision collision){
        if(ballTouchedWall && collision.gameObject.CompareTag("Floor") && !ballInHole) {
            //Debug.Log("TRANSFORM");
            gameManager.rewspawn(rb,tr,respawnPoint);
            ballTouchedWall = false;
        }
    }

    private void OnCollisionExit(Collision collision) {
        
        if (collision.gameObject.CompareTag("Wall")) {
            ballTouchedWall = true;

        }

    }

    

}
