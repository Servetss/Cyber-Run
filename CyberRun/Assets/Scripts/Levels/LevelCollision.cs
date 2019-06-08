using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCollision : MonoBehaviour
{
    [SerializeField] private GameObject ThisLevel;
    [SerializeField] private Collision _Collision;
    private LevelScript LS;

    private void Start()
    {
        LS = ThisLevel.GetComponent<LevelScript>();
    }


    private void OnTriggerEnter(Collider other)
    {
        switch (_Collision)
        {
            case Collision.Begin:
                BeginCollision();
                break;
            case Collision.End:
                EndCollision();
                break;
            default:
                break;
        }
    }

    private void BeginCollision()
    {
        LS.HideBeforeLevel();
    }

    private void EndCollision()
    {
        LS.CreateNewLevel();
    }
}
