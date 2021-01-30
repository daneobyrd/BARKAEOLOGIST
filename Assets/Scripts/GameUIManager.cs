using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameUIManager : MonoBehaviour, DogActions.IPlayerActions {
    private DogActions _actions;

    // Miki's Edit
    public AudioMixerSnapshot paused;
    public AudioMixerSnapshot unpaused;
    // Miki's Edit

    [SerializeField] private GameObject pawseMenu;
    [SerializeField] private GameObject winMenu;

    private void Start() {
        _actions = new DogActions();
        _actions.Player.SetCallbacks(this);
        _actions.Player.Pawse.Enable();
    }

    public void Unpawse() {
        StartCoroutine(ClosePawse());
    }

    public void Pawse() {
        pawseMenu.SetActive(true);
        Time.timeScale = 0;
        // Miki
        paused.TransitionTo(0.1f);
    }

    public void Quit() {
        // We may want to do this asynchronously, with some kind of loading indicator
        SceneManager.LoadScene(0);
    }

    [ContextMenu("Force win")]
    public void WinGame() {
        winMenu.SetActive(true);
        Time.timeScale = 0;
    }

    /* MIKI
    public void Lowpass()
    {
        if (Time.timeScale == 0)
        {
            paused.TransitionTo(0.1f);
        }
        else
        {
            unpaused.TransitionTo(0.1f);
        }
    }
    */ 

    public void OnMove(InputAction.CallbackContext context) { }

    public void OnLook(InputAction.CallbackContext context) { }

    public void OnFire(InputAction.CallbackContext context) { }

    public void OnSniff(InputAction.CallbackContext context) { }

    public void OnDig(InputAction.CallbackContext context) { }

    public void OnPawse(InputAction.CallbackContext context) {
        if (context.performed) {
            if (pawseMenu.activeSelf) {
                Unpawse();
            }
            else {
                Pawse();
            }
        }
    }

    IEnumerator ClosePawse() {
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1;
        pawseMenu.SetActive(false);
        unpaused.TransitionTo(0.1f);
    }
}
