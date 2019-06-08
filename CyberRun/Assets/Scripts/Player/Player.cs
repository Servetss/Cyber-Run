using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Parametrs")]
    [SerializeField] private GameObject NewPlayer;
    [Range(5f, 35)]    // Player Move
    public float Speed;


    [SerializeField] private ObstacleSpawner Spawner;
    [SerializeField] private Backgrounds BackgroundScript;


    [Header("Canvas")]
    [SerializeField] private Text Distance_T; // Distance
    private float Distance = 0;


    [Header("Next Platform")]
    public GameObject PlatformSpawn;    // Save Next Platform


    [HideInInspector] public float AdditionalSpeed = 0;


    public void BoxSpawn() // When new Level
    {
        Spawner.SetBoxOnPlatform();
        BackgroundScript.CurrentColorChange();
    }
    int screenCount = 0;
    // Update is called once per frame
    void FixedUpdate()
    {
        NewPlayer.transform.position += transform.forward * Time.deltaTime * (Speed + AdditionalSpeed);       // Move Player Forward 
        transform.GetChild(1).transform.GetChild(0).transform.Rotate(Speed,0,0);
        //print(transform.GetChild(1).transform.GetChild(0).gameObject.name);

        Speed += (Time.deltaTime * 0.2f);
        Speed = Mathf.Clamp(Speed, 5, 30); // Min - Max Speed

        CanvasRendere();

        if (Input.GetKeyDown(KeyCode.Space)) { ScreenCapture.CaptureScreenshot(screenCount.ToString()); screenCount++; }
    }

    int dist = 0;
    private void CanvasRendere()
    {
        Distance += Time.deltaTime * (Speed + AdditionalSpeed);
        dist = (int)Mathf.Round(Distance);
        Distance_T.text = "Distance: " + dist;
    }

    public int GetDistance()
    {
        return dist;
    }

    #region Trigers
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Pause(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            //print("Exit");
        }
    }
    #endregion

    private void Pause(bool GameOver)
    {
        gameObject.transform.parent.GetChild(1).GetComponent<LevelManager>().SetPause(true);
        //print("Game Over!!");
    }
}
