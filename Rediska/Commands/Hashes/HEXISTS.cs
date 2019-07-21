﻿using Rediska.Protocol.Requests;
using Rediska.Protocol.Responses.Visitors;

namespace Rediska.Commands.Hashes
{
    public sealed class HEXISTS : Command<HEXISTS.Response>
    {
        public enum Response
        {
            FieldExists,
            KeyOrFieldNotExists
        }

        private static readonly BulkString name = new BulkString("HEXISTS");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(
                response => response == 1
                    ? Response.FieldExists
                    : Response.KeyOrFieldNotExists
            );

        private readonly Key key;
        private readonly Key field;

        public HEXISTS(Key key, Key field)
        {
            this.key = key;
            this.field = field;
        }

        public override DataType Request => new Array(
            name,
            key.ToBulkString(),
            field.ToBulkString()
        );

        public override Visitor<Response> ResponseStructure => responseStructure;
    }
}