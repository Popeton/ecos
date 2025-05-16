using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Transform head;
    [SerializeField] float spawnDistance = 4f; 
    [SerializeField] GameObject Uimenu;
    [SerializeField] GameObject UimenuOptions;
    [SerializeField] bool activeMenuUi = true;
    [SerializeField] bool activeMenuOptions = false;
   // UiFollowPlayer followPlayer;
    void Start()
    {
        DisplayMenuUi();
       // followPlayer = new UiFollowPlayer();
    }


    public void PauseButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DisplayMenuUi();
        }
    }
    private void LateUpdate()
    {
        if(activeMenuOptions == true) {

            UiFollowPlayer.FollowCamera(UimenuOptions, head, spawnDistance);
          //UimenuOptions.transform.position = head.position + new Vector3(head.forward.x,0,head.forward.z).normalized * spawnDistance;
        }

        //UimenuOptions.transform.LookAt(new Vector3(head.position.x,UimenuOptions.transform.position.y,head.position.z));
    }
    public void DisplayMenuUi()
    {
        if (activeMenuUi)
        {
            Uimenu.SetActive(false);
            activeMenuUi = false;
            Time.timeScale = 1f;
            AudioManager.instance.ResumeAllAudio();
        }
        else if (!activeMenuUi) { 
          Uimenu.SetActive(true);
          activeMenuUi = true;
          Time.timeScale = 0f;
          AudioManager.instance.PauseAllAudio();
        }
    }


    public void KeepPlaying()
    {
        activeMenuUi = true;
        DisplayMenuUi();
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene(0);
    }
    public void OptionsMenu(bool active)
    {
        activeMenuOptions= active;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

