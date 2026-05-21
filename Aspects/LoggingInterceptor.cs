using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Castle.DynamicProxy;

namespace ProyectoFinal.Aspects;

public class LoggingInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        Console.WriteLine($"🔵 [LOG] Entrando: {invocation.TargetType.Name}.{invocation.Method.Name}");
        Console.WriteLine($"       Argumentos: {string.Join(", ", invocation.Arguments)}");

        invocation.Proceed(); // ejecuta el método real

        Console.WriteLine($"🟢 [LOG] Saliendo: {invocation.Method.Name} | Retorno: {invocation.ReturnValue ?? "void"}");
    }
}