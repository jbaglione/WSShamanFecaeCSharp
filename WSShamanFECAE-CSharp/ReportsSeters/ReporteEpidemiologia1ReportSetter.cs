using WSShamanFECAE_CSharp.report;
using WSShamanFECAE_CSharp.Models;
using System;

namespace WSShamanFECAE_CSharp.ReportsSeters
{
    public class ReporteEpidemiologia1ReportSetter
    {
        public void PersonalizarCamposInforme(reporteEpidemiologia1 objReport, InformeEpidemiologia inf)
        {
            objReport.casoSospechoso.Text = inf.CasoSospechoso;
            objReport.casoConfirmado.Text = inf.CasoConfirmado;
            objReport.casoConfirmadoTest.Text = inf.CasoConfirmadoTest;

            objReport.notificadorEstablecimiento.Text = "AGRUPACION DE COLABORACION GRUPO PARAMEDIC";
            objReport.notificadorLocalidad.Text = "CABA";
            objReport.notificadorProvincia.Text = "CABA";
            objReport.notificadorFechaDD.Text = inf.NotificadorFecha.Day.ToString("00");
            objReport.notificadorFechaMM.Text = inf.NotificadorFecha.Month.ToString("00");
            objReport.notificadorFechaAA.Text = inf.NotificadorFecha.Year.ToString().Substring(2, 2);
            objReport.notificadorRazonSocial.Text = inf.NotificadorRazonSocial;
            objReport.notificadorTelefono.Text = "+54 9 11 5777-5555";
            objReport.notificadorEmail.Text = "callcenter@paramedic.com.ar";
            objReport.notificadorRol.Text = inf.NotificadorRol;

            objReport.casoNroEvento.Text = inf.CasoNroEvento;
            objReport.casoPaciente.Text = inf.CasoPaciente;
            objReport.casoTipoDocumento.Text = inf.CasoTipoDocumento;
            objReport.casoNacionalidad.Text = inf.CasoNacionalidad;
            objReport.casoPrivadoLibertadSi.Text = inf.CasoFlgPrivadoLibertad == 1 ? "X" : "";
            objReport.casoPrivadoLibertadNo.Text = inf.CasoFlgPrivadoLibertad == 0 ? "X" : "";
            objReport.casoIndigenaSi.Text = inf.CasoFlgPuebloIndigena == 1 ? "X" : "";
            objReport.casoIndigenaNo.Text = inf.CasoFlgPuebloIndigena == 0 ? "X" : "";
            objReport.casoEtnia.Text = inf.CasoEtnia;
            objReport.casoProvincia.Text = inf.CasoProvincia;
            objReport.casoPartido.Text = inf.CasoPartido;
            objReport.casoLocalidad.Text = inf.CasoLocalidad;
            objReport.casoCalle.Text = inf.CasoCalle;
            objReport.casoAltura.Text = inf.CasoAltura;
            objReport.casoPiso.Text = inf.CasoPiso;
            objReport.casoDepartamento.Text = inf.CasoDepartamento;
            objReport.casoCodigoPostal.Text = inf.CasoCodigoPostal;
            objReport.casoBarrio.Text = inf.CasoBarrio;
            objReport.casoTelefono.Text = inf.CasoTelefono;
            objReport.casoFecNacimientoDD.Text = inf.CasoFecNacimiento.Day.ToString("00");
            objReport.casoFecNacimientoMM.Text = inf.CasoFecNacimiento.Month.ToString("00");
            objReport.casoFecNacimientoAA.Text = inf.CasoFecNacimiento.Year.ToString().Substring(2, 2);
            objReport.casoEdad.Text = inf.CasoEdad;
            objReport.casoSexo.Text = inf.CasoSexo;
            objReport.casoResidenciaMayoresSi.Text = inf.CasoFlgResidenciaMayores == 1 ? "X" : "";
            objReport.casoResidenciaMayoresNo.Text = inf.CasoFlgResidenciaMayores == 0 ? "X" : "";
            objReport.casoSexo.Text = inf.CasoSexo;


            objReport.infoFisDD.Text = inf.InfoFis.Day.ToString("00");
            objReport.infoFisMM.Text = inf.InfoFis.Month.ToString("00");
            objReport.infoFisAA.Text = inf.InfoFis.Year.ToString().Substring(2, 2);
            objReport.infoFisSemana.Text = inf.InfoFlgFisSemana == 1 ? "X" : "";
            objReport.infoAmbulatorio.Text = inf.InfoFlgAmbulatorio == 1 ? "X" : "";
            objReport.infoInternado.Text = inf.InfoFlgAmbulatorio == 0 ? "X" : "";

            objReport.infoFecConsultaDD.Text = inf.InfoFecConsulta.Day.ToString("00");
            objReport.infoFecConsultaMM.Text = inf.InfoFecConsulta.Month.ToString("00");
            objReport.infoFecConsultaAA.Text = inf.InfoFecConsulta.Year.ToString().Substring(2, 2);
            objReport.infoEstablecimiento1.Text = inf.InfoEstablecimiento1;
            objReport.infoFallecidoSi.Text = inf.InfoFlgFallecido == 1 ? "X" : "";
            objReport.infoFallecidoNo.Text = inf.InfoFlgFallecido == 0 ? "X" : "";
            objReport.infoFallecimientoHos.Text = inf.InfoFallecimientoHos;
            objReport.infoFallecimientoDom.Text = inf.InfoFallecimientoDom;
            objReport.infoOtro.Text = inf.InfoOtro;

            LoadChecksSintomas(objReport, inf);
            LoadChecksSx(objReport, inf);
            LoadChecksEnfermedades(objReport, inf);
            
            // Cuadro Dos
            objReport.enfermedadesObservaciones.Text = inf.EnfermedadesObservaciones;


            objReport.iraG.Text = inf.IraG;
            objReport.Eti.Text = inf.Eti;
            objReport.Neumonia.Text = inf.Neumonia;
            objReport.Otras.Text = inf.Otras;
        }

        private void LoadChecksSx(reporteEpidemiologia1 objReport, InformeEpidemiologia inf)
        {
            objReport.dx00.Text = inf.Dx00;
            objReport.dx01.Text = inf.Dx01;
            objReport.dx02.Text = inf.Dx02;
            objReport.dx03.Text = inf.Dx03;
            objReport.dx04.Text = inf.Dx04;
        }

        private void LoadChecksSintomas(reporteEpidemiologia1 objReport, InformeEpidemiologia inf)
        {
            objReport.signos00.Text = inf.Signos00;
            objReport.signos01.Text = inf.Signos01;
            objReport.signos02.Text = inf.Signos02;
            objReport.signos03.Text = inf.Signos03;
            objReport.signos04.Text = inf.Signos04;
            objReport.signos05.Text = inf.Signos05;

            objReport.signos10.Text = inf.Signos10;
            objReport.signos11.Text = inf.Signos11;
            objReport.signos12.Text = inf.Signos12;
            objReport.signos13.Text = inf.Signos13;
            objReport.signos14.Text = inf.Signos14;

        }

        private void LoadChecksEnfermedades(reporteEpidemiologia1 objReport, InformeEpidemiologia inf)
        {
            //objReport 49 a 71
            objReport.enfermedades00.Text = inf.Enfermedades00;
            objReport.enfermedades01.Text = inf.Enfermedades01;
            objReport.enfermedades02.Text = inf.Enfermedades02;
            objReport.enfermedades03.Text = inf.Enfermedades03;
            objReport.enfermedades04.Text = inf.Enfermedades04;

            objReport.enfermedades10.Text = inf.Enfermedades10;
            objReport.enfermedades11.Text = inf.Enfermedades11;
            objReport.enfermedades12.Text = inf.Enfermedades12;
            objReport.enfermedades13.Text = inf.Enfermedades13;
            objReport.enfermedades14.Text = inf.Enfermedades14;

            objReport.enfermedades20.Text = inf.Enfermedades20;
            objReport.enfermedades21.Text = inf.Enfermedades21;
            objReport.enfermedades22.Text = inf.Enfermedades22;
            objReport.enfermedades23.Text = inf.Enfermedades23;
            objReport.enfermedades24.Text = inf.Enfermedades24;

            objReport.enfermedades30.Text = inf.Enfermedades30;
            objReport.enfermedades31.Text = inf.Enfermedades31;
            objReport.enfermedades32.Text = inf.Enfermedades32;
            objReport.enfermedades33.Text = inf.Enfermedades33;
            objReport.enfermedades34.Text = inf.Enfermedades34;

            objReport.enfermedades40.Text = inf.Enfermedades40;
            objReport.enfermedades41.Text = inf.Enfermedades41;
            objReport.enfermedades42.Text = inf.Enfermedades42;
            objReport.enfermedades43.Text = inf.Enfermedades43;
            objReport.enfermedades44.Text = inf.Enfermedades44;

            if (inf.Enfermedades00 == "X" ||
                inf.Enfermedades01 == "X" ||
                inf.Enfermedades02 == "X" ||
                inf.Enfermedades03 == "X" ||
                inf.Enfermedades04 == "X" ||

                inf.Enfermedades10 == "X" ||
                inf.Enfermedades11 == "X" ||
                inf.Enfermedades12 == "X" ||
                inf.Enfermedades13 == "X" ||
                inf.Enfermedades14 == "X" ||

                inf.Enfermedades20 == "X" ||
                inf.Enfermedades21 == "X" ||
                inf.Enfermedades22 == "X" ||
                inf.Enfermedades23 == "X" ||
                inf.Enfermedades24 == "X" ||

                inf.Enfermedades30 == "X" ||
                inf.Enfermedades31 == "X" ||
                inf.Enfermedades32 == "X" ||
                inf.Enfermedades33 == "X" ||
                inf.Enfermedades34 == "X" ||

                inf.Enfermedades40 == "X" ||
                inf.Enfermedades41 == "X" ||
                inf.Enfermedades42 == "X" ||
                inf.Enfermedades43 == "X" ||
                inf.Enfermedades44 == "X"
                )
            {
                objReport.enfermedadesSi.Text = "X";
                objReport.enfermedadesNo.Text = "";
            }
            else
            {
                objReport.enfermedadesSi.Text = "";
                objReport.enfermedadesNo.Text = "X";
            }
        }
    }
}