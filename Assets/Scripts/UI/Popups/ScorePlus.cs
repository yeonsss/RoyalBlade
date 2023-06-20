using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using Utils;

public class ScorePlus : MonoBehaviour
{
    IEnumerator MoveUpCor()
    {
        var targetY = transform.position.y + 1f;
        LeanTween.moveY(gameObject, targetY, 0.5f);
        yield return new WaitForSeconds(0.5f);
        SpawnManager.instance.ReturnInstance(gameObject, PoolType.ScorePlus);
    }

    public void SetMessage(string message)
    {
        GetComponentInChildren<TMP_Text>().text = $"+{message}";
    }

    public void StartMoveUp()
    {
        StartCoroutine(MoveUpCor());
    }
}
