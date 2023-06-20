using System;
using System.Collections;
using UnityEngine;

namespace Controller.Creature.Player
{
    public class BodyController : MonoBehaviour
    {
        [SerializeField] private StepController step;
        private BoxCollider2D _bodyCol;
        public bool isJumped
        {
            get => step.isJumped;
            set => step.isJumped = value;
        }

        private void Awake()
        {
            _bodyCol = GetComponent<BoxCollider2D>();
            _bodyCol.isTrigger = true;
        }

        private void Update()
        {
            if (isJumped == true)
            {
                _bodyCol.isTrigger = false;
            }
            else
            {
                _bodyCol.isTrigger = true;
            }
        }
        
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                if (isJumped)
                {
                    var rb = transform.parent.GetComponent<Rigidbody2D>();
                    rb.velocity = new Vector2(0, 0);    
                }
            }
        }
    }
}