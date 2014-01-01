using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using ANDServices;
using AndServices.Interfaces;
using TDBCore.BLL;
using TDBCore.ControlObjects;
using TDBCore.EntityModel;
using TDBCore.Interfaces;
using TDBCore.ModelObjects;
using TDBCore.Types;


namespace SourceService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class SourceService : ISourceService
    {
        // sources methods

        public List<CensusPlace> Get1841CensusPlaces()
        {
            SourceBll sourceBll = new SourceBll();
            return sourceBll.Get1841Census();
        }

        public List<CensusSource> Get1841CensusSources(Guid sourceId)
        {
            SourceBll sourceBll = new SourceBll();
            return sourceBll.Get1841CensuSources(sourceId);
        }


        public ServiceFullSource GetSource(string sourceId)
        {

            ServiceFullSource ssobj = new ServiceFullSource();

            var iModel = new SourceEditorModel();
            var iControl = new SourceEditorControl();
            string retVal = "";
            try
            {

                iControl.SetModel(iModel);

                iControl.RequestSetSelectedIds(sourceId.ToGuid());

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestRefresh();

                ssobj = new ServiceFullSource()
                {
                    IsCopyHeld = iModel.IsCopyHeld.GetValueOrDefault(),
                    IsThackrayFound = iModel.IsThackrayFound.GetValueOrDefault(),
                    IsViewed = iModel.IsViewed.GetValueOrDefault(),
                    OriginalLocation = iModel.SourceOriginalLocation,
                    SourceDesc = iModel.SourceDescription,
                    SourceId = iModel.SelectedRecordId,
                    SourceRef = iModel.SourceRef,
                    SourceNotes = iModel.SourceNotes,
                    SourceDateStr = iModel.SourceDateStr,
                    SourceDateStrTo = iModel.SourceDateToStr,
                    Parishs = iModel.SourceParishs.ParseToCSV(),
                    SourceTypes = iModel.SourceTypeIdList.ParseToCSV(),
                    FileIds = iModel.SourceFileIds.ParseToCSV(),
                    SourceFileCount = iModel.SourceFileCount.ToInt32()

                };
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;
                ssobj.ErrorStatus = retVal;
            }

            return ssobj;
        }

        public List<string> GetSourceNames(string sourceIds)
        {

            // the results need to be returned in the same order as the list of 
            // ids used to look them up.

            SourceBll sourceBll = new SourceBll();

            List<Guid> sourceIdLst = new List<Guid>();
            List<string> returnList = new List<string>();

            sourceIds.Split(',').ToList().ForEach(p => sourceIdLst.Add(p.ToGuid()));

            List<Source> unsortedList = sourceBll.FillSources().Where(s => sourceIdLst.Contains(s.SourceId)).ToList();

            if (unsortedList.Count > 0)
            {
                foreach (Guid source in sourceIdLst)
                {
                    returnList.Add(unsortedList.First(o => o.SourceId == source).SourceRef);
                }
            }
            return returnList;


        }

        public ServiceSourceObject GetSources(string sourceTypes, string sourceRef, string sourceDesc, string origLoc,
            string dateLB, string toDateLB, string dateUB, string toDateUB, string fileCount, string isThackrayFound,
            string isCopyHeld, string isViewed, string isChecked, string page_number, string page_size, string sortColumn)
        {
            ISourceFilterModel iModel = new SourceFilterModel();
            ISourceFilterControl iControl = new SourceFilterControl();
            string retVal = "";
            try
            {

                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetFilterSourceTypeList(SourceFilterModel.GetSourceTypeList(sourceTypes));
                iControl.RequestSetFilterSourceRef(sourceRef);
                iControl.RequestSetFilterSourceDescription(sourceDesc ?? "");
                iControl.RequestSetFilterSourceOriginalLocation(origLoc ?? "");
                iControl.RequestSetFilterSourceDateLowerBound(dateLB ?? "");
                iControl.RequestSetFilterSourceToDateLowerBound(toDateLB ?? "");
                iControl.RequestSetFilterSourceDateUpperBound(dateUB ?? "");
                iControl.RequestSetFilterSourceToDateUpperBound(toDateUB ?? "");
                iControl.RequestSetFilterSourceFileCount((fileCount ?? ""), true);
                iControl.RequestSetFilterIsThackrayFound(isThackrayFound.ToBool(), isChecked.ToBool());
                iControl.RequestSetFilterIsCopyHeld(isCopyHeld.ToBool(), isChecked.ToBool());
                iControl.RequestSetFilterIsViewed(isViewed.ToBool(), isChecked.ToBool());


                iControl.RequestSetRecordStart(page_number.ToInt32());
                iControl.RequestSetRecordPageSize(page_size.ToInt32());


                iControl.RequestRefresh();
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;
                iModel.SourcesDataTable.ErrorStatus = retVal;
            }


            return iModel.SourcesDataTable;//.ToServiceSourceObject(sortColumn, page_number.ToInt32(), page_size.ToInt32()); 
        }

        public string DeleteSource(string sourceId)
        {
            List<Guid> sourceIdGuids = new List<Guid>();
            SourceFilterModel iModel = new SourceFilterModel();
            SourceFilterControl iControl = new SourceFilterControl();
            string retVal = "";
            try
            {
                string[] sourceIds = sourceId.Split(',');

                foreach (string _str in sourceIds)
                {
                    sourceIdGuids.Add(_str.ToGuid());
                }

                iControl.SetModel(iModel);
                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedIds(sourceIdGuids);

                iControl.RequestDelete();

            }
            catch (Exception ex1)
            {
                retVal = "Error: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;

            }


            return WebHelper.MakeReturn(iModel.SelectedRecordId.ToString(), retVal);
        }

        public string AddSource(string sourceId,
                                   string isCopyHeld,
                                   string isThackrayFound,
                                   string isViewed,
                                   string originalLocation,
                                   string sourceDesc,
                                   string sourceRef,
                                   string sourceNotes,
                                   string sourceDateStr,
                                   string sourceDateStrTo,
                                   string sourceFileCount,
                                   string parishs,
                                   string sourceTypes,
                                   string fileIds)
        {
            string retVal = "";

            var iModel = new SourceEditorModel();
            var iControl = new SourceEditorControl();

            List<Guid> parishsList = new List<Guid>();
            List<int> sourceTypeList = new List<int>();

            if (parishs != null && parishs != "")
                parishs.Split(',').Where(s => s != null).ToList().ForEach(s => parishsList.Add(s.ToGuid()));

            if (sourceTypes != null && sourceTypes != "")
                sourceTypes.Split(',').Where(s => s != null).ToList().ForEach(s => sourceTypeList.Add(s.ToInt32()));

            try
            {
                if (iModel != null)
                    iControl.SetModel(iModel);

                //     iControl.RequestSetUserId(
                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedId(sourceId.ToGuid());
                iControl.RequestRefresh();

                iControl.RequestSetIsCopyHeld(isCopyHeld.ToBool());
                iControl.RequestSetIsThackrayFound(isThackrayFound.ToBool());
                iControl.RequestSetIsViewed(isViewed.ToBool());

                iControl.RequestSetSourceOriginalLocation(originalLocation);

                iControl.RequestSetSourceDescription(sourceDesc);
                iControl.RequestSetSourceRef(sourceRef);

                iControl.RequestSetSourceNotes(sourceNotes);

                iControl.RequestSetSourceDateStr(sourceDateStr);
                iControl.RequestSetSourceDateToStr(sourceDateStrTo);

                iControl.RequestSetSourceFileCount(sourceFileCount);

                iControl.RequestSetSourceParishs(parishsList);

                iControl.RequestSetSourceTypeIdList(sourceTypeList);

                iControl.RequestUpdate();
            }

            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;
            }


            return WebHelper.MakeReturn(iModel.SelectedRecordId.ToString(), retVal);

        }


    }
}