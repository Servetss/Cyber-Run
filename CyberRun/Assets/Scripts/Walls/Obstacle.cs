using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Events _Event;
    [SerializeField] private bool StartScene = false;
    private ObstaclesData data;

    private GameObject Player;
    private GameObject Character;
    

    private void Start()
    {
        Player = GameObject.Find("NewPlayer");

    }

    public void Init(ObstaclesData _data)
    {
        data = _data;
        transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>().material = data.obstacleMaterial;
        // Поставить Рендер Кубика   --- GetComponent<ScriptableObject>().
    }


    public static Action<GameObject> OnObstacleOverFly;

    private void FixedUpdate()
    {
        if (transform.position.z < Player.transform.GetChild(0).transform.GetChild(1).transform.position.z - 7 && OnObstacleOverFly != null)
        {
            if (StartScene)
                gameObject.SetActive(false);
            else
                OnObstacleOverFly(gameObject);
        }
    }
}
