using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.BLL;
using System.Configuration;
using System.IO;
using TDBCore.EntityModel;
using GedItter.Tools;
using TDBCore.Types;

namespace TDBCore.BLL
{
    public class TreeBLL
    {

        public string SaveNewTree(string sourceId, string fileName, string sourceRef, string sourceDesc, string sourceYear, string sourceYearTo)
        {
            Guid _sourceId = sourceId.ToGuid();
            SourceBLL _sources = new SourceBLL();
            string errorStr = "";
            // add new entry 
            if (_sourceId == Guid.Empty)
            {
                string appPath = ConfigurationManager.AppSettings["StorageRoot"].ToString();
                string treePath = Path.Combine(appPath, fileName);
                _sourceId = Guid.NewGuid();

                if (System.IO.File.Exists(treePath))
                {
                    Source source = _sources.CreateBasicSource(_sourceId, sourceRef, sourceYear.ToInt32(), sourceYearTo.ToInt32(), sourceDesc);

                    SourceMappingsBLL smapBll = new SourceMappingsBLL();

                    smapBll.Insert(_sourceId, null, null, 1, null, DateTime.Today.ToShortDateString(), 39);


                    if (source != null)
                    {
                        CsImportGeds csImportGeds = new CsImportGeds();
                        errorStr = csImportGeds.ImportGeds(treePath, _sourceId);
                    }

                    System.IO.File.Delete(treePath);
                }

            }
            // edit existing
            else
            {
                //edit source

                _sources.UpdateBasic(_sourceId, sourceRef, sourceDesc, sourceYear.ToInt32(), sourceYearTo.ToInt32());



            }

            return errorStr;
        }
    }



}
