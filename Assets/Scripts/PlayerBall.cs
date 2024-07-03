using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.TestTools.Utils;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class PlayerBall : MonoBehaviour
{   
    //Floats and Ints
    [SerializeField] float rollSpeed = 10f;

    [SerializeField] int bounceMinX;
    [SerializeField] int bounceMaxX;

    [SerializeField] int bounceY;
    
    
    [SerializeField] int bounceZ;



    // Different Components
    [SerializeField] private GameManager gameManager;
    private Transform tr;

    private LineRenderer lineRenderer;


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
    
    //Bools
    private bool ballTouchedWall = false;
    private bool ballInHole = false;

    public bool BallInHole {set {ballInHole = value;} }

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spCollider = GetComponent<SphereCollider>();
        tr = GetComponent<Transform>();
        lineRenderer = GetComponent<LineRenderer>();
        
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
            

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if(isTouchingBall(ray,out hit)) {
                        firstTouchPosition = touchPosition;

                        }
                    break;

                case TouchPhase.Moved:
                        //DrawLine(firstTouchPosition, touchPosition);
                    break;

                case TouchPhase.Ended:
                        if(firstTouchPosition != Vector3.zero){
                            applyMoveVector(touchPosition);
                            lineRenderer.enabled = false;
                        }
                    
                    break;

                case TouchPhase.Canceled:
                    lineRenderer.enabled = false; 
                    break;
            }
        }
    }
    


    // Ball Movements
    private void MoveBall(Vector3 direction)
    {
        direction = direction.normalized;
        float xVector = -direction.x;
        float zVector = -direction.z;
    

        rb.AddForce(xVector, (float) 0.0, zVector * rollSpeed , ForceMode.Impulse);
    }
    private void applyMoveVector(Vector3 touchPosition){
        lastTouchPosition = touchPosition;
        Vector3 direction = (lastTouchPosition - firstTouchPosition).normalized;
        //Debug.Log($"This is my Direction Vector: {direction} ");
        MoveBall(direction);
    }

    private void BounceBack(){

        // X must have both types of values
        // Y must be positive
        // Z must be negative
        
        float xVector = UnityEngine.Random.Range(bounceMinX,bounceMaxX);
        float yVector = UnityEngine.Random.Range(0,bounceY);
        float zVector = UnityEngine.Random.Range(bounceZ,0);

        // maybe better option below, must test it out.

        //Vector3 bounceBack = new Vector3(xVector,yVector,zVector);
        //bounceBack = bounceBack.normalized;

        //Debug.Log($"This is my Vector3 {bounceBack}");
        rb.AddForce(new Vector3(xVector,yVector,zVector), ForceMode.Impulse);
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


    

    private void OnTriggerEnter(UnityEngine.Collider other){
        if(other.CompareTag("Hole")) {
            ballInHole = true;
        }
    }

    private void OnCollisionStay(Collision collision){
        if(ballTouchedWall && collision.gameObject.CompareTag("Floor") && !ballInHole) {
            //Debug.Log("TRANSFORM");
            // gameManager.rewspawn(rb,tr,respawnPoint);
            // ballTouchedWall = false;
        }
    }

    private void OnCollisionExit(Collision collision) {
        
        if (collision.gameObject.CompareTag("Wall")) {
            ballTouchedWall = true;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Wall") && !ballInHole) {
            BounceBack();
        }

    }

    private void DrawLine(Vector3 start, Vector3 end) {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }    


}
