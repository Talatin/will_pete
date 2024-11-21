using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Analytics;
using System.Linq;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button LevelSelect;
    [SerializeField] private Button Quit;
    private MainMenuView view;

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
        List<string> scenes = new List<string>();

        for (int i = 0; i < sceneCount; i++)
        {
            string path = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            if (path == "LoadingScene")
            {
                continue;
            }
            scenes.Add(path);
        }

        List<Button> buttons = view.CreatButtons(scenes.ToArray());
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => { LoadLevel(button.name); });
        }
    }
}
