using Controller.Creature.Player;
using Managers;
using UnityEngine;

namespace Controller.Creature.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _rotatePos;
        [SerializeField]
        private WeaponImageController _weapon;
        
        private bool _isPrevSlashLeft;

        public string weaponName { get; set; }

        private void Awake()
        {
            _isPrevSlashLeft = false;
        }

        public void WeaponInvisible()
        {
            _weapon.gameObject.SetActive(false);
        }
        
        public void WeaponVisible()
        {
            _weapon.gameObject.SetActive(true);
        }

        public virtual void SlashEffect(Vector3 pos)
        {
            GameManager.instance.StartEffectCor(
                "SlashEffect",
                "Sword_slash",
                pos,
                0.3f
            );
        }
        
        public float CalcDamage(out bool critical)
        {
            var criChance = DataManager.instance.weaponData[weaponName].criticalChance;
            var increase = DataManager.instance.weaponData[weaponName].criticalDamageIncrease;
            var damage = DataManager.instance.weaponData[weaponName].damage;
                
            var randomValue = Random.value;

            if (randomValue <= criChance)
            {
                var incDamage = damage * (increase / 100);
                damage += incDamage;
                critical = true;
            }
            else
            {
                critical = false;    
            }

            return damage;
        }

        public virtual float CalcSkillDamage() { return 0; }

        public float CalcGaugeMount()
        {
            return DataManager.instance.weaponData[weaponName].skillGaugeMount;
        }

        public virtual void Skill() { }

        public void Slash()
        {
            if (_isPrevSlashLeft)
            {
                SlashRight();
                _isPrevSlashLeft = false;
            }
            else
            {
                SlashLeft();
                _isPrevSlashLeft = true;
            }
        }

        private void SlashRight()
        {
            
            _weapon.transform.eulerAngles = new Vector3(0, 0, -90);
            
            LeanTween.rotateAround(_weapon.gameObject, Vector3.forward, 180f, 0.2f);
            LeanTween.value(_weapon.gameObject, 0, Mathf.PI, 0.2f)
                .setOnUpdate((float value) => {
                    var centerPoint = _rotatePos.transform.position;
                    
                    var newPos = new Vector3(
                        centerPoint.x + Mathf.Cos(value) * 0.5f, 
                        centerPoint.y + Mathf.Sin(value) * 0.5f, 
                        centerPoint.z
                    );
                    
                    _weapon.transform.position = newPos;
                })
                .setEase(LeanTweenType.linear);
        }

        private void SlashLeft()
        {
            _weapon.transform.eulerAngles = new Vector3(0, 0, 90);
            
            LeanTween.rotateAround(_weapon.gameObject, Vector3.forward, -180f, 0.2f);
            LeanTween.value(_weapon.gameObject, Mathf.PI, 0, 0.2f)
                .setOnUpdate((float value) => {
                    var centerPoint = _rotatePos.transform.position;
                    
                    var newPos = new Vector3(
                        centerPoint.x + Mathf.Cos(value) * 0.5f, 
                        centerPoint.y + Mathf.Sin(value) * 0.5f, 
                        centerPoint.z
                    );
                    
                    _weapon.transform.position = newPos;
                })
                .setEase(LeanTweenType.linear);
        }
    }
}