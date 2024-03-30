using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSliderScript : MonoBehaviour {
    
    // Variables
    public bool Active = true;
    public float Current = 0f;
    public int Selected;
    public float MaxA, MaxB = 1; // A is the displayed amount (preferably 1), B is the maximum amount of entries
    float PrevMax = -1;
    public int Size = 32;
    public float SliderSizer = 0f;
    public int MinSize = 5;
    public Vector2 Axis;

    // References
    public ButtonScript UpButton, DownButton, Slider;
    bool Held = false;

    void Start(){

        if(UpButton && DownButton){
            UpButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, Size/2f + UpButton.GetComponent<RectTransform>().sizeDelta.y/2f);
            DownButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, Size/-2f - UpButton.GetComponent<RectTransform>().sizeDelta.y/2f);
        }

    }

    void Update(){

        // SettingSize
        if(PrevMax != MaxA+MaxB){
            PrevMax = MaxA+MaxB;

            Vector2 SliderSize = Slider.GetComponent<RectTransform>().sizeDelta;
            if(MaxA >= MaxB) SliderSizer = Size;
            else SliderSizer = Mathf.Lerp(MinSize, Size, MaxA/MaxB);
            SliderSize.y = SliderSizer;
            Slider.GetComponent<RectTransform>().sizeDelta = SliderSize;
        }

        // Scrolling
        if(UpButton && UpButton.IsSelected && Input.GetMouseButtonDown(0)) Current = Mathf.Clamp(Current - MaxA/MaxB, 0, 1f);
        else if(DownButton && DownButton.IsSelected && Input.GetMouseButtonDown(0)) Current = Mathf.Clamp(Current + MaxA/MaxB, 0, 1f);

        Slider.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, Mathf.Lerp( (Size/2f)-(SliderSizer/2f) , (Size/-2f)+(SliderSizer/2f) , Current ) );

        if(Slider.IsSelected && Input.GetMouseButtonDown(0) && !Held && Active){
            Held = true;
        } else if((!Input.GetMouseButton(0) && Held) || (!Active && Held)){
            Held = false;
        }

        if(Held){
            float Force = (Input.GetAxis("Mouse X") * Axis.x + -Input.GetAxis("Mouse Y") * Axis.y)/5f;
            Current = Mathf.Clamp(Current + Force, 0f, 1f);
        }

        Selected = (int)Mathf.Lerp(0, MaxB, Current);

    }

}
