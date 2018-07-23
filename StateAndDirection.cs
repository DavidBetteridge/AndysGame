namespace RandomApp
{
    internal class StateAndDirection
    {
        public StateAndDirection(GameState gameState, Direction direction)
        {
            this.GameState = gameState;
            this.Direction = direction;
        }

        public GameState GameState { get; set; }
        public Direction Direction { get; set; }
    }
}
