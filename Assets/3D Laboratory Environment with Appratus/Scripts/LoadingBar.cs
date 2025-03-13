using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using System;
public class LoadingBar : MonoBehaviour
{
    public GameObject bar;
    public  int time;
    public int days;
    private Action onCompleteCallback;

    public void AnimateBar(){
        //LeanTween.scaleZ(bar, 1, time);
        // for setting what to do after progress is complete
         LeanTween.scaleZ(bar, 1, time).setOnComplete(DisableLoadingBar);
    }

    public void AnimateBar(int manualTime, Action callback = null){

        onCompleteCallback = callback;

        bar.transform.localScale = new Vector3(bar.transform.localScale.x, bar.transform.localScale.y, 0); // Reset Z scale
        LeanTween.scaleZ(bar, 1, manualTime).setOnComplete(OnAnimationComplete);
        // for setting what to do after progress is complete
        // LeanTween.scaleX(bar, 1, time).setOnComplete(random_func);
    }

    public void OnAnimationComplete()
    {
        Debug.Log("OnAnimationComplete() called!");
        this.gameObject.SetActive(false);

        if (onCompleteCallback != null)
    {
        Debug.Log("Executing callback...");
        onCompleteCallback.Invoke();
    }
    else
    {
        Debug.LogWarning("onCompleteCallback is NULL!");
    }
    }
    
    public void DisableLoadingBar()
    {
        this.gameObject.SetActive(false);
    }
}
