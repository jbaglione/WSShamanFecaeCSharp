using System;

namespace WSShamanFECAE_CSharp.Models
{
    public class InformeEpidemiologia
    {
        public string CasoSospechoso { get; set; }
        public string CasoConfirmado { get; set; }
        public string CasoConfirmadoTest { get; set; }

        public string NotificadorRazonSocial { get; set; }
        public DateTime NotificadorFecha { get; set; }
        public string NotificadorRol { get; set; }

        public string CasoPaciente { get; set; }
        public string CasoTipoDocumento { get; set; }
        public string CasoNacionalidad { get; set; }
        public short CasoFlgPrivadoLibertad { get; set; }
        public short CasoFlgPuebloIndigena { get; set; }
        public short CasoFlgResidenciaMayores { get; set; }
        public string CasoEtnia { get; set; }
        public string CasoProvincia { get; set; }
        public string CasoPartido { get; set; }
        public string CasoLocalidad { get; set; }
        public string CasoCalle { get; set; }
        public string CasoAltura { get; set; }
        public string CasoPiso { get; set; }
        public string CasoDepartamento { get; set; }
        public string CasoTelefono { get; set; }
        public string CasoEdad { get; set; }
        public string CasoSexo { get; set; }
        public DateTime CasoFecNacimiento { get; set; }

        public string CasoNroEvento { get; set; }
        public string CasoCodigoPostal { get; set; }
        public string CasoBarrio { get; set; }


        public DateTime InfoFis { get; set; }
        public short InfoFlgFisSemana { get; set; }
        public short InfoFlgAmbulatorio { get; set; }
        
        public DateTime InfoFecConsulta { get; set; }
        public string InfoEstablecimiento1 { get; set; }
        public string InfoFallecimientoHos { get; set; }
        public short InfoFlgFallecido { get; set; }
        public string InfoFallecimientoDom { get; set; }
        public string InfoOtro { get; set; }


        public string Signos00 { get; set; }
        public string Signos01 { get; set; }
        public string Signos02 { get; set; }
        public string Signos03 { get; set; }
        public string Signos04 { get; set; }
        public string Signos05 { get; set; }

        public string Signos10 { get; set; }
        public string Signos11 { get; set; }
        public string Signos12 { get; set; }
        public string Signos13 { get; set; }
        public string Signos14 { get; set; }

        public string Dx00 { get; set; }
        public string Dx01 { get; set; }
        public string Dx02 { get; set; }
        public string Dx03 { get; set; }
        public string Dx04 { get; set; }

        public string Enfermedades00 { get; set; }
        public string Enfermedades01 { get; set; }
        public string Enfermedades02 { get; set; }
        public string Enfermedades03 { get; set; }
        public string Enfermedades04 { get; set; }
        public string Enfermedades05 { get; set; }

        public string Enfermedades10 { get; set; }
        public string Enfermedades11 { get; set; }
        public string Enfermedades12 { get; set; }
        public string Enfermedades13 { get; set; }
        public string Enfermedades14 { get; set; }
        public string Enfermedades15 { get; set; }

        public string Enfermedades20 { get; set; }
        public string Enfermedades21 { get; set; }
        public string Enfermedades22 { get; set; }
        public string Enfermedades23 { get; set; }
        public string Enfermedades24 { get; set; }
        public string Enfermedades25 { get; set; }

        public string Enfermedades30 { get; set; }
        public string Enfermedades31 { get; set; }
        public string Enfermedades32 { get; set; }
        public string Enfermedades33 { get; set; }
        public string Enfermedades34 { get; set; }
        public string Enfermedades35 { get; set; }

        public string Enfermedades40 { get; set; }
        public string Enfermedades41 { get; set; }
        public string Enfermedades42 { get; set; }
        public string Enfermedades43 { get; set; }
        public string Enfermedades44 { get; set; }
        public string Enfermedades45 { get; set; }

        public string EnfermedadesObservaciones { get; set; }

        public string IraG { get; set; }
        public string Eti { get; set; }
        public string Neumonia { get; set; }
        public string Otras { get; set; }
    }

}