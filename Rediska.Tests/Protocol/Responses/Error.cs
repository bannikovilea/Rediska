﻿using Rediska.Tests.Protocol.Responses.Visitors;

namespace Rediska.Tests.Protocol.Responses
{
    public sealed class Error : DataType
    {
        public Error(string content)
        {
            Content = content;
        }

        public string Content { get; }
        public override string ToString() => $"-{Content}";

        public override T Accept<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}