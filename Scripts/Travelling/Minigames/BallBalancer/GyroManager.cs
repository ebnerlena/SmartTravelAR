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
               rigid.isKinematic = true;
               gameOver = true;
               StartCoroutine(WaitForNextStart());
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

    IEnumerator WaitForNextStart()
    {
        message.text = "Neuer Versuch wird vorbereitet...";
        yield return new WaitForSeconds(3);
        Reset();
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
        rigid = GetComponent<Rigidbody>();
        Reset();
        gameOver = false;
    }

    public override void StopGame()
    {
        success = !gameOver;
        base.StopGame();
    }
}
