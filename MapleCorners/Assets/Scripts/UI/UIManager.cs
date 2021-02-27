// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehavior<UIManager>
{
    private bool _pauseMenuOn = false;
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private GameObject[] menuTabs = null;
    [SerializeField] private Button[] menuButtons = null;
    [SerializeField] private UIInventoryBar uiInventoryBar = null;
    [SerializeField] private PauseMenuInventoryManagement pauseMenuInventoryManagement = null;

    public bool PauseMenuOn { get => _pauseMenuOn; set => _pauseMenuOn = value; }

    protected override void Awake()
    {
        base.Awake();

        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        PauseMenu();
    }

    /// <summary>
    /// Toggles the pause menu when the Escape key is pressed
    /// </summary>
    private void PauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenuOn)
            {
                DisablePauseMenu();
            }
            else
            {
                EnablePauseMenu();
            }
        }
    }


    private void EnablePauseMenu()
    {
        uiInventoryBar.DestroyCurrentlyDraggedItems();
        uiInventoryBar.ClearCurrentlySelectedItems();

        // Set the pause menu to on
        PauseMenuOn = true;
        // Disable player input
        Player.Instance.PlayerInputIsDisabled = true;
        // Pause the clock
        Time.timeScale = 0;
        // Set the pause menu canvas to active
        pauseMenu.SetActive(true);

        System.GC.Collect();

        HighlightButtonForSelectedTab();
    }

    public void DisablePauseMenu()
    {
        // Destroy any items currently being dragged
        pauseMenuInventoryManagement.DestroyCurrentlyDraggedItems();

        // Set the pause menu to off
        PauseMenuOn = false;
        // Enable player input
        Player.Instance.PlayerInputIsDisabled = false;
        // Unpause the clock
        Time.timeScale = 1;
        // Turn off the pause menu
        pauseMenu.SetActive(false);
    }


    private void HighlightButtonForSelectedTab()
    {
        for (int i = 0; i < menuTabs.Length; i++)
        {
            if (menuTabs[i].activeSelf)
            {
                SetButtonColorToActive(menuButtons[i]);
            }

            else
            {
                SetButtonColorToInactive(menuButtons[i]);
            }
        }
    }


    private void SetButtonColorToActive(Button button)
    {
        ColorBlock colors = button.colors;

        colors.normalColor = colors.pressedColor;

        button.colors = colors;

    }

    private void SetButtonColorToInactive(Button button)
    {
        ColorBlock colors = button.colors;

        colors.normalColor = colors.disabledColor;

        button.colors = colors;

    }

    public void SwitchPauseMenuTab(int tabNum)
    {
        for (int i = 0; i < menuTabs.Length; i++)
        {
            if (i != tabNum)
            {
                menuTabs[i].SetActive(false);
            }
            else
            {
                menuTabs[i].SetActive(true);

            }
        }

        HighlightButtonForSelectedTab();

    }
}
