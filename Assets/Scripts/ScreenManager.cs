using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenManager : MonoBehaviour {

    public GameObject MainMenuScreen;
    private CanvasGroup MainMenuCanvasGroup;
    public GameObject GameOverScreen;
    private CanvasGroup GameOverCanvasGroup;
    public GameObject ControlsScreen;
    private CanvasGroup ControlsCanvasGroup;

    public GameObject gameOverghostCount;
    private TextMeshProUGUI ghostCountText;
    public GameObject gameOvercashCount;
    private TextMeshProUGUI cashCountText;
    public GameObject gameOvertimeCount;
    private TextMeshProUGUI timeCountText;

    public GameObject muigi;
    private CharacterController_Luigi ccmuigi;
    public float fadeInTime = 3f;

    public GameObject spawnerGameObject;
    public Spawner spawner;

    public GameObject[] props;

    public float timePlaying = 0f;
    public bool counting = false;
    public GameObject uiTimer;
    private TextMeshProUGUI uiTimerText;

    // Use this for initialization
    void Start () {
        ghostCountText = gameOverghostCount.GetComponent<TextMeshProUGUI>();
        cashCountText = gameOvercashCount.GetComponent<TextMeshProUGUI>();
        timeCountText = gameOvertimeCount.GetComponent<TextMeshProUGUI>();
        uiTimerText = uiTimer.GetComponent<TextMeshProUGUI>();
        spawner = spawnerGameObject.GetComponent<Spawner>();
        MainMenuCanvasGroup = MainMenuScreen.GetComponent<CanvasGroup>();
        GameOverCanvasGroup = GameOverScreen.GetComponent<CanvasGroup>();
        ControlsCanvasGroup = ControlsScreen.GetComponent<CanvasGroup>();
        MainMenuScreen.SetActive(true);
        GameOverScreen.SetActive(false);
        ControlsScreen.SetActive(false);
        ccmuigi = muigi.GetComponent<CharacterController_Luigi>();
        ResetCanvas(GameOverCanvasGroup);
        ResetCanvas(ControlsCanvasGroup);
        EnableCanvas(MainMenuCanvasGroup);
    }

    void Update()
    {
        if (counting) {
            timePlaying += Time.deltaTime;

            uiTimerText.text = getTimeString(timePlaying);
        }
    }

    string getTimeString(float num){
        string minutes = Mathf.Floor(num / 60).ToString("00");
        string seconds = (num % 60).ToString("00.00");
        return string.Format("{0}:{1}", minutes, seconds);
    }

    public void play()
    {
        FadeOut(ControlsCanvasGroup);
        changePlayerState(true);
        spawner.setSpawn(true);
        counting = true;
    }

    public void showControls()
    {
        FadeOut(MainMenuCanvasGroup);
        ControlsScreen.SetActive(true);
        FadeIn(ControlsCanvasGroup);
    }

    public void replay()
    {
        timePlaying = 0;
        counting = true;
        FadeOut(GameOverCanvasGroup);
        ccmuigi.resetCharacter();
        changePlayerState(true);
        spawner.setSpawn(true);
        foreach(GameObject prop in props)
        {
            prop.GetComponent<PropInteract>().Reset();
        }
    }

    public void gameOver()
    {
        counting = false;
        GameOverScreen.SetActive(true);
        cashCountText.text = ccmuigi.cashCollected.ToString();
        ghostCountText.text = ccmuigi.ghostsCaptured.ToString();
        timeCountText.text = getTimeString(timePlaying);
        FadeIn(GameOverCanvasGroup);
        changePlayerState(false);
        spawner.deleteAll();
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
        while (canvasgroup.alpha < 1)
        {
            canvasgroup.alpha += Time.deltaTime * 2;
            yield return null;
        }
        canvasgroup.interactable = true;
        yield return null;
    }

    public void Quit()
    {
        //print("Quit");
        Application.Quit();
    }

}
