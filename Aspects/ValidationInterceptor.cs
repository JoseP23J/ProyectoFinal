using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Castle.DynamicProxy;

namespace ProyectoFinal.Aspects;

public class ValidationInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        try
        {
            // Valida que ningún argumento string sea nulo o vacío
            foreach (var arg in invocation.Arguments)
            {
                if (arg is string s && string.IsNullOrWhiteSpace(s))
                    throw new ArgumentException(
                        $"Argumento inválido en {invocation.Method.Name}: string vacío");

                if (arg is int n && n < 0)
                    throw new ArgumentException(
                        $"Argumento inválido en {invocation.Method.Name}: número negativo ({n})");
            }

            invocation.Proceed();
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"🔴 [VALIDACIÓN] Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"🔴 [ERROR] Excepción capturada en {invocation.Method.Name}: {ex.Message}");
        }
    }
}