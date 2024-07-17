using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;

    private Button _button;

    private void Start()
    {
        _document = GetComponent<UIDocument>();

        _button = _document.rootVisualElement.Q("StartGameButton") as Button;
        _button.RegisterCallback<ClickEvent>(OnPlayerGameClick);
    }

    private void OnDisable()
    {
        _button.UnregisterCallback<ClickEvent>(OnPlayerGameClick);
    }

    private void OnPlayerGameClick(ClickEvent evt)
    {
        SceneManager.LoadSceneAsync(1);
    }
}
