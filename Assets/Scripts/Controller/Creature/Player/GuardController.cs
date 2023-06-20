using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Controller.Creature.Player
{
    public class GuardController : MonoBehaviour
    {
        public GameObject detectEnemy { get; set; }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy"))
            {
                detectEnemy = col.gameObject;
            }
        }
        
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.CompareTag("Enemy"))
            {
                detectEnemy = null;
            }
        }
    }
}