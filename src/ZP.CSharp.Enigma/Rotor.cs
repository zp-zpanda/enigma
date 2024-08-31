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
        private int _Position;
        /**
        <summary>The position of this rotor.</summary>
        */
        public int Position
        {
            get => _Position;
            set => _Position = value;
        }

        private int[] _Notch;
        /**
        <summary>The notches of this rotor.</summary>
        */
        public int[] Notch
        {
            get => _Notch;
            [MemberNotNull(nameof(_Notch))]
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                if (value.Length == 0)
                {
                    throw new ArgumentException("Rotor must have at least one notch.");
                }
                _Notch = value;
            }
        }

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
        <summary>Creates a rotor with the position, notches, and pairs provided.</summary>
        <param name="pos">The position provided.</param>
        <param name="notch">The notches provided.</param>
        <param name="pairs">The pairs provided.</param>
        <returns>A rotor created with the position, notches, and pairs provided.</returns>
        */
        public Rotor(int pos, int[] notch, params (T Entrywheel, T Reflector)[] pairs)
        {
            Position = pos;
            Notch = notch;
            Pairs = pairs;
        }

        /**
        <summary>Creates a rotor with the position, notches, entrywheel-side, and reflector-side mappings provided.</summary>
        <param name="pos">The position provided.</param>
        <param name="notch">The notches provided.</param>
        <param name="entrywheel">The entrywheel-side mapping provided.</param>
        <param name="reflector">The reflector-side mapping provided.</param>
        <returns>A rotor created with the position, notches, and mappings provided.</returns>
        */
        public Rotor(int pos, int[] notch, T[] entrywheel, T[] reflector)
        {
            if (entrywheel.Length != reflector.Length)
            {
                throw new ArgumentException("Unable to create pairs: Array lengths do not match.");
            }
            Position = pos;
            Notch = notch;
            Pairs = entrywheel.Zip(reflector).ToArray();
        }

        /**
        <summary>The domain of this rotor.</summary>
        <para>TODO: Allow domain other than entrywheel order</para>
        */
        private T[] Domain => Pairs.Select(p => p.Entrywheel).ToArray();

        /**
        <summary>Shifts a datum entering the rotor according to this rotor's position.</summary>
        <param name="data">The datum to be shifted.</param>
        <returns>The shifted datum.</returns>
        */
        private T ShiftIn(T data)
        {
            var idx = Array.IndexOf(Domain, data);
            var len = Domain.Length;
            return Domain[(idx + Position) % len];
        }

        /**
        <summary>Shifts a datum leaving the rotor according to this rotor's position.</summary>
        <param name="data">The datum to be shifted.</param>
        <returns>The shifted datum.</returns>
        */
        private T ShiftOut(T data)
        {
            var idx = Array.IndexOf(Domain, data);
            var len = Domain.Length;
            return Domain[(idx - Position + len) % len];
        }

        /**
        <summary>Maps a datum coming from the entrywheel.</summary>
        <param name="data">The datum to map.</param>
        <returns>The mapped datum.</returns>
        */
        public T FromEntrywheel(T data)
        {
            try
            {
                return ShiftOut(Pairs.Where(p => ShiftIn(data).Equals(p.Entrywheel)).Single().Reflector);
            }
            catch (Exception ex)
            {
                throw new CharacterNotFoundException(ex);
            }
        }

        /**
        <summary>Maps a datum coming from the reflector.</summary>
        <param name="data">The datum to map.</param>
        <returns>The mapped datum.</returns>
        */
        public T FromReflector(T data)
        {
            try
            {
                return ShiftOut(Pairs.Where(p => ShiftIn(data).Equals(p.Reflector)).Single().Entrywheel);
            }
            catch (Exception ex)
            {
                throw new CharacterNotFoundException(ex);
            }
        }

        /**
        <summary>Determines whether the next rotor in line is allowed to step.</summary>
        <returns><see langword="true" /> if allowed, or <see langword="false" /> if not.</returns>
        */
        public bool AllowNextToStep() => Notch.Contains(Position);

        /**
        <summary>Steps the rotor.</summary>
        */
        public void Step() => Position = (Position + 1) % Pairs.Length;
    }
}
