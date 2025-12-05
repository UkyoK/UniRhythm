using UnityEngine;
using CriWare;
using UniRhythm_acf.Selector;
using Shine.Common;

public class MySoundManager : MonoBehaviour
{
    public static MySoundManager Instance;

    private CriAtomSource atomSrc;

    /// <summary>
    /// 曲が再生中か否か
    /// </summary>
    private bool isMusicPlaying;

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

        isMusicPlaying = false;
    }

    void Start()
    {
        atomSrc = gameObject.GetComponent<CriAtomSource>();
    }

    public void PlayMusic()
    {
        // 再生中なら処理しない
        if (isMusicPlaying)
        {
            return;
        }

        // 曲の再生を開始
        atomSrc.cueSheet = SettingManager.Instance.FolderName;
        atomSrc.cueName = SettingManager.Instance.FolderName;
        atomSrc.Play();
        isMusicPlaying = true;

        // 曲の再生が開始されたら、キューをSE用のものに切り替え
        atomSrc.cueSheet = "InGame";
        atomSrc.cueName = "SE";
    }

    /// <summary>
    /// 曲の停止
    /// </summary>
    public void StopMusic()
    {
        if (isMusicPlaying)
        {
            atomSrc.Stop();
            isMusicPlaying = false;
        }
    }

    /// <summary>
    /// 判定に応じてSEを再生
    /// </summary>
    public void PlaySE(Judgement nowJudgement)
    {
        atomSrc.player.SetSelectorLabel("Judgement", nowJudgement.ToString());
        atomSrc.Play();
    }

    public void PlayClearVoice(ClearState clearState)
    {
        atomSrc.cueSheet = "MusicEnd";
        atomSrc.cueName = "MusicEnd";

        atomSrc.player.SetSelectorLabel("ClearState", clearState.ToString());
        atomSrc.Play();
    }

    public void PlaySongSelectVoice(SelectState selectState)
    {
        atomSrc.cueSheet = selectState.ToString();
        if (selectState == SelectState.MusicStart)
        {
            atomSrc.cueName = selectState.ToString();
        }
        else
        {
            atomSrc.cueName = "Moca_voice_" + selectState.ToString();
        }

        atomSrc.Play();
    }
}
