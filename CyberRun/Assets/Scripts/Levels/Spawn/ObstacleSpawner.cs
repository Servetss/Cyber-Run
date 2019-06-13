using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Tooltip("Platforms on level")]
    [SerializeField] private List<GameObject> PlatformsPref;
    [SerializeField] private GameObject PlatformUp;
    public List<GameObject> Trampolines;
    [SerializeField] private GameObject Player;
    private LevelManager LM;
    [Space]

    [Tooltip("List of Obstacles Settings")]
    [SerializeField] private List<ObstaclesData> ObstaclesSettings;

    [Tooltip("Count emount object in the pool")]
    [SerializeField] private int poolCount;

    [Tooltip("Reference to base prefab")]
    [SerializeField] private GameObject ObstaclesPrefab;

    [Tooltip("Time between Obstacles Spawn")]
    [SerializeField] private float spawnTime;


    public static Dictionary<GameObject, Obstacle> obstacles;
    Queue<GameObject> currentObstacles;


    private SpawnType ST;
    private int PlatformLength;

    private Vector3 ActuallyPosition;
    

    // Start is called before the first frame update
    void Start()
    {
        ActuallyPosition = transform.position;

        ST = gameObject.GetComponent<SpawnType>();
        ST.PlatformUp = PlatformUp.GetComponent<Platform>();
        LM = Camera.main.GetComponent<LevelManager>();

        // Obstacles Sample
        obstacles = new Dictionary<GameObject, Obstacle>();
        currentObstacles = new Queue<GameObject>();
        //


        for (int i = 0; i < poolCount; i++)
        {
            var prefab = Instantiate(ObstaclesPrefab);
            var script = prefab.GetComponent<Obstacle>();
            prefab.SetActive(false);
            obstacles.Add(prefab, script);
            currentObstacles.Enqueue(prefab);
        }



        Obstacle.OnObstacleOverFly += ReturnObstacle;

        SetBoxOnPlatform();
    }

    public void SetBoxOnPlatform()
    {
        StartCoroutine(Spawn());
    }

    public int GetSaveType()
    {
        return SaveSpawnType;
    }

    private int SaveSpawnType = 0;
    private IEnumerator Spawn()
    {
        if (spawnTime == 0)
        {
            Debug.LogError("Do not create Spawn Time");
            spawnTime = 0.2f;
        }

        int SpawnType;
        do
            SpawnType = Random.Range(0, 4);
        while (SpawnType == 4 && SaveSpawnType == 4);


        ST.SetPlatform(Player.GetComponent<Player>().PlatformSpawn, SpawnType);

        PlatformLength = ST.PlatformScript.GetLength();
        bool On = true;


        if (SpawnType == 3 && SaveSpawnType != 3)
        {
            PlatformUp.transform.position = Player.GetComponent<Player>().PlatformSpawn.transform.position;
            PlatformUp.transform.position += new Vector3(8, 10 ,0);
        }

        if (SaveSpawnType == 3)
        {
            LM.ChangeCamera();
            Player.GetComponent<MouseMove>().UpDown(true);

        }              // Set Camera in down position When Platform collision Overllap
        else if (!LM.GetNormalPos())
        {
            LM.ChangeCamera();
            Player.GetComponent<MouseMove>().UpDown(false);
        }         // Set Camera in Normal Position When Platform collision Overllap

        if (SpawnType == 3 && SaveSpawnType == 3)       // if SpawnType = SpawnInTwoPlatform (3) And before was other level, ChangeCamera 
            SpawnType = Random.Range(0, 3);

        
        SaveSpawnType = SpawnType;


        while (On) // пока не будет заполнен сектор платформы
        {
            yield return new WaitForSeconds(spawnTime);
            if (currentObstacles.Count > 0) // Пока не заполнен сектор или 0
            {
                if (PlatformLength > ST.LastLenghtPos)
                {

                    var obst = currentObstacles.Dequeue();
                    var script = obstacles[obst];

                    obst.transform.SetParent(Player.GetComponent<Player>().PlatformSpawn.transform);

                    obst.SetActive(true);

                    script.Init(ObstaclesSettings[0]);

                    switch (SpawnType)
                    {
                        case 0:
                            ST.SpawnCuple(obst);
                            break;
                        case 1:
                            ST.SpawnTogether(obst);
                            break;
                        case 2:
                            ST.SpawnWithTrampline(obst);
                            break;
                        case 3:
                            ST.SpawnInTwoPlatform(obst);
                            break;
                        case 4:
                            ST.SpawnTramplinManyBox(obst);
                            break;
                        default:
                            break;
                    }
                    //ST.SpawnTogether(obst);
                    //ST.SpawnTramplinManyBox(obst);
                    //ST.SpawnInTwoPlatform(obst);
                }
                else
                {
                    On = false;
                }
            }
        }
    }

    public GameObject SpawnObjWithTrampo()
    {
        var obst = currentObstacles.Dequeue();
        var script = obstacles[obst];

        obst.transform.SetParent(Player.GetComponent<Player>().PlatformSpawn.transform);


        obst.SetActive(true);

        script.Init(ObstaclesSettings[1]);

        return obst;
    }

    public void ClearMemory()
    {
        currentObstacles.Clear();
        obstacles.Clear();
    }

    private void ReturnObstacle(GameObject _obstacle)
    {
        _obstacle.transform.position = ActuallyPosition;

        currentObstacles.Enqueue(_obstacle);
        _obstacle.SetActive(false);
    }
}