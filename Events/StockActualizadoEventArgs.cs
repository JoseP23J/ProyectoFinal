using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Events;

public class StockActualizadoEventArgs : EventArgs
{
    public int ProductoId { get; }
    public string NombreProducto { get; }
    public int StockAnterior { get; }
    public int StockNuevo { get; }
    public bool EsCritico => StockNuevo <= 5;  // alerta si queda poco stock

    public StockActualizadoEventArgs(int productoId, string nombreProducto,
                                      int stockAnterior, int stockNuevo)
    {
        ProductoId = productoId;
        NombreProducto = nombreProducto;
        StockAnterior = stockAnterior;
        StockNuevo = stockNuevo;
    }
}