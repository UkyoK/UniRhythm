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
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        FadeObject = gameObject;
        FadeImage = GetComponentInChildren<Image>();
    }

    void Start()
    {
        FadeIn();
    }

    public async void FadeIn()
    {
        FadeImage.color = Color.black;
        await FadeImage.DOFade(0.0f, FadeTime).AsyncWaitForCompletion();
        FadeObject.SetActive(false);
    }

    public async void FadeOut(string sceneName)
    {
        FadeObject.SetActive(true);
        await FadeImage.DOFade(1.0f, FadeTime).AsyncWaitForCompletion();
        SceneManager.LoadScene(sceneName);
    }


}
