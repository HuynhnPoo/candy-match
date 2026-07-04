using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxSlider : SliderBase
{
    float currentVolume = 0.7f;
   // private string googleAPI = "945438211481-7cb5pstsj910fmj1ub4u62pilqm3e0qo.apps.googleusercontent.com";
    protected override void Start()
    {
        base.Start();

        currentVolume = PlayerPrefs.GetFloat(StringManager.sfxSave, 0.7f);
        slider.value = SoundManager.Instance.SetSFXGame(currentVolume);
    }

    protected override void OnChange(float amount)
    {

        currentVolume = SoundManager.Instance.SetSFXGame(amount); // set sfx của game
        slider.value = currentVolume; // gán giá trị cho slider

        PlayerPrefs.SetFloat(StringManager.sfxSave, currentVolume); // lưu giá trị sfx
    }

    private void Update()
    {
        if (SoundManager.Instance.IsResseted)
        {
            slider.value = SoundManager.Instance.SetSFXGame(0.7f); // set gias tri slider vvà âm thanh
            PlayerPrefs.SetFloat(StringManager.sfxSave, currentVolume);
           
            SoundManager.Instance.IsResseted = false;
        }

    }


}
