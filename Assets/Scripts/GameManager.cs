using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] private PlayerBall playerBall;

    [SerializeField] float timeLeft;
    private int score;
    void Start()
    {
        score = 0;
        scoreText.text = "Score: " + score;
        UpdateTimer();
    }

    void Update() {
        if(timeLeft > 0){
            timeLeft -= Time.deltaTime;
            timeLeft = Mathf.Max(timeLeft, 0);
            UpdateTimer();
        } else {
            timeLeft = 0;
        }
    }

    public void UpdateScore(int scoreToAdd) {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void Respawn(Rigidbody rb, Transform tr, Vector3 respawnPoint)
    {
        tr.position = respawnPoint;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        playerBall.BallInHole = false;
        
    }

    private void UpdateTimer() {
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("Time Left: {0:00}",seconds);
    }

}   
