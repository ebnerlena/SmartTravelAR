using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager : Minigame
{
    public static GyroManager Instance { get; private set; }

    private Rigidbody rigid;

    private Gyroscope gyro;
    private Quaternion rotation;
    private bool gyroActive;
    private Vector3 ballStartPosition = new Vector3(200, 310, 0);
    private bool gameOver;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this);
    }
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        message.text = "Balanciere den Ball";
    }

    void Update()
    {
      
        if (gyroActive)
        {
            rotation = gyro.attitude;

            if (rigid.position.y < ballStartPosition.y - 40)
            {
               message.text = "Leider verloren, zurück nach " + GameManager.Instance.Player.Trip.CurrentTransport.From.City.Name;
               rigid.isKinematic = true;
                gameOver = true;
            }
        }
    }

    public void EnableGyro()
    {
        if (!MobileOnlyActivator.IsMobile || gyroActive)
            return;

        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            gyroActive = gyro.enabled;

        }
    }

    public Quaternion GetGyroRotation()
    {
        return rotation;
    }

    private void Reset()
    {
        rigid.isKinematic = false;
        rigid.position = ballStartPosition;
        message.text = "Balanciere den Ball ";
        gameOver = false;
    }

    public override void StartGame()
    {
        base.StartGame();
        rigid = GetComponent<Rigidbody>();
        Reset();       
    }

    public override void StopGame()
    {
        success = !gameOver;
        base.StopGame();
    }
}
