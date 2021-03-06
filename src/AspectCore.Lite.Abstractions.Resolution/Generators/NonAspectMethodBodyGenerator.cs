﻿using AspectCore.Lite.Abstractions.Extensions;
using AspectCore.Lite.Abstractions.Generator;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace AspectCore.Lite.Abstractions.Resolution.Generators
{
    internal sealed class NonAspectMethodBodyGenerator : MethodBodyGenerator
    {
        private readonly MethodInfo serviceMethod;
        private readonly FieldBuilder serviceInstanceFieldBuilder;

        public NonAspectMethodBodyGenerator(MethodBuilder declaringMember, MethodInfo serviceMethod, FieldBuilder serviceInstanceField)
            : base(declaringMember)
        {
            this.serviceMethod = serviceMethod;
            this.serviceInstanceFieldBuilder = serviceInstanceField;
        }

        protected override void GeneratingMethodBody(ILGenerator ilGenerator)
        {
            var parameters = serviceMethod.GetParameterTypes();
            ilGenerator.EmitThis();
            ilGenerator.Emit(OpCodes.Ldfld, serviceInstanceFieldBuilder);
            for (int i = 1; i <= parameters.Length; i++)
            {
                ilGenerator.EmitLoadArg(i);
            }
            ilGenerator.Emit(OpCodes.Callvirt, serviceMethod);
            ilGenerator.Emit(OpCodes.Ret);
        }
    }
}
