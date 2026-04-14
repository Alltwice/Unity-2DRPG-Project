using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void OnClickToGame()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.panelsStack.Clear();
        }
            Time.timeScale = 1;
        LoadManager.Instance.StartLoading("MainScene", true);
    }
    public void OnClickExit()
    {
        // 预编译指令：根据环境执行不同的退出方式
#if UNITY_EDITOR
        // 如果是在编辑器里运行，则停止播放模式
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 如果是打包出来的程序，则正常关闭
            Application.Quit();
#endif
    }
}
