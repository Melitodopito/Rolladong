using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    private int score;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.text = "Score: " + score;
    }

    public void UpdateScore(int scoreToAdd) {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void rewspawn(Rigidbody rb, Transform tr, Vector3 respawnPoint)
    {
        tr.position = respawnPoint;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
    }


}   
