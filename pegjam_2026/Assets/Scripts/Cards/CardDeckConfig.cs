using System.Collections.Generic;
using UnityEngine;

namespace lvl_0
{
    [CreateAssetMenu(fileName = "CardDeckConfig", menuName = "Cards/New Card Deck Config")]
    public class CardDeckConfig : ScriptableObject
    {
        public List<CardTypeLimit> cardTypeLimits = new List<CardTypeLimit>();

        public int GetMaxForCardType(ECardType type)
        {
            foreach (var limit in cardTypeLimits)
            {
                if (limit.cardType == type)
                {
                    return limit.maxAllowed;
                }
            }

            return int.MaxValue;
        }
    }
}