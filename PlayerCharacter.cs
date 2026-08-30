using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private GameObject boyVisual;
    [SerializeField] private GameObject girlVisual;

    private void Start()
    {
        // Read character from SaveSystem instead of PlayerPrefs
        string selectedCharacter = SaveSystem.Data.player.selectedCharacter;

        if (selectedCharacter == "Girl")
        {
            boyVisual.SetActive(false);
            girlVisual.SetActive(true);
            Debug.Log("Player spawned as GIRL");
        }
        else
        {
            boyVisual.SetActive(true);
            girlVisual.SetActive(false);
            Debug.Log("Player spawned as BOY");
        }
    }
}
