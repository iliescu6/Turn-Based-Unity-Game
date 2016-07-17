using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuOptions : MonoBehaviour {

    public GameObject mainMenu, controlsPanel;

    void Awake() {
        mainMenu = GameObject.Find("Main Menu Panel");
        controlsPanel = GameObject.Find("Controls Panel");
        controlsPanel.SetActive(false);
    }
    public void ControlsMenu() {
        if (!controlsPanel.activeInHierarchy) {
            controlsPanel.SetActive(true);
            mainMenu.SetActive(false);
        }
    }

    public void BackToMainMenu() {
        if (!mainMenu.activeInHierarchy)
        {
            controlsPanel.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
    public void StartGame(){
        SceneManager.LoadScene("Level1");
        }
}
