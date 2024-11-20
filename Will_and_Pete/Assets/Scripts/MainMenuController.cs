using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts;
using UnityEditor;

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
        if (EditorBuildSettings.scenes.Length > 0)
        {
            List<string> levels = new List<string>();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                string result = scene.path.Remove(0, 14);
                result = result.Replace(".unity", "");
                levels.Add(result);
            }
            var buttons = view.CreatButtons(levels);
            foreach (var button in buttons)
            {
                button.onClick.AddListener(() => { LoadLevel(button.name); });
            }
        }
    }
}
