using UnityEngine;

public class GameManger : MonoBehaviour
{
    public GameObject PauseUI;
    private void Update()
    {
        if(PauseUI.activeSelf==true)
        {
            Time.timeScale = 0;
        }
        else if(PauseUI.activeSelf==false)
        {
            Time.timeScale = 1;
        }
    }
}
