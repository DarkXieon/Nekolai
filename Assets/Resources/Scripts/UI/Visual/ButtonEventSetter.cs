using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonEventSetter : MonoBehaviour
{
    [SerializeField]
    private Button _resumeButton;

    [SerializeField]
    private Button _restartButton;

    [SerializeField]
    private Button _quitButton;

    private UnityAction _resumeAction;
    private UnityAction _restartAction;
    private UnityAction _quitAction;

    private void Start()
    {
        /*
        var uiManager = FindObjectOfType(typeof(UIManager)) as UIManager;

        _resumeAction = () => EventManager.Instance.ExecuteEvent(EventType.RESUME_ON_CLICK);
        _restartAction = () => EventManager.Instance.ExecuteEvent(EventType.RESTART_ON_CLICK);
        _quitAction = () => EventManager.Instance.ExecuteEvent(EventType.QUIT_ON_CLICK);

        EventManager.Instance.AddListener(EventType.RESUME_ON_CLICK, () => uiManager.pauseControl());
        EventManager.Instance.AddListener(EventType.RESTART_ON_CLICK, () => uiManager.Reload());
        EventManager.Instance.AddListener(EventType.QUIT_ON_CLICK, () => uiManager.LoadLevel("Main Menu"));
        */
    }

    private void OnEnable()
    {
        _resumeButton.onClick.AddListener(_resumeAction);
        _restartButton.onClick.AddListener(_restartAction);
        _quitButton.onClick.AddListener(_quitAction);
    }
}
