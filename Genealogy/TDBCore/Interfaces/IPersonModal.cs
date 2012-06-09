using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GedItter.Interfaces
{
    public interface IPersonModel
    {
        bool ValidBirthDate { get; }
        bool ValidOccupationDate { get; }
        bool ValidBaptismDate { get; }
        bool ValidDeathDate { get; }
        
        string BirthDateString { get; }
        string BirthLocation { get; }
        string BaptismDateString { get; }
        string BaptismLocation { get; }
        string OccupationDateString { get; }
        string OccupationLocation { get; }
        string OccupationDescription { get; }
        string DeathDateString { get; }
        string DeathLocation { get; }
        bool Sex { get; }

        void CreatePerson(Gedcom.GedcomDatabase gedcomDB);
        void SetBirth(string bDate, string bLocation);
        void SetBaptism(string bapDate, string bapLocation);
        void SetOccupation(string occDate, string occLocation, string occDescription);
        void SetDeath(string deathDate, string deathLocation);
        void SetSex(bool isMale);
        void Save();

        //void AddObserver(IPersonView paramView);
        //void RemoveObserver(IPersonView paramView);
        //void NotifyObservers();



        //string FormatDate(string date);

       // bool ValidateDate(string date);


    }
}
