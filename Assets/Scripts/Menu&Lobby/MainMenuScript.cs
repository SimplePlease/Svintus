using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject registrationPanel;

    public void Start()
    {
        settingsPanel.GetComponent<SettingsScript>().SetUp();
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SettingsButton()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        registrationPanel.SetActive(false);
    }

    public void RegistrationButton()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(false);
        registrationPanel.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion("ru");
    }
}