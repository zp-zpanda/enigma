using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ZP.CSharp.Enigma;

namespace ZP.CSharp.Enigma
{
    /**
    <summary>The interface for the Enigma.</summary>
    <typeparam name="T">The data this Enigma works with.</typeparam>
    */
    public class Enigma<T>
    {
        private Entrywheel<T> _Entrywheel;
        /**
        <summary>The entrywheel of this Enigma.</summary>
        */
        public Entrywheel<T> Entrywheel
        {
            get => _Entrywheel;
            [MemberNotNull(nameof(_Entrywheel))]
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                _Entrywheel = value;
            }
        }

        private Rotor<T>[] _Rotors;
        /**
        <summary>The rotors of this Enigma.</summary>
        */
        public Rotor<T>[] Rotors
        {
            get => _Rotors;
            [MemberNotNull(nameof(_Rotors))]
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                _Rotors = value;
            }
        }

        private Reflector<T> _Reflector;
        /**
        <summary>The reflector of this Enigma.</summary>
        */
        public Reflector<T> Reflector
        {
            get => _Reflector;
            [MemberNotNull(nameof(_Reflector))]
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                _Reflector = value;
            }
        }

        /**
        <summary>Creates an Enigma with the entrywheel, rotors, and reflector provided.</summary>
        <param name="entrywheel">The entrywheel provided.</param>
        <param name="rotors">The rotors provided.</param>
        <param name="reflector">The reflector provided.</param>
        <returns>An Enigma created with the entrywheel, rotors, and reflector provided.</returns>
        */
        public Enigma(Entrywheel<T> entrywheel, Reflector<T> reflector, params Rotor<T>[] rotors)
        {
            Entrywheel = entrywheel;
            Rotors = rotors;
            Reflector = reflector;
        }

        /**
        <summary>Steps the Enigma.</summary>
        <param name="doubleStepping"><see langword="true" /> to enable double stepping, or <see langword="false" /> to disable it.</param>
        <remarks>Both double stepping and normal stepping have been implemented because Enigmas use both of them, although in different models.</remarks>
        */
        protected void Step(bool doubleStepping = false)
        {
            Rotor<T>? prev, next;
            Rotor<T> curr;
            var canStep = new bool[Rotors.Length];
            for (var i = 0; i < Rotors.Length; i++)
            {
                prev = i != 0 ? Rotors[i - 1] : null;
                curr = Rotors[i];
                next = i != (Rotors.Length - 1) ? Rotors[i + 1] : null;
                canStep[i] = doubleStepping
                    ? (prev is null || prev.AllowNextToStep() || (curr.AllowNextToStep() && next is not null))
                    : (prev?.AllowNextToStep() ?? true);
            }

            for (var i = 0; i < Rotors.Length; i++)
            {
                if (canStep[i])
                {
                    Rotors[i].Step();
                }
            }
        }

        /**
        <summary>Steps the Enigma.</summary>
        */
        public virtual void Step() => Step(false);

        /**
        <summary>Runs the Enigma on a datum.</summary>
        <returns>The ciphered datum.</returns>
        */
        public virtual T Run(T data)
        {
            Step();
            var ret = data;
            ret = Entrywheel.FromPlugboard(ret);
            foreach (var rotor in Rotors)
            {
                ret = rotor.FromEntrywheel(ret);
            }
            ret = Reflector.Reflect(ret);
            foreach (var rotor in Rotors.Reverse())
            {
                ret = rotor.FromReflector(ret);
            }
            ret = Entrywheel.FromRotor(ret);
            return ret;
        }

        /**
        <summary>Runs the Enigma on a payload of data.</summary>
        <returns>The ciphered payload.</returns>
        */
        public IEnumerable<T> Run(IEnumerable<T> data) => data.Select(Run);
    }
}
