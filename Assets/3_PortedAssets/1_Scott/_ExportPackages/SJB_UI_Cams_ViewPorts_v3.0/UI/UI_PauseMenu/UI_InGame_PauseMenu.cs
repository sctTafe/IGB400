using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{
    public class UI_InGame_PauseMenu : MonoBehaviour
    {

        bool pausedGame;
        [SerializeField] GameObject pauseMenu;

        // Start is called before the first frame update
        void Start()
        {
            pausedGame = false;
            pauseMenu.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (pausedGame)
                {
                    fnc_ResumeGame();
                }
                else
                {
                    fnc_PauseGame();

                }
            }
        }


        public void fnc_ResumeGame()
        {
            pauseMenu.SetActive(false);
            pausedGame = false;
            Time.timeScale = 1f;
        }

        public void fnc_PauseGame()
        {
            pauseMenu.SetActive(true);
            pausedGame = true;
            Time.timeScale = 0f;
        }


    }
}