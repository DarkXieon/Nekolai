using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenuContinueButton : MonoBehaviour
{
    private void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => EventManager.Instance.ExecuteEvent(EventType.RESUME_ON_CLICK));
    }
}
