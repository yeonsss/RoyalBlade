using System;
using Controller.Creature;
using UnityEngine;

namespace Controller
{
    public class SpecialMoveController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy"))
            {
                col.GetComponent<MonsterController>().Attacked(99999, true);
            }
        }
    }
}