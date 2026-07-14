namespace PinionCore.Remote
{
    // Soul 端框架介面：SoulMethodHandler 藉此得知 Spirit 是否已 Dispose 並訂閱 Dispose 通知。
    public interface ISpiritSoul
    {
        bool Disposed { get; }

        // add 存取子具補發語意：若已 Dispose，加入 handler 時立即觸發（消除綁定與 Dispose 之間的競態）。
        event System.Action DisposeEvent;
    }
}
