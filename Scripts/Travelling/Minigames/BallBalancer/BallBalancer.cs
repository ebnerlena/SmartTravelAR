using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBalancer : Minigame
{
    private Rigidbody rigid;
    private Vector3 tilt;
    private bool isFlat = true;
    private Vector3 ballStartPosition = new Vector3(200, 310, 0);

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        message.text = "Balanciere den Ball";
    }

    void Update()
    {
        tilt = Input.acceleration;

        if (rigid.position.y < ballStartPosition.y - 60)
        {
            message.text = "Leider verloren, zurück nach " + GameManager.Instance.Player.Trip.CurrentTransport.From.City.Name;
            rigid.isKinematic = true;
            GameOver();
        }

        else if (isFlat)
        {
            tilt = Quaternion.Euler(90, 0, 0) * tilt;
            rigid.AddForce(tilt*Time.deltaTime*200);
        }
              
    }

    private void GameOver()
    {
        StopGame();
        success = false;
    }

    private void Reset()
    {
        rigid.isKinematic = false;
        rigid.position = ballStartPosition;
        message.text = "Balanciere den Ball";
    }

    public override void StartGame()
    {
        base.StartGame();
        Reset();
    }

    public override void StopGame()
    {
        base.StopGame();
    }
}
