using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class LoadingNextScene : MonoBehaviour
{
    #region   注释
    //private AsyncOperation _async;

    //public Slider _slider;

    //UnityAction<float> onProgress = null;

    //private void Awake()
    //{
    //    StartCoroutine(LoadLevel("1"));
    //}

    //IEnumerator LoadLevel(string name)
    //{
    //    //Debug.LogFormat("LoadLevel: {0}", name);
    //    AsyncOperation async = SceneManager.LoadSceneAsync(name);
    //    async.allowSceneActivation = true;
    //    async.completed += LevelLoadCompleted;
    //    while (!async.isDone)
    //    {
    //        onProgress?.Invoke(async.progress);
    //        yield return null;
    //    }
    //}

    //private void LevelLoadCompleted(AsyncOperation obj)
    //{
    //    onProgress?.Invoke(1f);
    //    //Debug.Log("LevelLoadCompleted:" + obj.progress);
    //}
    #endregion

    private void Awake()
    {
        MessageCenter.Instance.Register(MessageName.SCENE_JUMP, JumpToScene);
    }

    private void JumpToScene(MessageData data)
    {
        SceneManager.LoadScene(data.valueInt.ToString());
    }

}
