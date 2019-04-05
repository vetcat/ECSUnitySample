namespace Signals
{
    public struct SignalUiLayerWantsAddPlayer
    {
        public int Count { get; }

        public SignalUiLayerWantsAddPlayer(int count)
        {
            Count = count;
        }
    }
}