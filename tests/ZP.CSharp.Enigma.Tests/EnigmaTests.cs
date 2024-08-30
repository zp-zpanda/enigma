using System;
using System.Collections.Generic;
using Xunit;
using ZP.CSharp.Enigma;
using ZP.CSharp.Enigma.Tests;

namespace ZP.CSharp.Enigma.Tests
{
    public class EnigmaTests
    {
        public Enigma<int> TestEnigma => new Enigma<int>(
            new Entrywheel<int>((0, 0), (1, 1), (2, 2), (3, 3)),
            new Reflector<int>((0, 1), (2, 3)),
            new Rotor<int>(0, new[]{0}, (0, 2), (1, 0), (2, 3), (3, 1))
        );

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void WillNotReturnInputAsOutput(int c)
        {
            var enigma = TestEnigma;
            var result = enigma.Run(c);
            Assert.NotEqual(c, result);
        }

        [Theory]
        [InlineData(new[]{0, 1, 2, 3}, new[]{1, 3, 3, 1})]
        [InlineData(new[]{3, 2, 1, 0}, new[]{2, 0, 0, 2})]
        public void WillReturnCipheredOutput(int[] plain, int[] cipher)
        {
            var enigma = TestEnigma;
            var result = enigma.Run(plain);
            Assert.Equal(cipher, result);
        }
    }
}
