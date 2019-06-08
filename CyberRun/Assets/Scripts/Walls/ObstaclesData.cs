using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Obstacles/Wall", fileName = "New Box")]
public class ObstaclesData : ScriptableObject
{
    [Tooltip("Object")]
    [SerializeField] private GameObject Obstacle;
    [Tooltip("Object")]
    [SerializeField] private Material ObstacleMaterial;

    public GameObject obstacle
    {
        get { return Obstacle; }
        protected set { }
    }

    public Material obstacleMaterial
    {
        get { return ObstacleMaterial; }
        protected set { }
    }
}