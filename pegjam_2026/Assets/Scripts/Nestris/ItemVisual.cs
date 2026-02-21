using UnityEngine;

namespace lvl_0
{
    public class ItemVisual : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer m_itemSprite;

        public void Init(Sprite sprite)
        {
            m_itemSprite.sprite = sprite;
        }
    }
}
