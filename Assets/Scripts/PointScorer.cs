using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointScorer : MonoBehaviour
{

    [SerializeField] private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(UnityEngine.Collider other){
        if(other.CompareTag("Player")) {
            gameManager.UpdateScore(1);
        }
        
    }

}
