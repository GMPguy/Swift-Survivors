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
    float[] SelectedLerp = {0f, -1};
    public AudioSource SelectSound;
    public AudioSource ClickSound;
    Vector3 originalScale;
    Image TheImage;
    Color OrgColor;
    Outline TheOut;
    // Variables

    void Start() {
        originalScale = this.transform.localScale;
        if(this.GetComponent<Image>()) {
            TheImage = this.GetComponent<Image>();
            OrgColor = TheImage.color;
        }
        if(this.GetComponent<Outline>()) TheOut = this.GetComponent<Outline>();
        if(Effect == "NMPM-B") TheOut = this.transform.GetComponent<Outline>();
        else if(Effect == "NMPM") TheOut = this.transform.GetChild(0).GetComponent<Outline>();

        if(TheOut) OrgColor = TheOut.effectColor;
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

        if(Active && IsSelected) SelectedLerp[0] = Mathf.MoveTowards(SelectedLerp[0], 1f, Time.unscaledDeltaTime*10f);
        else SelectedLerp[0] = Mathf.MoveTowards(SelectedLerp[0], 0f, Time.unscaledDeltaTime*10f);

        if(Active && SelectedLerp[0] != SelectedLerp[1]){
            SelectedLerp[1] = SelectedLerp[0];
            switch(Effect){
                case "Alpha": TheImage.color = new Color(TheImage.color.r, TheImage.color.g, TheImage.color.b, Mathf.Lerp(0.5f, 1f, SelectedLerp[0])); break;
                case "Highlight": case "NMPM": case "NMPM-B": TheOut.effectColor = Color.Lerp(OrgColor, Color.white*0.8f, SelectedLerp[0]); break;
                default: this.transform.localScale = Vector3.Lerp(originalScale, originalScale*SelectScale, SelectedLerp[0]); break;
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
