using System.Collections;
using Managers;
using TMPro;
using UnityEngine;
using Utils;

namespace UI.Popups
{
    public class Damage : MonoBehaviour
    {
        IEnumerator MoveUpCor()
        {
            var targetY = transform.position.y + 1f;
            LeanTween.moveY(gameObject, targetY, 0.5f);
            yield return new WaitForSeconds(0.5f);
            SpawnManager.instance.ReturnInstance(gameObject, PoolType.Damage);
        }

        public void SetInfo(float damage, bool critical)
        {
            var message = GetComponentInChildren<TMP_Text>();
            message.text = $"{damage}";
            if (critical)
            {
                message.color = Color.red;
            }
            else
            {
                message.color = Color.white;
            }
        }

        public void StartMoveUp()
        {
            StartCoroutine(MoveUpCor());
        }
    }
}