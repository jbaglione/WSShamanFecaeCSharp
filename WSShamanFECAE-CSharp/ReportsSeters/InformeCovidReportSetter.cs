using WSShamanFECAE_CSharp.report;
using WSShamanFECAE_CSharp.Models;
using System;

namespace WSShamanFECAE_CSharp.ReportsSeters
{
    public class InformeCovidReportSetter
    {
        public void PersonalizarCamposInforme(repInformeCovid objReport, InformeCovid inf)
        {
            objReport.fecDiaNot.Text = inf.FecHorNotificacion.Day.ToString("00");
            objReport.fecMesNot.Text = inf.FecHorNotificacion.Month.ToString("00");
            objReport.fecAnioNot.Text = inf.FecHorNotificacion.Year.ToString().Substring(2,2);
            objReport.Medico.Text = inf.MedicoApellido + " " + inf.MedicoNombre;
            objReport.Paciente.Text = inf.PacienteNombre;
            objReport.TipoNroDoc.Text = inf.TipoNroDoc;
            objReport.Nacionalidad.Text = inf.Nacionalidad;
            objReport.flgPrivadoLibertadSI.Text = inf.flgPrivadoLibertad == 1? "X": "";
            objReport.flgPrivadoLibertadNO.Text = inf.flgPrivadoLibertad == 0 ? "X" : "";
            objReport.flgPuebloIndigenaSI.Text = inf.flgPuebloIndigena == 1 ? "X" : "";
            objReport.flgPuebloIndigenaNO.Text = inf.flgPuebloIndigena == 0 ? "X" : "";
            objReport.Etnia.Text = inf.Etnia;
            objReport.Provincia.Text = inf.Provincia;
            objReport.Partido.Text = inf.Partido;
            objReport.Localidad.Text = inf.Localidad;
            objReport.Calle.Text = inf.Calle;
            objReport.Altura.Text = inf.Altura;
            objReport.Piso.Text = inf.Piso;
            objReport.Departamento.Text = inf.Departamento;
            objReport.CodPostal.Text = ""; // inf.CodPostal;
            objReport.Barrio.Text = ""; //inf.Barrio;
            objReport.Telefono.Text = inf.Telefono;
            objReport.FecNacDia.Text = inf.FecNacimiento.Day.ToString("00");
            objReport.FecNacMes.Text = inf.FecNacimiento.Month.ToString("00");
            objReport.FecNacAnio.Text = inf.FecNacimiento.Year.ToString().Substring(2, 2);
            objReport.Edad.Text = inf.Edad;
            objReport.Sexo.Text = inf.Sexo;
            objReport.FisDia.Text = inf.FIS.Day.ToString("00");
            objReport.FisMes.Text = inf.FIS.Month.ToString("00");
            objReport.FisAnio.Text = inf.FIS.Year.ToString().Substring(2, 2);
            objReport.flgSemanaFIS.Text = inf.flgSemanaFIS == 1 ? "X" : "";
            objReport.FecConsultaDia.Text = inf.consulta1Fecha.Day.ToString("00");
            objReport.FecConsultaMes.Text = inf.consulta1Fecha.Month.ToString("00");
            objReport.FecConsultaAnio.Text = inf.consulta1Fecha.Year.ToString().Substring(2, 2);
            objReport.flgInternadoSI.Text = inf.flgInternado == 1 ? "X" : "";
            objReport.flgInternadoNO.Text = inf.flgInternado == 0 ? "X" : "";
            objReport.consulta1Establecimiento.Text = inf.consulta1Establecimiento;
            LoadChecksEnfermedades(objReport, inf);
            LoadChecksSintomas(objReport, inf);
            // Cuadro Dos
            objReport.EnfermedadesObservaciones.Text = inf.EnfermedadesObservaciones;
            objReport.SintomasObservaciones.Text = inf.SintomasObservaciones;

            objReport.digBronquitis.Text = inf.digBronquitis == 1 ? "X" : "";
            objReport.digNeumonia.Text = inf.digNeumonia == 1 ? "X" : "";
            objReport.digSindromeGripal.Text = inf.digSindromeGripal == 1 ? "X" : "";
            objReport.digOtros.Text = inf.digOtros;
        }

        private void LoadChecksEnfermedades(repInformeCovid objReport, InformeCovid inf)
        {
            //objReport 20 a 45
            objReport.xrLabel20.Text = inf.virSintomas0000;
            objReport.xrLabel21.Text = inf.virSintomas0001;
            objReport.xrLabel22.Text = inf.virSintomas0002;
            objReport.xrLabel23.Text = inf.virSintomas0003;
            objReport.xrLabel24.Text = inf.virSintomas0004;
            objReport.xrLabel25.Text = inf.virSintomas0005;

            objReport.xrLabel26.Text = inf.virSintomas0100;
            objReport.xrLabel27.Text = inf.virSintomas0101;
            objReport.xrLabel28.Text = inf.virSintomas0102;
            objReport.xrLabel29.Text = inf.virSintomas0103;
            objReport.xrLabel30.Text = inf.virSintomas0104;

            objReport.xrLabel31.Text = inf.virSintomas0200;
            objReport.xrLabel32.Text = inf.virSintomas0201;
            objReport.xrLabel33.Text = inf.virSintomas0202;
            objReport.xrLabel34.Text = inf.virSintomas0203;

            objReport.xrLabel35.Text = inf.virSintomas0300;
            objReport.xrLabel36.Text = inf.virSintomas0301;
            objReport.xrLabel37.Text = inf.virSintomas0302;
            objReport.xrLabel38.Text = inf.virSintomas0303;
            objReport.xrLabel39.Text = inf.virSintomas0304;

            objReport.xrLabel40.Text = inf.virSintomas0400;
            objReport.xrLabel41.Text = inf.virSintomas0401;
            objReport.xrLabel42.Text = inf.virSintomas0402;
            objReport.xrLabel43.Text = inf.virSintomas0403;
            objReport.xrLabel44.Text = inf.virSintomas0404;
        }

        private void LoadChecksSintomas(repInformeCovid objReport, InformeCovid inf)
        {
            //objReport 49 a 71
            objReport.xrLabel49.Text = inf.virEnfermedades0000;
            objReport.xrLabel50.Text = inf.virEnfermedades0001;
            objReport.xrLabel51.Text = inf.virEnfermedades0002;
            objReport.xrLabel52.Text = inf.virEnfermedades0003;
            objReport.xrLabel53.Text = inf.virEnfermedades0004;
            objReport.xrLabel54.Text = inf.virEnfermedades0005;

            objReport.xrLabel55.Text = inf.virEnfermedades0100;
            objReport.xrLabel56.Text = inf.virEnfermedades0101;
            objReport.xrLabel57.Text = inf.virEnfermedades0102;
            objReport.xrLabel58.Text = inf.virEnfermedades0103;

            objReport.xrLabel59.Text = inf.virEnfermedades0200;
            objReport.xrLabel60.Text = inf.virEnfermedades0201;
            objReport.xrLabel61.Text = inf.virEnfermedades0202;
            objReport.xrLabel62.Text = inf.virEnfermedades0203;
            objReport.xrLabel63.Text = inf.virEnfermedades0204;

            objReport.xrLabel64.Text = inf.virEnfermedades0300;
            objReport.xrLabel65.Text = inf.virEnfermedades0301;
            objReport.xrLabel66.Text = inf.virEnfermedades0302;

            objReport.xrLabel67.Text = inf.virEnfermedades0401;
            objReport.xrLabel68.Text = inf.virEnfermedades0402;
            objReport.xrLabel69.Text = inf.virEnfermedades0403;
            objReport.xrLabel70.Text = inf.virEnfermedades0404;
            objReport.xrLabel71.Text = inf.virEnfermedades0405;

            if (inf.virEnfermedades0000 == "X" ||
                inf.virEnfermedades0001 == "X" ||
                inf.virEnfermedades0002 == "X" ||
                inf.virEnfermedades0003 == "X" ||
                inf.virEnfermedades0004 == "X" ||
                inf.virEnfermedades0005 == "X" ||
                inf.virEnfermedades0100 == "X" ||
                inf.virEnfermedades0101 == "X" ||
                inf.virEnfermedades0102 == "X" ||
                inf.virEnfermedades0103 == "X" ||
                inf.virEnfermedades0200 == "X" ||
                inf.virEnfermedades0201 == "X" ||
                inf.virEnfermedades0202 == "X" ||
                inf.virEnfermedades0203 == "X" ||
                inf.virEnfermedades0204 == "X" ||
                inf.virEnfermedades0300 == "X" ||
                inf.virEnfermedades0301 == "X" ||
                inf.virEnfermedades0302 == "X" ||
                inf.virEnfermedades0400 == "X" ||
                inf.virEnfermedades0401 == "X" ||
                inf.virEnfermedades0402 == "X" ||
                inf.virEnfermedades0403 == "X" ||
                inf.virEnfermedades0404 == "X" ||
                inf.virEnfermedades0405 == "X")
            {
                objReport.flgPresentaEnfSI.Text = "X";
                objReport.flgPresentaEnfNO.Text = "";
            }
            else
            {
                objReport.flgPresentaEnfNO.Text = "X";
                objReport.flgPresentaEnfSI.Text = "";
            }
        }
    }
}