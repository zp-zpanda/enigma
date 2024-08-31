using System;
using ZP.CSharp.Enigma;

namespace ZP.CSharp.Enigma
{
    /**
    <summary>Represents the situation where no character could be found.</summary>
    <remarks>
        <para><seealso cref="Entrywheel{T}.FromPlugboard(T)" /></para>
        <para><seealso cref="Entrywheel{T}.FromRotor(T)" /></para>
        <para><seealso cref="Rotor{T}.FromEntrywheel(T)" /></para>
        <para><seealso cref="Rotor{T}.FromReflector(T)" /></para>
    </remarks>
    */
    public class CharacterNotFoundException : Exception
    {
        /**
        <summary>The error message.</summary>
        */
        public const string ErrorMessage = "Character not found.";

        /**
        <summary>Creates a <see cref="CharacterNotFoundException" />.</summary>
        <returns>A <see cref="CharacterNotFoundException" />.</returns>
        <remarks><seealso cref="Exception(string)" /></remarks>
        */
        public CharacterNotFoundException() : base(ErrorMessage)
        {}

        /**
        <summary>Creates a <see cref="CharacterNotFoundException" /> with a reference to the <see cref="Exception" /> that caused it.</summary>
        <param name="inner">The <see cref="Exception" /> that caused this exception.</param>
        <returns>A <see cref="CharacterNotFoundException" /> with a reference to the <see cref="Exception" /> that caused it.</returns>
        <remarks><seealso cref="Exception(string, Exception)" /></remarks>
        */
        public CharacterNotFoundException(Exception inner) : base(ErrorMessage, inner)
        {}
    }
}
