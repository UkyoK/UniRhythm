using Shine.Common;
using System.Collections;
using System.Collections.Generic;
using UniRhythm_acf.Selector;
using Unity.VisualScripting;
using UnityEngine;

public class LaneInput : MonoBehaviour
{
    KeyCode _Key;

    private Judgement NowJudgement;

    private string SongName;


    // Start is called before the first frame update
    void Awake()
    {
        SongName = SettingManager.Instance.Title;
        NowJudgement = Judgement.Miss;

        switch (gameObject.name)
        {
            case "Lane1":
                _Key = SettingManager.Instance.Lane1;
                break;
            case "Lane2":
                _Key = SettingManager.Instance.Lane2;
                break;
            case "Lane3":
                _Key = SettingManager.Instance.Lane3;
                break;
            case "Lane4":
                _Key = SettingManager.Instance.Lane4;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // -------- 子オブジェクトを2つ取得(A,Bとする)
        GameObject childA = null;
        HitNote hitNoteA = null;
        if (transform.childCount > 0)
        {
            childA = transform.GetChild(0).gameObject;
            hitNoteA = childA.GetComponent<HitNote>();
        }
        else
        {
            return;
        }

        GameObject childB = null;
        HitNote hitNoteB = null;
        if (transform.childCount > 1)
        {
            childB = transform.GetChild(1).gameObject;
            hitNoteB = childB.GetComponent<HitNote>();
        }
        // -------- 子オブジェクトを2つ取得(A,Bとする)

        float margin = 0.0f;

        // 子Bがnullでなければ、判定ノーツの確定を行う
        if (childB || hitNoteB)
        {
            // 子BのPerfect判定時間内に入ったら、判定オブジェクトを子Aから子Bに切り替え
            if (hitNoteB.JustTime - SettingManager.Instance.PerfectTime / 1000 < Time.time)
            {
                // まず子Aをミス判定する
                ScoreManager.Instance.Miss();
                Debug.Log("Miss...");
                Destroy(childA);

                MySoundManager.Instance.PlaySE(Judgement.Miss);
                ScoreManager.Instance.JudgementDisplay(Judgement.Miss);

                // 判定ノーツをBに切り替え
                childA = childB;
                hitNoteA = hitNoteB;
            }
            // 子Bはもう使わないので破棄
            childB = null;
            hitNoteB = null;

        }
        // 現在時間と判定時間の誤差を取得
        margin = Time.time - hitNoteA.JustTime;

        // 入力検知
        if (Input.GetKeyDown(_Key) && margin >= (-SettingManager.Instance.MissTime / 1000) && margin <= (SettingManager.Instance.MissTime / 1000))
        {
            // Perfect判定
            if (margin >= (-SettingManager.Instance.PerfectTime / 1000) && margin <= (SettingManager.Instance.PerfectTime / 1000))
            {
                NowJudgement = Judgement.Perfect;
                ScoreManager.Instance.Perfect();
                ScoreManager.Instance.AddCombo();
                Debug.Log("Perfect!!");
                Destroy(childA);
            }
            // Great判定
            else if (margin >= -(SettingManager.Instance.GreatTime / 1000) && margin <= (SettingManager.Instance.GreatTime / 1000))
            {
                NowJudgement = Judgement.Great;

                if (margin > 0)
                {
                    ScoreManager.Instance.Great(margin);
                    ScoreManager.Instance.AddCombo();
                    Debug.Log("Great (LATE)");
                    Destroy(childA);
                }
                else
                {
                    ScoreManager.Instance.Great(margin);
                    ScoreManager.Instance.AddCombo();
                    Debug.Log("Great (FAST)");
                    Destroy(childA);
                }
            }
            // Miss
            else
            {
                NowJudgement = Judgement.Miss;

                if (margin > 0)
                {
                    ScoreManager.Instance.Miss(margin);
                    Debug.Log("Miss... (LATE)");
                    Destroy(childA);
                }
                else
                {
                    ScoreManager.Instance.Miss(margin);
                    Debug.Log("Miss... (FAST)");
                    Destroy(childA);
                }
            }

            // SEを鳴らす
            MySoundManager.Instance.PlaySE(NowJudgement);

            // 判定表示
            ScoreManager.Instance.JudgementDisplay(NowJudgement);
        }
        // 放置検知
        else if (Time.time > hitNoteA.JustTime + SettingManager.Instance.MissTime / 1000)
        {
            ScoreManager.Instance.Miss();
            Debug.Log("Miss...");
            Destroy(childA);

            MySoundManager.Instance.PlaySE(Judgement.Miss);
            ScoreManager.Instance.JudgementDisplay(Judgement.Miss);
        }

    }

}
