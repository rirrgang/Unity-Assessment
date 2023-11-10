using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public GameObject mainMenuBackground;
    public GameObject optionsMenuBackground;

    public Slider mouseSensitivity;
    public Slider fovSlider;

    public CameraFollow cameraFollowScript;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mouseSensitivity.value = cameraFollowScript.sensitivity;
        fovSlider.value = mainCamera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // if game is already paused resume it
            if (Time.timeScale == 0)
                ResumeGame();
            else
            {
                PauseGame();
            }
        }
    }

    public void showOptions()
    {
        optionsMenuBackground.SetActive(true);
        mainMenuBackground.SetActive(false);
    }

    public void showMainMenu()
    {
        optionsMenuBackground.SetActive(false);
        mainMenuBackground.SetActive(true);
    }

    void toggleMenu(bool isVisible)
    {
        mainMenuBackground.SetActive(isVisible);
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        toggleMenu(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        //if (cameraFollowScript) cameraFollowScript.enabled = false;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        toggleMenu(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //if (cameraFollowScript) cameraFollowScript.enabled = enabled;

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        toggleMenu(false);
    }

    public void applyOptions()
    {
        cameraFollowScript.sensitivity = mouseSensitivity.value;
        mainCamera.fieldOfView = fovSlider.value;
        showMainMenu();
    }
}
