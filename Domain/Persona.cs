using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Domain;

public abstract class Persona
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }

    protected Persona(int id, string nombre, string email)
    {
        Id = id;
        Nombre = nombre;
        Email = email;
    }

    public abstract string ObtenerRol();

    public override string ToString() =>
        $"[{ObtenerRol()}] {Nombre} ({Email})";
}