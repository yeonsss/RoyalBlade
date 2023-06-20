using System;
using Controller.Creature.Player;
using Managers;
using UnityEngine;

namespace Scene
{
    public class GameSceneInit : MonoBehaviour
    {
        private void Start()
        {
            SpawnManager.instance.SpawnPlayer("MaskDude");
            GameManager.instance.player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }
    }
}