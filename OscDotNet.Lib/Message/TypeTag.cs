using System;
using System.Collections.Generic;
using System.Text;

namespace OscDotNet.Lib
{
    /// <summary>
    /// An Open Sound Control type tag value.
    /// See http://opensoundcontrol.org/spec-1_0
    /// </summary>
    public enum TypeTag 
        : byte
    {
        Unknown = 0,
        OscInt32 = (byte)'i',       // int32
        OscFloat32 = (byte)'f',     // float32
        OscString = (byte)'s',      // Osc-string
        OscBlob = (byte)'b',        // Osc-blob

/*      // TODO
        OscInt64 = (byte)'h',       // 64 bit big-endian two's complement integer
        OscTimetag = (byte)'t',     // OSC-timetag
        OscDouble = (byte)'d',      // 64 bit ("double") IEEE 754 floating point number
        OscSymbol = (byte)'S',      // Alternate type represented as an OSC-string (for example, for systems that differentiate "symbols" from "strings")
        OscChar = (byte)'c',        // an ascii character, sent as 32 bits(byte)'r',    // 32 bit RGBA color
        OscMidi = (byte)'m',        // 4 byte MIDI message. Bytes from MSB to LSB are: port id, status byte, data1, data2
        OscTrue = (byte)'T',        // True. No bytes are allocated in the argument data.
        OscFalse = (byte)'F',       // False. No bytes are allocated in the argument data.
        OscNil = (byte)'N',         // Nil. No bytes are allocated in the argument data.
        OscInfinitum = (byte)'I',	// Infinitum. No bytes are allocated in the argument data.
        OscArrayBegin = (byte)'[',	// Indicates the beginning of an array. The tags following are for data in the Array until a close brace tag is reached.
        OscArrayEnd = (byte)']'	    // Indicates the end of an array.
*/
    }
}

