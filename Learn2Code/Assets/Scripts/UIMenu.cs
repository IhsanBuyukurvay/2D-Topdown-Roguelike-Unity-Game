using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    public GameObject settingsPanel;

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenSettingsButton()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsButton()
    {
        settingsPanel.SetActive(false);
    }

    public void ExitButtton()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
