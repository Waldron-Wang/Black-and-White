using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; private set;}

    public UnitHealth playerHealth = new UnitHealth(100,100);

    private bool isDetectionWindowActive;
    private bool isAttackInputDetected;
    private float detectionWindowEndTime;

    void Awake()
    {
        if (gameManager != null && gameManager != this)
        {
            Destroy(this);
        }
        else
        {
            gameManager = this;
            DontDestroyOnLoad(gameManager);
        }
    }

    void Update()
    {
        if (isDetectionWindowActive && Time.time > detectionWindowEndTime)
        {
            isDetectionWindowActive = false;
            isAttackInputDetected = false;
            Debug.Log("Detection window is not active");
        }

        if (isDetectionWindowActive && Input.GetButtonDown("Attack"))
        {
            isAttackInputDetected = true;
            Debug.Log("Attack input detected");
        }
    }

    public void StartDetectionWindow(float duration)
    {
        isDetectionWindowActive = true;
        detectionWindowEndTime = Time.time + duration;
        isAttackInputDetected = false;
        Debug.Log("Detection window is active");
    }

    public bool IsAttackInputDetected()
    {
        Debug.Log("Attack input: " + isAttackInputDetected);
        return isAttackInputDetected;
    }
}
