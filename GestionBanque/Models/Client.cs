using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace GestionBanque.Models
{
    public class Client : INotifyPropertyChanged
    {
        //bogue mauvais pattern
        private static string pattern =@"^(?!\.)(?!.*\.\.)[A-Za-z0-9._%+\-]+(?<!\.)@" + @"(?:[A-Za-z0-9](?:[A-Za-z0-9\-]{0,61}[A-Za-z0-9])?\.)+" + @"[A-Za-z]{2,63}$";
        private static Regex regexCourriel = new Regex(pattern);

        private long _id;
        private string _nom;
        private string _prenom;
        private string _courriel;
        private List<Compte> _comptes;

        public Client(long id, string nom, string prenom, string courriel)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Courriel = courriel;
            Comptes = new List<Compte>();
        }

        public long Id
        {
            get { return _id; }
            set 
            { 
                _id = value; 
                OnPropertyChanged();
            }
        }

        public string Prenom
        {
            get { return _prenom; }
            set
            {
                if (value == null || value.Trim().Length == 0)
                    throw new ArgumentException("Le prénom est non valide.");
                _prenom = value.Trim();
                OnPropertyChanged();
            }
        }

        public string Nom
        {
            get { return _nom; }
            set
            {
                //bogue: pas de trim()
                if (value == null || value.Trim().Length == 0)
                    throw new ArgumentException("Le nom est non valide.");
                _nom = value.Trim();
                OnPropertyChanged();
            }
        }

        public string Courriel
        {
            get { return _courriel; }
            set
            {
                if (value == null || !regexCourriel.IsMatch(value.Trim()))
                    throw new ArgumentException("Le courriel est non valide.");
                _courriel = value.Trim();
                OnPropertyChanged();
            }
        }
            
        public List<Compte> Comptes
        {
            get { return _comptes; }
            set 
            { 
                _comptes = value;
                OnPropertyChanged();
            }
        }

        public override string ToString() => $"{Prenom} {Nom}";

        public override bool Equals(object? obj)
        {
            bool clientIdentidque = obj is Client client &&
                   _id == client._id &&
                   _nom == client._nom &&
                   _prenom == client._prenom &&
                   _courriel == client._courriel;

            if (!clientIdentidque) return false;
            
            Client? c = obj as Client;
            if (_comptes.Count != c?.Comptes.Count) return false;

            for (int i = 0; i < _comptes.Count; i++)
            { 
                if (!_comptes[i].Equals(c.Comptes[i])) return false;       
            }

            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
