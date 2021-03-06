﻿/*
* PROJECT:          Atomix Development
* LICENSE:          Copyright (C) Atomix Development, Inc - All Rights Reserved
*                   Unauthorized copying of this file, via any medium is
*                   strictly prohibited Proprietary and confidential.
* PURPOSE:          Stelem_I4 MSIL
* PROGRAMMERS:      Aman Priyadarshi (aman.eureka@gmail.com)
*/

using System;
using System.Reflection;

using Atomixilc.Machine;
using Atomixilc.Attributes;

namespace Atomixilc.IL
{
    [ILImpl(ILCode.Stelem_I4)]
    internal class Stelem_I4_il : MSIL
    {
        public Stelem_I4_il()
            : base(ILCode.Stelem_I4)
        {

        }

        /*
         * URL : https://msdn.microsoft.com/en-us/library/system.reflection.emit.opcodes.Stelem_I4(v=vs.110).aspx
         * Description : Replaces the array element at a given index with the int32 value on the evaluation stack.
         */
        internal override void Execute(Options Config, OpCodeType xOp, MethodBase method, Optimizer Optimizer)
        {
            if (Optimizer.vStack.Count < 3)
                throw new Exception("Internal Compiler Error: vStack.Count < 3");

            /* The stack transitional behavior, in sequential order, is:
             * An object reference to an array, array, is pushed onto the stack.
             * An index value, index, to an element in array is pushed onto the stack.
             * A value of the type specified in the instruction is pushed onto the stack.
             * The value, the index, and the array reference are popped from the stack; the value is put into the array element at the given index.
             */

            var itemA = Optimizer.vStack.Pop();
            var itemB = Optimizer.vStack.Pop();
            var itemC = Optimizer.vStack.Pop();

            switch (Config.TargetPlatform)
            {
                case Architecture.x86:
                    {
                        if (!itemA.SystemStack || !itemB.SystemStack || !itemC.SystemStack)
                            throw new Exception(string.Format("UnImplemented-RegisterType '{0}'", msIL));

                        Stelem_il.Executex86(4);
                    }
                    break;
                default:
                    throw new Exception(string.Format("Unsupported target platform '{0}' for MSIL '{1}'", Config.TargetPlatform, msIL));
            }

            Optimizer.SaveStack(xOp.NextPosition);
        }
    }
}
