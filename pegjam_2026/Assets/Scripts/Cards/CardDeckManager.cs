using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace lvl_0
{
	public class CardDeckManager : MonoBehaviour
	{
        [Header("References")]
        [SerializeField] private Transform m_cardContainer;
        [SerializeField] private GameObject m_cardPrefab;
        [SerializeField] private CardDeck m_cardDeck;
        [SerializeField] private CardDeckConfig m_cardDeckConfig;
        [SerializeField] private Button m_drawCardButton;

        private List<Card> m_deck;
        private List<Card> m_playersHand;
        List<GameObject> m_cardGameObjects;
        Dictionary<ECardType, int> m_cardTypeCounts = new Dictionary<ECardType, int>();
        private const int k_maxCardsInHand = 5;

        private void Awake()
		{
            m_deck = new List<Card>();
            m_playersHand = new List<Card>();
            m_cardGameObjects = new List<GameObject>();
            m_cardTypeCounts = new Dictionary<ECardType, int>();

            // Register Events
            m_drawCardButton.onClick.AddListener(OnDrawCardClick);

            InitCardDeck();

            DealCardDeck(k_maxCardsInHand);
        }

        private void Update()
		{	
		}

        private void OnDestroy()
        {
            // Unregister Events
            m_drawCardButton.onClick.RemoveListener(OnDrawCardClick);
        }

        private void InitCardDeck()
		{
            if (m_cardDeck == null)
			{
				Debug.LogError("[CardDeckManager] Card Deck is null.");
				return;
			}

            m_deck = m_cardDeck.GetCards();
        }

		public void DealCardDeck(int numOfCards)
		{
            if (m_deck == null || m_deck.Count == 0)
            {
                Debug.LogError("[CardDeckManager] There are no cards in deck.");
                return;
            }

            if (numOfCards < 1)
            {
                Debug.LogError("[CardDeckManager] You must deal at least one card.");
                return;
            }

            ShuffleDeck(m_deck);

            m_playersHand = DrawCards(numOfCards);

            InstantiateCards();
        }

		private void ShuffleDeck(List<Card> list)
		{
            for (int i = list.Count - 1; i > 0; i--)
			{
                int randomIndex = Random.Range(0, i + 1);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }

        private Card DrawCard()
        {
            return DrawCards(1).FirstOrDefault();
        }

        private List<Card> DrawCards(int numCards)
        {
            List<Card> pickedCards = new List<Card>();

            // Set count for each Card Type to 0
            foreach (ECardType type in System.Enum.GetValues(typeof(ECardType)))
            {
                m_cardTypeCounts[type] = 0;
            }

            var cardsToRemove = new List<Card>();
            // Select cards but no more then max amount of certain types
            foreach (var card in m_deck)
            {
                if (pickedCards.Count >= numCards)
                {
                    break;
                }

                int maxAllowed = m_cardDeckConfig.GetMaxForCardType(card.cardType);

                if (m_cardTypeCounts[card.cardType] < maxAllowed)
                {
                    pickedCards.Add(card);
                    cardsToRemove.Add(card);
                    m_cardTypeCounts[card.cardType]++;
                }
            }

            foreach (var card in cardsToRemove)
            {
                m_deck.Remove(card);
            }

            return pickedCards;
        }

        private void InstantiateCards()
        {
            m_cardGameObjects = new List<GameObject>();
            foreach (var dealtCard in m_playersHand)
            {
                GameObject newCardObj = Instantiate(m_cardPrefab, m_cardContainer);
                CardManager card = newCardObj.GetComponent<CardManager>();

                m_cardGameObjects.Add(newCardObj);
                card.InitCard(dealtCard);
            }
        }

        private void DestroyCards()
        {
            for (var i = 0; i< m_cardGameObjects.Count; i++)
            {
                Destroy(m_cardGameObjects[i]); 
            }
            m_cardGameObjects.Clear();
        }

        private void ClearDeck()
        {
            m_deck = new List<Card>();

            // Set count for each Card Type to 0
            foreach (ECardType type in System.Enum.GetValues(typeof(ECardType)))
            {
                m_cardTypeCounts[type] = 0;
            }
        }

        void OnDrawCardClick()
        {
            m_playersHand = GetPlayersHand();

            if (m_playersHand.Count < k_maxCardsInHand
                && m_deck.Count > 0)
            {
                Card newCard = DrawCard();
                m_playersHand.Add(newCard);
                
                RefreshCards();
                m_playersHand = GetPlayersHand();
            }
        }

        private void RefreshCards()
        {
            DestroyCards();
            InstantiateCards();
        }

        private List<Card> GetPlayersHand()
        {
            List<Card> playersHand = new List<Card>();
            for (var i = 0; i < m_cardGameObjects.Count; i++)
            {
                if (m_cardGameObjects[i] != null)
                {
                    CardManager cardManager = m_cardGameObjects[i].GetComponent<CardManager>();
                    Card card = cardManager.Card;
                    playersHand.Add(card);
                }
            }
            return playersHand;
        }
    }
}
