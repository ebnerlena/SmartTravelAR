using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using System.Collections;
using System.Collections.Generic;

public class Travelling : Screen
{
#pragma warning disable 0649
    [SerializeField]
    private Image transportTypeImage;

    [SerializeField]
    private Text timerText;

    [SerializeField]
    private Text fromTo, scanCueCity;

    [SerializeField]
    private GameObject quizScreen, quizQuestionScreen, quizSolutionScreen, nextQuestionBtn, scanMarkerButton;

    [SerializeField]
    private Text description, question, options, correct;

#pragma warning restore 0649

    public int initImageSearchAtSecondsRemain;

    private bool minigameSuccess;
    private bool minigamePlaying;

    private ImageRecognizer imageRecognizer;
    private string searchName;

    private readonly string eventGroupName = "travelling";
    private TimeEventGroup eventGroup;

    private bool arrivedInTime;
    private float t;
    private float timerUpdate = 0.25f;
    private Question currentQuestion;

    void Start()
    {
        imageRecognizer = FindObjectOfType<ImageRecognizer>(); 
    }

    void Update()
    {
        t += Time.deltaTime;
        if (t > timerUpdate && eventGroup != null)
        {
            timerText.text = eventGroup.GetRemainingSeconds().ToString("00");
        }
    }

    //called when transport model is pressed
    public void CityCheckIn()
    {
        if (MobileOnlyActivator.IsMobile)
            imageRecognizer.enabled = false;

        if (GameManager.Instance.Player.Trip.IsCompleted())
            GameManager.Instance.GameOver();
        else
        {
            GameManager.Instance.Status = GameStatus.InCity;

            // if (!minigameSuccess || !arrivedInTime)
            //     GameManager.Instance.SendCancelTripUpdate();
        }
    }

    public override void Show()
    {
        base.Show();
        arrivedInTime = true;
        imageRecognizer = FindObjectOfType<ImageRecognizer>();
        Trip trip = GameManager.Instance.Player.Trip;
        fromTo.text = trip.CurrentCity.Name + " - " + trip.CurrentTransport.Option.To.Name;
        LoadTransportIcon();
        scanMarkerButton.SetActive(false);

        SetupEventGroup();
        eventGroup.Start();

        if (eventGroup.GetRemainingSeconds() > 25)
        {
            //MinigameController.Instance.LoadGame();
            quizScreen.SetActive(true);
            LoadQuestion();
            minigameSuccess = false;
            minigamePlaying = false;
        }
        else
        {
            minigamePlaying = false;
            minigameSuccess = true;
        }
    }

    // called by button instead of scanning correct image
    public void ArrivedInCityManually()
    {
        GameManager.Instance.Player.Trip.ArrivedInTime();
        MinigameController.Instance.StopCurrentGame();
        TimeManager.Instance.CancelEventGroup(eventGroupName);

        CityCheckIn();
    }

    //OnImageFound
    public void ArrivedInSearchedCity()
    {
        if(TimeManager.Instance.Find(eventGroupName) != null)
        {
            arrivedInTime = eventGroup.GetRemainingSeconds() >= 0;
            if ((arrivedInTime && minigameSuccess) || (arrivedInTime && !minigamePlaying))
                GameManager.Instance.Player.Trip.ArrivedInTime();
            else
            {
                GameManager.Instance.Player.Trip.ArrivedNotInTime();
                GameManager.Instance.SetErrorMessage(ErrorMessageType.TravellingError , "Aufenthalt ohne Wissenspunkte");
            }
            TimeManager.Instance.CancelEventGroup(eventGroupName);
        }
    }

    //prepare for imagesearching
    private void On10SecondsRemain()
    {
        if (MobileOnlyActivator.IsMobile)
        {
            Handheld.Vibrate();
            searchName = GameManager.Instance.Player.Trip.CurrentTransport.To.City.Name;
            scanCueCity.text = searchName;
            imageRecognizer.enabled = true;
            imageRecognizer.SearchForImage(searchName, ArrivedInSearchedCity);
        }
        
        if (minigamePlaying)
        {
            MinigameController.Instance.StopCurrentGame();
            minigameSuccess = MinigameController.Instance.GetSuccess();    
        }

        scanMarkerButton.SetActive(true);  
        quizScreen.SetActive(false);
    }

    //end of travelling time - chancel trip, back to start city
    private void OnAbsoluteEnd()
    {
        quizScreen.SetActive(false);
        GameManager.Instance.Player.Trip.ArrivedNotInTime();

        if (MobileOnlyActivator.IsMobile)
        {
            Handheld.Vibrate();
            searchName = GameManager.Instance.Player.Trip.CurrentCity.Name;
            scanCueCity.text = searchName;
            imageRecognizer.enabled = true;
            imageRecognizer.SearchForImage(searchName, ArrivedInSearchedCity);
        }
        
        GameManager.Instance.SetErrorMessage(ErrorMessageType.TravellingError, "zu langsam...");
    }

    private void SetupEventGroup()
    {
        TimeManager.Instance.CancelEventGroup(eventGroupName);

        eventGroup = new TimeEventGroup(eventGroupName, GameManager.Instance.Player.Trip.CurrentTransport.MaxTimeInSeconds);
        //eventGroup = new TimeEventGroup(eventGroupName, 40);
        TimeEvent endEvent = new TimeEvent("absoluteEnd", OnAbsoluteEnd, 0f, true);
        TimeEvent tenSecRemainEvent = new TimeEvent("tenSecRemain", On10SecondsRemain, Mathf.Max(5f, initImageSearchAtSecondsRemain), true);
        eventGroup.RegisterEvent(endEvent);
        eventGroup.RegisterEvent(tenSecRemainEvent);

        TimeManager.Instance.RegisterEventGroup(eventGroup);
    }

    private void LoadTransportIcon()
    {
        TransportType type = GameManager.Instance.Player.Trip.CurrentTransport.Option.TransportType;
        Sprite icon = ResourceLoader.TransportType(type);
        if (icon != null)
            transportTypeImage.sprite = icon;
    }

    public override void Hide()
    {
        base.Hide();
        EmptyFields();
        TimeManager.Instance.CancelEventGroup(eventGroupName);
        quizScreen.SetActive(false);
    }

    private void LoadQuestion()
    {
        currentQuestion = QuizManager.NextQuestion();
        this.question.text = currentQuestion.question;
        this.options.text = currentQuestion.GetOptionsText();
        this.quizQuestionScreen.SetActive(true);
        this.quizSolutionScreen.SetActive(false);
    }

    public void AnswerQuestion(string answer)
    {
        bool answeredCorrect = currentQuestion.Answer(answer[0]);
        if(answeredCorrect)
        {
            GameManager.Instance.Player.AnsweredQuestionCorrect();
            //minigameSuccess = true;
            this.correct.text =$"{answer[0]} ist die richtige Antwort! Super!";
        }
        else {
            this.correct.text =$"{answer[0]} ist leider falsch";
        }

        ShowSolution(answeredCorrect);
    }

    public void NextQuestion()
    {
        LoadQuestion(); 
    }

    private void ShowSolution(bool correct)
    {
        this.description.text = currentQuestion.description;
        this.quizQuestionScreen.SetActive(false);
        this.quizSolutionScreen.SetActive(true);

        if (eventGroup.GetRemainingSeconds() > 30)
        {   
            StartCoroutine(Wait());
        }
        else {
            nextQuestionBtn.SetActive(false);
        }
    }

    private void EmptyFields()
    {
        this.description.text = "";
        this.question.text = "";
        this.options.text = "";
        this.correct.text= "";
    }

    IEnumerator Wait()
    {
        nextQuestionBtn.SetActive(false);
        yield return new WaitForSeconds(5);
        nextQuestionBtn.SetActive(true);
    }

}
