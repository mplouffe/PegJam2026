using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace lvl_0
{
	public class CardDeckManager : SingletonBase<CardDeckManager>
	{
        [Header("References")]
        [SerializeField] private Transform m_cardContainer;
        [SerializeField] private GameObject m_cardPrefab;
        [SerializeField] private List<Decks> m_cardDecks;
        [SerializeField] private Button m_drawCardButton;

        private Dictionary<EDeck, CardDeck> m_deckDictionary = new Dictionary<EDeck, CardDeck>();
        private Dictionary<EDeck, CardDeckConfig> m_configDictionary = new Dictionary<EDeck, CardDeckConfig>();
        
        private List<Card> m_currentDeck;
        private CardDeckConfig m_currentConfig;

        private List<Card> m_playersHand;
        List<GameObject> m_cardGameObjects;
        Dictionary<ECardType, int> m_cardTypeCounts = new Dictionary<ECardType, int>();
        
        private const int k_maxCardsInHand = 5;

        private DeckManagerState m_state;

        protected override void Awake()
		{
            base.Awake();
            m_playersHand = new List<Card>();
            m_cardGameObjects = new List<GameObject>();
            
            foreach(var deck in m_cardDecks)
            {
                m_deckDictionary.Add(deck.CardDeck.DeckType, deck.CardDeck);
                m_configDictionary.Add(deck.CardDeck.DeckType, deck.Config);
            }

            // Register Events
            m_drawCardButton.onClick.AddListener(OnDrawCardClick);
            SetState(DeckManagerState.PreGame);
        }

        private void SetState(DeckManagerState newState)
        {
            switch(newState)
            {
                case DeckManagerState.PreGame:
                    m_drawCardButton.gameObject.SetActive(false);
                    m_cardContainer.gameObject.SetActive(false);
                    break;
                case DeckManagerState.Dealing:
                    m_drawCardButton.gameObject.SetActive(true);
                    m_cardContainer.gameObject.SetActive(true);
                    break;
                case DeckManagerState.Scoring:
                    m_drawCardButton.gameObject.SetActive(false);
                    m_cardContainer.gameObject.SetActive(false);
                    break;
            }

            m_state = newState;
        }

        protected override void OnDestroy()
        {
            // Unregister Events
            m_drawCardButton.onClick.RemoveListener(OnDrawCardClick);
            base.OnDestroy();
        }

        public void SetCardDeck(EDeck deckToUse)
		{
            if (!m_deckDictionary.ContainsKey(deckToUse))
			{
				Debug.LogError("[CardDeckManager] Card Deck type cannot be found in dictionary.");
				return;
			}

            m_currentDeck = m_deckDictionary[deckToUse].GetCards();
            m_currentConfig = m_configDictionary[deckToUse];


        }

        public void DealHand()
        {
            SetState(DeckManagerState.Dealing);
            DealCardDeck(k_maxCardsInHand);
        }

        public void StopDealing()
        {
            SetState(DeckManagerState.Scoring);
        }

		private void DealCardDeck(int numOfCards)
		{
            if (m_currentDeck == null || m_currentDeck.Count == 0)
            {
                Debug.LogError("[CardDeckManager] There are no cards in deck.");
                return;
            }

            if (numOfCards < 1)
            {
                Debug.LogError("[CardDeckManager] You must deal at least one card.");
                return;
            }

            DestroyCards();

            ShuffleDeck(m_currentDeck);

            m_playersHand = DrawCards(numOfCards);

            InstantiateCards();
        }

		private void ShuffleDeck(List<Card> list)
		{
            for (int i = list.Count - 1; i > 0; i--)
			{
                int randomIndex = UnityEngine.Random.Range(0, i + 1);
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
            foreach (var card in m_currentDeck)
            {
                if (pickedCards.Count >= numCards)
                {
                    break;
                }

                int maxAllowed = m_currentConfig.GetMaxForCardType(card.cardType);

                if (m_cardTypeCounts[card.cardType] < maxAllowed)
                {
                    pickedCards.Add(card);
                    cardsToRemove.Add(card);
                    m_cardTypeCounts[card.cardType]++;
                }
            }

            foreach (var card in cardsToRemove)
            {
                m_currentDeck.Remove(card);
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
            m_currentDeck = new List<Card>();

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
                && m_currentDeck.Count > 0)
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

    public enum DeckManagerState
    {
        PreGame,
        Dealing,
        Scoring
    }

    public enum EDeck
    {
        ItemDeck,
        PersonDeck
    }

    [Serializable]
    public struct Decks
    {
        public CardDeck CardDeck;
        public CardDeckConfig Config;
    }
}
