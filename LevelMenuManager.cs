using UnityEngine;
using UnityEngine.UI;

public class LevelMenuManager : MonoBehaviour
{
    [Header("Level Buttons")]
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private Button level3Button;
    [SerializeField] private Button level4Button;
    [SerializeField] private Button level5Button;
    [SerializeField] private Button level6Button;
    [SerializeField] private Button level7Button;

    [Header("Button Sprites")]
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Sprite lockedSprite;

    private void Start()
    {
        // Read unlock state from SaveSystem (GameDatabase.json)
        // instead of PlayerPrefs
        SetLevelState(level1Button, SaveSystem.IsLevelUnlocked(1));
        SetLevelState(level2Button, SaveSystem.IsLevelUnlocked(2));
        SetLevelState(level3Button, SaveSystem.IsLevelUnlocked(3));
        SetLevelState(level4Button, SaveSystem.IsLevelUnlocked(4));
        SetLevelState(level5Button, SaveSystem.IsLevelUnlocked(5));
        SetLevelState(level6Button, SaveSystem.IsLevelUnlocked(6));
        SetLevelState(level7Button, SaveSystem.IsLevelUnlocked(7));
    }

    private void SetLevelState(Button button, bool unlocked)
    {
        if (button == null) return;

        button.interactable = unlocked;

        Image image = button.GetComponent<Image>();
        if (image != null && unlockedSprite != null && lockedSprite != null)
            image.sprite = unlocked ? unlockedSprite : lockedSprite;
    }

    // ── Navigation ────────────────────────────────────────────

    public void GoBack()
    {
        if (SceneController.instance != null)
            SceneController.instance.LoadScene("Charac_Select");
        else
            Debug.LogWarning("SceneController instance not found!");
    }

    // ── Level loaders ─────────────────────────────────────────

    public void OpenLevel1()
    {
        LoadScene("Level0_Fire_Interior");
    }

    public void OpenLevel2()
    {
        if (SaveSystem.IsLevelUnlocked(2)) LoadScene("Level2_Earthquake_Interior");
    }

    public void OpenLevel3()
    {
        if (SaveSystem.IsLevelUnlocked(3)) LoadScene("Level3_Earthquake");
    }

    public void OpenLevel4()
    {
        if (SaveSystem.IsLevelUnlocked(4)) LoadScene("Level4_Typhoon_In");
    }

    public void OpenLevel5()
    {
        if (SaveSystem.IsLevelUnlocked(5)) LoadScene("Level5_Typhoon");
    }

    public void OpenLevel6()
    {
        if (SaveSystem.IsLevelUnlocked(6)) LoadScene("Level6_Flood_In");
    }

    public void OpenLevel7()
    {
        if (SaveSystem.IsLevelUnlocked(7)) LoadScene("Level7_Flood");
    }

    private void LoadScene(string sceneName)
    {
        if (SceneController.instance != null)
            SceneController.instance.LoadScene(sceneName);
        else
            Debug.LogWarning("SceneController instance not found!");
    }
}
