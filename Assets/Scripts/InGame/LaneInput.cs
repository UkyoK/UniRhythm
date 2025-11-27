using UniRhythm_acf.Selector;
using UnityEngine;

public class LaneInput : MonoBehaviour
{
    KeyCode _Key;

    private Judgement NowJudgement;

    // Start is called before the first frame update
    void Awake()
    {
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

    void Update()
    {
        // 子オブジェクトを2つ取得(A,Bとする)
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
        float margin = Time.time - hitNoteA.JustTime;

        // 入力検知
        if (Input.GetKeyDown(_Key) && margin >= (-SettingManager.Instance.MissTime / 1000) && margin <= (SettingManager.Instance.MissTime / 1000))
        {
            if (margin >= (-SettingManager.Instance.PerfectTime / 1000) && margin <= (SettingManager.Instance.PerfectTime / 1000))
            {
                // Perfect判定
                NowJudgement = Judgement.Perfect;
                ScoreManager.Instance.Perfect();
                ScoreManager.Instance.AddCombo();
                Debug.Log("Perfect!!");
                Destroy(childA);
            }
            else if (margin >= -(SettingManager.Instance.GreatTime / 1000) && margin <= (SettingManager.Instance.GreatTime / 1000))
            {
                // Great判定
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
            else
            {
                // Miss
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
        else if (Time.time > hitNoteA.JustTime + SettingManager.Instance.MissTime / 1000)
        {
            // 放置検知(Miss)
            ScoreManager.Instance.Miss();
            Debug.Log("Miss...");
            Destroy(childA);

            MySoundManager.Instance.PlaySE(Judgement.Miss);
            ScoreManager.Instance.JudgementDisplay(Judgement.Miss);
        }

    }

}
