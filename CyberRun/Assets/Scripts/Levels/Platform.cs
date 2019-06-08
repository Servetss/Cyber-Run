using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Tooltip("Platform Settings")]
    [SerializeField] private int Length = 0;
    [SerializeField] private int Width = 0;
    [Space]
    public GameObject NextPlatform;

    private GameObject PlatformUP;

    [Space]
    [SerializeField] private GameObject EndObj;

    private bool[,] BoxMatrix;
    [HideInInspector] public Vector3[,] PositionMesh;

    [HideInInspector] public MouseMove MouseMove;

    private Backgrounds TakeBackground; // Script for Background Random Selected
    [HideInInspector] public GameObject BackgroundeUse;  // Background object



    Renderer renderer;


    private void Awake()
    {
        BoxMatrix = new bool[Length, Width];
        PositionMesh = new Vector3[Length, Width];
        MouseMove = GameObject.Find("Player").GetComponent<MouseMove>();
        TakeBackground = GameObject.Find("Backgrounds").GetComponent<Backgrounds>();
        PlatformUP = GameObject.Find("PlatformUP");
    }


    public int GetLength()
    {
        return Length;
    }

    private void Start()
    {
        // Set List Position And BoxActive On Platform 
        for (int i = 0; i < Length; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                //PositionMesh[i,j] = new Vector3((j + 1) * 2 + (MouseMove.transform.position.x - (MouseMove.DistanceBetweenSave * 2)), 0, (i + 1) * 2);
                PositionMesh[i, j] = new Vector3((j + 1) * (MouseMove.DistanceBetweenHorizontalSave), 1, (i + 1) * (MouseMove.DistanceBetweenHorizontalSave));
                BoxMatrix[i, j] = false;
            }
        }

        BackgroundeUse = TakeBackground.GetBackGround();

        // Background
        if (gameObject.name == "Platform1")
        {
            BackgroundeUse.transform.position = transform.position;
            BackgroundeUse.SetActive(true);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player playerScript = other.gameObject.GetComponent<Player>();
            if (playerScript.PlatformSpawn != NextPlatform)
            {
                EndLevelObj();

                NextPlatform.transform.position = new Vector3(0, 0, transform.position.z + 330);


                BackgroundSet(); // Set Background on next platform
                playerScript.PlatformSpawn = NextPlatform;

                playerScript.BoxSpawn();
            }
        }
    }

    private void BackgroundSet()
    {
        GameObject back;

        do
            back = TakeBackground.GetBackGround();
        while (back == BackgroundeUse);


        if(NextPlatform.GetComponent<Platform>().BackgroundeUse != BackgroundeUse)
            NextPlatform.GetComponent<Platform>().BackgroundeUse.SetActive(false);

        NextPlatform.GetComponent<Platform>().BackgroundeUse = back;
        NextPlatform.GetComponent<Platform>().BackgroundeUse.transform.position = NextPlatform.transform.position;
        NextPlatform.GetComponent<Platform>().BackgroundeUse.SetActive(true);

    }

    EndLevelScript ELS;
    private void EndLevelObj()
    {

        EndObj.transform.SetParent(gameObject.transform);
        EndObj.transform.localPosition = new Vector3(1, 0, 329);

        ELS = EndObj.transform.GetChild(0).GetComponent<EndLevelScript>();
        ELS.NewLevel();
    }
}
