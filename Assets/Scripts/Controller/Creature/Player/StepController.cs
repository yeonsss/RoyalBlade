using System;
using UnityEngine;

namespace Controller.Creature.Player
{
    public class StepController : MonoBehaviour
    {
        public bool isJumped { get; set; }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Ground"))
            {
                isJumped = false;
            }
        }
    }
}