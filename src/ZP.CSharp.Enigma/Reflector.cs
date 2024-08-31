using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ZP.CSharp.Enigma;

namespace ZP.CSharp.Enigma
{
    /**
    <summary>The reflector.</summary>
    <typeparam name="T">The data this reflector works with.</typeparam>
    */
    public class Reflector<T>
    {
        private (T, T)[] _Pairs;
        /**
        <summary>The pairs in this reflector.</summary>
        */
        public (T, T)[] Pairs
        {
            get => _Pairs;
            [MemberNotNull(nameof(_Pairs))]
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                _Pairs = value;
            }
        }

        /**
        <summary>Creates a reflector with the pairs provided.</summary>
        <param name="pairs">The pairs provided.</param>
        <returns>A reflector created with the pairs provided.</returns>
        */
        public Reflector(params (T, T)[] pairs)
        {
            Pairs = pairs;
        }

        /**
        <summary>Creates a reflector with the pairs-adjacent mapping provided.</summary>
        <param name="map">The mapping provided.</param>
        <returns>A reflector created with the mapping provided.</returns>
        */
        public Reflector(T[] map)
        {
            if (map.Length % 2 != 0)
            {
                throw new ArgumentException("Mapping has unpaired characters. Expected mapping: \"{pair1}{pair2}...\"");
            }
            var pairs = new List<(T, T)>();
            for (var i = 0; i < map.Length / 2; i++)
            {
                pairs.Add((map[i * 2], map[i * 2 + 1]));
            }
            Pairs = pairs.ToArray();
        }

        /**
        <summary>Reflects a datum.</summary>
        <param name="data">The datum to reflect.</param>
        <returns>The reflected datum.</returns>
        */
        public T Reflect(T data)
        {
            try
            {
                var foundPair = Pairs.Where(p => new[]{p.Item1, p.Item2}.Contains(data)).Single();
                return new[]{foundPair.Item1, foundPair.Item2}.Except(new[]{data}).Single();
            }
            catch (Exception ex)
            {
                throw new CharacterNotFoundException(ex);
            }
        }
    }
}
