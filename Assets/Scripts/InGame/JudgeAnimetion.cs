using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeAnimetion : MonoBehaviour
{
    void Start()
    {
        transform.DOMoveY(transform.position.y + 30.0f, 0.3f);
    }

}
