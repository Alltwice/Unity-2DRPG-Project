    using System.Collections;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using Button = UnityEngine.UI.Button;

    public class LoadManager : MonoBehaviour
    {
        public static LoadManager Instance { get; private set; }
        public TextMeshProUGUI loadText;
        public GameObject loadPanel;
        public Slider loadSlider;
        private void Awake()
        {
            if(Instance!=null&&Instance!=this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
    }
        private void Start()
        {
            loadPanel.SetActive(false);
        }
        public void StartLoading(string sceneName,bool loadStop)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName,loadStop));
        }
        private IEnumerator LoadSceneCoroutine(string sceneName,bool loadStop)
        {
            loadPanel.SetActive(true);
            loadSlider.value = 0;
            loadText.text = "0%";
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = loadStop;
            while(asyncLoad.isDone==false)
            {
                float targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                loadSlider.value = targetProgress;
                loadText.text = Mathf.RoundToInt(targetProgress * 100) + "%";
                //如果不加这一句Unity主线程会被彻底堵死在这个while里，没有多余的空间时间去处理其余逻辑
                yield return null;
            }
            loadPanel.SetActive(false);
            asyncLoad = null;
        }
    }
