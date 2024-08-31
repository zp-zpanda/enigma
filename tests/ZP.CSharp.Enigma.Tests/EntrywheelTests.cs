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

        public static TheoryData<int[], int[], (int, int)[]> CanMassAddIntPairsData => new ()
        {
            {new[]{0, 1, 2, 3}, new[]{2, 3, 0, 1}, new[]{(0, 2), (1, 3), (2, 0), (3, 1)}},
            {
                new[]{(int)short.MinValue, (int)short.MaxValue, int.MinValue, int.MaxValue},
                new[]{int.MinValue, int.MaxValue, (int)short.MinValue, (int)short.MaxValue},
                new[]{
                    ((int)short.MinValue, int.MinValue), ((int)short.MaxValue, int.MaxValue),
                    (int.MinValue, (int)short.MinValue), (int.MaxValue, (int)short.MaxValue)
                }
            }
        };

        public static TheoryData<char[], char[], (char, char)[]> CanMassAddCharPairsData => new ()
        {
            {
                new[]{'a', 'b', 'c', 'd'}, new[]{'c', 'd', 'a', 'b'},
                new[]{('a', 'c'), ('b', 'd'), ('c', 'a'), ('d', 'b')}
            },
            {
                new[]{(char)byte.MinValue, (char)byte.MaxValue, char.MinValue, char.MaxValue},
                new[]{char.MinValue, char.MaxValue, (char)byte.MinValue, (char)byte.MaxValue},
                new[]{
                    ((char)byte.MinValue, char.MinValue), ((char)byte.MaxValue, char.MaxValue),
                    (char.MinValue, (char)byte.MinValue), (char.MaxValue, (char)byte.MaxValue)
                }
            }
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
        [MemberData(nameof(CanMassAddIntPairsData))]
        [MemberData(nameof(CanMassAddCharPairsData))]
        public void CanMassAddPairs<T>(T[] pArr, T[] rArr, (T, T)[] pairs)
        {
            var entrywheel = new Entrywheel<T>(pArr, rArr);
            Assert.All(entrywheel.Pairs, (p, idx) => Assert.Equal(p, pairs[idx]));
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
