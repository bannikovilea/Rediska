﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rediska.Tests.Checks;
using Rediska.Tests.Protocol.Responses;

namespace Rediska.Tests
{
    public sealed class InputResponse : Connection.Response
    {
        private readonly Input input;

        public InputResponse(Input input)
        {
            this.input = input;
        }

        public override Task<DataType> ReadAsync()
        {
            return Task.FromResult(
                Read()
            );
        }

        private DataType Read()
        {
            var type = input.ReadMagic();
            if (type == Magic.SimpleString)
            {
                var content = input.ReadSimpleString();
                return new SimpleString(content);
            }

            if (type == Magic.Error)
            {
                var content = input.ReadSimpleString();
                return new Error(content);
            }

            if (type == Magic.Integer)
            {
                var value = input.ReadInteger();
                return new Protocol.Responses.Integer(value);
            }

            if (type == Magic.BulkString)
            {
                return input.ReadBulkString();
            }

            if (type == Magic.Array)
            {
                var count = input.ReadInteger();
                var items = new DataType[count];
                for (var i = 0; i < items.Length; i++)
                {
                    items[i] = Read();
                }

                return new PlainArray(items);
            }

            throw new NotSupportedException($"Could not recognize magic: {type}");
        }
    }
}