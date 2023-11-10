using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{

    public GameObject mainCamera;
    private CameraFollow cameraFollowScript;

    void PauseGame()
    {
        Time.timeScale = 0;
        if (cameraFollowScript) cameraFollowScript.enabled = false;
    }
    void ResumeGame()
    {
        Time.timeScale = 1;
        if (cameraFollowScript) cameraFollowScript.enabled = enabled;
        transform.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        PauseGame();
        cameraFollowScript = mainCamera.gameObject.GetComponent<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ResumeGame();
        }
    }
}
