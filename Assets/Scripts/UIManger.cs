using NUnit.Framework;
using TMPro;
using Unity.AppUI.UI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;
using Text = Unity.AppUI.UI.Text;

public class UIManger : MonoBehaviour
{
    public InputManger input;
    public GameObject pauseUI;
    public AudioSource BGM;
    public Slider BGMSilder;
    public TMP_Text BGMText;
    public int Percent;
    private void Awake()
    {
        input = GetComponent<InputManger>();
        BGM = GameObject.Find("Cameras").GetComponent<AudioSource>();
    }
    private void Start()
    {
        BGMSilder.value = BGM.volume;
    }
    public void ChangeVolume(float value)
    {
        Percent = Mathf.RoundToInt(value*100);
        BGM.volume = BGMSilder.value;
        BGMText.text = Percent + "%";
    }
    private void Update()
    {
        if (input.isPause == true && pauseUI.activeSelf == true) 
        {
            pauseUI.SetActive(false);
            input.isPause = false;
        }
        else if(input.isPause == true && pauseUI.activeSelf == false)
        {
            pauseUI.SetActive(true);
            input.isPause = false;
        }
    }
}
