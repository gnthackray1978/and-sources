//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using TDBCore.Types;

//namespace TDBCore.Interfaces
//{
//    public interface IAncestorModel
//    {

//        TreePerson TreePerson { get; }
//        void SetDescendant(Guid ancestorId);
//        void Refresh();

//    }

//    public interface IAncestorControl
//    {
//        void RequestSetDescendant(Guid ancestorId);
//        void RequestRefresh();

//        void SetModel(IAncestorModel paramModel);
//        void SetView(IAncestorView paramView);
//        void SetView();

//    }

//    public interface IAncestorView
//    {
//        void Update(IAncestorModel paramModel);
//    }
//}
