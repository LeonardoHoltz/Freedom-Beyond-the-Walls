using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FBTW.Resources;
using FBTW.InputManager;
using FBTW.Player;


namespace FBTW.Pause
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool GameIsPaused = false;

        public GameObject pauseMenuUI, CameraController, Input_Handler, PlayerManager;



        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            CameraController.GetComponent<CameraController>().enabled = true;
            Input_Handler.GetComponent<InputHandler>().enabled = true;
            PlayerManager.GetComponent<PlayerManager>().enabled = true;
            Time.timeScale = 1f;
            GameIsPaused = false;
        }

        public void Pause()
        {
            pauseMenuUI.SetActive(true);
            CameraController.GetComponent<CameraController>().enabled = false;
            Input_Handler.GetComponent<InputHandler>().enabled = false;
            PlayerManager.GetComponent<PlayerManager>().enabled = false;
            Time.timeScale = 0f;
            GameIsPaused = true;
        }

        public void LoadMenu()
        {
            Time.timeScale = 1f;
            Debug.Log("Loading Menu...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        public void QuitGame()
        {
            Debug.Log("Quitting Game...");
            Application.Quit();
        }

    }
}