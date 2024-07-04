using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Stack<GameObject> menuStack = new Stack<GameObject>();
    public GameObject StartingMenu;
    public Transform menuParent;

    void Start()
    {
        OpenMenu(StartingMenu);
    }

    public void OpenMenu(GameObject menu)
    {
        GameObject instance = Instantiate(menu, menuParent);
        if (menuStack.Count > 0)
        {
            menuStack.Peek().SetActive(false);
        }

        menuStack.Push(instance);
        MenuPage page = instance.GetComponent<MenuPage>();
        page.manager = this;
        page.OnBackButtonPressed.AddListener(CloseMenu);
        page.OnLoadNewMenu.AddListener(OpenMenu);
    }

    public void CloseMenu()
    {
        GameObject instance = menuStack.Pop();
        Destroy(instance);

        if (menuStack.Count > 0)
        {
            menuStack.Peek().SetActive(true);
        }
    }
}
