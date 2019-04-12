using RegisterApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegisterApp
{
    public class ProprietesGlobales
    {
        private static User currentUser;

        //Simple proprieté permettant de récupérer l'utilisateur en cours, pas fort utile ici vue qu'on a créé qu'un seul utilisateur, mais en vue d'un formulaire d'enregistrement avec plusieurs utilisateurs
        public static User CurrentUser { get => currentUser; set => currentUser = value; }
    }
}
