using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GedItter.Interfaces
{
    public interface IPersonView
    {
        void ShowInvalidBirthDateWarning(bool valid);
        void ShowInvalidDeathDateWarning(bool valid);
        void ShowInvalidBaptismDateWarning(bool valid);
        void ShowInvalidOccupationWarning(bool valid);
        void DisableUpdating(bool valid);
        void DisableAddition(bool valid);
        
      //  void Update(IPersonModel paramModel);
       
    }
}
