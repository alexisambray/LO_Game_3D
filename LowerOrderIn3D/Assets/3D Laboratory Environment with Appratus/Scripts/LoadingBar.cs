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
    // // Start is called before the first frame update
    // void Start()
    // {
    //     AnimateBar();
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void AnimateBar(){
        //LeanTween.scaleZ(bar, 1, time);
        // for setting what to do after progress is complete
         LeanTween.scaleZ(bar, 1, time).setOnComplete(DisableLoadingBar);
    }

    public void AnimateBar(int manualTime){
        LeanTween.scaleZ(bar, 1, manualTime);
        // for setting what to do after progress is complete
        // LeanTween.scaleX(bar, 1, time).setOnComplete(random_func);
    }

    public void DisableLoadingBar()
    {
        this.gameObject.SetActive(false);
    }
}
