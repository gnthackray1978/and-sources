using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GedItter.Interfaces
{
    public interface IPersonControl

    {
        void RequestSetBirth(string bDate, string bLocation);
        void RequestSetBaptism(string bapDate, string bapLocation);
        void RequestSetOccupation(string occDate, string occLocation, string occDescription);
        void RequestSetDeath(string deathDate, string deathLocation);
        void RequestSetSex(bool isMale);
        void RequestSaveData();


        
        void SetModel(IPersonModel paramModel);
        void SetView(IPersonView paramView);
        void SetView();
    }
}
