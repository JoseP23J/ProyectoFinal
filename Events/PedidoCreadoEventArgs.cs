using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Events;

public class PedidoCreadoEventArgs : EventArgs
{
    public int PedidoId { get; }
    public string NombreCliente { get; }
    public decimal Total { get; }
    public DateTime FechaCreacion { get; }

    public PedidoCreadoEventArgs(int pedidoId, string nombreCliente,
                                  decimal total, DateTime fechaCreacion)
    {
        PedidoId = pedidoId;
        NombreCliente = nombreCliente;
        Total = total;
        FechaCreacion = fechaCreacion;
    }
}