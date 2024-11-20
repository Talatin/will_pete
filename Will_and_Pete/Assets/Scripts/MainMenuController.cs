using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuController : MonoBehaviour
{
    private MainMenuView view;
    [SerializeField] private Button LevelSelect;
    [SerializeField] private Button Quit;

    private void Awake()
    {
        view = GetComponent<MainMenuView>();
        LevelSelect.onClick.AddListener(ToggleLevelSelection);
        FetchLevels();
    }

    private void LoadLevel(string name)
    {
        SceneLoader.LoadScene(name);
    }

    private void ToggleLevelSelection()
    {
        view.ToggleLevelSelection();
    }

    private void FetchLevels()
    {

        int sceneCount = SceneManager.sceneCountInBuildSettings;
        string[] scenes = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)
        {
            scenes[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
        }
        List<Button> buttons = view.CreatButtons(scenes);
        foreach (var button in buttons)
        {
            button.onClick.AddListener(() => { LoadLevel(button.name); });
        }
    }
}
