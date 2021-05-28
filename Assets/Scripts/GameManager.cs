using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FBTW.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance; // Singleton
        bool gameEnded = false;

        public float restartDelay = 1f;

        public GameObject completeLevelUI;
        public GameObject EndGameUI;

        void Start()
        {
            instance = this;
        }

        public void CompleteLevel()
        {
            completeLevelUI.SetActive(true);
            Invoke("GoToTitleScreen", restartDelay);
        }

        public void EndGame()
        {
            if (!gameEnded)
            {
                gameEnded = true;
                EndGameUI.SetActive(true);
                Invoke("Restart", restartDelay);
            }
        }

        void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        void GoToTitleScreen()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
