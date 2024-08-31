using System;
using Xunit;
using ZP.CSharp.Enigma;
using ZP.CSharp.Enigma.Tests;

namespace ZP.CSharp.Enigma.Tests
{
    public class ReflectorTests
    {
        public static TheoryData<(int, int), (int, int)> CanAddIntPairsData => new ()
        {
            {(0, 1), (2, 3)},
            {(short.MinValue, short.MaxValue), (int.MaxValue, int.MinValue)}
        };

        public static TheoryData<(char, char), (char, char)> CanAddCharPairsData => new ()
        {
            {('a', 'b'), ('c', 'd')},
            {((char)byte.MinValue, (char)byte.MaxValue), (char.MinValue, char.MaxValue)}
        };

        public static TheoryData<int[], (int, int)[]> CanMassAddIntPairsData => new ()
        {
            {new[]{0, 1, 2, 3}, new[]{(0, 1), (2, 3)}},
            {
                new[]{short.MinValue, short.MaxValue, int.MinValue, int.MaxValue},
                new[]{(short.MinValue, short.MaxValue), (int.MinValue, int.MaxValue)}
            }
        };

        public static TheoryData<char[], (char, char)[]> CanMassAddCharPairsData => new ()
        {
            {new[]{'a', 'b', 'c', 'd'}, new[]{('a', 'b'), ('c', 'd')}},
            {
                new[]{(char)byte.MinValue, (char)byte.MaxValue, char.MinValue, char.MaxValue},
                new[]{((char)byte.MinValue, (char)byte.MaxValue), (char.MinValue, char.MaxValue)}
            }
        };

        [Theory]
        [MemberData(nameof(CanAddIntPairsData))]
        [MemberData(nameof(CanAddCharPairsData))]
        public void CanAddPairs<T>((T, T) pair1, (T, T) pair2)
        {
            var reflector = new Reflector<T>(pair1, pair2);
            Assert.Contains(pair1, reflector.Pairs);
            Assert.Contains(pair2, reflector.Pairs);
        }

        [Theory]
        [MemberData(nameof(CanMassAddIntPairsData))]
        [MemberData(nameof(CanMassAddCharPairsData))]
        public void CanMassAddPairs<T>(T[] maps, (T, T)[] pairs)
        {
            var reflector = new Reflector<T>(maps);
            Assert.All(reflector.Pairs, (p, idx) => Assert.Equal(p, pairs[idx]));
        }

        [Theory]
        [InlineData(new[]{0, 1, 2, 3, 4, 5}, 2, false, 3)]
        [InlineData(new[]{0, 1, 2, 3, 4, 5}, 6, true, null)]
        [InlineData(new[]{'a', 'b', 'c', 'd', 'e', 'f'}, 'c', false, 'd')]
        [InlineData(new[]{'a', 'b', 'c', 'd', 'e', 'f'}, 'g', true, null)]
        public void CanReflect<T>(T[] maps, T input, bool willThrow, T? expected)
        {
            var action = () => Assert.Equal(expected, new Reflector<T>(maps).Reflect(input));
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
