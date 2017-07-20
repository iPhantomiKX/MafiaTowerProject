using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechScript : MonoBehaviour {

    [Space]
    public UnitType unitType;
    public SpeechType speechType;

    [Space]
    public Image BackgroundImage;
    public Text SpeechText;

    private float original_bgimage_alpha;
    private float original_speech_alpha;

    [Space]
    public float FadeDuration = 3.0f;
    private float fade_counter = 0.0f;

    [Space]
    public float TextDuration = 5.0f;
    private float text_duration_counter = 0.0f;

    [Space]
    public float TextIntervalMin = 1.0f;
    public float TextIntervalMax = 10.0f;
    private float text_interval;
    private float text_interval_counter = 0.0f;

    private bool is_fading;
    private bool is_displaying_text;

    private SpeechDatabase speech_database;

	// Use this for initialization
	void Start ()
    {
        original_bgimage_alpha = BackgroundImage.color.a;
        original_speech_alpha = SpeechText.color.a;

        text_interval = Random.Range(TextIntervalMin, TextIntervalMax);

        speech_database = GameObject.Find("SpeechDatabase").GetComponent<SpeechDatabase>();

        BackgroundImage.gameObject.SetActive(true);

        DisplayText();
    }

    // Update is called once per frame
    void Update ()
    {
        if (is_fading)
        {
            fade_counter += Time.deltaTime;

            if (fade_counter >= FadeDuration)
            {
                fade_counter = 0.0f;
                is_fading = false;
            }
            else
                return;
        }

        if (is_displaying_text)
        {
            text_duration_counter += Time.deltaTime;

            if (text_duration_counter >= TextDuration)
            {
                BeginFade();
                text_duration_counter = 0.0f;
            }
        }

        else
        {
            text_interval_counter += Time.deltaTime;

            if(text_interval_counter >= text_interval)
            {
                Reset();
                DisplayText();
                text_interval_counter = 0.0f;
            }
        }
    }

    public void SetDisplayText()
    {
        Reset();
        DisplayText();
        is_displaying_text = true;
        is_fading = false;
        text_interval_counter = fade_counter = 0.0f;
    }

    public void SetDisplayText(SpeechType speech_type)
    {
        this.speechType = speech_type;
        Reset();
        DisplayText();
        is_displaying_text = true;
        is_fading = false;
        text_interval_counter = fade_counter = 0.0f;
    }

    public void SetDisplayText(int speech_type)
    {
        this.speechType = (SpeechType)speech_type;
        Reset();
        DisplayText();
        is_displaying_text = true;
        is_fading = false;
        text_interval_counter = fade_counter = 0.0f;
    }

    void DisplayText()
    {
        SpeechText.text = speech_database.GetRandomString(this);
        is_displaying_text = true;
    }

    void Reset()
    {
        BackgroundImage.CrossFadeAlpha(original_bgimage_alpha, 0.5f, false);
        SpeechText.CrossFadeAlpha(original_speech_alpha, 0.5f, false);
    }

    void BeginFade()
    {
        BackgroundImage.CrossFadeAlpha(0, FadeDuration, false);
        SpeechText.CrossFadeAlpha(0, FadeDuration, false);

        is_fading = true;
        is_displaying_text = false;
    }
}
