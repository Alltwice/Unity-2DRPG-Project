using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public void OpenPauseUI()
    {
        if (pauseUI == null)
        {
            Debug.LogWarning("[UIManger] pauseUI 未绑定。");
            return;
        }
        bool newState = !pauseUI.activeSelf;
        pauseUI.SetActive(newState);
        Debug.Log("[UIManger] Pause UI toggled -> " + newState);
    }
}
