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

    [SerializeField] int bounceMinX = -5;
    [SerializeField] int bounceMaxX = 5;

    [SerializeField] int bounceY = 5;
    
    
    [SerializeField] int bounceZ = -5;



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

        
        
    }

    // Update is called once per frame
    void Update()
    {
        TouchInput();
    }
    


    // Ball Movements
    private void MoveBall(Vector3 direction, float dragDistance)
    {
        direction = direction.normalized;
        float xVector = -direction.x;
        float zVector = -direction.z;
        float dragDistanceForce;

        if(dragDistance <= 0.05)
        {
            dragDistanceForce = 1;

        }
        else if ( dragDistance > 0.050 && dragDistance < 0.09)
        {
            dragDistanceForce = (float)1.3;
        }
        else
        {
            dragDistanceForce = (float)1.55;
        }

        rb.AddForce(xVector * dragDistanceForce, (float) 0.0, (zVector * rollSpeed) * dragDistanceForce , ForceMode.Impulse);
    }
    private void applyMoveVector(Vector3 touchPosition){

        lastTouchPosition = touchPosition;
        Vector3 direction = (lastTouchPosition - firstTouchPosition);
        float dragDistance = direction.magnitude;
        //Debug.Log($"Drag Distance is: {dragDistance}");
        // Debug.Log($"This is my Direction Vector: {direction} ");
        MoveBall(direction,dragDistance);
    }

    private void BounceBack(){

        // X must have both types of values
        // Y must be positive
        // Z must be negative
        
        float xVector = UnityEngine.Random.Range(bounceMinX,bounceMaxX);
        float yVector = UnityEngine.Random.Range(0,bounceY);
        float zVector = UnityEngine.Random.Range(bounceZ,0);

        

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

    
    // Line Renderere(Maybe Create own script in future?)
    private void ChangLineRendererColor(float dragDistance)
    {

        if (dragDistance <= 0.05)
        {
           

            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;



        }
        else if (dragDistance > 0.050 && dragDistance < 0.09)
        {
            lineRenderer.startColor = Color.yellow;
            lineRenderer.endColor = Color.yellow;
        }
        else
        {
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;

        }
    }

    private void HandleLineRenderer(Vector3 touchPosition)
    {

        lineRenderer = GetComponent<LineRenderer>();


        lineRenderer.SetPosition(0, tr.position);
        lineRenderer.SetPosition(1, touchPosition);
        Vector3 direction = (firstTouchPosition - lineRenderer.GetPosition(1));


        // Debug.Log($"This is Position 0: {lineRenderer.GetPosition(0)}, This is 1: {lineRenderer.GetPosition(1)}, this is the conta{direction.normalized}");

        float dragDistance = direction.magnitude;
        ChangLineRendererColor(dragDistance);

        //Debug.Log($"this is the magnitude{direction.magnitude}");

    }

    // THIS NEEDS TO CHANGE
    private void ResetLineRender()
    {
        lineRenderer.SetPosition(0, tr.position);
        lineRenderer.SetPosition(1, tr.position);
    }



    // TOUCH 
    private void TouchInput()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));
            //Debug.Log("Touch position (screen): " + touchPosition);


            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;


            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (isTouchingBall(ray, out hit))
                    {
                  
                        firstTouchPosition = touchPosition;

          
                    }
                    break;

                case TouchPhase.Moved:

                    if (firstTouchPosition != Vector3.zero)
                    {
                       HandleLineRenderer(touchPosition);

                    }


                    break;

                case TouchPhase.Ended:
                    if (firstTouchPosition != Vector3.zero)
                    {
                        applyMoveVector(touchPosition);
                        firstTouchPosition = Vector3.zero;


                    }
                    // Reset Line Render
                    ResetLineRender();

                    break;

                case TouchPhase.Canceled:

                    break;
            }
        }
    }

}
