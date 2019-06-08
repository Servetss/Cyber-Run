using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    // Spawn On Level
    [SerializeField] private GameObject SpawnThis;
    [SerializeField] private GameObject SpawnNext;

    private LevelManager LM;

    // Start is called before the first frame update
    void Start()
    {
        LM = Camera.main.transform.GetComponent<LevelManager>();
    }


    public void CreateNewLevel()
    {
        //LM.CreateNewLevel(SpawnNext.transform.position);
    }

    public void HideBeforeLevel()
    {

    }

    public float GetBeginLevelPos_Z()
    {
        return Mathf.Abs(SpawnThis.transform.localPosition.z);
    }

}
