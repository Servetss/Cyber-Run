using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManuManager : MonoBehaviour
{
    [SerializeField] private GameObject MM_Panel;
    [SerializeField] private GameObject LeaderBoardPanel;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        
    }

    public void OpenNewGame()
    {
        Application.LoadLevel("SampleScene");
    }

    public void OpenLeaderBoard()
    {
        MM_Panel.SetActive(false);
        LeaderBoardPanel.SetActive(true);
    }

    public void BackToMM()
    {
        MM_Panel.SetActive(true);
        LeaderBoardPanel.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
