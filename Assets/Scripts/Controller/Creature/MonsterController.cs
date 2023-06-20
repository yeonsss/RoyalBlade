using System;
using System.Collections;
using Controller.Creature.Player;
using Managers;
using UnityEngine;
using Utils;

namespace Controller.Creature
{
    public class MonsterController : MonoBehaviour
    {
        public string monsterName { get; set; }
        public float hp { get; set; }
        public float maxHp { get; set; }
        
        public bool isDied { get; set; }
        public bool isSlowed { get; set; }
        private float _slowSpeed;

        private Rigidbody2D _rigid;
        private Animator _anim;

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();
            _slowSpeed = 50;
        }

        private void Update()
        {
            if (isSlowed == true)
            {
                _rigid.velocity = _rigid.velocity.normalized * _slowSpeed * Time.deltaTime;    
            }
        }

        public void SetInfo(string mName)
        {
            monsterName = mName;
            maxHp = DataManager.instance.monsterData[monsterName].hp;
            hp = maxHp;
            isDied = false;
            isSlowed = false;
            _rigid.gravityScale = DataManager.instance.monsterData[monsterName].dropPower;
            _rigid.isKinematic = false;
        }

        public void AnimParamClear()
        {
            _anim.SetBool("isAttacked", false);
            _anim.SetBool("isDied", false);
        }
  
        public void PushForce(float force)
        {
            if (_rigid == null) return;
            if (isDied == true) return;
            _rigid.velocity = new Vector2(0, force);
        }
        
        public void InitVelocity()
        {
            if (_rigid == null) return;
            if (isDied == true) return;
            _rigid.velocity = new Vector2(0, 0);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                var pc = col.gameObject.GetComponent<PlayerController>();
                if (pc.isJumped == true)
                {
                    GameManager.instance.InitTotalVelocity();
                    pc.SetVelocityZero();
                }
                else
                {
                    col.gameObject.GetComponent<PlayerController>().Attacked();    
                }
            }
        }

        public void Attacked(float damage, bool critical)
        {
            _anim.Play("Hurt");
            hp = Mathf.Clamp(hp - damage, 0, maxHp);
            GameManager.instance.DisplayDamage(damage, critical);

            if (hp == 0)
            {
                Die();
                isDied = true;
            }
        }

        private void Die()
        {
            GameManager.instance.combo += 1;
            StartCoroutine(DieCor());
        }

        IEnumerator DieCor()
        {
            GetComponent<BoxCollider2D>().enabled = false;
            _rigid.isKinematic = true;
            _anim.Play("Die");
            var rewardScore = DataManager.instance.monsterData[monsterName].score;
            GameManager.instance.AddScore(rewardScore);
            yield return new WaitForSeconds(1.3f);
            GetComponent<BoxCollider2D>().enabled = true;
            SpawnManager.instance.ReturnInstance(gameObject, PoolType.Monster);
            GameManager.instance.remainMonsterCount -= 1;
        }
    }
}
