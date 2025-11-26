using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Fade : MonoBehaviour
{
    public static Fade Instance;

    private GameObject FadeObject;
    private Image FadeImage;

    [SerializeField]
    public float FadeTime = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        FadeObject = this.gameObject;
        FadeImage = FadeObject.GetComponent<Image>();

        FadeObject.SetActive(true);
    }

    void Start()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        FadeObject.SetActive(true);
        FadeImage.color = Color.black;
        FadeImage.DOFade(0.0f, FadeTime).OnComplete(() =>
        {
            FadeObject.SetActive(false);
        });
    }

    public void FadeOut(string sceneName)
    {
        FadeObject.SetActive(true);
        FadeImage.DOFade(1.0f, FadeTime).OnComplete(() =>
        {
            Destroy(this);
            SceneManager.LoadScene(sceneName);
        });
    }


}
