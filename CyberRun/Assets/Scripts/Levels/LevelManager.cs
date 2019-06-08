using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerObj;
    //[SerializeField] private List<Transform> CameraTransform;  // [0] - Camera Normal Position
    [SerializeField] private List<Vector3> CameraLocation;
    [SerializeField] private List<Quaternion> CameraRotation;

    [SerializeField] private ObstacleSpawner OS;


    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject ButtonResume;
    [SerializeField] private List<Sprite> MainTextTexture; // 1. Pause 2. GameOver
    [SerializeField] private Image MainText;

    private Backgrounds BackgroundSettings;

    private bool CameraNormalPosition = true;
    private bool CameraChange = false;

    float LerpPos = 0;
    float PosZ = 6;

    private void Awake()
    {
        BackgroundSettings = GameObject.Find("Backgrounds").GetComponent<Backgrounds>();
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, PlayerObj.transform.position.z - PosZ);
        CameraChangeFunk();
    }

    private void CameraChangeFunk()
    {
        if (CameraChange)
        {
            LerpPos += Time.deltaTime;
            if (LerpPos <= 1)
            {
                if (!CameraNormalPosition)
                {
                    float RotY = Mathf.Lerp(CameraRotation[0].x, CameraRotation[1].x, LerpPos);
                    float PosY = Mathf.Lerp(CameraLocation[0].y, CameraLocation[1].y, LerpPos);
                    PosZ = Mathf.Lerp(CameraLocation[0].z, CameraLocation[1].z, LerpPos);
  
                    transform.localPosition = new Vector3(0, PosY, PlayerObj.transform.localPosition.z - PosZ);
                    //transform.localPosition = Vector3.Lerp(CameraLocation[0], CameraLocation[1], LerpPos);
                    transform.localRotation = Quaternion.Euler(RotY, 0, 0);
                }
                else
                {
                    float RotY = Mathf.Lerp(CameraRotation[1].x, CameraRotation[0].x, LerpPos);
                    float PosY = Mathf.Lerp(CameraLocation[1].y, CameraLocation[0].y, LerpPos);
                    PosZ = Mathf.Lerp(CameraLocation[1].z, CameraLocation[0].z, LerpPos);
 
                    transform.localPosition = new Vector3(0, PosY, PlayerObj.transform.localPosition.z - PosZ);
                   //transform.localPosition = Vector3.Lerp(CameraLocation[1], CameraLocation[0], LerpPos);
                    transform.localRotation = Quaternion.Euler(RotY, 0, 0);
                }
            }
            else
            {
                CameraChange = false;
                LerpPos = 0;
            }
        }
    }

    public void ChangeCamera()
    {
        CameraNormalPosition = CameraNormalPosition ? false : true;
        CameraChange = true;
    }


    public bool GetNormalPos()
    {
        return CameraNormalPosition;  // false - Camera set down / true - Camera set Up   
    }

    public void SetPause(bool GameOver)
    {
        if (!PausePanel.active)
        {
            Time.timeScale = 0;
            MainText.sprite = MainTextTexture[0];
            MainText.rectTransform.sizeDelta = new Vector2(240, 132);
            ButtonResume.SetActive(true);
            PausePanel.SetActive(true);

            if (GameOver)
            {
                ButtonResume.SetActive(false);
                MainText.sprite  = MainTextTexture[1];
                MainText.color = BackgroundSettings.GetLevelColor();
                MainText.rectTransform.sizeDelta = new Vector2(276, 71);
            }
        }
    }

    public void Resume()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void RefreshGame()
    {
        OS.ClearMemory();
        SaveData();
        Application.LoadLevel("SampleScene");
    }

    public void ToMainMenu()
    {
        OS.ClearMemory();
        SaveData();
        Application.LoadLevel("MainMenu");
    }

    private void SaveData()
    {
        int PlayerDistance = PlayerObj.GetComponent<Player>().GetDistance();
        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.GetInt(i.ToString()) < PlayerDistance)
            {
                PlayerPrefs.SetInt(i.ToString(), PlayerDistance);
                break;
            }
        }

        Time.timeScale = 1;

    }
}
