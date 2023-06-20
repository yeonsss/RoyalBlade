using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using static Utils.Define;

namespace Controller
{
    public class CameraController : MonoBehaviour
    {
        private GameObject _backGround;
        private float _maxMoveHight;
        private float _minMoveHight;
        
        private void Awake()
        {
            _backGround = transform.GetChild(0).gameObject;
            SetBackground(GameManager.instance.stageNum - 1);
            var height = gameObject.GetComponent<Camera>().orthographicSize / 2;
            _maxMoveHight = height;
            _minMoveHight = -1 * height;
        }

        private void RangeCheck()
        {
            var playerPosY = GameManager.instance.player.transform.position.y;
            var cameraPosY = transform.position.y;

            float posY = 0;

            if (playerPosY > cameraPosY)
            {
                posY = GameManager.instance.player.transform.position.y;
            }
            else
            {
                posY = GameManager.instance.player.transform.position.y;
                if (posY < 0) return;
            }
            
            var pos = transform.position;
            transform.position = new Vector3(pos.x, posY, pos.z);
        }

        void Update()
        {
            if (GameManager.instance.player == null) return;
            RangeCheck();
        }

        public void SetBackground(int backNum)
        {
            var childCount = _backGround.transform.childCount;
            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    var prevTile = _backGround.transform.GetChild(i);
                    ResourceManager.instance.Destroy(prevTile.gameObject);
                }
            }

            var path = backGroundPaths[backNum];
            ResourceManager.instance.Instantiate(path, parent: _backGround.transform);
        }
    }
}
