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
        private float visualProgress;
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
            Time.timeScale = 1;
            visualProgress = 0;
            loadPanel.SetActive(true);
            loadSlider.value = 0;
            loadText.text = "0%";
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;
            while(asyncLoad.isDone==false)
            {
                float targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                visualProgress = Mathf.MoveTowards(visualProgress, targetProgress, Time.deltaTime * 0.9f);
                loadSlider.value = visualProgress;
                loadText.text = Mathf.RoundToInt(visualProgress * 100) + "%";
                if(visualProgress>=1f&&loadStop==true)
                {
                asyncLoad.allowSceneActivation = true;
                }
                //如果不加这一句Unity主线程会被彻底堵死在这个while里，没有多余的空间时间去处理其余逻辑
                yield return null;
            }
            loadPanel.SetActive(false);
            asyncLoad = null;
        }
    }
