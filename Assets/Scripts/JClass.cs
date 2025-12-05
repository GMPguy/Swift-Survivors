using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class JClass {
    [SerializeReference]
    public List<JEntry> Values;

    //

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
                SetInt(JType.StackQuantity, 1);
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

        JInt getInt = (JInt)Values[fetch];
        return getInt.Value;
    }

    public void SetInt(JType what, int value, Maths operation = Maths.Set) {
        int fetch = FetchStruct(what);

        if (fetch == -1)
            Values.Add(new JInt (what, value));
        else {
            JInt getInt = (JInt)Values[fetch];
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

        JFloat getFloat = (JFloat)Values[fetch];
        return getFloat.Value;
    }

    public void SetFloat(JType what, float value, Maths operation = Maths.Set) {
        int fetch = FetchStruct(what);

        if (fetch == -1)
            Values.Add(new JFloat (what, value));
        else {
            JFloat getFloat = (JFloat)Values[fetch];
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

        JString getString = (JString)Values[fetch];
        return getString.Value;
    }

    public void SetString(JType what, string value) {
        int fetch = FetchStruct(what);

        if (fetch == -1)
            Values.Add(new JString (what, value));
        else {
            JString getString = (JString)Values[fetch];
            getString.Value = value;
        }
    }

    // List
    public JList GetList(JType what) {
        int fetch = FetchStruct(what);
        if (fetch == -1) return null;

        JList getList = (JList)Values[fetch];
        return getList;
    }

    public void SetList(JType what, List<JClass> value) {
        int fetch = FetchStruct(what);

        if (fetch == -1)
            Values.Add(new JList (what, value));
        else {
            JList getString = (JList)Values[fetch];
            getString.Value = new List<JClass>(value);
        }
    }

    // Default
    public void SetEntry(JType what) {
        int fetch = FetchStruct(what);

        if (fetch == -1)
            Values.Add(new JEntry (what));
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
            if (Values[f].Name == what)
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
    Attachment,
    Cacheable,
    StackQuantity,
    Color,
    ClothingCategory,
    ScanOption,
    NightVision,
    AmmoStack,
    StraightToInvID,
    Repairable,
    CasualAmmo,
    CraftingFunction,
    InteractableType,
    SpawnStuffString,
    InvEqCache_Inventory,
    InvEqCache_Equipment
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

[CustomPropertyDrawer(typeof(JEntry), true)]
public class EntryDrawer : PropertyDrawer {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.PropertyField(position, property, label, true);
    }
}

[CustomPropertyDrawer(typeof(List<JEntry>), true)]
public class EntryListDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty arrayProp = property;

        // Draw label
        position = EditorGUI.PrefixLabel(position, label);

        // Reduce indent
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Button height
        float buttonHeight = 18f;

        // Buttons row
        Rect buttonRect = new Rect(position.x, position.y, position.width, buttonHeight);

        float third = position.width / 3f;

        if (GUI.Button(new Rect(buttonRect.x, buttonRect.y, third, buttonHeight), "Int"))
            AddEntry(arrayProp, typeof(JInt));

        if (GUI.Button(new Rect(buttonRect.x + third, buttonRect.y, third, buttonHeight), "Float"))
            AddEntry(arrayProp, typeof(JFloat));

        if (GUI.Button(new Rect(buttonRect.x + third * 2, buttonRect.y, third, buttonHeight), "String"))
            AddEntry(arrayProp, typeof(JString));

        // Move down for list itself
        Rect listRect = new Rect(position.x, position.y + buttonHeight + 4, position.width,
            EditorGUI.GetPropertyHeight(arrayProp, true));
        EditorGUI.PropertyField(listRect, arrayProp, GUIContent.none, true);

        EditorGUI.indentLevel = indent;
    }

    private void AddEntry(SerializedProperty listProp, Type type)
    {
        listProp.arraySize++;
        SerializedProperty element = listProp.GetArrayElementAtIndex(listProp.arraySize - 1);

        element.managedReferenceValue = Activator.CreateInstance(type);
        listProp.serializedObject.ApplyModifiedProperties();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Buttons + list
        return 22f + EditorGUI.GetPropertyHeight(property, true);
    }
}