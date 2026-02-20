using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace lvl_0
{
    public class CardManager : MonoBehaviour
    {
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI m_itemDesciption;
        [SerializeField] private TextMeshProUGUI m_itemValue;

        [Header("Images")]
        [SerializeField] private Image m_backgroundImage;
        [SerializeField] private Image m_gridImage;
        [SerializeField] private Image m_itemImage;

        [Header("Card Data")]
        [SerializeField] private int m_cardValue;
        [SerializeField] private string m_cardDescription;
        [SerializeField] private Sprite m_cardSprite;

        public int CardValue => m_cardValue;
        public string CardDescription => m_cardDescription;
        public Sprite CardSprite => m_cardSprite;

        private void Awake()
        {
            RefreshAll();
        }

        private void Update()
        {
        }

        public void InitCard(int value, string desciption, Sprite itemSprite)
        {
            m_cardValue = value;
            m_cardDescription = desciption;
            m_cardSprite = itemSprite;

            RefreshAll();
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
    }
}