﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {

    public GameObject MainMenuScreen;
    private CanvasGroup MainMenuCanvasGroup;
    public GameObject GameOverScreen;
    private CanvasGroup GameOverCanvasGroup;

    public GameObject muigi;
    private CharacterController_Luigi ccmuigi;
    public float fadeInTime = 3f;
	// Use this for initialization
	void Start () {
        MainMenuCanvasGroup = MainMenuScreen.GetComponent<CanvasGroup>();
        GameOverCanvasGroup = GameOverScreen.GetComponent<CanvasGroup>();
        MainMenuScreen.SetActive(true);
        GameOverScreen.SetActive(false);
        ccmuigi = muigi.GetComponent<CharacterController_Luigi>();
        ResetCanvas(GameOverCanvasGroup);
        EnableCanvas(MainMenuCanvasGroup);
    }

    public void play()
    {
        FadeOut(MainMenuCanvasGroup);
        changePlayerState(true);
    }

    public void replay()
    {
        FadeOut(GameOverCanvasGroup);
        ccmuigi.resetCharacter();
        changePlayerState(true);
    }

    public void gameOver()
    {
        FadeIn(GameOverCanvasGroup);
        changePlayerState(false);
    }


    public void FadeOut(CanvasGroup canvasgroup)
    {
        StartCoroutine(FadeOutCor(canvasgroup));
    }

    public void FadeIn(CanvasGroup canvasgroup)
    {
        StartCoroutine(FadeInCor(canvasgroup));
    }

    public void ResetCanvas(CanvasGroup canvasgroup)
    {
        canvasgroup.alpha = 0;
        canvasgroup.interactable = false;
    }

    public void EnableCanvas(CanvasGroup canvasgroup)
    {
        canvasgroup.alpha = 1;
        canvasgroup.interactable = true;
    }

    public void changePlayerState(bool state)
    {
        ccmuigi.controlsEnabled = state;
    }

    IEnumerator FadeOutCor(CanvasGroup canvasgroup)
    {
        while(canvasgroup.alpha > 0)
        {
            canvasgroup.alpha -= Time.deltaTime / 2;
            yield return null;
        }
        canvasgroup.interactable = false;
        yield return null;
    }

    IEnumerator FadeInCor(CanvasGroup canvasgroup)
    {
        while (canvasgroup.alpha < 255)
        {
            canvasgroup.alpha += Time.deltaTime / 2;
            yield return null;
        }
        canvasgroup.interactable = true;
        yield return null;
    }

}
