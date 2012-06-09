using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using GedItter.Interfaces;
using System.Web.SessionState;
using System.Web.UI;
using System.Diagnostics;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;


namespace TancWebApp
{
    /// <summary>
    /// Summary description for WebExtensions
    /// </summary>
    public static class WebExtensions
    {

        public static Guid ToGuid(this long value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public static string ToCSVString<T>(this List<T> list)
        {
            return string.Join(",", list.ConvertAll<string>(delegate(T i) { return i.ToString(); }).ToArray());
        }

        public static void AddSources(this ListBox listBox1, ISourceFilterModel iModel)
        {

            listBox1.Items.Clear();
            if (iModel.SourceRefs.Count == 0)
            {
                listBox1.Items.Add("Nothing Selected");
            }
            else
            {
                foreach (string _ref in iModel.SourceRefs)
                {
                    listBox1.Items.Add(_ref);
                }
            }
        }


        #region mostly listview

        public static void SetSelection<T>(this ListView ListView1,
                            DataPager DataPager1,
                            List<T> selectedRecords,
                            string keyId,
                            string _rowControl = "bdid")
        {


            if (selectedRecords == null) return;

            if (selectedRecords.Count == 0)
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



                    if (selectedRecords.Contains(personId))
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


        public static void SaveKey<T>(this ListView list, List<T> saveList, string fieldName = "") where T : struct
        {
            T _key = list.GetKey<T>(null, fieldName).FirstOrDefault();

            if (saveList.Contains(_key))
                saveList.Remove(_key);
            else
                saveList.Add(_key);
        }


        public static List<T> GetKey<T>(this ListView list, ListViewCommandEventArgs e, string fieldName = "") where T : struct
        {
            List<T> selectedUniqRefs = new List<T>();
            int idx = 0;

            if (e != null)
            {
                ListViewDataItem dataItem = (ListViewDataItem)e.Item;

                // string employeeID = list.DataKeys[dataItem.DisplayIndex].Value.ToString();

                idx = dataItem.DisplayIndex;
            }
            else
            {
                idx = list.SelectedIndex;

            }

            if (idx >= 0)
            {
                if (list.DataKeys[idx].Values.Contains(fieldName))
                {
                    if (list.DataKeys[idx].Values[fieldName].ToString() != "")
                        selectedUniqRefs.Add((T)list.DataKeys[idx].Values[fieldName]);
                }
                else
                {
                    if (list.DataKeys[idx].Value.ToString() != "")
                    {
                        selectedUniqRefs.Add((T)list.DataKeys[idx].Value);
                    }
                }
            }
            else
            {

            }

            if (selectedUniqRefs.Count == 0)
                selectedUniqRefs.Add(default(T));

            return selectedUniqRefs;
        }


        #endregion


        public static List<T> GetSelectionObject<T>(this UserControl _userControl, StateBag _viewState, HttpSessionState _session)
        {
            List<T> selection = new List<T>();

            if (_viewState["sessionIdx"] == null)
            {
                _viewState.Add("sessionIdx", "select" + Guid.NewGuid().ToString());
                selection = new List<T>();
            }
            else
            {
                selection = (List<T>)_session[_viewState["sessionIdx"].ToString()];

                if (selection == null)
                    selection = new List<T>();
            }

            return selection;
        }

        public static void SaveSelection<T>(this HttpSessionState _session, StateBag _viewState, List<T> selection)
        {
            if (_viewState["sessionIdx"] != null)
            {

                _session[_viewState["sessionIdx"].ToString()] = selection;
            }

        }


        public static void UpdateErrorLog<T>(this UserControl _userControl, IDBRecordModel<T> iModel)
        {
            Label lblHeading;

            lblHeading = (Label)_userControl.Page.Master.FindControl("lblPermissions");

            if (iModel.PermissionState != "")
            {

                lblHeading.Text = lblHeading.Text + iModel.PermissionState;

            }
            else
            {
                lblHeading.Text = "no error";
            }
        }




        public static void Redirect<T>(this HttpResponse _response, string url, HttpSessionState _cache, IDBRecordModel<T> baseModel)
        {
            if (baseModel != null)
                baseModel.SetIsDataChanged(true);

            csUtils.SaveToCache(_cache, baseModel, null);

            int qryStart = url.IndexOf("?");
            NameValueCollection qscoll = new NameValueCollection ();
            string initialURL = url;

            if (qryStart >= 0)
            {
                qscoll = HttpUtility.ParseQueryString(url.Substring(qryStart));
                initialURL = url.Substring(0, qryStart);
            }

            string qryStr = "";

            if (qscoll.AllKeys.Contains("id")) qscoll.Remove("id");
            if (qscoll.AllKeys.Contains("error")) qscoll.Remove("error");
            if (qscoll.AllKeys.Contains("permission")) qscoll.Remove("permission");

            qscoll.Add("id", baseModel.SelectedRecordId.ToString());
            qscoll.Add("error", baseModel.ErrorState);
            qscoll.Add("permission", baseModel.PermissionState);

          


            //if (url.Contains("?"))
            //{
            //    url += qryStr;
            //}
            //else
            //{
            //    url += "?" + qryStr.Substring(1, qryStr.Length-1);
            //}

           // qryStr = "?id=" + baseModel.SelectedRecordId.ToString()+ "&error=" + baseModel.ErrorState + "&permission=" + baseModel.PermissionState;

            _response.Redirect(initialURL+ "?" + qscoll);
        }


    }


}