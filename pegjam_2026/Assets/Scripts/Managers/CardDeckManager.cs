using System.Collections.Generic;
using UnityEngine;

namespace lvl_0
{
	public class CardDeckManager : MonoBehaviour
	{
        [Header("References")]
        [SerializeField] private Transform m_cardContainer;
        [SerializeField] private GameObject m_cardPrefab;
        [SerializeField] private CardDeck m_cardDeck;

		private List<Card> m_cards;

        private void Start()
		{
			InitCardDeck();
		}

		private void Update()
		{
			
		}

		private void InitCardDeck()
		{
			if (m_cardDeck == null)
			{
				Debug.LogError("[CardDeckManager] Card Deck is null.");
				return;
			}

			m_cards = m_cardDeck.GetCards();
        }

		public void DealCardDeck(int numOfCards)
		{
            if (m_cards == null || m_cards.Count == 0)
            {
                Debug.LogError("[CardDeckManager] There are no cards in deck.");
                return;
            }

            if (numOfCards < 1)
            {
                Debug.LogError("[CardDeckManager] You must deal at least one card.");
                return;
            }

            ShuffleDeck(m_cards);

            List<Card> hand = m_cards.GetRange(0, numOfCards - 1);

            InstantiateCards(hand);
        }

		private void ShuffleDeck(List<Card> list)
		{
            for(int i = list.Count - 1; i > 0; i--)
			{
                int randomIndex = Random.Range(0, i + 1);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }

        private void InstantiateCards(List<Card> dealtHand)
        {
            foreach (var dealtCard in dealtHand)
            {
                GameObject newCardObj = Instantiate(m_cardPrefab, m_cardContainer);
                CardManager card = newCardObj.GetComponent<CardManager>();

                card.InitCard(dealtCard.cardValue, dealtCard.cardDesciption, dealtCard.cardSprite);
            }
        }
    }
}
