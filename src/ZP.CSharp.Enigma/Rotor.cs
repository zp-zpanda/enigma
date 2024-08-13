using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ZP.CSharp.Enigma;

namespace ZP.CSharp.Enigma
{
    /**
    <summary>The rotor.</summary>
    <typeparam name="T">The data this rotor works with.</typeparam>
    */
    public class Rotor<T>
    {
        private (T Entrywheel, T Reflector)[] _Pairs;
        /**
        <summary>The pairs in this rotor.</summary>
        */
        public (T Entrywheel, T Reflector)[] Pairs
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
        <summary>Creates a rotor with the pairs provided.</summary>
        <param name="pairs">The pairs provided.</param>
        <returns>A rotor created with the pairs provided.</returns>
        */
        public Rotor(params (T Entrywheel, T Reflector)[] pairs)
        {
            Pairs = pairs;
        }

        /**
        <summary>Maps a datum coming from the entrywheel.</summary>
        <param name="data">The datum to map.</param>
        <returns>The mapped datum.</returns>
        */
        public T FromEntrywheel(T data) => Pairs.Where(p => p.Entrywheel.Equals(data)).Single().Reflector;

        /**
        <summary>Maps a datum coming from the reflector.</summary>
        <param name="data">The datum to map.</param>
        <returns>The mapped datum.</returns>
        */
        public T FromReflector(T data) => Pairs.Where(p => p.Reflector.Equals(data)).Single().Entrywheel;
    }
}
