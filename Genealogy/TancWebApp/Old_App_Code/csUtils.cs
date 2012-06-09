using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.SessionState;
//using System.Web.UI.MobileControls;
using GedItter.Interfaces;
using System.Diagnostics;
using GedItter.BirthDeathRecords;
using System.Web.Caching;
using GedItter.ModelObjects;
using GedItter.ControlObjects;
using GedItter.MarriageRecords;
using System.Collections.Specialized;

/// <summary>
/// Summary description for csUtils
/// </summary>
/// 
namespace TancWebApp
{
    public class csUtils
    {

        public static bool _showDebug = true;

        public csUtils()
        {
            //
            // TODO: Add constructor logic here
            //
        }





        public static void UpdateErrorControl<T>(UserControl _userControl, IDBRecordModel<T> iModel)
        {
            Label lblHeading;

            lblHeading = (Label)_userControl.Page.Master.FindControl("lblPermissions");

            if (iModel.PermissionState != "")
            {

                lblHeading.Text = lblHeading.Text + iModel.PermissionState;

            }
            else
            {
                //  lblHeading.Text = "no error";
            }
        }


        public static void DateBoundV2<T>(DataPager DataPager1,
                                        ListView ListView1,
                                        IDBRecordsModel<T> selectedRecords,
                                        string keyId,
                                        string _rowControl)
        {


            if (selectedRecords == null) return;

            if (selectedRecords.SelectedRecordIds.Count == 0)
            {
                ListView1.SelectedIndex = -1;

            }
            else
            {
                int endRow = 0;
                if (ListView1.DataKeys.Count > DataPager1.MaximumRows)
                    endRow = DataPager1.MaximumRows;
                else
                    endRow = ListView1.DataKeys.Count;



                int idx = 0;
                T personId = default(T);

                while (idx < endRow)
                {

                    try
                    {//surely this will fuck up!
                        if (keyId != "")

                            personId = (T)(ListView1.DataKeys[idx].Values[keyId]);
                        else
                            personId = (T)(ListView1.DataKeys[idx].Value);
                    }
                    catch (Exception ex1)
                    {

                        Debug.WriteLine(ex1.Message);
                    }



                    if (selectedRecords.SelectedRecordIds.Contains(personId))
                    {
                        HtmlTableRow tr = (HtmlTableRow)ListView1.Items[idx].FindControl(_rowControl);
                        if (tr != null)
                        {
                            tr.Style.Value = "background-color: #3399FF";
                        }
                    }

                    idx++;
                }
            }
        }




        public static IDBRecordControl<T> GetCachedControl<T>(HttpSessionState _cache, IDBRecordModel<T> iModel, IDBRecordView iDBFView, IDBRecordControl<T> newRecordControl)
        {
            IDBRecordControl<T> iDBRecordControl = null;

            // this should never ever be null!
            if (iModel == null)
                Debugger.Break();

            string perFiltCon = iModel.Name + "_control";

            object objControl = _cache[perFiltCon];

            if (objControl != null)
            {
                iDBRecordControl = (IDBRecordControl<T>)objControl;
            }
            else
            {
                iDBRecordControl = newRecordControl;
            }

            if (iModel != null)
                iDBRecordControl.SetModel(iModel);
            if (iDBFView != null)
                iDBRecordControl.SetView(iDBFView);

            return iDBRecordControl;
        }



        public static IDBRecordModel<T> GetCachedModel<T>(IDBRecordView observer, IDBRecordModel<T> model, bool? isPostBack, HttpSessionState _cache, bool createNew, HttpRequest request = null)
        {
            string perFiltMod = model.Name;




            // there doesnt seem to be any difference between
            // creating a new one and not creating a new one
            // todo need to remove this.
            if (createNew)
            {
                Debug.WriteLineIf(_showDebug, "Creating new model");
                return internalGetCacheModel(perFiltMod, observer, null, _cache, model);
            }
            else
            {
                //Debug.WriteLineIf(_showDebug, "Use existing model");
                //EditorBaseModel<T> _tp = new EditorBaseModel<T>();
                if (request != null)
                {
                    model.SetFromQueryString(request.Url.Query);
                }

                //if (!isPostBack.HasValue || !isPostBack.Value)
                //{
                //    model.Refresh();
                //}



                return internalGetCacheModel("", observer, null, _cache, model);
            }


        }


        private static IDBRecordModel<T> internalGetCacheModel<T>(string modStr, IDBRecordView observer, EventHandler editor, HttpSessionState _cache, IDBRecordModel<T> model)
        {
            string perFiltMod = modStr;

            IDBRecordModel<T> iModel = null;

            object objModel = _cache[perFiltMod];

            if (objModel != null)
            {
                iModel = (IDBRecordModel<T>)objModel;
                iModel.RemoveAllObservers();

            }
            else
            {
                iModel = model;
            }

            if (iModel != null)
            {
                iModel.SetShowDialogEdit(editor);
                iModel.SetShowDialogInsert(editor);

                if (observer != null)
                    iModel.AddObserver(observer);

            }
            return iModel;
        }


        public static void SaveToCache<T>(HttpSessionState _cache, IDBRecordModel<T> iModel, IDBRecordControl<T> iControl, HttpRequest request = null, HttpServerUtility httpContext = null)
        {
            string perEdMod = iModel.Name;
            string perEdCon = iModel.Name + "_control";





            _cache[perEdMod] = iModel;

            if (iControl != null)
                _cache[perEdCon] = iControl;

        }


    }
}