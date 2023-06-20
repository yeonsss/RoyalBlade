using System;
using System.Collections;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class MainUI : BaseUI
    {
        private enum Buttons
        {
            JumpBtn,
            GuardBtn,
            AttackBtn,
            RestartBtn,
            GameStartBtn,
            GamePauseBtn
        }
        
        private enum Texts
        {
            Score,
            ResultMessage,
            ResultScore,
            ComboMessage
        }
        
        private enum Sliders
        {
            SpecialSlider,
            SkillSlider,
        }
        
        private enum Grids
        {
            HpPopUp
        }
        
        private enum Images
        {
            SpecialGauge,
            SkillGauge,
            SpecialBtnImage,
            SkillBtnImage,
        }
        
        private enum Panels
        {
            NormalPanel,
            ButtonPanel,
            ResultPanel
        }

        protected override void Init()
        {
            Bind<Button>(typeof(Buttons));
            Bind<TMP_Text>(typeof(Texts));
            Bind<GridLayoutGroup>(typeof(Grids));
            Bind<Image>(typeof(Images));
            Bind<CanvasGroup>(typeof(Panels));
            Bind<Slider>(typeof(Sliders));
            
            Get<Button>((int)Buttons.JumpBtn).gameObject.BindEvent(JumpBtnHandler);
            Get<Slider>((int)Sliders.SpecialSlider).gameObject.BindEvent(() =>
            {
                var value = Get<Slider>((int)Sliders.SpecialSlider).value;
                if (value < 1)
                {
                    Get<Slider>((int)Sliders.SpecialSlider).value = 0;
                    JumpBtnHandler();
                }
            }, Define.UIEvent.PointerUp);
            
            
            Get<Button>((int)Buttons.AttackBtn).gameObject.BindEvent(AttackBtnHandler);
            Get<Slider>((int)Sliders.SkillSlider).gameObject.BindEvent(() =>
            {
                var value = Get<Slider>((int)Sliders.SkillSlider).value;
                if (value < 1)
                {
                    Get<Slider>((int)Sliders.SkillSlider).value = 0;
                    AttackBtnHandler();
                }
            }, Define.UIEvent.PointerUp);

            Get<Button>((int)Buttons.GuardBtn).gameObject.BindEvent(GuardBtnHandler);
            
            Get<Button>((int)Buttons.GameStartBtn).gameObject.BindEvent(GameStartBtnHandler);
            Get<Button>((int)Buttons.GamePauseBtn).gameObject.BindEvent(GamePauseBtnHandler);
            Get<Button>((int)Buttons.RestartBtn).gameObject.BindEvent(GameRestartBtnHandler);

            Get<Slider>((int)Sliders.SpecialSlider).onValueChanged.AddListener(SpecialSliderHandler);
            Get<Slider>((int)Sliders.SkillSlider).onValueChanged.AddListener(SkillSliderHandler);
            
            Get<Slider>((int)Sliders.SpecialSlider).gameObject.SetActive(false);
            Get<Slider>((int)Sliders.SkillSlider).gameObject.SetActive(false);
            
            Get<CanvasGroup>((int)Panels.ResultPanel).gameObject.SetActive(false);

        }

        private void Update()
        {
            UpdateScore();
            ToggleBtn();
            UpdateLifePoint();
            UpdateResultMessage();
            UpdateSkillGauge();
            UpdateSpecialGauge();
            UpdateCombo();
        }

        private void UpdateCombo()
        {
            var comboMessage = Get<TMP_Text>((int)Texts.ComboMessage);
            var comboCount = GameManager.instance.combo;

            if (comboCount == 0)
            {
                comboMessage.gameObject.SetActive(false);
            }
            else
            {
                comboMessage.gameObject.SetActive(true);
                comboMessage.text = $"{comboCount.ToString()} Combo";
            }
        }

        private void UpdateScore()
        {
            Get<TMP_Text>((int)Texts.Score).text = GameManager.instance.score.ToString();
        }

        private void UpdateSkillGauge()
        {
            var value = GameManager.instance.skillGauge;
            if (value >= 100)
            {
                var slider = Get<Slider>((int)Sliders.SkillSlider).gameObject;
                if (slider.activeSelf == false)
                    slider.SetActive(true);
                
                var btn = Get<Button>((int)Buttons.AttackBtn).gameObject;
                if (btn.activeSelf == true)
                    btn.SetActive(false);
            }
            else
            {
                var slider = Get<Slider>((int)Sliders.SkillSlider).gameObject;
                if (slider.activeSelf == true)
                    slider.SetActive(false);
                
                var btn = Get<Button>((int)Buttons.AttackBtn).gameObject;
                if (btn.activeSelf == false)
                    btn.SetActive(true);
                
            }
            
            Get<Image>((int)Images.SkillGauge).fillAmount = value / 100;
        }

        private void UpdateSpecialGauge()
        {
            var value = GameManager.instance.specialGauge;
            if (value >= 100)
            {
                var slider = Get<Slider>((int)Sliders.SpecialSlider).gameObject;
                if (slider.activeSelf == false)
                    slider.SetActive(true);

                var btn = Get<Button>((int)Buttons.JumpBtn).gameObject;
                if (btn.activeSelf == true)
                    btn.SetActive(false);
            }
            else
            {
                var slider = Get<Slider>((int)Sliders.SpecialSlider).gameObject;
                if (slider.activeSelf == true)
                    slider.SetActive(false);
                
                var btn = Get<Button>((int)Buttons.JumpBtn).gameObject;
                if (btn.activeSelf == false)
                    btn.SetActive(true);
            }
            
            Get<Image>((int)Images.SpecialGauge).fillAmount = value / 100;
        }

        private void SetPanelInteractiveBlock()
        {
            Get<CanvasGroup>((int)Panels.ButtonPanel).interactable = false;
            Get<CanvasGroup>((int)Panels.ButtonPanel).blocksRaycasts = false;
            Get<CanvasGroup>((int)Panels.NormalPanel).interactable = false;
            Get<CanvasGroup>((int)Panels.NormalPanel).blocksRaycasts = false;
        }
        
        private void SetPanelInteractiveOn()
        {
            Get<CanvasGroup>((int)Panels.ButtonPanel).interactable = true;
            Get<CanvasGroup>((int)Panels.ButtonPanel).blocksRaycasts = true;
            Get<CanvasGroup>((int)Panels.NormalPanel).interactable = true;
            Get<CanvasGroup>((int)Panels.NormalPanel).blocksRaycasts = true;
        }

        private void SetButtonPanelBlock()
        {
            Get<CanvasGroup>((int)Panels.ButtonPanel).interactable = false;
            Get<CanvasGroup>((int)Panels.ButtonPanel).blocksRaycasts = false;
        }
        
        private void SetButtonPanelActive()
        {
            Get<CanvasGroup>((int)Panels.ButtonPanel).interactable = true;
            Get<CanvasGroup>((int)Panels.ButtonPanel).blocksRaycasts = true;
        }
        
        private void UpdateResultMessage()
        {
            var resultPanel = Get<CanvasGroup>((int)Panels.ResultPanel);
            if (resultPanel.gameObject.activeSelf == true) return;
            
            var resultText = Get<TMP_Text>((int)Texts.ResultMessage);
            var resultScore = Get<TMP_Text>((int)Texts.ResultScore);
            
            if (GameManager.instance.isGameClear == true)
            {
                if (resultPanel.gameObject.activeSelf == true) return;
                resultText.text = "Game Clear";
                resultScore.text = $"Score : {GameManager.instance.score}";
                resultPanel.gameObject.SetActive(true);
                SetPanelInteractiveBlock();
                return;
            }

            if (GameManager.instance.lifePoint != 0) return;
            resultText.text = "You Lose";
            resultScore.text = $"Score : {GameManager.instance.score}";
            resultPanel.gameObject.SetActive(true);
            SetPanelInteractiveBlock();
        }

        private void UpdateLifePoint()
        {
            var hpPopup = Get<GridLayoutGroup>((int)Grids.HpPopUp);
            var prevLp = hpPopup.transform.childCount;
            var currentLp = GameManager.instance.lifePoint;
            
            if (prevLp > currentLp)
            {
                var diff = prevLp - currentLp;
                for (int i = 0; i < diff; i++)
                {
                    ResourceManager.instance.Destroy(hpPopup.transform.GetChild(0).gameObject);
                }
            }
            else if (prevLp < currentLp)
            {
                var diff = currentLp - prevLp;
                for (int i = 0; i < diff; i++)
                {
                    ResourceManager.instance.Instantiate("UI/Heart", parent: hpPopup.transform);
                }
            }
        }

        private void ToggleBtn()
        {
            if (GameManager.instance.isGameStart)
            {
                Get<Button>((int)Buttons.GameStartBtn).gameObject.SetActive(false);
                Get<Button>((int)Buttons.GamePauseBtn).gameObject.SetActive(true);
            }
            else
            {
                Get<Button>((int)Buttons.GameStartBtn).gameObject.SetActive(true);
                Get<Button>((int)Buttons.GamePauseBtn).gameObject.SetActive(false);
            }
        }

        private void SkillSliderHandler(float value)
        {
            if (value >= 1)
            {
                GameManager.instance.player.Skill();
                GameManager.instance.skillGauge = 0;
                Get<Slider>((int)Sliders.SkillSlider).value = 0;
            }
        }
        
        private void SpecialSliderHandler(float value)
        {
            if (value >= 1)
            {
                GameManager.instance.player.SpecialMove();
                GameManager.instance.specialGauge = 0;
                Get<Slider>((int)Sliders.SpecialSlider).value = 0;
            }
        }

        private void GameStartBtnHandler()
        {
            GameManager.instance.GameResume();
            SetButtonPanelActive();
        }

        private void GamePauseBtnHandler()
        {
            GameManager.instance.GameStop();
            SetButtonPanelBlock();
        }

        private void GameRestartBtnHandler()
        {
            Get<Slider>((int)Sliders.SpecialSlider).value = 0;
            Get<Slider>((int)Sliders.SkillSlider).value = 0;
            Get<Image>((int)Images.SkillGauge).fillAmount = 0;
            Get<Image>((int)Images.SpecialGauge).fillAmount = 0;

            SetPanelInteractiveOn();
            Get<CanvasGroup>((int)Panels.ResultPanel).gameObject.SetActive(false);
            GameManager.instance.GameReStart();
        }

        private void JumpBtnHandler()
        {
            GameManager.instance.player.Jump();
        }
        
        private void AttackBtnHandler()
        {
            GameManager.instance.player.Attack();
        }
        
        private void GuardBtnHandler()
        {
            var guardCheck = GameManager.instance.player.Guard();

            if (guardCheck == true)
            {
                var btnGameObject = Get<Button>((int)Buttons.GuardBtn).gameObject;
                var charName = GameManager.instance.player.characterName;
                var coolDown = DataManager.instance.characterData[charName].guardCoolDown;

                LeanTween.value(btnGameObject, 0, 1, coolDown)
                    .setOnUpdate((float value) =>
                    {
                        var image = btnGameObject.GetComponent<Image>();
                        image.fillAmount = value;
                    }).setEase(LeanTweenType.linear);
            }
        }
    }
}