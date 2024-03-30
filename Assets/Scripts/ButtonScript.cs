using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

    // Variables
    public string Effect = "";
    public bool Active = true;
    public bool IsSelected = false;
    public bool IsHovering = false;
    public float SelectScale = 1f;
    public AudioSource SelectSound;
    public AudioSource ClickSound;
    Vector3 originalScale;
    Image TheImage;
    Outline TheOut;
    // Variables

    void Start() {
        originalScale = this.transform.localScale;
        if(this.GetComponent<Image>()) TheImage = this.GetComponent<Image>();
        if(this.GetComponent<Outline>()) TheOut = this.GetComponent<Outline>();
    }

    void Update () {

        if (IsHovering == true && Active == true) {
            if (SelectSound != null && IsSelected == false) {
                SelectSound.Play();
            }
            IsSelected = true;
        } else if (IsHovering == false) {
            IsSelected = false;
        }

        if (Effect == "" && SelectScale != 1f && Active == true) {
            if (IsSelected == true) {
                this.transform.localScale = Vector3.MoveTowards(this.transform.localScale, Vector3.one * SelectScale, 0.25f * (Time.unscaledDeltaTime * 100f));
            } else {
                this.transform.localScale = Vector3.MoveTowards(this.transform.localScale, originalScale, 0.1f * (Time.unscaledDeltaTime * 100f));
            }
        } else if (Effect == "Alpha" && Active){
            if (IsSelected == true) {
                TheImage.color = Color.Lerp(TheImage.color, new Color(TheImage.color.r, TheImage.color.g, TheImage.color.b, 1f), 0.1f * (Time.unscaledDeltaTime * 100f));
            } else {
                TheImage.color = Color.Lerp(TheImage.color, new Color(TheImage.color.r, TheImage.color.g, TheImage.color.b, SelectScale), 0.1f * (Time.unscaledDeltaTime * 100f));
            }
        } else if (Effect == "Highlight" && Active){
            if (IsSelected == true) {
                TheOut.effectColor = Color.Lerp(TheOut.effectColor, Color.white/2f, 0.1f * (Time.unscaledDeltaTime * 100f));
            } else {
                TheOut.effectColor = Color.Lerp(TheOut.effectColor, Color.black/6f, 0.1f * (Time.unscaledDeltaTime * 100f));
            }
        } else if (Effect == "NMPM" && Active){
            if (IsSelected == true) {
                this.transform.GetChild(0).GetComponent<Outline>().effectColor = Color.Lerp(this.transform.GetChild(0).GetComponent<Outline>().effectColor, Color.white, 0.1f * (Time.unscaledDeltaTime * 100f));
            } else {
                this.transform.GetChild(0).GetComponent<Outline>().effectColor = Color.Lerp(this.transform.GetChild(0).GetComponent<Outline>().effectColor, Color.black/6f, 0.1f * (Time.unscaledDeltaTime * 100f));
            }
        } else if (Effect == "NMPM-B" && Active){
            if (IsSelected == true) {
                this.transform.GetComponent<Outline>().effectColor = Color.Lerp(this.transform.GetComponent<Outline>().effectColor, Color.white, 0.1f * (Time.unscaledDeltaTime * 100f));
            } else {
                this.transform.GetComponent<Outline>().effectColor = Color.Lerp(this.transform.GetComponent<Outline>().effectColor, Color.black/6f, 0.1f * (Time.unscaledDeltaTime * 100f));
            }
        }

        if (ClickSound != null && IsSelected && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))) {
            ClickSound.Play();
        }
		
	}

    public void OnPointerEnter() {
        IsHovering = true;
    }
    public void OnPointerExit() {
        IsHovering = false;
    }

}
