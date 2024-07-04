using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuPage : MonoBehaviour
{
    public UnityEvent OnBackButtonPressed;
    public UnityEvent<GameObject> OnLoadNewMenu;
    public MenuManager manager;

    public void RequestBackButtonPress()
    {
        OnBackButtonPressed.Invoke();
    }

    public void RequestLoadNewMenu(GameObject newMenu)
    {
        OnLoadNewMenu.Invoke(newMenu);
    }
}
