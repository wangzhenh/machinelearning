// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma warning disable 420 // volatile with Interlocked.CompareExchange

using Float = System.Single;

using System;
using System.Threading;

namespace Microsoft.ML.Runtime.Data
{
    // REVIEW: Need better names for these and possibly a distinct namespace. These are too
    // specialized to have such prominent fully qualified names.
    public abstract class Combiner<T>
    {
        public abstract bool IsDefault(T value);
        public abstract void Combine(ref T dst, T src);
    }

    public sealed class TextCombiner : Combiner<ReadOnlyMemory<char>>
    {
        private static volatile TextCombiner _instance;
        public static TextCombiner Instance
        {
            get
            {
                if (_instance == null)
                    Interlocked.CompareExchange(ref _instance, new TextCombiner(), null);
                return _instance;
            }
        }

        private TextCombiner()
        {
        }

        public override bool IsDefault(ReadOnlyMemory<char> value) { return value.Length == 0; }
        public override void Combine(ref ReadOnlyMemory<char> dst, ReadOnlyMemory<char> src)
        {
            Contracts.Check(IsDefault(dst));
            dst = src;
        }
    }

    public sealed class FloatAdder : Combiner<Float>
    {
        private static volatile FloatAdder _instance;
        public static FloatAdder Instance
        {
            get
            {
                if (_instance == null)
                    Interlocked.CompareExchange(ref _instance, new FloatAdder(), null);
                return _instance;
            }
        }

        private FloatAdder()
        {
        }

        public override bool IsDefault(Float value) { return value == 0; }
        public override void Combine(ref Float dst, Float src) { dst += src; }
    }

    public sealed class R4Adder : Combiner<Single>
    {
        private static volatile R4Adder _instance;
        public static R4Adder Instance
        {
            get
            {
                if (_instance == null)
                    Interlocked.CompareExchange(ref _instance, new R4Adder(), null);
                return _instance;
            }
        }

        private R4Adder()
        {
        }

        public override bool IsDefault(Single value) { return value == 0; }
        public override void Combine(ref Single dst, Single src) { dst += src; }
    }

    public sealed class R8Adder : Combiner<Double>
    {
        private static volatile R8Adder _instance;
        public static R8Adder Instance
        {
            get
            {
                if (_instance == null)
                    Interlocked.CompareExchange(ref _instance, new R8Adder(), null);
                return _instance;
            }
        }

        private R8Adder()
        {
        }

        public override bool IsDefault(Double value) { return value == 0; }
        public override void Combine(ref Double dst, Double src) { dst += src; }
    }

    // REVIEW: Delete this!
    public sealed class U4Adder : Combiner<uint>
    {
        private static volatile U4Adder _instance;
        public static U4Adder Instance
        {
            get
            {
                if (_instance == null)
                    Interlocked.CompareExchange(ref _instance, new U4Adder(), null);
                return _instance;
            }
        }

        private U4Adder()
        {
        }

        public override bool IsDefault(uint value) { return value == 0; }
        public override void Combine(ref uint dst, uint src) { dst += src; }
    }
}