using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public Slider slider;
    private AsyncOperation operation;

    private string recvName;
    private float recordTime;

    private void Awake()
    {
        recvName = DataUtility.SceneName;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Loading")
        {
            StartCoroutine(ToLoadScene(recvName));
        }
    }

    private IEnumerator ToLoadScene(string sceneName)
    {
        operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        yield return operation;
    }

    private void Update()
    {
        SetDelayLoadTime(2f);
    }

    /// <summary>
    /// 单位:秒
    /// </summary>
    /// <param name="time"></param>
    private void SetDelayLoadTime(float time)
    {
        if (operation.progress < 0.9f)
            slider.value = operation.progress * 100;
        else
            slider.value = 100;

        recordTime += Time.deltaTime;
        if (recordTime >= time && slider.value == 100)
        {
            operation.allowSceneActivation = true;
        }
    }
}
