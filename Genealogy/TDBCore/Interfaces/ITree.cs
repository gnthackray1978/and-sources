using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.Types;

namespace TDBCore.Interfaces
{



    public interface ITreeBase
    {
        double BoxHeight { get; set; }
        double BoxWidth { get; set; }


        double ZoomPercentage { get; }
        double CentrePoint { get; }
        double CentreXPoint { get; set; }
        double CentreYPoint { get; set; }   
     
        void ResetOffset();
        void SetCentrePoint(int param_x, int param_y);
        void SetFather(Guid fatherId);
        void SetMouse(int x, int y);
        void SetZoom(int percentage);
        void SetZoomStart();

        void ZoomIn();
        void ZoomOut();

        
        double DrawingWidth { get; }

        double DrawingHeight { get; }


        TDBCore.Types.TreePerson TreePerson { get; }
        System.Collections.Generic.List<TDBCore.Types.TreePoint> ChildlessMarriages { get; set; }

        System.Collections.Generic.List<int> FamiliesPerGeneration { get; set; }
        System.Collections.Generic.List<System.Collections.Generic.List<System.Collections.Generic.List<TDBCore.Types.TreePoint>>> FamilySpanLines { get; set; }
        System.Collections.Generic.List<System.Collections.Generic.List<TDBCore.Types.TreePerson>> Generations { get; set; }
       
        void GetDetail(TDBCore.Types.TreePerson _tp, out string name, out string detail, out int name_size, out int detail_size);
       // void MoveTo(int x_param, int y_param, Guid personId);
    
        void Refresh();
        void ComputeBoxLocations();

    }



    public interface ITreeModel : ITreeBase
    {

        void SetVisibility(TreePerson parent, bool isDisplay);
        void NotifyObservers();
        void AddObserver(TDBCore.Interfaces.ITreeView paramView);
        void RemoveAllObservers();
        void RemoveObserver(TDBCore.Interfaces.ITreeView paramView);

    }



    public interface ITreeControl
    {
        void RequestSetFather(Guid fatherId);
        void RequestRefresh();

        void SetModel(ITreeModel paramModel);
        void SetView(ITreeView paramView);
        void SetView();

    }

    public interface ITreeView
    {
        void Update(ITreeModel paramModel);
    }










}
