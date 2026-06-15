using Studio.Data;

namespace Studio.Core
{
    public sealed class GameContext
    {
        public GameConfig Config { get; }

        public GameContext(GameConfig config)
        {
            Config = config;
        }
    }
}
