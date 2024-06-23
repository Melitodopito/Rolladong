using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WallScript : MonoBehaviour
{

    private MeshCollider mycol;

    private SceneManager myscene;


    private Collision collision1;

    //public MeshCollider myCol { get {return myCol}}
    // Start is called before the first frame update
    void Start()
    {
        mycol = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.tag == "Player") {
            Respawn();
        }
    }


    private void Respawn() 
    {
        SceneManager.LoadScene("SampleScene");
    }


}
