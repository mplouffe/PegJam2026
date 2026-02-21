using System;
using UnityEngine;

namespace lvl_0
{
    [Serializable]
    public class Card
    {
        public string internalDescription;
        public string cardDesciption;
        public Sprite cardSprite;
        public int cardValue;
        public ECardType cardType;
        public BoolGrid itemShape;
        public Color cardColor;
    }
}
