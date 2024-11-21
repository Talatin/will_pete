using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectionHolder;
    [SerializeField] private GameObject levelSelectBTNPrefab;
    [SerializeField] private Transform scrollViewContent;

    public void ToggleLevelSelection()
    {
        levelSelectionHolder.SetActive(!levelSelectionHolder.activeSelf);
    }

    public List<Button> CreatButtons(string[] levelNames)
    {
        DeleteContentObjects();

        List<Button> buttons = new List<Button>();
        foreach (string levelName in levelNames)
        {
            GameObject button = Instantiate(levelSelectBTNPrefab, scrollViewContent);
            button.name = levelName;
            if (button.transform.GetChild(0).TryGetComponent<TMP_Text>(out var text))
            {
                text.text = levelName;
                buttons.Add(button.GetComponent<Button>());
            }
            else
            {
                Debug.LogWarning("Levelselect button tmp text not found");
            }
        }
        return buttons;
    }

    private void DeleteContentObjects()
    {
        if (scrollViewContent.childCount >= 0)
        {
            for (int i = scrollViewContent.childCount - 1; i >= 0; i--)
            {
                GameObject child = scrollViewContent.GetChild(i).gameObject;
                child.GetComponent<Button>().onClick.RemoveAllListeners();
                Destroy(scrollViewContent.GetChild(i).gameObject);
            }
        }
    }
}