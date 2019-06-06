using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace WSShamanFECAE_CSharp.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum Certificado : int
    {
        crtArba = 0,
        crtAgip = 1,
        crtGanancias = 2,
        crtIVA = 3,
        crtCajaPrevisional = 4,
        crtContratoVenta = 4,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ContratoVenta : int
    {
        cvTarjetaDeCredito,
        cvDebitoAutomatico,
        cvTransferenciaBancaria,
        cvPagoMisCuentas
    }
}