using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxSlider : SliderBase
{
    float currentVolume = 0.7f;

   protected override void Start()
    {
        base.Start();

        currentVolume = PlayerPrefs.GetFloat(StringManager.sfxSave,0.7f);
        slider.value = SoundManager.Instance.SetSFXGame(currentVolume);
    }

    protected override void OnChange(float amount)
    {
        Debug.Log(" thay đổi slider"+ amount); 

        currentVolume = SoundManager.Instance.SetSFXGame(amount); // set sfx của game
        slider.value = currentVolume; // gán giá trị cho slider

        PlayerPrefs.SetFloat(StringManager.sfxSave, currentVolume); // lưu giá trị sfx
    }

    private void Update()
    {
       if(SoundManager.Instance.IsResseted)
        {
            Debug.Log("hien  thi ra thuwc hienj"+ transform.name);
        }
            
    }


}
