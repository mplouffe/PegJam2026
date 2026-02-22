using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace lvl_0
{
    [CreateAssetMenu(fileName = "New Level Deck", menuName = "Cards/New Level Deck")]
    public class LevelDeck : ScriptableObject
    {
        public List<Level> levelMap = new List<Level>();

        public List<Level> GetLevels()
        {
            return new List<Level>(levelMap);
        }

        public Level GetLevelByName(string name)
        {
            foreach (Level level in levelMap)
            {
                if (level.LevelName == name)
                {
                    return new Level()
                    {
                        LevelName = level.LevelName,
                        LevelGoals = new List<Goal>(level.LevelGoals)
                    };

                }
            }
            return null;
        }

        public List<Level> GetRandomLevels(int count = 3)
        {
            return levelMap.OrderBy(_ => Random.Range(0, int.MaxValue)).Take(count).ToList();
        }
    }
}
