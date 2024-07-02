using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    [SerializeField] float timeForRespawn;
    [SerializeField] PlayerBall playerBall;
    [SerializeField] private GameManager gameManager;

    private float collisionTime;

    private bool isColliding;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision){
        isColliding = true;
        collisionTime = 0;
    }

    void OnCollisionStay(Collision collision){
        if(isColliding && collision.gameObject.CompareTag("Player")){
            collisionTime += Time.deltaTime;
        }

        if(collisionTime > timeForRespawn) {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            Transform tr = collision.gameObject.GetComponent<Transform>();
            gameManager.Respawn(rb,tr,playerBall.RespawnPoint);
        
        }
    }

}
