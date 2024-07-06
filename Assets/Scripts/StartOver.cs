using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WallScript : MonoBehaviour
{

    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerBall playerBall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.tag == "Player") {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            Transform tr = other.GetComponent<Transform>();
            gameManager.Respawn(rb,tr,playerBall.RespawnPoint);
        }
    }


}
