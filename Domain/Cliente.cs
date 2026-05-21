using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProyectoFinal.Interfaces;

namespace ProyectoFinal.Domain;

public class Cliente : Persona, INotificable
{
    public string Direccion { get; set; }
    public List<Pedido> HistorialPedidos { get; private set; } = new();

    public Cliente(int id, string nombre, string email, string direccion)
        : base(id, nombre, email)
    {
        Direccion = direccion;
    }

    public override string ObtenerRol() => "Cliente";

    // Implementación de INotificable
    public void EnviarNotificacion(string mensaje)
    {
        Console.WriteLine($"📧 Notificación para {Nombre}: {mensaje}");
    }

    public void AgregarPedido(Pedido pedido)
    {
        HistorialPedidos.Add(pedido);
    }
}