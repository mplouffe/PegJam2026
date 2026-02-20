using System;
using UnityEngine;

namespace lvl_0
{
    [Serializable]
    public class Card
    {
        public string internalDescription;
        public string cardDesciption;
        public int cardValue;
        public Sprite cardSprite;
        public ECardType cardType;
    }
}
