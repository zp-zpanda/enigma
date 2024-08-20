using System;
using Xunit;
using ZP.CSharp.Enigma;
using ZP.CSharp.Enigma.Tests;

namespace ZP.CSharp.Enigma.Tests
{
    public class EntrywheelTests
    {
        public static TheoryData<(int, int), (int, int)> CanAddIntPairsData => new ()
        {
            {(0, 1), (1, 0)},
            {(int.MinValue, int.MaxValue), (int.MaxValue, int.MinValue)}
        };

        public static TheoryData<(char, char), (char, char)> CanAddCharPairsData => new ()
        {
            {('a', 'b'), ('b', 'a')},
            {(char.MinValue, char.MaxValue), (char.MaxValue, char.MinValue)}
        };

        [Theory]
        [MemberData(nameof(CanAddIntPairsData))]
        [MemberData(nameof(CanAddCharPairsData))]
        public void CanAddPairs<T>((T, T) pair1, (T, T) pair2)
        {
            var entrywheel = new Entrywheel<T>(pair1, pair2);
            Assert.Contains(pair1, entrywheel.Pairs);
            Assert.Contains(pair2, entrywheel.Pairs);
        }

        [Theory]
        [InlineData(new[]{0, 1, 2, 3}, new[]{2, 3, 0, 1}, new[]{0, 1, 2, 3}, new[]{2, 3, 0, 1})]
        [InlineData(new[]{'a', 'b', 'c', 'd'}, new[]{'c', 'd', 'a', 'b'}, new[]{'a', 'b', 'c', 'd'}, new[]{'c', 'd', 'a', 'b'})]
        public void CanMassConstructPairs<T>(T[] pArr, T[] rArr, T[] pExpected, T[] rExpected)
        {
            var entrywheel = new Entrywheel<T>(pArr, rArr);
            Assert.All(entrywheel.Pairs, (p, idx) => Assert.Equal(p, (pExpected[idx], rExpected[idx])));
        }

        [Theory]
        [InlineData(new[]{0, 1, 2, 3, 4}, new[]{1, 2, 3, 4, 0}, 2, false, 3)]
        [InlineData(new[]{0, 1, 2, 3, 4}, new[]{1, 2, 3, 4, 0}, 5, true, null)]
        [InlineData(new[]{'a', 'b', 'c', 'd', 'e'}, new[]{'b', 'c', 'd', 'e', 'a'}, 'c', false, 'd')]
        [InlineData(new[]{'a', 'b', 'c', 'd', 'e'}, new[]{'b', 'c', 'd', 'e', 'a'}, 'f', true, null)]
        public void CanPassFromPlugboard<T>(T[] pArr, T[] rArr, T input, bool willThrow, T? expected)
        {
            var action = () => Assert.Equal(expected, new Entrywheel<T>(pArr, rArr).FromPlugboard(input));
            if (willThrow)
            {
                var ex = Record.Exception(action);
                Assert.IsType<CharacterNotFoundException>(ex);
            }
            else
            {
                action();
            }
        }

        [Theory]
        [InlineData(new[]{1, 2, 3, 4, 0}, new[]{0, 1, 2, 3, 4}, 2, false, 3)]
        [InlineData(new[]{1, 2, 3, 4, 0}, new[]{0, 1, 2, 3, 4}, 5, true, null)]
        [InlineData(new[]{'b', 'c', 'd', 'e', 'a'}, new[]{'a', 'b', 'c', 'd', 'e'}, 'c', false, 'd')]
        [InlineData(new[]{'b', 'c', 'd', 'e', 'a'}, new[]{'a', 'b', 'c', 'd', 'e'}, 'f', true, null)]
        public void CanPassFromRotor<T>(T[] pArr, T[] rArr, T input, bool willThrow, T? expected)
        {
            var action = () => Assert.Equal(expected, new Entrywheel<T>(pArr, rArr).FromRotor(input));
            if (willThrow)
            {
                var ex = Record.Exception(action);
                Assert.IsType<CharacterNotFoundException>(ex);
            }
            else
            {
                action();
            }
        }
    }
}
