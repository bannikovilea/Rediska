﻿namespace Rediska.Commands.Keys
{
    using System;
    using Protocol;
    using Protocol.Visitors;

    public sealed class PERSIST : Command<PERSIST.Response>
    {
        public enum Response : byte
        {
            TimeoutNotRemoved = 0,
            TimeoutRemoved = 1
        }

        private static readonly PlainBulkString name = new PlainBulkString("PERSIST");
        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton.Then(Parse);
        private readonly Key key;

        public PERSIST(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString()
        );

        public override Visitor<Response> ResponseStructure => responseStructure;

        private static Response Parse(long response) => response switch
        {
            0 => Response.TimeoutNotRemoved,
            1 => Response.TimeoutRemoved,
            _ => throw new ArgumentException("Expected 0 or 1", nameof(response))
        };
    }
}