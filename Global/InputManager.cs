using UnityEngine;

namespace DuperMod.Global;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public bool IsMenuOpen { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DisableInput()
    {
        IsMenuOpen = true;
    }

    public void EnableInput()
    {
        IsMenuOpen = false;
    }
}