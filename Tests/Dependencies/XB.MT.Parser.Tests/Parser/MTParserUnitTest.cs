using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MT103XUnitTestProject.Parser
{
    public class MTParserUnitTest
    {
        internal object ReflectionCall(Type typeToCall, string methodToCall, object[] methodParameters)
        {
            MethodInfo method = typeToCall.GetMethod(methodToCall,
                                                     BindingFlags.NonPublic |
                                                     BindingFlags.Static |
                                                     BindingFlags.Instance |
                                                     BindingFlags.Public);
            var result = method.Invoke(null, methodParameters);

            return result;
        }

    }
}
