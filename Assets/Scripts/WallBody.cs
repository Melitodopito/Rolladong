using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpining : MonoBehaviour
{
    [SerializeField] float spinningSpeed = (float) 10.0;
    Transform rotationController;

    [SerializeField] Vector3 rotationVector;

    // Start is called before the first frame update
    void Start()
    {
        rotationController = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        rotationController.Rotate(rotationVector * spinningSpeed);
    }
}
