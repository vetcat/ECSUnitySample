namespace Signals
{
    public struct SignalEcsLayerEnemyCountUpdate
    {
        public int Count { get; }

        public SignalEcsLayerEnemyCountUpdate(int count)
        {
            Count = count;
        }
    }
}