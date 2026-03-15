using System.Collections;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    //单例设置便于其他地方获取
    public static HitStopManager Instance { get; private set; }
    //这是一个存储携程的容器
    public Coroutine stopRoutine;
    //Awake中确保了单例的方法

    private void Awake()
    {
        if(Instance!=null&&Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    //这里通过外部调用

    public void HitStop(float stunTime)
    {
        if(stunTime<=0)
        {
            return;
        }
        //有携程优先清除防止出错
        if (stopRoutine != null)
        {
            StopCoroutine(stopRoutine);
        }
        //携程开启！
        stopRoutine = StartCoroutine(DoHitStop(stunTime));
    }
    //携程的具体方法
    private IEnumerator DoHitStop(float stunTime)
    {
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0;
        //代码执行到这一行停stunTime时间
        yield return new WaitForSecondsRealtime(stunTime);
        Time.timeScale = originalTimeScale;
        stopRoutine = null;
    }
}