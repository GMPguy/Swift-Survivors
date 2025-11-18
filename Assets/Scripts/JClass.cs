using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JClass {
    List<JEntry> Values;

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

        JFLoat getFloat = (JFLoat)Values[fetch];
        return getFloat.Value;
    }

    public void SetFloat(JType what, float value, Maths operation = Maths.Set) {
        int fetch = FetchStruct(what);

        if (fetch == -1)
            Values.Add(new JFLoat (what, value));
        else {
            JFLoat getFloat = (JFLoat)Values[fetch];
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
                case JFLoat:
                    JFLoat getFloat = (JFLoat)Values[c];
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

public class JFLoat : JEntry { 
    public float Value;
    public JFLoat (JType type, float value) : base (type) {
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
    StraightToInvID
}

public enum Maths {
    Set,
    Add,
    Multiply
}
