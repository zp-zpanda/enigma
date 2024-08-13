using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ZP.CSharp.Enigma;

namespace ZP.CSharp.Enigma
{
    /**
    <summary>The entrywheel.</summary>
    <typeparam name="T">The data this entrywheel works with.</typeparam>
    */
    public class Entrywheel<T>
    {
        private (T Plugboard, T Rotor)[] _Pairs;
        /**
        <summary>The pairs in this entrywheel.</summary>
        */
        public (T Plugboard, T Rotor)[] Pairs
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
        <summary>Creates an entrywheel with the pairs provided.</summary>
        <param name="pairs">The pairs provided.</param>
        <returns>An entrywheel created with the pairs provided.</returns>
        */
        public Entrywheel(params (T Plugboard, T Rotor)[] pairs)
        {
            Pairs = pairs;
        }

        /**
        <summary>Maps a datum coming from the plugboard.</summary>
        <param name="data">The datum to map.</param>
        <returns>The mapped datum.</returns>
        */
        public T FromPlugboard(T data) => Pairs.Where(p => p.Plugboard.Equals(data)).Single().Rotor;

        /**
        <summary>Maps a datum coming from the rotor.</summary>
        <param name="data">The datum to map.</param>
        <returns>The mapped datum.</returns>
        */
        public T FromRotor(T data) => Pairs.Where(p => p.Rotor.Equals(data)).Single().Plugboard;
    }
}
