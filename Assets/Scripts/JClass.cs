using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JClass {
    List<JEntry> Values;

    //

    // Empty constructor
    public JClass () =>
        Values = new ();

    // Default constructor
    public JClass (JEntry[] newEntries) {
        Values = new ();
        Values.AddRange(newEntries);
    }

    // Basic item constructor
    public JClass (int itemID, JTemplate template) {
        Values = new ();
        SetInt(JType.ID, itemID);
        SetFloat(JType.VariableA, 0f);

        if (template == JTemplate.BasicItemStackable)
            SetInt(JType.StackQuantity, 1);
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
                    JInt getInt = (JInt)Values[c];
                    SetInt(entryType, getInt.Value);
                    break;
                case JFloat:
                    JFloat getFloat = (JFloat)Values[c];
                    SetFloat(entryType, getFloat.Value);
                    break;
                case JString:
                    JString getString = (JString)Values[c];
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

public class JEntry { 
    public JType Name; 
    public JEntry (JType type) {
        Name = type;
    }
}

public class JInt : JEntry { 
    public int Value; 
    public JInt (JType type, int value) : base (type) {
        Name = type;
        Value = value;
    }
}

public class JString : JEntry { 
    public string Value;
    public JString (JType type, string value) : base (type) {
        Name = type;
        Value = value;
    }
}

public class JFloat : JEntry { 
    public float Value;
    public JFloat (JType type, float value) : base (type) {
        Name = type;
        Value = value;
    }
}

public class JList : JEntry {
    public List<JClass> Value;
    public JList (JType type, List<JClass> value) : base (type) {
        Name = type;
        Value = value;
    }
}

public enum JType {
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
    CasualAmmo
}

public enum JTemplate {
    BasicItem,
    BasicItemStackable
}

public enum Maths {
    Set,
    Add,
    Multiply
}
