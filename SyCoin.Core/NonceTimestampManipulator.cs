using System;
using System.Threading.Tasks;
using System.Threading;

namespace SyCoin.Core
{
    internal class NonceTimestampManipulator : IDisposable
    {
        public long CurrentTimestamp { get; private set; }
        public uint CurrentNonce { get; private set; }
        public bool IsDisposed { get; private set; }
        private readonly Task RefreshValuesTask;

        public NonceTimestampManipulator()
        {
            ResetValues();
            IsDisposed = false;
            RefreshValuesTask = new Task(() =>
            {
                if (IsDisposed) return;
                ResetValues();
                Thread.Sleep(1000);
            });
        }

        public uint GetNextNonce()
        {
            return CurrentNonce++;
        }

        void ResetValues()
        {
            CurrentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            CurrentNonce = 1;
        }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
