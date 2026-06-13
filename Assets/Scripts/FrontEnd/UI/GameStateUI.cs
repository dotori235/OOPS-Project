using Backend;
using UnityEngine;

public class GameStateUI : MonoBehaviour, IGameStateObserver
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private UIView _prBtnTxt;
    [SerializeField] private UIView _timeScaleTxt;
    [SerializeField] private UIView _gameOverTxt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameManager.RegisterObserver(this);
        UIUpdateArgs arg = new UIUpdateArgs(1);
        _timeScaleTxt.SetValue(arg);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnGameStateChanged(IGameState state)
    {
        string s = "";
        if(state is GameOverState) _gameOverTxt.gameObject.SetActive(true);
        if (state is PlayingState) s = "Pause";
        else if (state is PausedState) s = "Resume";
        UIUpdateArgs arg = new TextUpdateArgs(s);
        _prBtnTxt.SetValue(arg);
    }

    public void OnNotify(ISubject subject)
    {

    }

    public void OnPauseResumeClick()
    {
        _gameManager.PauseResume();
    }
    public void OnRestartClick()
    {
        _gameManager.ResetGame();
    }
    public void OnTimeScaleChanged(float f)
    {
        _gameManager.SetTimeScale(f);
        float v = Mathf.Round(f*10f)/10f;
        UIUpdateArgs arg = new UIUpdateArgs(v);
        _timeScaleTxt.SetValue(arg);
    }
}
