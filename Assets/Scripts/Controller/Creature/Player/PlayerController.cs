using System;
using System.Collections;
using System.Linq;
using Controller.Creature.Weapon;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using static Utils.Define;


namespace Controller.Creature.Player
{
    public class PlayerController : MonoBehaviour
    {
        public WeaponController weapon;
        [SerializeField] private BodyController body;
        [SerializeField] private GuardController guard;
        [SerializeField] private HitBoxController hitBox;
        [SerializeField] private GameObject specialEffect;

        private Rigidbody2D _rigid;
        private Animator _anim;

        public string characterName { get; set; }
        public bool isJumped => body.isJumped;
        private bool _isAttackPossible;
        private bool _isGuardPossible;
        private int _weaponIdx;

        public void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();
            specialEffect.SetActive(false);

            _weaponIdx = -1;
            _isAttackPossible = true;
            _isGuardPossible = true;
            
            ChangeWeapon();
        }

        public void SetVelocityZero()
        {
            _rigid.velocity = new Vector2(0, 0);
        }

        private void SetWeapon(string weaponName)
        {
            if (weapon != null)
            {
                ResourceManager.instance.Destroy(weapon.gameObject);
                weapon = null;
            }
            
            var obj = ResourceManager.instance.Instantiate(DataManager.instance.weaponData[weaponName].spawnPath, parent:transform);
            weapon = obj.GetComponent<WeaponController>();
            weapon.weaponName = weaponName;
            weapon.transform.localPosition = new Vector3(0, 0.35f, 0);
            weapon.WeaponInvisible();
        }

        public void WeaponInit()
        {
            _weaponIdx = -1;
            ChangeWeapon();
        }

        public void ChangeWeapon()
        {
            var weaponKeys = DataManager.instance.weaponData.Keys.ToList();
            
            _weaponIdx += 1;
            if (_weaponIdx >= weaponKeys.Count) _weaponIdx = 0;

            var weaponKey = weaponKeys[_weaponIdx];
            SetWeapon(weaponKey);
            GameManager.instance.skillGauge = 0;
        }

        public void Jump()
        {
            if (hitBox.isHitBoxCollision == true ) return;
            if (body.isJumped == false)
            {
                body.isJumped = true;
                _anim.SetBool("isGround", false);
                GameManager.instance.StartEffectCor(
                    "JumpEffect",
                    "Explosion_dust",
                    new Vector3(0, -2.3f, 0),
                    0.7f
                );
                
                GameManager.instance.AddSpecialGauge(10f);
                var power = DataManager.instance.characterData[characterName].jumpPower;
                _rigid.AddForce(new Vector2(0, power), ForceMode2D.Impulse);
            }
        }

        private void Update()
        {
            _anim.SetFloat("jumpValue", _rigid.velocity.y);
            if (body.isJumped == false)
            {
                _anim.SetBool("isGround", true);
            }
        }

        public void Attack()
        {
            if (_isAttackPossible == true)
            {
                weapon.WeaponVisible();
                weapon.gameObject.SetActive(true);
                StartCoroutine(AttackCor());
            }
        }

        IEnumerator AttackCor()
        {
            _isAttackPossible = false;
            weapon.Slash();
            yield return new WaitForSeconds(attackSpeed);
            _isAttackPossible = true;
            weapon.WeaponInvisible();
        }

        public bool Guard()
        {
            if (_isGuardPossible == false) return false;
            if (guard.detectEnemy == null) return false;
            // 트리거해서 적인 녀석을 띄운다.
            // 적 개수 * 힘
            GameManager.instance.StartEffectCor(
                "GuardEffect",
                "Poison_nova",
                guard.transform.position,
                0.55f
            );
            
            StartCoroutine(GuardCol());

            var power = DataManager.instance.characterData[characterName].guardPower;
            
            GameManager.instance.InitTotalVelocity();
            GameManager.instance.SetAllMonsterVelocity(power);

            return true;
        }

        IEnumerator GuardCol()
        {
            _isGuardPossible = false;
            var coolDown = DataManager.instance.characterData[characterName].guardCoolDown;
            yield return new WaitForSeconds(coolDown);
            _isGuardPossible = true;
        }

        public void Skill()
        {
            weapon.Skill();
        }

        public void SpecialMove()
        {
            StartCoroutine(SpecialMoveCor());
        }

        IEnumerator SpecialMoveCor()
        {
            specialEffect.SetActive(true);
            body.isJumped = true;
            _anim.SetBool("isGround", false);
            GameManager.instance.StartEffectCor(
                "JumpEffect",
                "Explosion_dust",
                new Vector3(0, -2.3f, 0),
                0.7f
            );

            var posY = transform.position.y;
            _rigid.isKinematic = true;
            LeanTween.moveY(gameObject, posY + specialJumpHeight,0.7f);
            
            // _rigid.AddForce(new Vector2(0, specialJumpPower), ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.7f);
            _rigid.isKinematic = false;
            specialEffect.SetActive(false);
        }

        public void Attacked()
        {
            if (hitBox.isHitBoxCollision == false) return;
            GameManager.instance.SubLifePoint();
            GameManager.instance.combo = 0;
            StartCoroutine(AttackedCor());
        }
        
        IEnumerator AttackedCor()
        {
            _anim.SetBool("isAttacked", true);
            yield return new WaitForSeconds(0.1f);
            _anim.SetBool("isAttacked", false);
        }
    }
}


