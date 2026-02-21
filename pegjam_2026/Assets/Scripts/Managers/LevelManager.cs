using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace lvl_0
{
    public class LevelManager : SingletonBase<LevelManager>
    {
        [SerializeField]
        private List<Level> m_levels;

        [SerializeField]
        private Duration m_waitDuration;

        private Level m_activeLevel;

        private LevelManagerState m_state;

        protected override void Awake()
        {
            base.Awake();
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

        public void PickLevel(int pickedLevel)
        {
            m_activeLevel = m_levels[pickedLevel];
            SetState(LevelManagerState.PlayingLevel);
        }

        public void EvaluateLevel()
        {
            SetState(LevelManagerState.EvaluatingLevel);
        }

        private void SetState(LevelManagerState newState)
        {
            if (m_state == newState) return;
            switch(newState)
            {
                case LevelManagerState.PickingLevel:
                    PopupsManager.Instance.ShowLevels(m_levels);
                break;
                case LevelManagerState.PlayingLevel:
                    PopupsManager.Instance.ClearLevels();
                    CardDeckManager.Instance.DealHand();
                    break;
                case LevelManagerState.EvaluatingLevel:
                    if (m_activeLevel.LevelPassed())
                    {
                        PopupsManager.Instance.LevelPassed(m_activeLevel);
                    }
                    else
                    {
                        PopupsManager.Instance.LevelFailed(m_activeLevel);
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
        PlayingLevel,
        EvaluatingLevel,
        ResolvingLevel
    }
}
