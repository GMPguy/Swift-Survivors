using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BeforeMenuScript : MonoBehaviour {
     
    // References
    GameScript GS;
    public Transform[] ShowHideAnchors;
    public Image BGimage;
    float2 BGimageLerp = new (-1f, 1f);

    // While disclaimer
    public Transform DisclaimerWindow;
    public Image DisclaimerImage;
    public Text DisclaimerText;
    public Text DisclaimerContinue;
    public Sprite[] DisclaimerImages;

    // While setup up
    public Transform SetupWindow;
    public Text[] SetupTexts;

    public ButtonScript SetupGraphicsButton;
    public Text SetupGraphicsText;
    public int GraphicsSettings;
    public ButtonScript SetupLanguageButton;
    public ButtonScript SetupContinueButton;
    public InputField SetupProfileNameButton;

    // Main variables
    public int State;
    int prevState = -1;
    float TimeSinceChange = 0f;

    void Start() {
        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
    }

    void Update() {

        if (!GS) {
            GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
            return;
        }
        
        if (prevState != State) {
            prevState = State;
            TimeSinceChange = 0f;
        }

        switch (State) {
            case 0:
                WhileDisclaimer(true);
                WhileSetup();
                break;
            case 1:
                WhileDisclaimer();
                WhileSetup();

                if (TimeSinceChange > .5f)
                    State = 2;
                break;
            case 2:
                WhileDisclaimer(true);
                WhileSetup();
                break;
            case 3:
                WhileDisclaimer();
                WhileSetup();

                if (TimeSinceChange > 1f)
                    State = 4;

                if (PlayerPrefs.HasKey("FirstLogin"))
                    BGimageLerp.y = 0f;
                break;
            case 4:
                if (PlayerPrefs.HasKey("FirstLogin")) {
                    NewMenuScript.LoadingAdditionalInfo = "";
                    GS.ChangeLevel("BackToMenu");
                }
                
                WhileDisclaimer();
                WhileSetup(true);
                break;
        }

        BGimageLerp.x = Mathf.MoveTowards(BGimageLerp.x, BGimageLerp.y, Time.deltaTime);
        BGimage.color = new (1f, 1f, 1f, Mathf.Clamp01(BGimageLerp.x));

        TimeSinceChange += Time.deltaTime;

    }

    void WhileDisclaimer (bool shown = false) {
        
        if (shown) {

            DisclaimerWindow.position = Vector3.Lerp(ShowHideAnchors[1].position, ShowHideAnchors[0].position, TimeSinceChange * 2f);
            DisclaimerText.color = DisclaimerImage.color = new (1f, 1f, 1f, Mathf.Lerp(-3f, 1f, TimeSinceChange * 2f));

            string findDisclaimerImage = State switch {
                0 => GS.Language + "-Warning",
                2 => "PlayTestWarning",
                _ => ""
            };

            if (DisclaimerImage.sprite == null || DisclaimerImage.sprite.name != findDisclaimerImage)
                for (int i = 0; i < DisclaimerImages.Length; i++)
                    if (DisclaimerImages[i].name == findDisclaimerImage) {
                        DisclaimerImage.sprite = DisclaimerImages[i];
                        break;
                    }

            DisclaimerText.text = State switch {
                0 => GS.SetString(
                    "Epilepsy – the game contains a small amount of flashing lights, which might cause seizures to some people with certain health problems\n\nImitation – this game is not realistic at all; attempting to imitate in-game behaviors in real life, might cause injuries or even death\n\nMaturity – the game should not be played by kids under 16, as it contains cartoonish violence, crude language, and use of light drugs\n\nViewer discretion is advised",
                    "Epilepsja – gra posiada małą ilość błyskotliwych świateł, które mogą wywoływać padaczki u osób, z pewnymi problemami zdrowotnymi\n\nImitacja – gra nie jest w żadnym stopniu realistyczna; próba naśladowania zachowań z gry w życiu prawdziwym, może zagrażać życiu lub zdrowiu\n\nWiek – osoby poniżej 16 roku życia nie powinny grać w tę grę, gdyż posiada przemoc, wulgarny język, oraz wykorzystanie lekkich używek\n\nGrasz na własną odpowiedzialność"
                ),
                2 => GS.SetString(
                    "You are about to participate in a public test, of the upcoming Update 1.4. The key word being, TEST, so be fully conscious of following points:\n\n- It’s still a work in progress, and it doesn’t have all of the planned content yet\n- There is still a lot of bugs, unfinished systems, and glitches\n- Your data will NOT be saved\n\nPlease report any of the encountered bugs here [TODO: INSERT DEVLOG LINK HERE]\nAdditional ideas and suggestions are welcome too!",
                    "Zaraz weźmiesz udział w publicznym teście nadchodzącego Update 1.4. Słowo klucz, TEŚCIE, więc bądź świadom poniższych uwag:\n\n- W dalszym ciągu jest to work in progress, więc nie ma jeszcze całej planowanej zawartości\n- Jest jeszcze sporo bugów, niedokończonych systemów, oraz gliczy\n- Dane I postęp z gry NIE ZOSTANĄ zapisane\n\nProszę zgłaszać wszelkie napotkane usterki tutaj [TODO: INSERT DEVLOG LINK HERE]\nDodatkowe pomysły I sugestje również będą mile widziane!"
                ),
                _ => ""  
            };

            if (TimeSinceChange > 1f) {
                DisclaimerContinue.text = GS.SetString("Press any key to continue", "Naciśnij dowolny klawisz by kontynuować");
                DisclaimerContinue.color = new (1f, 1f, 1f, Mathf.Abs(Mathf.Sin((TimeSinceChange - 1f) * Mathf.PI)));

                if (Input.anyKeyDown)
                    State += 1;
            } else
                DisclaimerContinue.color = Color.clear;

        } else if (DisclaimerWindow.position.y > ShowHideAnchors[1].position.y) {
            
            DisclaimerWindow.position = Vector3.Lerp(ShowHideAnchors[0].position, ShowHideAnchors[1].position, TimeSinceChange * 2f);
            DisclaimerText.color = DisclaimerImage.color = new (1f, 1f, 1f, Mathf.Lerp(1f, -3f, TimeSinceChange * 2f));

        }

    }

    void WhileSetup (bool shown = false) {
        
        if (shown) {

            SetupWindow.position = Vector3.Lerp(ShowHideAnchors[1].position, ShowHideAnchors[0].position, TimeSinceChange * 2f);

            foreach (Text setText in SetupTexts)
                setText.text = setText.name switch {
                    "SetupInfo" => GS.SetString(
                        "It appears, it is your first time playing the game. Let’s set up a few things first:",
                        "Wygląda na to, że grasz w to po raz pierwszy. Ustalmy parę rzeczy na szybko:"
                    ),
                    "Graphics" => GS.SetString(
                        "Graphics preset: ",
                        "Ustawienia graficzne: "
                    ),
                    "Language" => GS.SetString(
                        "Language: ",
                        "Język: "
                    ),
                    "Language2" => GS.Language,
                    "ProfileName" => GS.SetString(
                        "Profile name: ",
                        "Nazwa profilu: "
                    ),
                    "Done" => GS.SetString(
                        "Done",
                        "Gotowe"
                    ),
                    _ => ""
                };
            
            SetupGraphicsText.text = GraphicsSettings switch {
                0 => GS.SetString("Low end", "Niższe"),
                1 => GS.SetString("Mid end", "Średnie"),
                _ => GS.SetString("High end", "Wyższe")  
            };

            if (Input.GetMouseButtonDown(0)) {
                if (SetupGraphicsButton.IsSelected)
                    GraphicsSettings = (GraphicsSettings + 1) % 3;

                if (SetupLanguageButton.IsSelected)
                    GS.Language = GS.Language == "English" ? "Polski" : "English";
                
                if (SetupContinueButton.IsSelected) {
                    State = 3;
                    PlayerPrefs.SetString("FirstLogin", "yes");
                    GS.PS.Profilename = SetupProfileNameButton.text;

                    switch (GraphicsSettings) {
                        case 0:
                            GS.GraphicsQuality = 0;
                            GS.LightingQuality = 1;
                            GS.ParticlesQuality = 1;
                            GS.GrassQuality = 0;

                            GS.DestructionQuality = 0;
                            GS.EffectsQuality = 0;

                            GS.Ragdolls = false;
                            GS.SkyboxType = 0;
                            break;
                        case 1:
                            GS.GraphicsQuality = 2;
                            GS.LightingQuality = 4;
                            GS.ParticlesQuality = 5;
                            GS.GrassQuality = 2;

                            GS.DestructionQuality = 1;
                            GS.EffectsQuality = 1;
                            
                            GS.Ragdolls = true;
                            GS.SkyboxType = 1;
                            break;
                        default:
                            GS.GraphicsQuality = 4;
                            GS.LightingQuality = 4;
                            GS.ParticlesQuality = 4;
                            GS.GrassQuality = 4;

                            GS.DestructionQuality = 2;
                            GS.EffectsQuality = 2;
                            
                            GS.Ragdolls = true;
                            GS.SkyboxType = 2;
                            break;
                    }
                }

            }

        } else if (SetupWindow.position.y > ShowHideAnchors[1].position.y) {
            
            SetupWindow.position = Vector3.Lerp(ShowHideAnchors[0].position, ShowHideAnchors[1].position, TimeSinceChange * 2f);

        }

    }

}
