namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;

    using SPL.WebApp.Domain.DTOs;

    public class ArtifactRecordService : IArtifactRecordService
    {
        private readonly char[] _alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public List<ValuesChanges> GenerateLetters(int position, int nominal, bool reverse = false)
        {
            List<ValuesChanges> listResult = new();

            for (int iterador = 0; iterador < position; iterador++)
            {
                if (iterador + 1 == nominal)
                    listResult.Add(new() { Selected = true, Value = this._alpha[iterador].ToString() });
                else
                    listResult.Add(new ValuesChanges() { Selected = false, Value = this._alpha[iterador].ToString() });
            }
            if (reverse)
                listResult.Reverse();
            return listResult;
        }

        public List<ValuesChanges> GenerateArrayNum(int position, int nominal, bool reverse = false)
        {
            List<ValuesChanges> listResult = new();

            for (int iterador = 0; iterador < position; iterador++)
            {
                if (iterador + 1 == nominal)
                    listResult.Add(new() { Selected = true, Value = (iterador + 1).ToString() });
                else
                    listResult.Add(new() { Selected = false, Value = (iterador + 1).ToString() });
            }
            if (reverse)
                listResult.Reverse();
            return listResult;
        }

        public List<ValuesChanges> GenerateArrayRandL(string letterSup, string letterInf, int posSup, int posInf, bool reverse)
        {
            List<ValuesChanges> listResult = new();
            for (int iterador = posSup; iterador > 0; iterador--)
                listResult.Add(new ValuesChanges() { Selected = false, Value = iterador.ToString() + letterSup });
            listResult.Add(new ValuesChanges() { Selected = true, Value = "NOM" });
            for (int iterador = 0; iterador < posInf; iterador++)
                listResult.Add(new ValuesChanges() { Selected = false, Value = (iterador + 1).ToString() + letterInf });
            if (reverse)
                listResult.Reverse();
            return listResult;
        }

        public void FixConnections(IEnumerable<CatSidcoOthersDTO> catSidcoOthers, ref InformationArtifactDTO artifactDTO)
        {
            foreach (CatSidcoOthersDTO other in catSidcoOthers)
            {
                if (artifactDTO.Connections.IdConexionAltaTension == Convert.ToInt16(other.Id) && this.GetAcronym(artifactDTO.GeneralArtifact.ClaveIdioma) == this.GetAcronym(other.ClaveIdioma))
                    artifactDTO.Derivations.ConexionEquivalente = other.Descripcion;
                else if (artifactDTO.Connections.IdConexionAltaTension == 0)
                    artifactDTO.Derivations.ConexionEquivalente = "";

                if (artifactDTO.Connections.IdConexionBajaTension == Convert.ToInt16(other.Id) && this.GetAcronym(artifactDTO.GeneralArtifact.ClaveIdioma) == this.GetAcronym(other.ClaveIdioma))
                    artifactDTO.Derivations.ConexionEquivalente_2 = other.Descripcion;
                else if (artifactDTO.Connections.IdConexionBajaTension == 0)
                    artifactDTO.Derivations.ConexionEquivalente_2 = "";

                if (artifactDTO.Connections.IdConexionSegundaBaja == Convert.ToInt16(other.Id) && this.GetAcronym(artifactDTO.GeneralArtifact.ClaveIdioma) == this.GetAcronym(other.ClaveIdioma))
                    artifactDTO.Derivations.ConexionEquivalente_3 = other.Descripcion;
                else if (artifactDTO.Connections.IdConexionSegundaBaja == 0)
                    artifactDTO.Derivations.ConexionEquivalente_3 = "";

                if (artifactDTO.Connections.IdConexionTercera == Convert.ToInt16(other.Id) && this.GetAcronym(artifactDTO.GeneralArtifact.ClaveIdioma) == this.GetAcronym(other.ClaveIdioma))
                    artifactDTO.Derivations.ConexionEquivalente_4 = other.Descripcion;
                else if (artifactDTO.Connections.IdConexionTercera == 0)
                    artifactDTO.Derivations.ConexionEquivalente_4 = "";
            }
        }

        public void FixDerivations(ref InformationArtifactDTO artifactDTO)
        {
            artifactDTO.Derivations.TipoDerivacionAltaTension = artifactDTO.Derivations.TipoDerivacionAltaTension is "RCBN" ? "0" : artifactDTO.Derivations.TipoDerivacionAltaTension is "FCBN" ? "1" : "";
            artifactDTO.Derivations.TipoDerivacionBajaTension = artifactDTO.Derivations.TipoDerivacionBajaTension is "RCBN" ? "0" : artifactDTO.Derivations.TipoDerivacionBajaTension is "FCBN" ? "1" : "";
            artifactDTO.Derivations.TipoDerivacionSegundaTension = artifactDTO.Derivations.TipoDerivacionSegundaTension is "RCBN" ? "0" : artifactDTO.Derivations.TipoDerivacionSegundaTension is "FCBN" ? "1" : "";
            artifactDTO.Derivations.TipoDerivacionTerceraTension = artifactDTO.Derivations.TipoDerivacionTerceraTension is "RCBN" ? "0" : artifactDTO.Derivations.TipoDerivacionTerceraTension is "FCBN" ? "1" : "";
        }

        #region Private Methods
        private string GetAcronym(string language) => language.ToLower() switch
        {
            "inglés" => "EN",
            "español" => "ES",
            "bilingüe" => "BI",
            _ => "EN",
        };
        #endregion
    }
}
