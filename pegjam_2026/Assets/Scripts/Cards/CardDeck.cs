using System.Collections.Generic;
using UnityEngine;

namespace lvl_0
{
    [CreateAssetMenu(fileName = "New Card Deck", menuName = "Cards/New Card Deck")]
    public class CardDeck : ScriptableObject
    {
        public List<Card> cardsMap = new List<Card>();

        public List<Card> GetCards()
        {
            return new List<Card>(cardsMap);
        }
    }
}
