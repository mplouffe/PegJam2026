using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace lvl_0
{
    public class CardManager : MonoBehaviour, IPointerClickHandler
    {
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI m_itemDesciption;
        [SerializeField] private TextMeshProUGUI m_itemValue;

        [Header("Images")]
        [SerializeField] private Image m_backgroundImage;
        [SerializeField] private Image m_itemImage;

        [Header("Card Data")]
        [SerializeField] private Card m_card;
        [SerializeField] private int m_cardValue;
        [SerializeField] private string m_cardDescription;
        [SerializeField] private Sprite m_cardSprite;

        [SerializeField]
        private Item m_itemPrefab;

        public Card Card => m_card;
        public int CardValue => m_cardValue;
        public string CardDescription => m_cardDescription;
        public Sprite CardSprite => m_cardSprite;

        private BoolGrid m_itemShape;

        private CardState m_state;

        private void Awake()
        {
            RefreshAll();
        }

        public CardState GetState()
        {
            return m_state;
        }

        public void SetState(CardState newState)
        {
            switch (newState)
            {
                case CardState.Dealt:
                    gameObject.SetActive(true);
                    break;
                case CardState.Picked:
                    gameObject.SetActive(false);
                    break;
                case CardState.Used:
                    Destroy(gameObject);
                    break;
            }
            m_state = newState;
        }

        public void InitCard(Card card)
        {
            m_card = card;
            m_cardValue = card.cardValue;
            m_cardDescription = card.cardDesciption;
            m_cardSprite = card.cardSprite;
            m_itemShape = card.itemShape;

            RefreshAll();
            SetState(CardState.Dealt);
        }

        private void RefreshAll()
        {
            if (m_itemImage != null)
            {
                m_itemImage.sprite = m_cardSprite;
            }

            if (m_itemValue != null)
            {
                m_itemValue.text = m_cardValue.ToString();
            }

            if (m_itemDesciption != null)
            {
                m_itemDesciption.text = m_cardDescription.ToString();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            m_itemPrefab.ItemShape = m_itemShape;
            var activeItem = Instantiate(m_itemPrefab, eventData.pointerPressRaycast.worldPosition, Quaternion.identity);
            MouseFollower.Instance.SelectPiece(activeItem, this);
            SetState(CardState.Picked);
        }
    }

    public enum CardState
    {
        Dealt,
        Picked,
        Used
    }
}