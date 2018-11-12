using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenuQuitButton : MonoBehaviour
{
    private void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => EventManager.Instance.ExecuteEvent(EventType.QUIT_ON_CLICK));
    }
}
