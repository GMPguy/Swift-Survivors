using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class JClass {
    [SerializeReferenceDropdown]
    [SerializeReference]
    public List<JEntry> Values;

    // Empty constructor
    public JClass () =>
        Values = new ();

    // Single constructor
    public JClass (JEntry newEntry) {
        Values = new ();
        Values.Add(newEntry);
    }

    // Default constructor
    public JClass (JEntry[] newEntries) {
        Values = new ();
        Values.AddRange(newEntries);
    }

    // ID based constructor
    public JClass (int itemID, JTemplate template) {
        Values = new ();
        SetInt(JType.ID, itemID);

        switch (template) {
            case JTemplate.BasicItem:
                SetFloat(JType.VariableA, 0f);
                break;
            case JTemplate.BasicItemStackable:
                SetFloat(JType.VariableA, 0f);
                SetInt(JType.Item_StackQuantity, 1);
                break;
        }
    }

    // Construct from
    public JClass (JClass target) {
        Values = new ();
        CopyFrom(target);
    }

    // Adding and manipulating entries

    // Ints
    public int GetInt(JType what) {
        int fetch = FetchStruct(what);
        if (fetch == -1) return 0;

        return Values[fetch] is JInt getInt ? getInt.Value : 0;
    }

    public void SetInt(JType what, int value, Maths operation = Maths.Set) {
        int fetch = FetchStruct(what);

        if (fetch == -1)
            Values.Add(new JInt (what, value));
        else {
            if (Values[fetch] is not JInt getInt) {
                Values[fetch] = new JInt(what, value);
                return;
            }
            getInt.Value = operation switch{
                Maths.Add => getInt.Value + value,
                Maths.Multiply => getInt.Value * value,
                _ => value
            };
        }
    }

    // Float
    public float GetFloat(JType what) {
        int fetch = FetchStruct(what);
        if (fetch == -1) return 0;

        return Values[fetch] is JFloat getFloat ? getFloat.Value : 0f;
    }

    public void SetFloat(JType what, float value, Maths operation = Maths.Set) {
        int fetch = FetchStruct(what);

        if (fetch == -1)
            Values.Add(new JFloat (what, value));
        else {
            if (Values[fetch] is not JFloat getFloat) {
                Values[fetch] = new JFloat(what, value);
                return;
            }
            getFloat.Value = operation switch{
                Maths.Add => getFloat.Value + value,
                Maths.Multiply => getFloat.Value * value,
                _ => value
            };
        }
    }

    // String
    public string GetString(JType what) {
        int fetch = FetchStruct(what);
        if (fetch == -1) return "";

        return Values[fetch] is JString getString ? getString.Value : "";
    }

    public void SetString(JType what, string value) {
        int fetch = FetchStruct(what);

        if (fetch == -1)
            Values.Add(new JString (what, value));
        else {
            if (Values[fetch] is JString getString)
                getString.Value = value;
            else
                Values[fetch] = new JString(what, value);
        }
    }

    // List
    public JList GetList(JType what) {
        int fetch = FetchStruct(what);
        if (fetch == -1) return null;

        return Values[fetch] is JList getList ? getList : null;
    }

    public void SetList(JType what, List<JClass> value) {
        int fetch = FetchStruct(what);

        if (fetch == -1)
            Values.Add(new JList (what, value));
        else {
            if (Values[fetch] is JList getList)
                getList.Value = new List<JClass>(value);
            else
                Values[fetch] = new JList(what, value);
        }
    }

    // Default
    public void SetEntry(JType what) {
        int fetch = FetchStruct(what);

        if (fetch == -1)
            Values.Add(new JTag (what));
    }

    // Misc functions
    public bool Exists(JType what) =>
        FetchStruct(what) != -1;

    public void Remove(JType what) {
        int fetch = FetchStruct(what);

        if (fetch != -1)
            Values.RemoveAt(fetch);
    }

    int FetchStruct(JType what) {
        for (int f = 0; f < Values.Count; f++)
            if (Values[f] != null && Values[f].Name == what)
                return f;
        
        return -1;
    }

    public void CopyFrom(JClass where) {
        
        Values = new ();
        for (int c = 0; c < where.Values.Count; c++) {
            JType entryType = where.Values[c].Name;

            switch (where.Values[c]) {
                case JInt:
                    JInt getInt = (JInt)where.Values[c];
                    SetInt(entryType, getInt.Value);
                    break;
                case JFloat:
                    JFloat getFloat = (JFloat)where.Values[c];
                    SetFloat(entryType, getFloat.Value);
                    break;
                case JString:
                    JString getString = (JString)where.Values[c];
                    SetString(entryType, getString.Value);
                    break;
                default:
                    SetEntry(entryType);
                    break;
            }
        }

    }

    public bool CompareTo(JClass to) {
        
        for (int c = 0; c < Values.Count; c++)
            if (!to.Exists(Values[c].Name))
                return false;

        return true;

    }

}

[Serializable]
public class JEntry { 
    public JType Name;
    public JEntry (JType type) {
        Name = type;
    }
}

[Serializable]
public class JTag : JEntry { 
    public JTag (JType type) : base (type) {
        Name = type;
    }
}

[Serializable]
public class JInt : JEntry { 
    public int Value; 
    public JInt (JType type, int value) : base (type) {
        Name = type;
        Value = value;
    }
}

[Serializable]
public class JString : JEntry { 
    public string Value;
    public JString (JType type, string value) : base (type) {
        Name = type;
        Value = value;
    }
}

[Serializable]
public class JFloat : JEntry { 
    public float Value;
    public JFloat (JType type, float value) : base (type) {
        Name = type;
        Value = value;
    }
}

[Serializable]
public class JList : JEntry {
    public List<JClass> Value;
    public JList (JType type, List<JClass> value) : base (type) {
        Name = type;
        Value = new List<JClass>(value);
    }
}

public enum JType {
    none,
    ID,
    VariableA,
    Item_Attachment,
    Item_Cacheable,
    Item_StackQuantity,
    Item_Color,
    Item_ClothingCategory,
    Item_ScanOption,
    Item_NightVision,
    Item_AmmoStack,
    StraightToInvID,
    Item_Repairable,
    Item_CasualAmmo,
    Item_CraftingFunction,
    Interactable_Type,
    Spawn_String,
    InvEqCache_Inventory,
    InvEqCache_Equipment,
    RoundSettings_GameMode,
    RoundSettings_Difficulty,
    RoundSettings_ProfileDependance,
    RoundSettings_HordeMap,
    RoundSettings_Score,
    RoundSettings_Round,
    RoundSettings_FileName,
    Achievement_Data,
    Achievement_Name,
    SaveAmmo,
    Amount,
    Biome,
    Buffs,
    Options_CameraBobbing,
    Options_CameraShifting,
    Clothes,
    ClothingBody,
    ClothingHair,
    ClothingHat,
    ClothingSkin,
    Description,
    Options_DestructionQuality,
    Options_EarPiercing,
    Options_EffectsQuality,
    Experience,
    Options_KeybindFunction,
    Options_FoliageQuality,
    Options_FieldOfView,
    Options_FrameRateCap,
    FileSeed,
    Options_CameraFieldOfView,
    Options_GraphicsQuality,
    AttackGunSpread,
    HungerCurrent,
    HungerMaximum,
    HighestScore,
    Hints,
    Options_HoloSight,
    Options_HudColor,
    Options_Hue,
    Hunger,
    Identifier,
    ImageMode,
    InventoryClothes,
    AttackInventory,
    InventoryInUse,
    InventoryKits,
    InventoryMisc,
    HealthCurrent,
    HealthMaximum,
    Options_Language,
    Options_LaserColor,
    Level,
    Options_LightingQuality,
    LongestSurvivedTime,
    Options_MasterVolume,
    MeleeDurability,
    MaxInventory,
    Options_InvertedMouse,
    Money,
    MostCasualRounds,
    MostRounds,
    MostWaves,
    ItemRemoval,
    Options_MouseSensitivity,
    Options_MouseSmoothness,
    Options_MusicVolume,
    RecordName,
    Options_ParticlesQuality,
    PlayerInventory,
    ProfileName,
    AttackPower,
    PlayerSpeed,
    RoundPunishment,
    PlaythroughStats,
    PlayerWet,
    RoundReward,
    Options_Ragdolls,
    RecordDate,
    Records,
    RoundNumber,
    RoundSettings,
    ScoreSave,
    BlackSceneColor,
    SkyboxType,
    SaveName,
    SaveScore,
    SoundVolume,
    PopupSource,
    RoundsSeed,
    Statistics,
    RecordTime,
    Title,
    TotalCasualRounds,
    TotalRounds,
    TotalScore,
    TotalWaves,
    PopupType,
    UiResolutionX,
    UiResolutionY,
    VariableAStat,
    AchievementCarp,
    AchievementGarbage,
    VictoryImage,
    AchievementNpc,
    AchievementCompleted
    ,RoundPunishment_ItemLost
    ,RoundPunishment_Tired
    ,RoundPunishment_Wet
    ,RoundPunishment_Damaged
    ,RoundPunishment_NoAmmo
    ,RoundPunishment_Dirty
    ,RoundReward_Item
    ,RoundReward_Healed
    ,RoundReward_Adrenaline
    ,RoundReward_Treasure
    ,RoundReward_Drunk
    ,RoundReward_Money
    ,RoundScore_Hunger
    ,RoundScore_Stats_SurvivedTime
    ,RoundScore_Stats_TradeBuy
    ,RoundScore_Stats_TradeSell
    ,RoundScore_Stats_TreasuresSold
    ,RoundScore_Stats_ChestsOpened
    ,RoundScore_Stats_ChestsDestroyed
    ,RoundScore_Stats_ObjectsDestroyed
    ,RoundScore_Stats_MapDiscovered
    ,RoundScore_Stats_Damage
    ,RoundScore_Stats_Killed
    ,RoundScore_Stats_KillMutant
    ,RoundScore_Stats_KillBandit
    ,RoundScore_Stats_KillSurvivor
    ,RoundScore_Stats_KillGuard
    ,RoundScore_Stats_PickedLocks
    ,RoundScore_Stats_ItemsFound
    ,RoundScore_Stats_TreasuresFound
    ,RoundScore_Stats_ItemsUnderwaterFound
    ,Stats_TotalRounds
    ,Stats_TotalCasualRounds
    ,Stats_TotalWaves
    ,Stats_TotalScore
    ,Stats_HighestScore
    ,Stats_MostRounds
    ,Stats_MostCasualRounds
    ,Stats_MostWaves
    ,Stats_LongestSurvivedTime
}

public enum JTemplate {
    JustID,
    BasicItem,
    BasicItemStackable
}

public enum Maths {
    Set,
    Add,
    Multiply
}