using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Domain;

public class Empleado : Persona
{
    public string Cargo { get; set; }
    public decimal Salario { get; private set; }

    public Empleado(int id, string nombre, string email, string cargo, decimal salario)
        : base(id, nombre, email)
    {
        Cargo = cargo;
        Salario = salario;
    }

    public override string ObtenerRol() => $"Empleado - {Cargo}";

    public void AplicarAumento(decimal porcentaje)
    {
        Salario *= (1 + porcentaje / 100);
        Console.WriteLine($"💼 Salario de {Nombre} actualizado a {Salario:C}");
    }
}