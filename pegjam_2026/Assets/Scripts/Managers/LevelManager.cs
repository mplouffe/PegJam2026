using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace lvl_0
{
    public class LevelManager : SingletonBase<LevelManager>
    {
        [SerializeField]
        private LevelDeck m_levelDeck;

        [SerializeField]
        private Duration m_waitDuration;

        [SerializeField]
        private AudioClip m_levelMusic;

        private Level m_activeLevel;
        private List<Level> m_beatenLevels = new List<Level>();

        private bool m_endGame = false;

        private LevelManagerState m_state;

        protected override void Awake()
        {
            base.Awake();
            m_endGame = false;

        }

        private void Update()
        {
            switch (m_state)
            {
                case LevelManagerState.Waiting:
                    if (m_waitDuration.UpdateCheck())
                    {
                        SetState(LevelManagerState.PickingLevel);
                    }
                    break;
            }
        }

        public void PickLevel(string pickedLevel)
        {
            m_activeLevel = m_levelDeck.GetLevelByName(pickedLevel);
            SetState(LevelManagerState.PlacingPerson);
        }

        public void StartBuilding()
        {
            SetState(LevelManagerState.PlayingLevel);
        }

        public void EvaluateLevel()
        {
            SetState(LevelManagerState.EvaluatingLevel);
        }

        public void EndLevel()
        {
            if (m_endGame)
            {
                LevelAttendant.Instance.LoadGameState(GameState.GameOver);
            }
            else
            {
                SetState(LevelManagerState.PickingLevel);
            }
        }

        private void SetState(LevelManagerState newState)
        {
            if (m_state == newState) return;
            switch(newState)
            {
                case LevelManagerState.PickingLevel:
                    if (m_state == LevelManagerState.Waiting) AudioManager.Instance.PlayMusic(m_levelMusic);
                    Nest.Instance.CleanNest();
                    PopupsManager.Instance.ClearLevelInfo();
                    var levels = m_levelDeck.GetRandomLevels(m_beatenLevels);
                    if (levels.Count > 0)
                    {
                        PopupsManager.Instance.ShowLevels(levels);
                    }
                    else
                    {
                        LevelAttendant.Instance.LoadGameState(GameState.GameOver);
                    }
                    break;
                case LevelManagerState.PlacingPerson:
                    PopupsManager.Instance.ClearLevelInfo();
                    CardDeckManager.Instance.SetCardDeck(EDeck.PersonDeck);
                    CardDeckManager.Instance.DealHand();
                    break;
                case LevelManagerState.PlayingLevel:
                    PopupsManager.Instance.ClearLevelInfo();
                    CardDeckManager.Instance.SetCardDeck(EDeck.ItemDeck);
                    CardDeckManager.Instance.DealHand();
                    break;
                case LevelManagerState.EvaluatingLevel:
                    if (m_activeLevel.LevelPassed())
                    {
                        PopupsManager.Instance.LevelPassed(m_activeLevel);
                        m_beatenLevels.Add(m_activeLevel);
                    }
                    else
                    {
                        PopupsManager.Instance.LevelFailed(m_activeLevel);
                        m_endGame = true;
                    }
                    break;
            }
            m_state = newState;
        }
    }

    public enum LevelManagerState
    {
        Waiting,
        PickingLevel,
        PlacingPerson,
        PlayingLevel,
        EvaluatingLevel,
        ResolvingLevel
    }
}
