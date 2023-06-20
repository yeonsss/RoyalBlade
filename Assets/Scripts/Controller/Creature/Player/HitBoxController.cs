using UnityEngine;

namespace Controller.Creature.Player
{
    public class HitBoxController : MonoBehaviour
    {
        public bool isHitBoxCollision { get; set; }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            isHitBoxCollision = true;
        }
        
        private void OnTriggerExit2D(Collider2D col)
        {
            isHitBoxCollision = false;
        }
    }
}