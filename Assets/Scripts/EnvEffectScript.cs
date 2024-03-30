using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvEffectScript : MonoBehaviour {

    // Variables
    public string EffectName = "";
    public float Lifetime = 0f;
    public float Epicentrum = 0f;
    public float Size = 1f;
    public Color32 EffectColor;
    // Misc
    float AcidPulse = 0f;
    // Misc
    // Variables
    // References
    public Image[] Splashes;
    public Image[] NormalSplashes;
    public GameObject Droplet;
    int PickSplash = 0;
    // References

	// Use this for initialization
	void Start () {

        if (QualitySettings.GetQualityLevel() == 0) {
            Destroy(this.gameObject);
        }

        this.transform.SetParent(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().EnvironmentalWindow.transform);
        this.transform.localScale = Vector3.one;

        // Quality Removal
        if (QualitySettings.GetQualityLevel() <= 0 && (EffectName == "BloodSplash")) {
            Destroy(this.gameObject);
        }

        if (EffectName == "BloodSplash") {
            Lifetime = 5f * (1f - (Epicentrum / 5f));
            PickSplash = (int)Random.Range(0f, 2.9f);
            Splashes[PickSplash].gameObject.SetActive(true);
            Splashes[PickSplash].GetComponent<Image>().color = new Color32((byte)Random.Range(100f, 255f), 0, 0, (byte)Random.Range(55f, 255f));
            int HorOrVer = (int)Random.Range(0f, 1.9f);
            if (HorOrVer == 0) {
                this.GetComponent<RectTransform>().position = new Vector3(Random.Range(0f, 1f) * Screen.width, (int)Random.Range(0f, 1.9f) * Screen.height, 0f);
            } else {
                this.GetComponent<RectTransform>().position = new Vector3((int)Random.Range(0f, 1.9f) * Screen.width, Random.Range(0f, 1f) * Screen.height, 0f);
            }
            this.GetComponent<RectTransform>().position += new Vector3(Random.Range(-64f, 64f), Random.Range(-64f, 64f), 0f);
            Splashes[PickSplash].GetComponent<RectTransform>().sizeDelta = Vector2.one * Random.Range(250f, 500f);
            this.GetComponent<RectTransform>().Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
        } else if (EffectName == "AcidSplash") {
            Lifetime = 3f;
            PickSplash = (int)Random.Range(0f, 2.9f);
            NormalSplashes[PickSplash].gameObject.SetActive(true);
            NormalSplashes[PickSplash].GetComponent<Image>().color = new Color32(125, 255, 0, 255);
            this.GetComponent<RectTransform>().position = new Vector3(Random.Range(0f, 1f) * Screen.width, Random.Range(0f, 1f) * Screen.height, 0f);
            NormalSplashes[PickSplash].GetComponent<RectTransform>().sizeDelta = Vector2.one * Random.Range(250f, 500f);
            this.GetComponent<RectTransform>().Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
        } else if (EffectName == "Dirt") {
            Lifetime = 3f;
            NormalSplashes[2].gameObject.SetActive(true);
            NormalSplashes[2].GetComponent<Image>().color = new Color32(EffectColor.r, EffectColor.g, EffectColor.b, (byte)(Epicentrum * 255f));
            this.GetComponent<RectTransform>().position = new Vector3(Random.Range(0f, 1f) * Screen.width, Random.Range(0f, 1f) * Screen.height, 0f);
            NormalSplashes[2].GetComponent<RectTransform>().sizeDelta = Vector2.one * Size;
            this.GetComponent<RectTransform>().Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
            if (255 * Epicentrum < 25) {
                Destroy(this.gameObject);
            }
        } else if (EffectName == "WaterDrop") {
            Lifetime = 2f;
            Droplet.gameObject.SetActive(true);
            Droplet.GetComponent<Image>().color = EffectColor;
            this.GetComponent<RectTransform>().position = new Vector3(Random.Range(0f, 1f) * Screen.width, Random.Range(0f, 1f) * Screen.height, 0f);
            Droplet.GetComponent<RectTransform>().sizeDelta = Vector2.one * 24f;
        }
		
	}
	
	// Update is called once per frame
	void Update () {

        if (EffectName == "BloodSplash") {
            if (Lifetime > 0f) {
                Lifetime -= (0.01f * Mathf.Lerp(3f, 1f, Mathf.Clamp(QualitySettings.GetQualityLevel() / 2f, 0f, 1f))) * (Time.deltaTime * 50f);
                Splashes[PickSplash].GetComponent<Image>().color = new Color(Splashes[PickSplash].GetComponent<Image>().color.r, Splashes[PickSplash].GetComponent<Image>().color.g, Splashes[PickSplash].GetComponent<Image>().color.b, Lifetime / 5f);
            } else {
                Destroy(this.gameObject);
            }
        } else if (EffectName == "AcidSplash") {
            if (Lifetime > 0f) {
                Lifetime -= (0.01f * Mathf.Lerp(3f, 1f, Mathf.Clamp(QualitySettings.GetQualityLevel() / 2f, 0f, 1f))) * (Time.deltaTime * 50f);
                NormalSplashes[PickSplash].GetComponent<Image>().color = new Color(NormalSplashes[PickSplash].GetComponent<Image>().color.r, NormalSplashes[PickSplash].GetComponent<Image>().color.g, NormalSplashes[PickSplash].GetComponent<Image>().color.b, Lifetime / 3f);
                if (AcidPulse > 0f) {
                    AcidPulse -= 0.01f;
                } else {
                    Lifetime += 0.1f;
                    AcidPulse = 0.5f;
                }
            } else {
                Destroy(this.gameObject);
            }
        } else if (EffectName == "Dirt") {
            if (Lifetime > 0f) {
                Lifetime -= (0.01f * Mathf.Lerp(3f, 1f, Mathf.Clamp(QualitySettings.GetQualityLevel() / 3f, 0f, 1f))) * (Time.deltaTime * 50f);
                NormalSplashes[2].GetComponent<Image>().color = new Color(NormalSplashes[2].GetComponent<Image>().color.r, NormalSplashes[2].GetComponent<Image>().color.g, NormalSplashes[2].GetComponent<Image>().color.b, (Lifetime / 3f) * Epicentrum);
            } else {
                Destroy(this.gameObject);
            }
        } else if (EffectName == "WaterDrop") {
            if (Lifetime > 0f) {
                Lifetime -= (0.01f * Mathf.Lerp(3f, 1f, Mathf.Clamp(QualitySettings.GetQualityLevel() / 3f, 0f, 1f))) * (Time.deltaTime * 50f);
                Droplet.GetComponent<Image>().color = new Color(Droplet.GetComponent<Image>().color.r, Droplet.GetComponent<Image>().color.g, Droplet.GetComponent<Image>().color.b, Lifetime / 2f);
                Droplet.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0f, 0.32f) * (Time.deltaTime * 100f);
            } else {
                Destroy(this.gameObject);
            }
        }
		
	}
}
