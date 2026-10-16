using System;
using UnityEngine;

public class InfoCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _infoPanel;
    
    private void Start()
    {
        MenuButton.OnShowInfoPressed += MenuButton_OnShowInfoPressed;
        MenuButton.OnHideInfoPressed += MenuButton_OnHideInfoPressed;
        _infoPanel.SetActive(false);
    }

    private void MenuButton_OnHideInfoPressed(object sender, EventArgs e)
    {
        _infoPanel.SetActive(false);
    }

    private void MenuButton_OnShowInfoPressed(object sender, EventArgs e)
    {
        _infoPanel.SetActive(true);
    }

    private void OnDisable()
    {
        MenuButton.OnShowInfoPressed -= MenuButton_OnShowInfoPressed;
        MenuButton.OnHideInfoPressed -= MenuButton_OnHideInfoPressed;
    }
}
