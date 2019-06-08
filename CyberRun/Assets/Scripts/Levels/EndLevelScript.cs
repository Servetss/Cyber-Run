using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelScript : MonoBehaviour
{
    [SerializeField] private ObstacleSpawner OS;

    [HideInInspector] private GameObject Player;
    private float Distance = 0;
    

    private Renderer render;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("NewPlayer");
        render = gameObject.transform.GetChild(0).GetComponent<Renderer>();
    }



    public void NewLevel()
    {
        int MaskType = Random.Range(0,4);
        int EndSceneType = Random.Range(0, 2);
        EndSceneType = OS.GetSaveType() == 3 ? 0 : EndSceneType;

        render.material.SetInt("_MaskSelected", MaskType);
        render.material.SetInt("_EndKind", EndSceneType);
    }
}
