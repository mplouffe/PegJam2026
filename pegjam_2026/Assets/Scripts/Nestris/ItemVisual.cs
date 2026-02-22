using UnityEngine;

namespace lvl_0
{
    public class ItemVisual : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer m_itemSprite;

        [SerializeField]
        private Duration m_scoringDuration;

        [SerializeField]
        private float m_popScale;

        private ItemVisualState m_state;

        public void Init(Sprite sprite)
        {
            m_itemSprite.sprite = sprite;
        }

        public void Pop()
        {
            SetState(ItemVisualState.Scoring);
        }

        private void SetState(ItemVisualState newState)
        {
            switch(newState)
            {
                case ItemVisualState.Scoring:
                    m_itemSprite.transform.localScale = new Vector3(m_popScale, m_popScale, 1);
                    m_scoringDuration.Reset();
                    break;
                case ItemVisualState.None:
                    m_itemSprite.transform.localScale = Vector3.one;
                    break;
            }
            m_state = newState;
        }

        private void Update()
        {
            switch (m_state)
            {
                case ItemVisualState.Scoring:
                    if (m_scoringDuration.UpdateCheck())
                    {

                        SetState(ItemVisualState.None);
                    }
                    else
                    {
                        var currentScale = Mathf.Lerp(m_popScale, 1, m_scoringDuration.CurvedDelta());
                        m_itemSprite.transform.localScale = new Vector3(currentScale, currentScale, 1);
                    }
                    break;
            }
        }
    }

    public enum ItemVisualState
    {
        None,
        Scoring
    }
}
