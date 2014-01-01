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
using TDBCore.ModelObjects;
using TDBCore.Types;

namespace SourceTypeService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class SourceTypeService : ISourceTypes
    {
        // source types

        public List<string> GetSourceTypeNames(string typeIds)
        {

            // the results need to be returned in the same order as the list of 
            // ids used to look them up.          
            SourceTypesBll sourceTypesBll = new SourceTypesBll();
            List<int> typeIdLst = new List<int>();

            typeIds.Split(',').ToList().ForEach(p => typeIdLst.Add(p.ToInt32()));

            return sourceTypesBll.GetSourceTypesFromIdList(typeIdLst);
        }

        public ServiceSourceType GetSourceType(string TypeId)
        {
            SourceTypeEditorModel iModel = new SourceTypeEditorModel();
            SourceTypeEditorControl iControl = new SourceTypeEditorControl();
            ServiceSourceType ssTO = new ServiceSourceType();
            string retVal = "";
            try
            {
                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedId(TypeId.ToInt32());

                iControl.RequestRefresh();

                if (iModel.IsValidEntry)
                {
                    ssTO = new ServiceSourceType() { Description = iModel.SourceTypeDesc, Order = iModel.SourceTypeOrder, TypeId = TypeId.ToInt32() };
                }
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;
                ssTO.ErrorStatus = retVal;
            }


            return ssTO;
        }

        public ServiceSourceTypeObject GetSourceTypes(string description, string page_number, string page_size, string sort_col)
        {
            SourceTypeFilterModel iModel = new SourceTypeFilterModel();
            SourceTypeFilterControl iControl = new SourceTypeFilterControl();
            ServiceSourceTypeObject ssTO = new ServiceSourceTypeObject();
            string retVal = "";

            try
            {

                iControl.SetModel(iModel);

                iControl.RequestSetSourceTypesDescrip(description);

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestRefresh();

                ssTO.serviceSources = iModel.SourceTypesDataTable.Select(s => new ServiceSourceType()
                {
                    Description = s.SourceTypeDesc,
                    TypeId = s.SourceTypeId,
                    Order = s.SourceTypeOrder
                }).ToList();

                ssTO.Batch = page_number.ToInt32();
                ssTO.BatchLength = page_size.ToInt32();
                ssTO.Total = ssTO.serviceSources.Count;

                ssTO.serviceSources = ssTO.serviceSources.Skip(page_number.ToInt32() * page_size.ToInt32()).Take(page_size.ToInt32()).ToList();
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;
                ssTO.ErrorStatus = retVal;
            }


            return ssTO;
        }

        public string DeleteSourceTypes(string sourceIds)
        {
            List<int> sourceTypeIds = new List<int>();

            sourceIds.Split(',').ToList().ForEach(id => sourceTypeIds.Add(id.ToInt32()));

            SourceTypeFilterModel iModel = new SourceTypeFilterModel();
            SourceTypeFilterControl iControl = new SourceTypeFilterControl();
            string retVal = "";
            try
            {
                iControl.SetModel(iModel);
                iControl.RequestSetUser(WebHelper.GetUser());
                iControl.RequestSetSelectedIds(sourceTypeIds);

                iControl.RequestDelete();
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

        public string AddSourceType(string TypeId, string Description, string Order)
        {

            WebHelper.WriteParams(Description, Order);

            string retVal = "";

            SourceTypeEditorModel iModel = new SourceTypeEditorModel();
            SourceTypeEditorControl iControl = new SourceTypeEditorControl();

            try
            {
                if (iModel != null)
                    iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());
                iControl.RequestSetSelectedId(TypeId.ToInt32());
                iControl.RequestRefresh();
                iControl.RequestSetSourceTypeDesc(Description);
                iControl.RequestSetSourceTypeOrder(Order);
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