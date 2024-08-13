using System;
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
        <summary>Reflects a datum.</summary>
        <param name="data">The datum to reflect.</param>
        <returns>The reflected datum.</returns>
        */
        public T Reflect(T data)
        {
            var foundPair = Pairs.Where(p => new[]{p.Item1, p.Item2}.Contains(data)).Single();
            return new[]{foundPair.Item1, foundPair.Item2}.Except(new[]{data}).Single();
        }
    }
}
