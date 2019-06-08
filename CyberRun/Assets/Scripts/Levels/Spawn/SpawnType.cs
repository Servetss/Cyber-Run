using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnType : MonoBehaviour
{
    private GameObject platform;
    [HideInInspector] public Platform PlatformScript;
    [HideInInspector] public Platform PlatformUp;

    private const int row = 3;

    [Tooltip("Spawn Box in 'Spawn Cuple' Func. How Many Box need pass to Spawn new Box")]
    [SerializeField] private int SpawnThrow;

    [HideInInspector] public int LastLenghtPos = 20;   // Для заполнения сетки кубами и смотреть какая позиция была последняя 
    private int WidthRandSave = -1;

    private ObstacleSpawner OS;
    private int LevelType = 0;


    private void Start()
    {
        OS = gameObject.GetComponent<ObstacleSpawner>();
        SaveLenght = LastLenghtPos;
    }

    enum Levels { Together, Single, WithTrampo, SeveralPlatform, SpawnWithTrampline }
    private Levels LevelSelected;
    
    public void SetPlatform(GameObject _platform, int _LevelType)
    {
        LevelType = _LevelType;
        platform = _platform;
        PlatformScript = platform.GetComponent<Platform>();

        // Default Settings
        SpawnThree = 0;
        LastLenghtPos = 20;
        WidthRandSave = -1;

        BoxPlatform = 10;  // 20 Boxes Create 1 Platform way (on the level)
        PlatformEndWithTrampoline = false; // On the last box in platform, Spawn Trampoline or make second platform close
        Obst = 0;
        randPos = -1;

    }


    #region Spawn Type

    private int SpawnThree = 0;
    public void SpawnTramplinManyBox(GameObject Box)
    {
        
        LevelSelected = Levels.WithTrampo;
        if (LastLenghtPos == 20)
        {
            TwoInARow(Box, false);
        }
        else if (LastLenghtPos < PlatformScript.GetLength() - 9)
        {
            if (SpawnThree < 4)
            {
                ThreeInARow(Box, false);
            }
            else
            {
                TwoInARow(Box, false);

                if (SpawnThree == 0)
                    LastLenghtPos++;
            }
        }
        else
            Box.SetActive(false);
    }

    public void SpawnCuple(GameObject Box)
    {
        LevelSelected = Levels.Single;

        if (LastLenghtPos < PlatformScript.GetLength())
        {
            int WidthRand = Random.RandomRange(0, 3);
            Vector3 GetPosition = PlatformScript.PositionMesh[LastLenghtPos, WidthRand];

            Box.transform.localPosition = GetPosition;

            LastLenghtPos += SpawnThrow;   // Спавнить кубики через десять (Например)
        }
    }

    public void SpawnTogether(GameObject Box)
    {
        LevelSelected = Levels.Together;

        if (LastLenghtPos < PlatformScript.GetLength())
        {
            TwoInARow(Box, false);

            if (WidthRandSave == -1)
                LastLenghtPos += SpawnThrow;   // Спавнить кубики через десять (Например)
        }
    }

    private bool SelectUpDown = false; // False - Spawn on the down platform
    private int SaveLenght;
    int Type = 0;
    int NextPos = 1;
    public void SpawnInTwoPlatform(GameObject Box)
    {
        LevelSelected = Levels.SeveralPlatform;
        if (LastLenghtPos < PlatformScript.GetLength())
        {
            if (SaveLenght != LastLenghtPos)
            {
                SelectUpDown = SelectUpDown ? false : true;
                SaveLenght = LastLenghtPos;

                Type = Random.Range(0, 3);
                NextPos = Random.Range(3, 7);
            }
            if(Type == 0 || Type == 1)
                TwoInARow(Box, SelectUpDown);
            else 
                ThreeInARow(Box, SelectUpDown);

            if (WidthRandSave == -1)
                LastLenghtPos += NextPos;   // Спавнить кубики через десять (Например)
        }
    }


    int BoxPlatform = 10;  // 20 Boxes Create 1 Platform way (on the level)
    bool PlatformEndWithTrampoline = false; // On the last box in platform, Spawn Trampoline or make second platform close
    int PositionHorizontal;
    int Obst = 0;
    int randPos = -1;
    public void SpawnWithTrampline(GameObject Box)
    {
        LevelSelected = Levels.SpawnWithTrampline;
        if (LastLenghtPos < PlatformScript.GetLength())
        {
            if (LastLenghtPos == 20)
            {
                TwoInARow(Box, false);
                if (WidthRandSave == -1)
                {
                    LastLenghtPos += 4;     // Спавнить кубики через десять (Например)
                    PositionHorizontal = 1;
                }
            }
            else if(LastLenghtPos >= 24)
            {
                if (randPos != -1)
                {
                    Vector3 GetPositionLeft = PlatformScript.PositionMesh[LastLenghtPos, randPos];
                    Box.transform.localPosition = GetPositionLeft;

                    randPos = -1;
                }
                else
                {
                    switch (PositionHorizontal)
                    {
                        case 0:
                            Vector3 GetPositionLeft = PlatformScript.PositionMesh[LastLenghtPos, PositionHorizontal];
                            Box.transform.localPosition = GetPositionLeft;

                            if (Obst == 0)
                            {
                                randPos = Random.Range(1, 3);
                                Obst = 2;
                            }
                            else
                                Obst--;

                            LastLenghtPos += 1;   // Спавнить кубики через десять (Например)
                            break;
                        case 1:
                            Vector3 GetPositionCentr = PlatformScript.PositionMesh[LastLenghtPos, PositionHorizontal];
                            Box.transform.localPosition = GetPositionCentr;

                            LastLenghtPos += 1;   // Спавнить кубики через десять (Например)
                            break;
                        case 2:
                            Vector3 GetPositionRight = PlatformScript.PositionMesh[LastLenghtPos, PositionHorizontal];
                            Box.transform.localPosition = GetPositionRight;

                            if (Obst == 0)
                            {
                                randPos = Random.Range(0, 2);
                                Obst = 2;
                            }
                            else
                                Obst--;

                            LastLenghtPos += 1;   // Спавнить кубики через десять (Например)
                            break;
                        default:
                            break;
                    }
                }

                BoxPlatform--;
                if (BoxPlatform == 1)
                {
                    foreach (GameObject Trampoline in OS.Trampolines)           // Trampline Spawn
                    {
                        if (!Trampoline.active)
                        {
                            Vector3 GetPosition = PlatformScript.PositionMesh[LastLenghtPos, PositionHorizontal];
                            GetPosition.y = 2;

                            Trampoline.transform.SetParent(PlatformScript.gameObject.transform);
                            Trampoline.transform.localPosition = GetPosition;
                            //Trampoline.transform.position = new Vector3(Trampoline.transform.position.x, 0, Trampoline.transform.position.z);
                            Trampoline.SetActive(true);

                            break;
                        }
                    }
                }
                if (BoxPlatform == 0)
                {
                    PositionHorizontal = Random.Range(0, 3);
                    

                    BoxPlatform = 10;
                    LastLenghtPos += 4;
                }
                
            }
        }
    }

    #endregion


    #region Setting Spawn
    private void TwoInARow(GameObject Box, bool UpDown)
    {
        int _WidthRand = 0;
        Vector3 GetPosition = new Vector3(0, 0, 0);

        _WidthRand = Random.RandomRange(0, 3);  // Рандомь куб


        //
        // Если зарандомилась в таком же месте, то переставь в другое место
        if (WidthRandSave == _WidthRand)
        {
            if (_WidthRand == 2)                          // Max
                _WidthRand--;
            else if (_WidthRand == 0)                     // Min
                _WidthRand++;
            else
            {
                int r = Random.RandomRange(0, 2);
                if (r == 0)
                    _WidthRand++;
                else
                    _WidthRand--;
            }
        }


        // Поставить Куб в позицию на платформе
        GetPosition = PlatformScript.PositionMesh[LastLenghtPos, _WidthRand];
        GetPosition.y = UpDown ? GetPosition.y + 8 : GetPosition.y;


        Box.transform.localPosition = GetPosition;

        float rotZ = UpDown ? -180 : 0;
        Box.transform.localRotation = Quaternion.Euler(0,0,rotZ); 

        if (WidthRandSave == -1) { WidthRandSave = _WidthRand; }
        else { SpawnThree = 0; TrampolineSpawn(_WidthRand); WidthRandSave = -1; }
    }

    private void ThreeInARow(GameObject Box, bool UpDown)
    {

        Vector3 GetPosition = new Vector3(0, 0, 0);
        WidthRandSave++;

        GetPosition = PlatformScript.PositionMesh[LastLenghtPos, WidthRandSave];
        GetPosition.y = UpDown ? GetPosition.y + 8 : GetPosition.y;

        Box.transform.localPosition = GetPosition;

        float rotZ = UpDown ? -180 : 0;
        Box.transform.localRotation = Quaternion.Euler(0, 0, rotZ);

        if (WidthRandSave == 2)     // When WidthSave == 3, go to next Height Row
        {
            WidthRandSave = -1;

            LastLenghtPos++;
            SpawnThree++;
        }
    }

    // Spawn Trampline or something else
    private void TrampolineSpawn(int PositionWidth)
    {
        if ((LevelSelected == Levels.WithTrampo || LevelSelected == Levels.SpawnWithTrampline) && LastLenghtPos == 20) // Events When function need work
        {
            foreach (GameObject Trampoline in OS.Trampolines)           // Trampline Spawn
            {
                if (!Trampoline.active)
                {
                    int TramplineSpawnPos = row - (WidthRandSave + PositionWidth);
                    Vector3 GetPosition = PlatformScript.PositionMesh[LastLenghtPos, TramplineSpawnPos];


                    Trampoline.transform.SetParent(PlatformScript.gameObject.transform);
                    Trampoline.transform.localPosition = GetPosition;
                    Trampoline.transform.position = new Vector3(Trampoline.transform.position.x, 0, Trampoline.transform.position.z);
                    Trampoline.SetActive(true);
                    WidthRandSave = -1;

                    LastLenghtPos++;
                    SpawnThree++;

                    break;
                }
            }
        }
        else if (LevelSelected == Levels.WithTrampo && LastLenghtPos != 20)
        {
            int TramplineSpawnPos = row - (WidthRandSave + PositionWidth);
            Vector3 GetPosition = PlatformScript.PositionMesh[LastLenghtPos, TramplineSpawnPos];

            GameObject obj = OS.SpawnObjWithTrampo();
  

            obj.transform.localPosition = GetPosition;
            obj.transform.position = new Vector3(obj.transform.position.x, 3, obj.transform.position.z);

        }
    }
    #endregion
}