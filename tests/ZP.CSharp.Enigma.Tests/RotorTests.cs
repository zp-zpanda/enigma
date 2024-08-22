using System;
using Xunit;
using System.Linq;
using ZP.CSharp.Enigma;
using ZP.CSharp.Enigma.Tests;

namespace ZP.CSharp.Enigma.Tests
{
    public class RotorTests
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
            var rotor = new Rotor<T>(0, new[]{0}, pair1, pair2);
            Assert.Contains(pair1, rotor.Pairs);
            Assert.Contains(pair2, rotor.Pairs);
        }
        [Theory]
        [InlineData(new[]{0, 1, 2, 3}, new[]{2, 3, 0, 1}, new[]{0, 1, 2, 3}, new[]{2, 3, 0, 1})]
        [InlineData(new[]{'a', 'b', 'c', 'd'}, new[]{'c', 'd', 'a', 'b'}, new[]{'a', 'b', 'c', 'd'}, new[]{'c', 'd', 'a', 'b'})]
        public void CanMassConstructPairs<T>(T[] eArr, T[] rArr, T[] eExpected, T[] rExpected)
        {
            var rotor = new Rotor<T>(0, new[]{0}, eArr, rArr);
            Assert.All(rotor.Pairs, (p, idx) => Assert.Equal(p, (eExpected[idx], rExpected[idx])));
        }

        [Theory]
        [InlineData(new[]{0, 1, 2, 3, 4}, new[]{1, 2, 3, 4, 0}, 2, false, 3)]
        [InlineData(new[]{0, 1, 2, 3, 4}, new[]{1, 2, 3, 4, 0}, 5, true, null)]
        [InlineData(new[]{'a', 'b', 'c', 'd', 'e'}, new[]{'b', 'c', 'd', 'e', 'a'}, 'c', false, 'd')]
        [InlineData(new[]{'a', 'b', 'c', 'd', 'e'}, new[]{'b', 'c', 'd', 'e', 'a'}, 'f', true, null)]
        public void CanPassFromEntryWheel<T>(T[] eArr, T[] rArr, T input, bool willThrow, T? expected)
        {
            var action = () => Assert.Equal(expected, new Rotor<T>(0, new[]{0}, eArr, rArr).FromEntrywheel(input));
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
        public void CanPassFromReflector<T>(T[] eArr, T[] rArr, T input, bool willThrow, T? expected)
        {
            var action = () => Assert.Equal(expected, new Rotor<T>(0, new[]{0}, eArr, rArr).FromReflector(input));
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
        [InlineData(new[]{0, 1, 2, 3, 4}, new[]{4, 3, 2, 1, 0}, 0, new[]{4, 2, 0, 3, 1})]
        public void CanRotate<T>(T[] eArr, T[] rArr, T e, T[] rExpectedArr)
        {
            var rotor = new Rotor<T>(0, new[]{0}, eArr, rArr);
            Assert.All(rExpectedArr, rExpected => {
                var res = rotor.FromEntrywheel(e);
                Assert.Equal(rExpected, res);
                rotor.Step();
            });
        }

        [Theory]
        [InlineData(new[]{0, 1, 2, 3, 4}, new[]{4, 3, 2, 1, 0}, 0, 0, 4)]
        [InlineData(new[]{0, 1, 2, 3, 4}, new[]{4, 3, 2, 1, 0}, 2, 0, 0)]
        [InlineData(new[]{0, 1, 2, 3, 4}, new[]{4, 3, 2, 1, 0}, 4, 0, 1)]
        public void CanPassBasedOnPosition<T>(T[] eArr, T[] rArr, int pos, T eExpected, T rExpected)
        {
            var rotor = new Rotor<T>(pos, new[]{0}, eArr, rArr);
            Assert.Equal(rExpected, rotor.FromEntrywheel(eExpected));
            Assert.Equal(eExpected, rotor.FromReflector(rExpected));
        }

        [Theory]
        [InlineData(5, new[]{2})]
        [InlineData(14, new[]{7, 13})]
        [InlineData(345, new[]{13, 96, 169})]
        public void CanAllowNextToStepBasedOnPosition(int total, int[] notch)
        {
            var map = Enumerable.Range(0, total).ToArray();
            var rotor = new Rotor<int>(0, notch, map, map);
            var canStepArr = Enumerable.Range(0, total).Select(pos => notch.Contains(pos));
            Assert.All(canStepArr, canStep => {
                Assert.Equal(canStep, rotor.AllowNextToStep());
                rotor.Step();
            });
        }
    }
}
