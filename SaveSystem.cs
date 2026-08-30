using System;
using System.IO;
using UnityEngine;

// ─────────────────────────────────────────────────────────────
// SaveSystem.cs
// Handles all reading and writing of game data to/from JSON.
//
// HOW IT WORKS:
// 1. On first load, it reads GameDatabase.json from
//    Application.persistentDataPath (device storage).
//    If no save file exists yet, it copies the default data
//    from Assets/Data/GameDatabase.json (via Resources).
// 2. All scripts access data through SaveSystem.Data
//    e.g.  SaveSystem.Data.player.selectedCharacter
// 3. After making any change, call SaveSystem.Save()
//    to write the updated data back to the JSON file.
// ─────────────────────────────────────────────────────────────

public static class SaveSystem
{
    // File name stored on the device
    private const string SAVE_FILE = "GameDatabase.json";

    // Cached in-memory data — loaded once, used everywhere
    private static GameData _data;

    // ── Public access ─────────────────────────────────────────

    /// <summary>
    /// The active game data. Auto-loads on first access.
    /// Use this to read or write any game state.
    /// Example: SaveSystem.Data.player.selectedCharacter = "Girl";
    /// </summary>
    public static GameData Data
    {
        get
        {
            if (_data == null) Load();
            return _data;
        }
    }

    // Full path to the save file on the device
    private static string SavePath => Path.Combine(Application.persistentDataPath, SAVE_FILE);

    // ── Load ──────────────────────────────────────────────────

    /// <summary>
    /// Loads game data from the save file.
    /// If no save file exists, loads default data from
    /// Assets/Resources/Data/GameDatabase.json
    /// Call this once at game startup.
    /// </summary>
    public static void Load()
    {
        if (File.Exists(SavePath))
        {
            // Load existing save file from device storage
            try
            {
                string json = File.ReadAllText(SavePath);
                _data = JsonUtility.FromJson<GameData>(json);
                Debug.Log("[SaveSystem] Save file loaded from: " + SavePath);
            }
            catch (Exception e)
            {
                Debug.LogError("[SaveSystem] Failed to load save file: " + e.Message);
                LoadDefaults();
            }
        }
        else
        {
            // First time playing — load defaults from Resources
            Debug.Log("[SaveSystem] No save file found. Loading defaults.");
            LoadDefaults();
        }
    }

    // ── Save ──────────────────────────────────────────────────

    /// <summary>
    /// Writes current game data to the save file on device.
    /// Call this after any change you want to keep.
    /// Example: after completing a level, selecting a character, etc.
    /// </summary>
    public static void Save()
    {
        if (_data == null)
        {
            Debug.LogWarning("[SaveSystem] Nothing to save — data is null.");
            return;
        }

        try
        {
            string json = JsonUtility.ToJson(_data, prettyPrint: true);
            File.WriteAllText(SavePath, json);
            Debug.Log("[SaveSystem] Game saved to: " + SavePath);
        }
        catch (Exception e)
        {
            Debug.LogError("[SaveSystem] Failed to save: " + e.Message);
        }
    }

    // ── Reset ─────────────────────────────────────────────────

    /// <summary>
    /// Deletes the save file and resets to default data.
    /// Use this for a "New Game" or debug reset.
    /// </summary>
    public static void Reset()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);

        LoadDefaults();
        Debug.Log("[SaveSystem] Save data reset to defaults.");
    }

    // ── Level helpers ─────────────────────────────────────────

    /// <summary>
    /// Returns the LevelData for a given level ID (1-based).
    /// Returns null if not found.
    /// </summary>
    public static LevelData GetLevel(int levelId)
    {
        return Data.levels.Find(l => l.levelId == levelId);
    }

    /// <summary>
    /// Marks a level as completed and unlocks the next one.
    /// Automatically saves after updating.
    /// </summary>
    public static void CompleteLevel(int levelId)
    {
        // Mark this level completed
        LevelData current = GetLevel(levelId);
        if (current != null)
        {
            current.completed = true;
            Debug.Log("[SaveSystem] Level " + levelId + " completed.");
        }

        // Unlock the next level
        LevelData next = GetLevel(levelId + 1);
        if (next != null)
        {
            next.unlocked = true;
            Debug.Log("[SaveSystem] Level " + (levelId + 1) + " unlocked.");
        }

        // Update current level tracker
        Data.player.currentLevel = Mathf.Max(Data.player.currentLevel, levelId + 1);

        Save();
    }

    /// <summary>
    /// Returns true if a level is unlocked.
    /// </summary>
    public static bool IsLevelUnlocked(int levelId)
    {
        LevelData level = GetLevel(levelId);
        return level != null && level.unlocked;
    }

    /// <summary>
    /// Returns true if a level has been completed.
    /// </summary>
    public static bool IsLevelCompleted(int levelId)
    {
        LevelData level = GetLevel(levelId);
        return level != null && level.completed;
    }

    // ── Internal ──────────────────────────────────────────────

    /// <summary>
    /// Loads the default GameDatabase.json from Resources/Data/
    /// This is the "factory default" bundled with the game.
    /// </summary>
    private static void LoadDefaults()
    {
        TextAsset defaultFile = Resources.Load<TextAsset>("Data/GameDatabase");

        if (defaultFile != null)
        {
            _data = JsonUtility.FromJson<GameData>(defaultFile.text);
            Debug.Log("[SaveSystem] Default data loaded from Resources.");
        }
        else
        {
            // No default file found — create blank data
            Debug.LogWarning("[SaveSystem] Default GameDatabase.json not found in Resources/Data/. Creating blank data.");
            _data = new GameData();
        }
    }
}
