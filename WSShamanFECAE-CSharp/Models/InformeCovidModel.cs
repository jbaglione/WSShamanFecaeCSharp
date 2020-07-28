using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace WSShamanFECAE_CSharp.Models
{
    public class InformeCovid
    {
        public long IncidenteId { get; set; }
        public string MedicoApellido { get; set; }
        public string MedicoNombre { get; set; }
        public DateTime FecHorNotificacion { get; set; }
        public string PacienteNombre { get; set; }
        public string TipoNroDoc { get; set; }
        public string Nacionalidad { get; set; }
        public short flgPrivadoLibertad { get; set; }
        public short flgPuebloIndigena { get; set; }
        public string Etnia { get; set; }
        public string Provincia { get; set; }
        public string Partido { get; set; }
        public string Localidad { get; set; }
        public string Calle { get; set; }
        public string Altura { get; set; }
        public string Piso { get; set; }
        public string Departamento { get; set; }
        public string Telefono { get; set; }
        public string Edad { get; set; }
        public string Sexo { get; set; }
        public DateTime FecNacimiento { get; set; }
        public DateTime FIS { get; set; }
        public short flgSemanaFIS { get; set; }
        public DateTime consulta1Fecha { get; set; }
        public short flgInternado { get; set; }
        public string consulta1Establecimiento { get; set; }
        public string virSintomas0000 { get; set; }
        public string virSintomas0001 { get; set; }
        public string virSintomas0002 { get; set; }
        public string virSintomas0003 { get; set; }
        public string virSintomas0004 { get; set; }
        public string virSintomas0005 { get; set; }
        public string virSintomas0100 { get; set; }
        public string virSintomas0101 { get; set; }
        public string virSintomas0102 { get; set; }
        public string virSintomas0103 { get; set; }
        public string virSintomas0104 { get; set; }
        public string virSintomas0200 { get; set; }
        public string virSintomas0201 { get; set; }
        public string virSintomas0202 { get; set; }
        public string virSintomas0203 { get; set; }
        public string virSintomas0300 { get; set; }
        public string virSintomas0301 { get; set; }
        public string virSintomas0302 { get; set; }
        public string virSintomas0303 { get; set; }
        public string virSintomas0304 { get; set; }
        public string virSintomas0400 { get; set; }
        public string virSintomas0401 { get; set; }
        public string virSintomas0402 { get; set; }
        public string virSintomas0403 { get; set; }
        public string virSintomas0404 { get; set; }
        public string virSintomas0405 { get; set; }
        public string SintomasObservaciones { get; set; }
        public string virEnfermedades0000 { get; set; }
        public string virEnfermedades0001 { get; set; }
        public string virEnfermedades0002 { get; set; }
        public string virEnfermedades0003 { get; set; }
        public string virEnfermedades0004 { get; set; }
        public string virEnfermedades0005 { get; set; }
        public string virEnfermedades0100 { get; set; }
        public string virEnfermedades0101 { get; set; }
        public string virEnfermedades0102 { get; set; }
        public string virEnfermedades0103 { get; set; }
        public string virEnfermedades0200 { get; set; }
        public string virEnfermedades0201 { get; set; }
        public string virEnfermedades0202 { get; set; }
        public string virEnfermedades0203 { get; set; }
        public string virEnfermedades0204 { get; set; }
        public string virEnfermedades0300 { get; set; }
        public string virEnfermedades0301 { get; set; }
        public string virEnfermedades0302 { get; set; }
        public string virEnfermedades0400 { get; set; }
        public string virEnfermedades0401 { get; set; }
        public string virEnfermedades0402 { get; set; }
        public string virEnfermedades0403 { get; set; }
        public string virEnfermedades0404 { get; set; }
        public string virEnfermedades0405 { get; set; }
        public string EnfermedadesObservaciones { get; set; }
        public short digNeumonia { get; set; }
        public short digSindromeGripal { get; set; }
        public short digBronquitis { get; set; }
        public string digOtros { get; set; }
    }
}