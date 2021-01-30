using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour, DogActions.IPlayerActions {
    private DogActions _actions;

    [SerializeField] private GameObject pawseMenu;
    [SerializeField] private GameObject winMenu;

    private void Start() {
        _actions = new DogActions();
        _actions.Player.SetCallbacks(this);
        _actions.Player.Pawse.Enable();
    }

    public void Unpawse() {
        pawseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Pawse() {
        pawseMenu.SetActive(true);
        Time.timeScale = 0;
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

    public void OnMove(InputAction.CallbackContext context) { }

    public void OnLook(InputAction.CallbackContext context) { }

    public void OnFire(InputAction.CallbackContext context) { }

    public void OnSniff(InputAction.CallbackContext context) { }

    public void OnDig(InputAction.CallbackContext context) { }

    public void OnPawse(InputAction.CallbackContext context) {
        if (pawseMenu.activeSelf) {
            Unpawse();
        }
        else {
            Pawse();
        }
    }
}
