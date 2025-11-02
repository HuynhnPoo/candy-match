using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LoadSld : SliderBase
{
    private AsyncOperation operation;
    float timer = 0.2f;
    bool isLoadComplete = false;
    protected override void OnChange(float amount)
    {
        if (slider != null) 
        slider.value = amount;

    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        slider.value = 0;
        slider.minValue = 0;
        slider.maxValue = 1;
        StartCoroutine(LoadingSceneCorutine());
    }

    IEnumerator LoadingSceneCorutine()
    {
        if (UIManager.Instance.CurrentScene == UIManager.SceneType.LEVELMENU)
        {
            operation = UIManager.Instance.ChangeScene(UIManager.SceneType.GAMEPLAY);
        }
        else if (UIManager.Instance.CurrentScene == UIManager.SceneType.GAMEPLAY)
        {
            operation = UIManager.Instance.ChangeScene(UIManager.SceneType.MAINMENU);
        }
        if (operation != null)
        {
            operation.allowSceneActivation = false;

            StartCoroutine(IncrementLoadingBar());

            // nếu mà isloadincompete và progress ,0.9 sẽ trả về null, không thực hiện gì

            while (!isLoadComplete || operation.progress < 0.9f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(this.timer);
            operation.allowSceneActivation = true;
        }
    }

    IEnumerator IncrementLoadingBar()
    {
        for (int i = 30; i < 101; i +=5)
        {

            float progress =(float) i / 100;
            OnChange(progress);
            
            yield return new WaitForSeconds(this.timer);
        }
        isLoadComplete = true;
    }
}

