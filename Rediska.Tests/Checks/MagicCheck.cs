﻿using Rediska.Protocol;

namespace Rediska.Tests.Checks
{
    public sealed class MagicCheck : Check
    {
        private readonly Magic magic;

        public MagicCheck(Magic magic)
        {
            this.magic = magic;
        }

        public override void Verify(Input input)
        {
            var actualMagic = input.ReadMagic();
            if (actualMagic != magic)
                throw new CheckException("Different magic", this, input, actualMagic);
        }
    }
}