﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rediska.Tests
{
    public sealed class Cashier : IDisposable
    {
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly AtomicLong currentNumber = new AtomicLong();

        public async Task<Ticket> AcquireTicketAsync()
        {
            await semaphore.WaitAsync().ConfigureAwait(false);
            return new Ticket(currentNumber.Read(), this);
        }

        public bool Busy => semaphore.CurrentCount == 0;

        public void Dispose()
        {
            semaphore.Dispose();
        }

        private void Release(long number)
        {
            var result = currentNumber.IfEqualTo(number).Write(number + 1);
            if (result.Success)
            {
                semaphore.Release();
            }
            else
            {
                throw new InvalidOperationException("Ticket is outdated");
            }
        }
        
        public struct Ticket : IDisposable
        {
            private readonly Cashier cashier;

            public Ticket(long number, Cashier cashier)
            {
                this.cashier = cashier;
                Number = number;
            }

            public long Number { get; }
            public void Dispose() => cashier.Release(Number);
        }
    }
}