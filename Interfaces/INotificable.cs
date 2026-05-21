using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Interfaces;

public interface INotificable
{
    void EnviarNotificacion(string mensaje);
}