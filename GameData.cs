using System;
using System.Collections.Generic;

// ─────────────────────────────────────────────────────────────
// GameData.cs
// C# model classes that mirror GameDatabase.json exactly.
// SaveSystem reads/writes these classes to the JSON file.
// ─────────────────────────────────────────────────────────────

/// <summary>
/// Root object — matches the top level of GameDatabase.json
/// </summary>
[Serializable]
public class GameData
{
    public PlayerData   player    = new PlayerData();
    public List<LevelData> levels = new List<LevelData>();
    public StoryData    story     = new StoryData();
    public List<string> inventory = new List<string>();
    public SettingsData settings  = new SettingsData();
}

/// <summary>
/// Player profile — name, character choice, health, current level
/// </summary>
[Serializable]
public class PlayerData
{
    public string playerName        = "Player";
    public string selectedCharacter = "Boy";   // "Boy" or "Girl"
    public int    health            = 100;
    public int    currentLevel      = 1;
}

/// <summary>
/// One entry per level — tracks unlock and completion state
/// </summary>
[Serializable]
public class LevelData
{
    public int    levelId   = 1;
    public string sceneName = "";
    public bool   unlocked  = false;
    public bool   completed = false;
}

/// <summary>
/// Story/narrative progress
/// </summary>
[Serializable]
public class StoryData
{
    public bool introCompleted  = false;
    public int  storyProgress   = 0;
}

/// <summary>
/// Audio and display preferences
/// </summary>
[Serializable]
public class SettingsData
{
    public float musicVolume = 1.0f;
    public float soundVolume = 1.0f;
}
