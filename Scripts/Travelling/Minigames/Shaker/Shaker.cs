using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : Minigame
{
    private Vector3 accelerationDir;
    private int counter;
    public float shakeDetectionThreshold;
    public float minShakeInterval;
    private float sqrShakeDetectionThreshold;
    private float timeSinceLastShake;

    void Start()
    {

        message.text = "Schüttle dein Handy: " + counter;
        sqrShakeDetectionThreshold = Mathf.Pow(shakeDetectionThreshold, 2);

    }
    void Update()
    {
        
        accelerationDir = Input.acceleration;

        if (accelerationDir.sqrMagnitude >= sqrShakeDetectionThreshold && counter > 0
            && Time.unscaledTime >= timeSinceLastShake + minShakeInterval)
        {
            counter--;
            message.text = "Schüttle dein Handy: " + counter;
            timeSinceLastShake = Time.unscaledTime;
        }        
    }


    public override void StartGame()
    {
        //todo: setting counter according to travelling time
        counter = 20;

        message.text = "Schüttle dein Handy: " + counter;
        base.StartGame();  
    }

    public override void StopGame()
    {
        base.StopGame();

        if(counter > 0)
        {
            success = false;
            message.text = "";
        }
    }
}
