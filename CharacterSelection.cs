using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject boyHighlight;
    public GameObject girlHighlight;

    private void Start()
    {
        // Restore previously saved character selection
        string saved = SaveSystem.Data.player.selectedCharacter;
        if (saved == "Girl")
            ApplySelection("Girl");
        else
            ApplySelection("Boy");
    }

    public void SelectBoy()
    {
        ApplySelection("Boy");
        Debug.Log("Boy selected!");
    }

    public void SelectGirl()
    {
        ApplySelection("Girl");
        Debug.Log("Girl selected!");
    }

    private void ApplySelection(string character)
    {
        // Write to SaveSystem instead of PlayerPrefs
        SaveSystem.Data.player.selectedCharacter = character;
        SaveSystem.Save();

        bool isBoy = (character == "Boy");
        if (boyHighlight != null) boyHighlight.SetActive(isBoy);
        if (girlHighlight != null) girlHighlight.SetActive(!isBoy);
    }
}
