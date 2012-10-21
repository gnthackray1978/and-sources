using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using System.Collections;
using GedItter.BLL;
using System.Web.Security;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using TDBCore.BLL;


namespace GedItter.ModelObjects
{
    //TODO: move me to a better location!!!!!!
    public class EditorBaseModel<T> : IDBRecordModel<T>
    {

        private string user = "";
        private ISourceEditorModel sourceEditorModel = null;
        private List<T> parentRecordIds = new List<T> ();
        public EventHandler ShowDialogEditEvent;
        public EventHandler ShowDialogInsertEvent;
        private event EventHandler dataSaved;
        private List<Guid> sourceGuidList = new List<Guid>();
        private List<int> linkIntIds = new List<int>();

        private bool isNOTReadyForUpdate = true;
        private bool isNOTReadyForInsert = true;
        private int userId = 1;//guest
        private bool isValidUserId = false;
        private bool isValidDateAddedFilterLower = false;
        private bool isValidDateAddedFilterUpper = false;
        private bool isReadOnly = false;
        protected bool isDataChanged = false;

        protected ArrayList aList = new ArrayList();

        private List<T> selectedRecordIds = new List<T>();
        string sourceIdsFound = "";

        private DateTime filterDateAddedFrom = new DateTime(2008, 1, 1);
        private string filterDateAddedFromStr = "01/01/2008";
        private DateTime filterDateAddedTo = new DateTime(2020, 1, 1);
        private string filterDateAddedToStr = "01/01/2020";

        private string permissionState = "";
        private string errorState = "";

        protected Guid uniqueRef = Guid.Empty;

        protected int totalEvents = 0;
        protected int eventPriority = 0;

        private int recordCount = 0;
        private int recordPageSize = 0;
        private int recordStart = 0;
        private bool asc = true;



        private bool isSecurityEnabled = true;

        
        private string name = "";


        private string sortColumn = "";

        #region validation properties


        public bool IsSecurityEnabled
        {
            get { return isSecurityEnabled; }
           
        }

        public string StatusMessage
        {
            get
            {
                string statusMessage = "";

                if (this.PermissionState != "")
                {
                    statusMessage += this.PermissionState;
                }

                if (!this.IsValidEntry)
                {
                    if (statusMessage != "") statusMessage += Environment.NewLine;

                    statusMessage += this.errorState;
                }

                return statusMessage;
            }
        }



        public bool IsValidEdit()
        {
         


            if (!isSecurityEnabled)
            {

                Debug.WriteLine("security bypassed for test");
                return true;

            }



            //Guid userKey = new Guid(this.user);

            //MembershipUser meminfo = Membership.GetUser((object)userKey);

            

            //string[] roles = Roles.GetRolesForUser();
            ////Admin
            ////Standard

            //List<string> roleList = new List<string>(roles);

           // if (roleList.Contains("Administrators"))

            UserBLL users = new UserBLL();
         //   users.GetUserRole(this.user);
            if (users.GetUserRole(this.user) == 1)
            {
                this.permissionState = "";
                return true;
            }
            else
            {
                this.permissionState = "User not in required role group";
                return false;
            }




        }

        public bool IsValidDelete()
        {
            if (!isSecurityEnabled)
            {

                Debug.WriteLine("security bypassed for test");
                return true;

            }
            //MembershipUser meminfo = Membership.GetUser();

            //string[] roles = Roles.GetRolesForUser();
            ////Admin
            ////Standard

            //List<string> roleList = new List<string>(roles);

            //if (roleList.Contains("Administrators"))
            //{
            //    this.permissionState = "";
            //    return true;
            //}
            //else
            //{
            //    this.permissionState = "User not in required role group";
            //    return false;
            //}

            UserBLL users = new UserBLL();
        //    users.GetUserRole(this.user);
            if (users.GetUserRole(this.user) == 1)
            {
                this.permissionState = "";
                return true;
            }
            else
            {
                this.permissionState = "User not in required role group";
                return false;
            }
        }

        public bool IsValidInsert()
        {
            if (!isSecurityEnabled)
            {

                Debug.WriteLine("security bypassed for test");
                return true;

            }
            //MembershipUser meminfo = Membership.GetUser();


            //string[] roles = Roles.GetRolesForUser();
            ////Admin
            ////Standard

            //List<string> roleList = new List<string>(roles);

            //if (roleList.Contains("Administrators"))
            //{
            //    this.permissionState = "";
            //    return true;
            //}
            //else
            //{
            //    this.permissionState = "User not in required role group";
            //    return false;
            //}

            UserBLL users = new UserBLL();
         //   users.GetUserRole(this.user);
            if (users.GetUserRole(this.user) == 1)
            {
                this.permissionState = "";
                return true;
            }
            else
            {
                this.permissionState = "User not in required role group";
                return false;
            }
        }

        public bool IsvalidSelect()
        {
         //   var temp = OperationContext.Current.RequestContext.RequestMessage.Headers;

            return true;

            //if (!isSecurityEnabled)
            //{

            //    Debug.WriteLine("security bypassed for test");
            //    return true;

            //}


            //MembershipUser meminfo = Membership.GetUser();

            //string[] roles = Roles.GetRolesForUser();
           

            //List<string> roleList = new List<string>(roles);

            //if (roleList.Count == 0)
            //{
            //    this.permissionState = "No user logged in";
            //}

            //if (roleList.Contains("Registered") ||
            //    roleList.Contains("Administrators"))
            //{

            //    // dont reset this because after every action there is a select
            //    // and it just resets the message
            //    // need a seperate message for invalid selects..


            //    //this.permissionState = "";
            //    return true;
            //}
            //else
            //{

            //    this.permissionState = "User not in required role group";
            //    return false;
            //}


        }
      

        public bool IsValidSelectedRecordId
        {
            get
            {
                if (selectedRecordIds != null)
                {
                    if (selectedRecordIds.Count > 0)
                    {
                        
                        if (selectedRecordIds[0].ToString() != default(T).ToString())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsValidDateAddedLower
        {
            get
            {
                return this.isValidDateAddedFilterLower;
            }
        }

        public bool IsValidDateAddedUpper
        {
            get
            {
                return this.isValidDateAddedFilterUpper;
            }
        
        }
      
        public virtual bool IsValidEntry
        {

            get
            {
                return true;
            }
        }

        public bool IsValidUser
        {
            get
            {
                return this.isValidUserId;
            }
        }
        
        
        #endregion


        #region read only props

        public string User
        {
            get
            {
                return this.user;
            }
        }

        public bool IsDataChanged
        {
            get
            {
                
                return this.isDataChanged;
            }
        }

        public EventHandler DataSaved
        {
            get
            {
                return dataSaved;
            }
        }

        public bool ASC
        {
            get { return this.asc; }
        }

        public int RecordCount
        {
            get { return recordCount; }

        }


        public int RecordPageSize
        {
            get { return recordPageSize; }

        }


        public int RecordStart
        {
            get { return recordStart; }

        }





        public T SelectedRecordId
        {
            get
            {
                if (this.selectedRecordIds.Count > 0)
                {
                    return this.selectedRecordIds[0];
                }
                else
                {
                    return default(T);

                }
            }
        }

        public List<T> SelectedRecordIds
        {
            get
            {
                if(this.selectedRecordIds != null)
                    return this.selectedRecordIds;
                else
                    return new List<T>();
            }
        }

        public ISourceEditorModel ISourceEditorModel 
        {
            get 
            {
                return this.sourceEditorModel;
            }
        
        }

        public string SourceIdsFound
        {
            get
            {
                return this.sourceIdsFound;
            }
        }

        public bool IsDataUpdated
        {
            get
            {
                return this.isNOTReadyForUpdate;
            }
        }
        public bool IsDataInserted
        {
            get
            {
                return this.isNOTReadyForInsert;
            }
        }



        public Guid EditorUniqueRef
        {
            get
            {
                return this.uniqueRef;
            }
        }

        public int EditorTotalEvents
        {
            get
            {
                return this.totalEvents;
            }
        }

        public int EditorEventPriority
        {
            get
            {
                return this.eventPriority;
            }
        }




        public DateTime FilterDateAddedFrom
        {
            get
            {
                return this.filterDateAddedFrom;
            }
        }

        public DateTime FilterDateAddedTo
        {
            get
            {
                return this.filterDateAddedTo;
            }
        }

        public string FilterDateAddedFromStr
        {
            get
            {
                return this.filterDateAddedFromStr;
            }
        }

        public string FilterDateAddedToStr
        {
            get
            {
                return this.filterDateAddedToStr;
            }
        }

        public int SelectedUserId
        {
            get
            {
                return this.userId;
            }

        }


        public List<Guid> SourceGuidList
        {
            get
            {
                return this.sourceGuidList;
            }
        }

        public List<int> LinkIntIds
        {
            get
            {
                return this.linkIntIds;
            }
        }



        public bool IsReadOnly
        {
            get
            {
                return this.isReadOnly;
            }
        }

        public string SourceGuidListAsString
        {
            get
            {
                string retVal = "";
                foreach (Guid _guid in sourceGuidList)
                {
                    retVal += "," + _guid.ToString();
                }

                if (retVal.StartsWith(",")) retVal = retVal.Remove(0, 1);

                return retVal;

            }
        }

        public string PermissionState
        {
            get
            {
                return this.permissionState;
            }
        }

        public virtual string ErrorState
        {
            get
            {
                return this.errorState;
            }
        }


        public List<T> ParentRecordIds
        {
            get
            {
                return this.parentRecordIds;
            }
        }




        EventHandler IDBRecordModel<T>.ShowDialogEditEvent
        {
            get
            {
                return this.ShowDialogEditEvent;
            }
        }

        EventHandler IDBRecordModel<T>.ShowDialogInsertEvent
        {
            get
            {
                return this.ShowDialogInsertEvent;
            }
        }


        #endregion


        public string SortColumn
        {
            get 
            {
                return this.sortColumn;
            }
        }

        public void SetSortColumn(string param)
        {
            if (this.sortColumn != param)
            {
                this.sortColumn = param;

                
            }
        }

        #region setters


        protected void SetName(string param)
        {
            this.name = param;
        }

        public void SetIsDataChanged(bool param)
        {
            this.isDataChanged = param;
        }

        public void SetIsSecurityEnabled(bool param)
        {
            this.isSecurityEnabled = param;
        }

        public void SetParentRecordIds(List<T> recordIds)
        {
            this.parentRecordIds = recordIds;
        }

        public void SetParentRecordIds(T recordIds)
        {


            if (this.parentRecordIds.Contains(recordIds))
            {
                this.parentRecordIds.Remove(recordIds);
            }
            else
            {
                this.parentRecordIds.Add(recordIds);
            }
        }


        public void SetLinkInts(List<int> param)
        {
            this.linkIntIds = param;
        }

        public void SetSourceGuidList(List<Guid> param)
        {
            if (param == null)
                this.sourceGuidList = new List<Guid>();
            else
                this.sourceGuidList = param;
        }

        public void SetIsReadOnly(bool paramIsReadOnly)
        {
            this.isReadOnly = paramIsReadOnly;
        }


        public void SetSelectedUser(string param)
        {
            this.user = param;
        }

        public void SetSelectedUserId(int _id)
        {
            if (this.userId != _id)
            {
                //todo validate against db
                if (_id > 0)
                {
                    this.userId = _id;
                    this.isValidUserId = true;
                }
                else
                {
                    this.isValidUserId = false;
                }
            }
        }

        public void SetDataSaved(EventHandler param)
        {

            this.dataSaved = param;
        }

        public void SetPermissionState(string param)
        {
            if (this.permissionState != param)
            {
                this.permissionState = param;

                // SetModelStatusFields();
            }
        }

        public void SetErrorState(string param)
        {
            if (this.errorState != param)
            {
                this.errorState = param;

               // SetModelStatusFields();
            }
        }

        public void SetEditorUniqueRef(Guid param)
        {
            if (this.uniqueRef != param)
            {
                this.uniqueRef = param;

                SetModelStatusFields();
            }
        }

        public void SetEditorTotalEvents(int param)
        {
            if (this.totalEvents != param)
            {
                this.totalEvents = param;

                SetModelStatusFields();
            }
        }

        public void SetEditorEventPriority(int param)
        {
            if (this.eventPriority != param)
            {
                this.eventPriority = param;

                SetModelStatusFields();
            }
        }


        public void SetRecordCount(int param)
        {
            if (param != recordCount)
            {
                recordCount = param;
                SetModelStatusFields();
            }
        }


        public void SetRecordPageSize(int param)
        {
            if (param != recordPageSize)
            {
                recordPageSize = param;
                SetModelStatusFields();
            }
            
        }


        public void SetRecordStart(int param)
        {
            if (param != recordStart)
            {
                recordStart = param;
                SetModelStatusFields();
            }
         
        }

        public void SetASC(bool param)
        {
            if (param != asc)
            {
                asc = param;
                SetModelStatusFields();
            }

        }

        public void SetShowDialogEdit(EventHandler paramEventHandler)
        {
            this.ShowDialogEditEvent = paramEventHandler;
        
        }

        public void SetShowDialogInsert(EventHandler paramEventHandler)
        {
            this.ShowDialogInsertEvent = paramEventHandler;

        }



        public void SetFilterAddedDateLower(string date)
        {
            if (this.filterDateAddedFromStr != date)
            {
                this.filterDateAddedFromStr = date;

                if(DateTime.TryParse(this.filterDateAddedFromStr, out this.filterDateAddedFrom))
                {
                    this.isValidDateAddedFilterLower = true;
                }
                else
                {
                    this.isValidDateAddedFilterLower = false;
                }
            }
            


        }

        public void SetISourceEditorModel(ISourceEditorModel param)
        {
            this.sourceEditorModel = param;
        }

        public void SetFilterAddedDateUpper(string date)
        {
            if (this.filterDateAddedToStr != date)
            {
                this.filterDateAddedToStr = date;

                if (DateTime.TryParse(this.filterDateAddedToStr, out this.filterDateAddedTo))
                {
                    this.isValidDateAddedFilterUpper = true;
                }
                else
                {
                    this.isValidDateAddedFilterUpper = false;
                }
            }
        }

        public void SetSelectedRecordId(T recordId)
        {
            // todo cheeky!
            //Debug.Assert(false, "shouldnt be using this");
          
            try
            {
                if (selectedRecordIds.Count > 0)
                {
                    selectedRecordIds[0] = recordId;
                }
                else
                {
                    selectedRecordIds.Add(recordId);
                }
            }
            catch (Exception ex1)
            {
                Debug.WriteLine(ex1.Message);
              
            }
        



            SetModelStatusFields();
        }

        public void SetSelectedRecordIds(List<T> recordIds)
        {
            // todo find a way to get rid of the ToString! im drunk right now, so i cant think of one
            recordIds.RemoveAll(new System.Predicate<T>(delegate(T val) { return (val.ToString() == default(T).ToString()); }));

            selectedRecordIds = recordIds;

            //todo whats this for???? need to check!
            this.sourceIdsFound = "";

            foreach (T recordId in recordIds)
            {
                this.sourceIdsFound = this.sourceIdsFound + recordId.ToString() + ",";
            }
           
            if (this.sourceIdsFound != "")
                this.sourceIdsFound = this.sourceIdsFound.Remove(this.sourceIdsFound.Length - 1);


            SetModelStatusFields();
        }


        public void SetSelectedRecordIds(T recordIds)
        {
            // if record is already in there then remove it
            // if not then add it 
            if (selectedRecordIds.Contains(recordIds))
            {
                selectedRecordIds.Remove(recordIds);
            }
            else
            {
                selectedRecordIds.Add(recordIds);
            }


        }

        public virtual void SetModelStatusFields()
        {
            if (!this.IsValidEntry)
            {
                this.isNOTReadyForUpdate = true;
                this.isNOTReadyForInsert = true;
            }
            else
            {
                // if there is no valid marriage selected then 
                // we must be inserting.
                if (this.IsValidSelectedRecordId)
                {
                    this.isNOTReadyForUpdate = false;
                    this.isNOTReadyForInsert = true;
                }
                else
                {
                    this.isNOTReadyForUpdate = true;
                    this.isNOTReadyForInsert = false;
                }
            }

        }

        #endregion


        public void OnDataSaved()
        {
            if (DataSaved != null)
                DataSaved(this, new EventArgs());
        }

        public void ShowDialogEdit(object sender)
        {
            if (this.ShowDialogEditEvent != null)
                this.ShowDialogEditEvent(sender, new EventArgs());
        }

        public void ShowDialogInsert(object sender)
        {
            if (this.ShowDialogInsertEvent != null)
                this.ShowDialogInsertEvent(sender, new EventArgs());
        }

        public virtual void DeleteSelectedRecords()
        {
            throw new NotImplementedException();
        }

        public virtual void EditSelectedRecord()
        {
            this.isNOTReadyForUpdate = true;
        }

        public virtual void InsertNewRecord()
        {
            this.isNOTReadyForInsert = true;
        }

        public virtual void Refresh()
        {
            throw new NotImplementedException();
        }



        public void AddObserver(IDBRecordView paramView)
        {
            aList.Add(paramView);
        }

        public void RemoveObserver(IDBRecordView paramView)
        {
            aList.Remove(paramView);
        }

        public void NotifyObservers<T>(T model)
        {

            foreach (IDBRecordView view in aList)
            {
                view.Update<T>(model);
            }
        }


        public void RemoveAllObservers()
        {
            aList = new ArrayList();
        }






        public string Name
        {
            get 
            {
                return this.GetType().ToString().Replace(".","");
            }
        }


        public virtual void SetFromQueryString(string param)
        {
            Debug.WriteLine("SetFromQueryString: " + param);
        }





        
    }


    public class EditorBaseControl<T> : IDBRecordControl<T>
    {
        protected EditorBaseModel<T> Model = null;
        protected IDBRecordView View = null;


        public void RequestSetIsDataChanged(bool param)
        {
            if (Model != null)
            {
                Model.SetIsDataChanged(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetRecordCount(int param)
        {
            if (Model != null)
            {
                Model.SetRecordCount(param);

                if (View != null) SetView();
            }
        }
        public void RequestSetASC(bool param)
        {
            if (Model != null)
            {
                Model.SetASC(param);

                if (View != null) SetView();
            }
        }
        public void RequestSetRecordPageSize(int param)
        {
            if (Model != null)
            {
                Model.SetRecordPageSize(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetRecordStart(int param)
        {
            if (Model != null)
            {
                Model.SetRecordStart(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetErrorState(string param)
        {
            if (Model != null)
            {
                Model.SetErrorState(param);

                if (View != null) SetView();
            }
        }


        public void RequestSetSelectedId(T Id)
        {
            if (Model != null)
            {
                Model.SetSelectedRecordId(Id);

                if (View != null) SetView();
            }
        }

        public void RequestSetSelectedIds(T Ids)
        {
            if (Model != null)
            {
                Model.SetSelectedRecordIds(Ids);

                if (View != null) SetView();
            }
        }

        public void RequestSetSelectedIds(List<T> Ids)
        {
            if (Model != null)
            {
                Model.SetSelectedRecordIds(Ids);

                if (View != null) SetView();
            }
        }
        public void RequestRefresh()
        {
            if (Model != null)
            {
                Model.Refresh();

                if (View != null) SetView();
            }
        }
        public void RequestSetUserId(int userId)
        {
            if (Model != null)
            {
                Model.SetSelectedUserId(userId);

                if (View != null) SetView();
            }
        }

        public void RequestSetUser(string param)
        {
            if (Model != null)
            {
                Model.SetSelectedUser(param);

                if (View != null) SetView();
            }
        }

        public virtual void RequestDelete()
        {
            if (Model != null)
            {
                Model.DeleteSelectedRecords();

                if (View != null) SetView();
            }
        }

        public virtual void RequestUpdate()
        {
            if (Model != null)
            {
                if(object.Equals(Model.SelectedRecordId,default(T)))
                {
                    Model.InsertNewRecord();
                }
                else
                {
                    Model.EditSelectedRecord();
                }
                
                if (View != null) SetView();
            }
        }

        public virtual void RequestInsert()
        {
            

            if (Model != null)
            {
                Model.InsertNewRecord();

                if (View != null) SetView();
            }
        }

     


        public void RequestSetLinkInts(List<int> param)
        {
            if (Model != null)
            {
                Model.SetLinkInts(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetSourceGuidsList(List<Guid> param)
        {
            if (Model != null)
            {
                Model.SetSourceGuidList(param);

                if (View != null) SetView();
            }
        }




        public void RequestSetParentRecordIds(T Ids)
        {
            if (Model != null)
            {
                Model.SetParentRecordIds(Ids);

                if (View != null) SetView();
            }
        }

        public void RequestSetParentRecordIds(List<T> Ids)
        {
            if (Model != null)
            {
                Model.SetParentRecordIds(Ids);

                if (View != null) SetView();
            }
        }



        public void RequestSetISourceEditorModel(ISourceEditorModel param)
        {
            if (Model != null)
            {
                Model.SetISourceEditorModel(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetDataSaved(EventHandler param)
        {
            if (Model != null)
            {
                Model.SetDataSaved(param);

                if (View != null) SetView();
            }

        }

        public virtual void SetView()
        {

        }

        public virtual void SetModel(IDBRecordModel<T> paramModel)
        {
            // 
            Debug.WriteLine("shouldnt get here");
            //Debug.Assert(false);
        }

        public virtual void SetView(IDBRecordView paramView)
        {
            Debug.WriteLine("shouldnt get here");
            // shouldnt get here
        //    Debug.Assert(false);
        }


        public void RequestSetSortColumn(string param)
        {
            if (Model != null)
            {
                Model.SetSortColumn(param);

                if (View != null) SetView();
            }
        }
    }

   
}


 