using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JudgeAnimetion : MonoBehaviour
{
    const string _InGameScene = "InGameScene";

    void Start()
    {
        if (SceneManager.GetActiveScene().name == _InGameScene)
        {
            transform.DOMoveY(transform.position.y + 30.0f, 0.3f);
        }
    }

}
