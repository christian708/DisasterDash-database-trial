using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    [Header("Level Info")]
    [Tooltip("The ID of this level (1-7). Must match levelId in GameDatabase.json")]
    [SerializeField] private int levelId = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Debug.Log("Player reached finish point! Level " + levelId + " completed.");

        // Mark level completed and unlock the next one via SaveSystem
        // This replaces PlayerPrefs.SetInt("Level1Completed", 1)
        SaveSystem.CompleteLevel(levelId);

        // Load the next scene
        if (SceneController.instance != null)
            SceneController.instance.LoadNextLevel();
        else
            Debug.LogWarning("[FinishPoint] SceneController instance not found!");
    }
}
