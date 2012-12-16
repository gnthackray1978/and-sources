using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.Interfaces;
using System.Collections;
using TDBCore.Types;
using System.Runtime.Serialization;

#if! SILVERLIGHT
using TDBCore.EntityModel;
using GedItter.BirthDeathRecords.BLL;
using TDBCore.BLL;
using System.Data;


#endif

using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace TDBCore.ModelObjects
{

   
    public class TreeBase
    {
        //public List<DoublePoint> _debugzoomHist = new List<DoublePoint>();
        protected double boxWidth;
        protected double boxHeight;
        protected List<List<TreePerson>> generations = new List<List<Types.TreePerson>>();
        protected List<int> familiesPerGeneration = new List<int>();
        protected List<List<List<TreePoint>>> familySpanLines = new List<List<List<TreePoint>>>();
        protected List<TreePoint> childlessMarriages = new List<TreePoint>();

        protected double centrePoint = 750;
        protected double centreVerticalPoint = 0;

        protected double zoomLevel = 0;

        protected double centrePointXOffset = 0;
        protected double centrePointYOffset = 0;

        protected double original_distanceBetweenBoxs;
        protected double original_distanceBetweenGens;
        protected double original_boxWidth;
        protected double original_boxHeight;
        protected double original_distancesbetfam;
        protected double original_lowerStalkHeight;

        protected double original_middleSpan = 40;
        protected double original_topSpan = 20;

        protected double zoomPercentage;
        protected double distanceBetweenBoxs;
        protected double distanceBetweenGens;
        protected double halfBox;
        protected double halfBoxHeight;

        protected Guid fatherId = Guid.Empty;
        protected TreePerson treePerson = new TreePerson();


        protected int mouse_x = 0;
        protected int mouse_y = 0;

        protected int initial_mouse_x = 0;
        protected int initial_mouse_y = 0;

        //  private int lenOffset = 300;



        protected double xFromCentre = 0;
        protected double yFromCentre = 0;


        protected double drawingX1 = 0;
        protected double drawingX2 = 0;
        protected double drawingY1 = 0;
        protected double drawingY2 = 0;
        // double drawingCentreVertical = 0;
        protected double drawingCentre = 0;
        protected double drawingWidth = 0;
        protected double drawingHeight = 0;


        protected double mouseXPercLocat = 0;
        protected double mouseYPercLocat = 0;

        protected int zoomAmount = 8;


#if! SILVERLIGHT

        protected TreePerson FillBasicDetails(GetDescendants_Result descendant, int currentIdx, Guid spouseId, List<Person> persons)
        {
            TreePerson treePerson = new TreePerson();

            treePerson.IsDisplayed = true;


            //treePerson.ChildCount
            treePerson.ChildLst = new List<Guid>();
            treePerson.SpouseLst = new List<Guid>();
            treePerson.ChildIdxLst = new List<int>();
            treePerson.SpouseIdxLst = new List<int>();

            treePerson.MotherId = Guid.Empty;
            treePerson.FatherId = Guid.Empty;

            if (currentIdx == 0)
            {
                treePerson.GenerationIdx = 0;
                //treePerson.PersonId = descendant.ParentId.GetValueOrDefault();

                if (spouseId != Guid.Empty)
                {
                    treePerson.PersonId = spouseId;
                }
                else
                {
                    treePerson.PersonId = descendant.ChildId.GetValueOrDefault();
                }
            }
            else
            {
               
                treePerson.GenerationIdx = descendant.level.GetValueOrDefault();

                if (spouseId == Guid.Empty)
                {
                    treePerson.PersonId = descendant.ChildId.GetValueOrDefault();
                    treePerson.MotherId = descendant.motherId.GetValueOrDefault();
                    treePerson.FatherId = descendant.ParentId.GetValueOrDefault();
                }
                else
                {
                    treePerson.PersonId = spouseId;
                }

            }

            Person tp = persons.FirstOrDefault(p => p.Person_id == treePerson.PersonId);

            if (tp != null)
            {


                treePerson.BirthLocation = tp.BirthLocation;
                treePerson.DOB = tp.BirthInt.ToString();


                if (tp.DeathInt == 0)
                    treePerson.DOD = "";
                else
                    treePerson.DOD = tp.DeathInt.ToString();

                treePerson.DeathLocation = tp.DeathLocation;

                treePerson.IsHtmlLink = false;
                treePerson.Name = tp.ChristianName + " " + tp.Surname;
                treePerson.Occupation = tp.Occupation;

       
            }
            else
            {

                treePerson.Name = "Unknown";
                 
            }



            return treePerson;
        }


        protected TreePerson FillPersonBasicDetails(Guid child)
        {
            DeathsBirthsBLL deathsBirthsBll = new DeathsBirthsBLL();
            TreePerson treep = new TreePerson();

            var personsDataTable = deathsBirthsBll.GetDeathBirthRecordById2(child).FirstOrDefault();

            if (personsDataTable != null)
            {
                treep.BirthLocation = personsDataTable.BirthLocation;
                treep.DOB = personsDataTable.BirthInt.ToString();
                treep.PersonId = child;

                if (personsDataTable.DeathInt == 0)
                    treep.DOD = "";
                else
                    treep.DOD = personsDataTable.DeathInt.ToString();

                treep.DeathLocation = personsDataTable.DeathLocation;

                treep.IsHtmlLink = false;
                treep.Name = personsDataTable.ChristianName + " " + personsDataTable.Surname;
                treep.Occupation = personsDataTable.Occupation;

                treep.SpouseLst = new List<Guid>();
                treep.ChildLst = new List<Guid>();
            }
            else
            {
                treep.PersonId = child;
                treep.Name = "Unknown";
                treep.SpouseLst = new List<Guid>();
                treep.ChildLst = new List<Guid>();
            }

            return treep;
        }



        protected TreePerson FillPersonDetails(List<Relation> spouseTable,
            List<Relation> parentTable,
            IList<uvw_ParentMapChildren> _childTable,
            uvw_ParentMapChildren _currentRel,
            int currentGen,
            bool isHtmlLink = false)
        {
            //RelationsBLL relationsBll = new BLL.RelationsBLL();
            int relationType = _currentRel.RelationType.GetValueOrDefault();
            Guid parent = _currentRel.ParentId.GetValueOrDefault();
            Guid child= _currentRel.PersonA.GetValueOrDefault();
            TreePerson parentPerson = generations[currentGen - 1].Last();
            List<Guid> parentSpouses = generations[currentGen - 1].Last().SpouseLst;
            //IList<uvw_ParentMapChildren> _childTable = null;

            TreePerson treep = FillPersonBasicDetails(child);
            treep.IsHtmlLink = isHtmlLink;

            //if (treep.name.Contains("Ann Blan"))
            //{
            //    Debug.WriteLine("");
            //}

            //if (treep.name.Contains("George Thackray") && treep.datebirth == "1756")
            //{
            //    Debug.WriteLine("");
            //}

            

            treep.ChildCount = _childTable.Count;

            treep.ChildLst = _childTable.Select(o => o.Person_id).ToList();

            if (_childTable.Count == 0)
            {
                treep.SpouseLst.AddRange(spouseTable.Where(o => o.PersonA.Person_id == _currentRel.Person_id).
                    Where(pr => (!treep.SpouseLst.Contains(pr.PersonB.Person_id))).Select(p => p.PersonB.Person_id).ToList());

                treep.SpouseLst.AddRange(spouseTable.Where(o => o.PersonB.Person_id == _currentRel.Person_id).
                    Where(pr => (!treep.SpouseLst.Contains(pr.PersonA.Person_id))).Select(p => p.PersonA.Person_id).ToList());

            }


            if (relationType == 2)
            {
                treep.FatherId = parent;

                if (parentTable != null)
                {
                    Relation rr = parentTable.AsEnumerable().Where(o => o.PersonA.Person_id == child).FirstOrDefault();

                    if (rr != null)
                    {
                        treep.MotherId = rr.PersonB.Person_id;

                        if (!parentSpouses.Contains(treep.MotherId))
                            parentSpouses.Insert(0, treep.MotherId);
                    }
                }



                parentSpouses.InsertRange(0, spouseTable.Where(o => o.PersonB.Person_id == parent)
                        .Where(c => (!parentSpouses.Contains(c.PersonA.Person_id))).Select(s => s.PersonA.Person_id).ToList());


            }
            else
            {
                 treep.MotherId = parent;

                if (parentTable != null)
                {
                    Relation rr = parentTable.AsEnumerable().Where(o => o.PersonA.Person_id == child).FirstOrDefault();

                    if (rr != null)
                    {
                        treep.FatherId = rr.PersonB.Person_id;

                        if (!parentSpouses.Contains(treep.FatherId))
                            parentSpouses.Insert(0, treep.FatherId);
                    }
                }



                parentSpouses.InsertRange(0, spouseTable.Where(o => o.PersonA.Person_id == parent)
                     .Where(c => (!parentSpouses.Contains(c.PersonB.Person_id))).Select(s => s.PersonB.Person_id).ToList());



            }




            return treep;
        }


        protected TreePerson FillPersonDetails2(Guid child)
        {
            TreePerson treep = new TreePerson();



            DeathsBirthsBLL deathsBirthsBll = new DeathsBirthsBLL();
        //    DsDeathsBirths.PersonsDataTable personsDataTable = new DsDeathsBirths.PersonsDataTable();
            var personsDataTable = deathsBirthsBll.GetDeathBirthRecordById2(child).FirstOrDefault();

            if (personsDataTable != null)
            {
                //   Debug.WriteLine(personsDataTable[0].BirthLocation);
                //   Debug.WriteLine(personsDataTable[0].BirthInt.ToString());



                treep.BirthLocation = personsDataTable.BirthLocation;// "Hillam,Monk Frystone";
                treep.DOB = personsDataTable.BirthInt.ToString();// "1734";

                //treep.birthLocation = personsDataTable[0].BirthLocation;
                //treep.datebirth = personsDataTable[0].BirthInt.ToString();


                if (personsDataTable.DeathInt == 0)
                    treep.DOD = "";
                else
                    treep.DOD = personsDataTable.DeathInt.ToString();

                treep.DeathLocation = personsDataTable.DeathLocation;
                treep.IsHtmlLink = false;

                treep.Name = personsDataTable.ChristianName + " " + personsDataTable.Surname;
                treep.Occupation = personsDataTable.Occupation;

                treep.SpouseLst = new List<Guid>();
                treep.ChildLst = new List<Guid>();


                treep.PersonId = new Guid(child.ToString());

                //treep.personId = child;

            }
            else
            {
                treep.PersonId = child;
                treep.Name = "Unknown";
                treep.SpouseLst = new List<Guid>();
                treep.ChildLst = new List<Guid>();
            }


            return treep;
        }


#endif

        #region properties


        public double DrawingWidth
        {
            get
            {
                return this.drawingWidth;
            }
        }

        public double DrawingHeight
        {
            get
            {
                return this.drawingHeight;
            }
        }

        public double BoxHeight
        {
            get
            {
                return this.boxHeight;
            }
            set
            {
                this.boxHeight = value;
            }
        }
        
        public double BoxWidth
        {
            get
            {
                return this.boxWidth;
            }
            set
            {
                this.boxHeight = value;
            }
        }

      
        public List<List<TreePerson>> Generations
        {
            get
            {
                return this.generations;
            }
            set
            {
                this.generations = value;
            }
        }
       
        public List<int> FamiliesPerGeneration
        {
            get
            {
                return this.familiesPerGeneration;
            }
            set
            {
                this.familiesPerGeneration = value;
            }
        }

      
        public List<List<List<TreePoint>>> FamilySpanLines
        {
            get
            {
                return this.familySpanLines;
            }
            set
            {
                this.familySpanLines = value;
            }
        }

        
        public List<TreePoint> ChildlessMarriages
        {
            get
            {
                return this.childlessMarriages;
            }
            set
            {
                this.childlessMarriages = value;
            }
        }



       
        public double CentreXPoint
        {
            get
            {
                return this.centrePoint;
            }
            set
            {
                this.centrePoint = value;
            }
        }

        
        public double CentreYPoint
        {
            get
            {
                return this.centreVerticalPoint;
            }
            set
            {
                this.centreVerticalPoint = value;
            }
        }


        
        public Double ZoomPercentage
        {
            get
            {
                return this.zoomPercentage;
            }
            set
            {
                this.zoomPercentage = value;
            }
        }


        
        public int ZoomLevel
        {
            get
            {
                return TreeBase.CalcZoomLevel(this.zoomPercentage);
            }
            set
            {
               
            }
        }

        public double CentrePoint
        {
            get
            {
                return centrePoint;
            }

        }

        
        public Types.TreePerson TreePerson
        {
            get
            {
                return treePerson;

            }
            set
            {
                treePerson = value;
            }
        }


        #endregion


        public virtual void Refresh()
        {
            //overridden in children 
        }

        public virtual void ComputeBoxLocations()
        {

        }



        public void GetDetail(TreePerson _tp, out string name, out string detail, out int name_size, out int detail_size)
        {
            string retDetail = "";


            string deathStr = "";
            string birthStr = "";

            if (_tp.DOD != "")
                deathStr = "Died: " + _tp.DOD + Environment.NewLine;

            if (_tp.DeathLocation != "")
                deathStr += "In. " + _tp.DeathLocation;


            if (_tp.DOB != "")
                birthStr = "Born: " + _tp.DOB + Environment.NewLine;

            if (_tp.BirthLocation != "")
                birthStr += "In. " + _tp.BirthLocation;



            name = "";
            detail = "";
            name_size = 8;
            detail_size = 8;


            switch (_tp.zoom)
            {
                case 1:
                    name = "";
                    break;
                case 2:
                    if (_tp.Name != null)
                    {
                        string[] parts = _tp.Name.Split(' ');

                        foreach (string _part in parts)
                        {
                            if (_part.Length > 0)
                                name += _part.Substring(0, 1) + " ";
                        }
                    }
                    break;
                case 3:
                    name = _tp.Name;
                    detail = _tp.DOB;
                    break;
                case 4:
                    name = _tp.Name + Environment.NewLine;
                    detail = _tp.DOB;
                    break;
                case 5:
                    name = _tp.Name + Environment.NewLine;
                    detail = birthStr;
                    break;
                case 6:
                    name = _tp.Name + Environment.NewLine;

                    detail = birthStr + Environment.NewLine + _tp.Occupation + Environment.NewLine + deathStr;
                    name_size = 10;
                    detail_size = 10;
                    break;
                case 7:
                    name = _tp.Name;
                    detail = birthStr + Environment.NewLine + _tp.Occupation + Environment.NewLine + deathStr;
                    name_size = 12;
                    detail_size = 12;
                    break;
            }




            if (retDetail != null && retDetail.Length > 0)
            {
                retDetail = retDetail.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
            }





        }





        public void SetMouse(int x, int y)
        {

            this.mouse_x = x;
            this.mouse_y = y;

            if (initial_mouse_x == 0)
                initial_mouse_x = this.mouse_x;

            if (initial_mouse_y == 0)
                initial_mouse_y = this.mouse_y;



            if (this.mouse_x < this.centrePoint)
            {
                xFromCentre = this.centrePoint - this.mouse_x;
            }
            else
            {
                if (this.centrePoint < 0)
                {
                    xFromCentre = mouse_x + Math.Abs(this.centrePoint);
                }
                else
                {
                    xFromCentre = mouse_x - centrePoint;
                }

                xFromCentre = xFromCentre - (xFromCentre * 2);
            }


            if (this.mouse_y < this.centreVerticalPoint)
            {
                yFromCentre = this.centreVerticalPoint - this.mouse_y;
            }
            else
            {
                if (centreVerticalPoint < 0)
                {
                    yFromCentre = Math.Abs(centreVerticalPoint) + mouse_y;
                }
                else
                {
                    yFromCentre = mouse_y;
                }


                yFromCentre = yFromCentre - (yFromCentre * 2);
            }

         //   Debug.WriteLine("set mouse: " +initial_mouse_x + "-" + xFromCentre + "-" + this.centrePoint);
        }

        public void SetCentrePoint(int param_x, int param_y)
        {

            if (param_x == 1000000 && param_y == 1000000)
            {
                this.ResetOffset();
            }
            else
            {

                if (centrePointXOffset == 0)
                {
                    centrePointXOffset = this.centrePoint - param_x;
                }
                else
                {

                    this.centrePoint = param_x + centrePointXOffset;
                }


                if (centrePointYOffset == 0)
                {
                    centrePointYOffset = this.centreVerticalPoint - param_y;
                }
                else
                {

                    this.centreVerticalPoint = param_y + centrePointYOffset;
                }

            }




        //    Debug.WriteLine("setcentrepoint:" + centrePointXOffset + "," + xFromCentre);
        }

        public void SetZoomStart()
        {
            GetPercDistances(ref this.mouseXPercLocat, ref this.mouseYPercLocat);
        }



        /// <summary>
        /// get the distances the mouse X1 and Y1 coordinates are from the edges of the drawing as a percentage of the overall width/height of the drawing
        /// </summary>i didn
        private void GetPercDistances(ref double percX1, ref double percY1)
        {
            /*
             the drawing x and y changes as the drawing is moved around
             
             * 
             * 
             */


            double _distanceFromX1 = 0;
            double _distanceFromY1 = 0;
            double onePercentDistance = 0;
            percX1 = 0;
            percY1 = 0;


            drawingWidth = drawingX2 - drawingX1;
            drawingHeight = drawingY2 - drawingY1;

            if (drawingWidth == 0 || drawingHeight == 0) return;

            if (drawingX1 > 0)
            {
                _distanceFromX1 = this.mouse_x - drawingX1;//;
            }
            else
            {
                // 
                _distanceFromX1 = Math.Abs(drawingX1) + this.mouse_x;
            }

            onePercentDistance = drawingWidth / 100;



            percX1 = _distanceFromX1 / onePercentDistance;




            if (drawingY1 > 0)
            {
                _distanceFromY1 = this.mouse_y - drawingY1;// ;                

            }
            else
            {
                _distanceFromY1 = Math.Abs(drawingY1) + this.mouse_y;
            }

            onePercentDistance = drawingHeight / 100;
            percY1 = _distanceFromY1 / onePercentDistance;

            //    Debug.WriteLine("mouse position in diagram: " + _distanceFromX1.ToString() + "," + _distanceFromY1.ToString());
        }


        public void ResetOffset()
        {

            //  Debug.WriteLine("ResetOffset");
            centrePointXOffset = 0;
            centrePointYOffset = 0;
        }

        public void ZoomIn()
        {
            zoomAmount++;
            SetZoom(zoomAmount);
        }

        public void ZoomOut()
        {

            if (zoomAmount > 7)
                zoomAmount--;


            SetZoom(zoomAmount - (zoomAmount * 2));
        }

        public virtual void SetZoom(int percentage)
        {
            // dont zoom anything if we dont have an amount to zoom
            if (percentage == 0)
            {
                return;
            }

            double workingtp = 0;
            double _percLocal_x = 0;
            double _percLocal_y = 0;

            //zoom drawing components 
            zoomPercentage += percentage;
            zoomLevel += percentage;
            workingtp = original_distanceBetweenBoxs / 100;
            distanceBetweenBoxs = workingtp * zoomPercentage;
            workingtp = original_boxWidth / 100;
            boxWidth = workingtp * zoomPercentage;
            halfBox = boxWidth / 2;
            workingtp = original_distancesbetfam / 100;
            workingtp = original_distanceBetweenGens / 100;
            distanceBetweenGens = workingtp * zoomPercentage;
            workingtp = original_boxHeight / 100;
            boxHeight = workingtp * zoomPercentage;

            halfBoxHeight = boxHeight / 2;



            // draw zoomed drawing
            this.ComputeBoxLocations();

            //get new cursor position percentages
            GetPercDistances(ref  _percLocal_x, ref  _percLocal_y);



            centreVerticalPoint += (this.drawingHeight / 100) * (_percLocal_y - this.mouseYPercLocat);


            //   Debug.WriteLine("zoom percentage: " + zoomPercentage + " adjusted: " + (this.drawingWidth / 100) * (_percLocal_x - this.mouseXPercLocat) + " pl " + _percLocal_x + " mx " + this.mouseXPercLocat + " dx " + this.drawingX1);

            //    Debug.WriteLine("zoom percentage: " + zoomPercentage + " drawing height: " + this.drawingHeight);


            centrePoint += (this.drawingWidth / 100) * (_percLocal_x - this.mouseXPercLocat);

            // refresh drawing
            this.ComputeBoxLocations();

         //   Debug.WriteLine("zoom lev: " + this.ZoomLevel);


        }


        public void SetFather(Guid fatherId)
        {
            if (this.fatherId != fatherId)
            {
                this.fatherId = fatherId;


            }
        }




        protected void CalcTPZoom(TreePerson _tp)
        {
            double boxarea = (_tp.X2 - _tp.X1) * (_tp.Y2 - _tp.Y1);

            _tp.zoom = TreeBase.CalcAreaLevel(boxarea);



        }

        public static int CalcZoomLevel(double zoomPercentage)
        {

            if (zoomPercentage > 0 && zoomPercentage < 40)
            {
                return 1;
            }
            else if (zoomPercentage >= 40 && zoomPercentage < 60)
            {
                return 2;
            }
            else if (zoomPercentage >= 60 && zoomPercentage <= 150)
            {
                return 3;
            }
            else if (zoomPercentage > 150 && zoomPercentage <= 200)
            {
                return 4;
            }
            else if (zoomPercentage > 200 && zoomPercentage <= 250)
            {
                return 5;
            }
            else if (zoomPercentage > 250 && zoomPercentage <= 300)
            {
                return 6;
            }
            else if (zoomPercentage > 300)
            {
                return 7;
            }

            return 0;
        }

        public static int CalcAreaLevel(double area)
        {

            if (area > 0 && area < 1000)
            {
                return 1;
            }
            else if (area >= 1000 && area < 2500)
            {
                return 2;
            }
            else if (area >= 2500 && area <= 5000)
            {
                return 3;
            }
            else if (area > 5000 && area <= 10000)
            {
                return 4;
            }
            else if (area > 10000 && area <= 15000)
            {
                return 5;
            }
            else if (area > 15000 && area <= 20000)
            {
                return 6;
            }
            else if (area > 20000)
            {
                return 7;
            }

            return 0;
        }

    }



    public class AncestorModel : TreeBase//, ITreeModel
    {   
        List<double> adjustedDistances = new List<double>();
        List<double> adjustedBoxWidths = new List<double>();
        List<double> adjustedBoxHeights = new List<double>();

        public AncestorModel()
        {
            Debug.WriteLine("created ancestor model");
            zoomPercentage = 100;

            original_distanceBetweenBoxs = 10;
            original_distanceBetweenGens = 125;
            original_boxWidth = 100;
            original_boxHeight = 70;
            original_distancesbetfam = 100;
            original_lowerStalkHeight = 5;

            original_middleSpan = 40;
            original_topSpan = 20;

            distanceBetweenBoxs = (int)original_distanceBetweenBoxs;
            distanceBetweenGens = (int)original_distanceBetweenGens;
            boxWidth = (int)original_boxWidth;
            boxHeight = (int)original_boxHeight;
            // distancesbetfam = (int)original_distancesbetfam;
            halfBox = boxWidth / 2;
            halfBoxHeight = boxHeight / 2;


            //   this.centreVerticalPoint = 500;
            //   lowerSpan = (int)original_lowerStalkHeight;

            //   middleSpan = (int)original_middleSpan;

            //   topSpan = (int)original_topSpan;

            //   boxLocats = createBoxLocations(this.generations.Count);
        }

        public override void ComputeBoxLocations()
        {
            if (this.generations.Count == 0)
            {
                Debug.WriteLine("No records found for supplied GUID");
                return;
            }




            int genidx = 0;


            this.drawingX2 = 0;
            this.drawingX1 = 0;

            double _y = CentreYPoint;




            double percentageLess = 0;

            adjustedDistances = new List<double>(generations.Count);
            adjustedBoxWidths = new List<double>(generations.Count);
            adjustedBoxHeights = new List<double>(generations.Count);

            this.generations[0][0].X1 = CentrePoint;
            this.generations[0][0].X2 = CentrePoint + this.boxWidth;
            this.generations[0][0].Y1 = _y;
            this.generations[0][0].Y2 = _y + this.boxHeight;

            double newX1 = 0;
            double newX2 = 0;


            genidx = 0;
            while (genidx < generations.Count)
            {


                int personIdx = 0;

                percentageLess += 2;

               // Debug.WriteLine("generation: " + genidx.ToString());

                while (personIdx < this.generations[genidx].Count)
                {

                    newX1 = 0;
                    newX2 = 0;


                    GetNewX(genidx,
                        percentageLess,
                        personIdx,
                        ref newX1,
                        ref newX2
                        );

                    #region handle overlaps

                    double overlap = 0;
                    double requiredSpace = 0;

                    //  check there hasnt been a collision


                    #region part 1
                    if (personIdx > 0)
                    {

                        if (this.generations[genidx][personIdx - 1].X2 > newX1)
                        {
                            overlap = this.generations[genidx][personIdx - 1].X2 - newX1;
                            overlap += adjustedDistances[genidx];
                        }

                        int newChildidx = this.generations[genidx][personIdx].ChildIdx;
                        int oldChildidx = this.generations[genidx][personIdx - 1].ChildIdx;
                        int countPersonSpaces = newChildidx - oldChildidx;

                        if (countPersonSpaces > 1)
                        {

                            countPersonSpaces--;
                            //needed space
                            requiredSpace = (countPersonSpaces * adjustedBoxWidths[genidx - 1]) + ((countPersonSpaces + 1) * (adjustedDistances[genidx - 1] + 5));
                            try
                            {
                                double spaceSoFarCreated = (this.generations[genidx - 1][newChildidx].X1 - this.generations[genidx - 1][oldChildidx].X2) + overlap;

                                // we dont have enough space!
                                if (requiredSpace > spaceSoFarCreated)
                                {
                                    // increase the overlap so enough space if provided
                                    overlap += (requiredSpace - spaceSoFarCreated);
                                }
                                else if (overlap == 0)
                                {
                                    overlap = (requiredSpace - spaceSoFarCreated);
                                }
                            }
                            catch (Exception ex1)
                            {
                                Debug.WriteLine(ex1.Message);
                            }
                        }

                    }

                    #endregion


                    if (overlap > 0)
                    {

                    //    Debug.WriteLine("overlapped: " + personIdx.ToString());
                   

                        #region part 2
                        List<TreePerson> moveList = new List<Types.TreePerson>();


                        getMoveList(personIdx - 1, genidx, ref moveList);




                        var sorted = moveList.OrderByDescending(o => o.GenerationIdx);

                        foreach (TreePerson _treePerson in sorted)
                        {

                            try
                            {
                                int tpPersonIdx = _treePerson.Index;

                                while (tpPersonIdx >= 0)
                                {

                                //    Debug.WriteLine("moving: " + _treePerson.Name + " " + _treePerson.X1 + " " + _treePerson.X2);


                                    TreePerson _movePerson = this.generations[_treePerson.GenerationIdx][tpPersonIdx];

                                    TreePerson _prevPerson = null;
                                    TreePerson _nextPerson = null;


                                    if (tpPersonIdx > 0)
                                        _prevPerson = this.generations[_treePerson.GenerationIdx][tpPersonIdx - 1];

                                    if ((tpPersonIdx + 1) < this.generations[_treePerson.GenerationIdx].Count)
                                        _nextPerson = this.generations[_treePerson.GenerationIdx][tpPersonIdx + 1];

                                    double workingX1 = 0;
                                    double workingX2 = 0;

                                    if ((_movePerson.FatherIdx == -1 && _movePerson.MotherIdx == -1) || (_movePerson.GenerationIdx == genidx))
                                    {
                                        if (_movePerson.GenerationIdx == genidx)
                                        {
                                            workingX1 = _movePerson.X1 - overlap;
                                            workingX2 = _movePerson.X2 - overlap;
                                        }
                                        else
                                        {

                                            double parentlessPersonStartX = _movePerson.X1 - overlap;// GetX1ForParentlessPerson(_movePerson.generation, _movePerson.index);

                                            if (parentlessPersonStartX == 0)
                                            {
                                                parentlessPersonStartX = 15;
                                                workingX2 = _nextPerson.X1 - parentlessPersonStartX;
                                                workingX1 = workingX2 - adjustedBoxWidths[_nextPerson.GenerationIdx];
                                            }
                                            else
                                            {
                                                workingX1 = parentlessPersonStartX;
                                                workingX2 = workingX1 + adjustedBoxWidths[_nextPerson.GenerationIdx];
                                            }
                                        }

                                    }
                                    else
                                    {
                                        CreateChildPositionFromParent(_movePerson, ref workingX1, ref workingX2);
                                    }

                               //     Debug.WriteLine("wk 1 and 2:" + workingX1.ToString() + " - " + workingX2.ToString());

                                    _movePerson.X1 = workingX1;// -adjustedDistanceApart;
                                    _movePerson.X2 = workingX2;// -adjustedDistanceApart;

                                    tpPersonIdx--;
                                }



                            }
                            catch (Exception ex1)
                            {
                                Debug.WriteLine(ex1.Message);
                                Debugger.Break();
                            }


                        }


                        #endregion
                    }


                    #endregion

                    this.generations[genidx][personIdx].X1 = newX1;// _x - adjustedBoxWidth;
                    this.generations[genidx][personIdx].X2 = newX2;// _x + adjustedBoxWidth;

                    this.generations[genidx][personIdx].Y1 = _y;
                    this.generations[genidx][personIdx].Y2 = _y + adjustedBoxHeights[genidx];


                    CalcTPZoom(this.generations[genidx][personIdx]);

                    personIdx++;
                }



                _y -= this.distanceBetweenGens;

                genidx++;



            }

            CreateChildPositionFromParent(generations[0][0], ref newX1, ref newX2);

            generations[0][0].X1 = newX1;
            generations[0][0].X2 = newX2;



            genidx = 0;

            drawingX1 = generations[0][0].X1;
            drawingX2 = generations[0][0].X2;

            while (genidx < this.generations.Count)
            {
                if (drawingX1 > this.generations[genidx][0].X1)
                    drawingX1 = this.generations[genidx][0].X1;

                if (drawingX2 < this.generations[genidx][this.generations[genidx].Count - 1].X2)
                    drawingX2 = this.generations[genidx][this.generations[genidx].Count - 1].X2;

                genidx++;
            }



            if (drawingX1 == 0)
                Debugger.Break();



            // top of the screen
            drawingY1 = generations[generations.Count - 1][0].Y2;

            //bottom of the screen
            drawingY2 = generations[0][0].Y1;



            drawingHeight = generations[0][0].Y1 - generations[generations.Count - 1][0].Y2;

            drawingCentre = (drawingX2 - drawingX1) / 2;
            drawingWidth = drawingX2 - drawingX1;




            CreateConnectionLines();


        }

        private void CreateConnectionLines()
        {
            // this.FamilySpanLines = new List<List<List<TreePoint>>>();

            double middleGeneration = 0;
            double middleXChild = 0;
            double middleParent = 0;
            double middleTopChild = 0;
            double bottomParent = 0;

            double parentHeight = 0;
            double distanceBetweenGens = 0;


            int genidx = 0;
            while (genidx < generations.Count)
            {
                try
                {
                    int personIdx = 0;
                    //if (genidx == 9 )
                    //{
                    //    Debug.WriteLine("");
                    //}
                    if (genidx + 1 >= familySpanLines.Count)
                    {
                        genidx++;
                        continue;
                    }


                    while (personIdx < this.generations[genidx].Count)
                    {

                        try
                        {
                            familySpanLines[genidx][personIdx].Clear();
                            middleTopChild = generations[genidx][personIdx].Y1 + 10;
                            if (generations.Count > (genidx + 1))
                            {
                                parentHeight = (generations[genidx + 1][0].Y2 - generations[genidx + 1][0].Y1);
                                bottomParent = generations[genidx + 1][0].Y1 + parentHeight + 10;
                                distanceBetweenGens = (generations[genidx][personIdx].Y1 - generations[genidx + 1][0].Y2);
                                if (generations[genidx][personIdx].FatherIdx > 0 || generations[genidx][personIdx].MotherIdx > 0)
                                {
                                    // top middle of child 
                                    middleXChild = (generations[genidx][personIdx].X1 + generations[genidx][personIdx].X2) / 2;
                                    middleGeneration = generations[genidx][personIdx].Y1 - (distanceBetweenGens / 2) + 10;
                                    // move to top and middle of child
                                    familySpanLines[genidx][personIdx].Add(new TreePoint(middleXChild, middleTopChild));
                                    // move to middle of generations about child
                                    familySpanLines[genidx][personIdx].Add(new TreePoint(middleXChild, middleGeneration));
                                    int patIdx = generations[genidx][personIdx].FatherIdx;
                                    if (patIdx != -1)
                                    {
                                        // move to middle generation under parent
                                        middleParent = (generations[genidx + 1][patIdx].X1 + generations[genidx + 1][patIdx].X2) / 2;
                                        familySpanLines[genidx][personIdx].Add(new TreePoint(middleParent, middleGeneration));
                                        if (drawingHeight > 200)
                                        {
                                            // move to bottom of parent
                                            familySpanLines[genidx][personIdx].Add(new TreePoint(middleParent, bottomParent));
                                        }
                                        else
                                        {
                                            familySpanLines[genidx][personIdx].Add(new TreePoint(middleParent, middleGeneration - 4));
                                        }
                                        // move to middle generation under parent
                                        familySpanLines[genidx][personIdx].Add(new TreePoint(middleParent, middleGeneration));
                                        // move to middle of child
                                        familySpanLines[genidx][personIdx].Add(new TreePoint(middleXChild, middleGeneration));
                                        // move to top and middle of child
                                        familySpanLines[genidx][personIdx].Add(new TreePoint(middleXChild, middleTopChild));
                                    }
                                    patIdx = generations[genidx][personIdx].MotherIdx;
                                    if (patIdx != -1)
                                    {
                                        middleParent = (generations[genidx + 1][patIdx].X1 + generations[genidx + 1][patIdx].X2) / 2;
                                        // move to middle of generations about child
                                        familySpanLines[genidx][personIdx].Add(new TreePoint(middleXChild, middleGeneration));
                                        familySpanLines[genidx][personIdx].Add(new TreePoint(middleParent, middleGeneration));
                                        if (drawingHeight > 200)
                                        {
                                            // move to bottom of parent
                                            familySpanLines[genidx][personIdx].Add(new TreePoint(middleParent, bottomParent));
                                        }
                                        else
                                        {
                                            familySpanLines[genidx][personIdx].Add(new TreePoint(middleParent, middleGeneration - 4));
                                        }
                                        // move to middle generation under parent
                                        familySpanLines[genidx][personIdx].Add(new TreePoint(middleParent, middleGeneration));
                                        // move to middle of child
                                        familySpanLines[genidx][personIdx].Add(new TreePoint(middleXChild, middleGeneration));
                                        // move to top and middle of child
                                        familySpanLines[genidx][personIdx].Add(new TreePoint(middleXChild, middleTopChild));
                                    }
                                }
                            }

                        }
                        catch (Exception ex1)
                        {
                            Debug.WriteLine(ex1.Message);
                            Debugger.Break();
                        }

                        personIdx++;
                    }
                }
                catch (Exception ex1)
                {
                    Debug.WriteLine(ex1.Message);
                    Debugger.Break();
                }
                genidx++;
            }



        }

        private void CreateChildPositionFromParent(TreePerson movePerson, ref double newX1, ref double newX2)
        {

            double boxWidth = 0;

            if (adjustedBoxWidths.Count > movePerson.GenerationIdx)
                boxWidth = adjustedBoxWidths[movePerson.GenerationIdx];
            else
                boxWidth = this.boxWidth;

            if (movePerson.FatherIdx == -1)
            {
                newX1 = ((this.generations[movePerson.GenerationIdx + 1][movePerson.MotherIdx].X1 + this.generations[movePerson.GenerationIdx + 1][movePerson.MotherIdx].X2) / 2) - (boxWidth / 2);


                newX2 = newX1 + boxWidth;
            }

            if (movePerson.MotherIdx == -1)
            {
                newX1 = ((this.generations[movePerson.GenerationIdx + 1][movePerson.FatherIdx].X1 + this.generations[movePerson.GenerationIdx + 1][movePerson.FatherIdx].X2) / 2) - (boxWidth / 2);
                newX2 = newX1 + boxWidth;
            }

            double parentX1 = 0;
            double parentX2 = 0;
            if (movePerson.FatherIdx != -1 && movePerson.MotherIdx != -1)
            {
                parentX2 = this.generations[movePerson.GenerationIdx + 1][movePerson.MotherIdx].X2;
                parentX1 = this.generations[movePerson.GenerationIdx + 1][movePerson.FatherIdx].X1;

                if (movePerson.FatherIdx > movePerson.MotherIdx)
                {
                    parentX2 = this.generations[movePerson.GenerationIdx + 1][movePerson.FatherIdx].X2;
                    parentX1 = this.generations[movePerson.GenerationIdx + 1][movePerson.MotherIdx].X1;
                }



                newX1 = ((parentX2 + parentX1) / 2) - ((movePerson.X2 - movePerson.X1) / 2);
                newX2 = newX1 + (movePerson.X2 - movePerson.X1);


            }
        }

        private void GetNewX(int genidx, double percentageLess, int personIdx, ref double newX1, ref double newX2)
        {

            double adjustedBoxHeight = 0;
            double adjustedDistanceApart = 0;
            double adjustedBoxWidth = 0;

            int childIdx = this.generations[genidx][personIdx].ChildIdx;

            if (genidx > 0)
            {
                adjustedBoxHeight = this.boxHeight - ((this.boxHeight / 100) * percentageLess);
                double childBoxWidth = (this.generations[genidx - 1][childIdx].X2 - this.generations[genidx - 1][childIdx].X1);
                double childCentrePoint = this.generations[genidx - 1][childIdx].X1 + (childBoxWidth / 2);
                adjustedDistanceApart = this.distanceBetweenBoxs - ((this.distanceBetweenBoxs / 100) * percentageLess);
                adjustedBoxWidth = childBoxWidth - ((childBoxWidth / 100) * percentageLess);


                #region calc. first parent last parent single parent

                bool isFirstParent = false;
                bool isLastParent = false;
                bool isSingleParent = false;
                //trying to determine which of the parents we are refering to
                // because if its the first then x value will be lower than it would be for 2nd 

                if (this.generations[genidx].Count > personIdx + 1)
                {
                    if (this.generations[genidx][personIdx + 1].ChildIdx == this.generations[genidx][personIdx].ChildIdx)
                    {
                        isFirstParent = true;
                    }

                }

                if (personIdx > 0)
                {
                    if (this.generations[genidx][personIdx].ChildIdx == this.generations[genidx][personIdx - 1].ChildIdx)
                    {
                        isLastParent = true;
                    }
                }


                if (!isFirstParent && !isLastParent)
                {
                    isSingleParent = true;
                }

                #endregion



                if (isFirstParent)
                {
                    newX1 = childCentrePoint - (adjustedDistanceApart / 2) - adjustedBoxWidth;

                }

                if (isLastParent)
                {
                    newX1 = childCentrePoint + (adjustedDistanceApart / 2);
                }

                if (isSingleParent)
                {
                    newX1 = childCentrePoint - (adjustedBoxWidth / 2);
                }


                // newX1 = initialCentrePoint - newX1;
            }
            else
            {
                adjustedBoxHeight = this.boxHeight;
                adjustedBoxWidth = this.boxWidth;
                newX1 = CentrePoint;
            }

            #region store adjusted distances boxwidths and boxheights

            if (adjustedDistances.Count > genidx)
            {
                adjustedDistances[genidx] = adjustedDistanceApart;
            }
            else
            {
                adjustedDistances.Add(adjustedDistanceApart);
            }

            if (adjustedBoxWidths.Count > genidx)
            {
                adjustedBoxWidths[genidx] = adjustedBoxWidth;
            }
            else
            {
                adjustedBoxWidths.Add(adjustedBoxWidth);
            }


            if (adjustedBoxHeights.Count > genidx)
            {
                adjustedBoxHeights[genidx] = adjustedBoxHeight;
            }
            else
            {
                adjustedBoxHeights.Add(adjustedBoxHeight);
            }

            #endregion


            newX2 = newX1 + adjustedBoxWidth;


        }

        private void getMoveList(int person, int startGen, ref List<TreePerson> moveList)
        {
            int moveGenIdx = startGen;

            try
            {

                while (moveGenIdx > 0)
                {

                    if (!moveList.Contains(this.generations[moveGenIdx][person]))
                    {
                        moveList.Add(this.generations[moveGenIdx][person]);
                    }

                    person = this.generations[moveGenIdx][person].ChildIdx;

                    moveGenIdx--;
                }
            }
            catch (Exception ex2)
            {
                Debug.WriteLine(ex2.Message);
                Debugger.Break();
            }

        }

        #region server code
#if! SILVERLIGHT
        public TreePerson FillFromResult(GetAncestors_Result _result)
        {
            TreePerson treep = new TreePerson();
            
            if (_result != null)
            {

                treep.BirthLocation = _result.BirthLocation;// "Hillam,Monk Frystone";
                treep.DOB = _result.BirthInt.ToString();// "1734";

                if (_result.DeathInt == 0)
                    treep.DOD = "";
                else
                    treep.DOD = _result.DeathInt.ToString();

                treep.DeathLocation = _result.DeathLocation;
                treep.IsHtmlLink = false;

                treep.Name = _result.ChristianName + " " + _result.Surname;
                treep.Occupation = _result.Occupation;

                treep.SpouseLst = new List<Guid>();
                treep.ChildLst = new List<Guid>();


                treep.PersonId = _result.ParentId.GetValueOrDefault();

            }
            else
            {
                treep.PersonId = Guid.Empty;
                treep.Name = "Unknown";
                treep.SpouseLst = new List<Guid>();
                treep.ChildLst = new List<Guid>();
            }

            return treep;
        }

        public override void Refresh()
        {

            RelationsBLL relationsBll = new BLL.RelationsBLL();

            SimpleTimer s1 = new SimpleTimer();
            s1.StartTimer();


            this.FamilySpanLines = new List<List<List<TreePoint>>>();

            this.FamilySpanLines.Add(new List<List<TreePoint>>());

            Stack<Relation> newStack = new Stack<Relation>();

            //relationsBll.
            int currentGen = 0;

            List<GetDescendants_Result> descendantResults = relationsBll.GetDescendants(fatherId);
            List<GetAncestors_Result> ancestorResults = relationsBll.GetAncestors(fatherId);
   

            //var initialParents = relationsBll.GetRelationByParents2(fatherId);

            //if (initialParents.Count() == 0)
            //{
            //    Debug.WriteLine("Father ID returned Zero");
            //}

            generations = new List<List<TreePerson>>();

            // fetch parents
            // put them on the stack and look up the parents parents and so on upwards 
            // then move on to the next parent
            //    Guid me = new Guid("98a7c9a7-9514-4d16-a80a-8faa0739cfa4");

            //    int genPersonsCount = 1;

            generations.Add(new List<Types.TreePerson>(1));

            TreePerson tp = FillFromResult(ancestorResults.FirstOrDefault(f=>f.ParentId == fatherId));
            //TreePerson tp = FillPersonBasicDetails(fatherId);
            tp.Index = 1;
            tp.GenerationIdx = 0;


            generations[0].Add(tp);

            // test stuff
            List<Guid> tpGuids = new List<Guid>();


            while (currentGen < 9)
            {
                this.familySpanLines.Add(new List<List<TreePoint>>());
                int idx = 0;
                // the next generations
                generations.Add(new List<Types.TreePerson>());


                int parentIdx = 0;

                // bool islimitFound = false;


                while (idx < generations[currentGen].Count)
                {

                    this.familySpanLines[currentGen].Add(new List<TreePoint>());



                    var nextgenParents = ancestorResults.Where(w => w.ChildId == generations[currentGen][idx].PersonId).ToList();

                    //var nextgenParents = relationsBll.GetRelationByParents2(generations[currentGen][idx].PersonId).ToList();
 

                    int newPositions = 0;

                    if (currentGen == 0)
                    {
                        newPositions = 1;
                    }
                    else
                    {
                        newPositions = (generations[currentGen][idx].Index * 2) - 1;
                    }


                    TreePerson treep = new Types.TreePerson();



                    if (nextgenParents.Count() > 0)
                    {
                        // treep = FillPersonBasicDetails(nextgenParents[0].ParentId); FillFromResult

                        treep = FillFromResult(nextgenParents[0]);

                        treep.IsHtmlLink = true;
                        treep.GenerationIdx = currentGen + 1;

                        treep.ChildIdx = idx;
                        treep.IsDisplayed = true;
                        treep.FatherIdx = -1;
                        treep.MotherIdx = -1;
                        //treep.s


                        treep.Index = parentIdx;

                        if (nextgenParents.Count() > 1)
                        {
                            treep.SpouseIdxLst = new List<int>();
                            treep.SpouseIdxLst.Add(parentIdx + 1);

                        }


                        generations[currentGen + 1].Add(treep);
                        parentIdx++;


                        //if (nextgenParents[0].RelationType.RelationTypeId == 2)
                        if (nextgenParents[0].RelationType == 2)
                        {
                            generations[currentGen][idx].FatherIdx = generations[currentGen + 1].Count - 1;
                        }
                        else if (nextgenParents[0].RelationType == 4)
                        {
                            generations[currentGen][idx].MotherIdx = generations[currentGen + 1].Count - 1;
                        }



                        //   islimitFound = true;

                    }
                    if (nextgenParents.Count() > 1)
                    {

                        //treep = FillPersonBasicDetails(nextgenParents[1].PersonB.Person_id);//nextgenParents[0]

                        treep = FillFromResult(nextgenParents[1]);//nextgenParents[0]
                        treep.IsHtmlLink = true;
                        treep.GenerationIdx = currentGen + 1;
                        //  treep.index = newPositions+1;
                        treep.ChildIdx = idx;
                        treep.IsDisplayed = true;
                        treep.FatherIdx = -1;
                        treep.MotherIdx = -1;
                        treep.SpouseIdxLst = new List<int>();
                        treep.SpouseIdxLst.Add(parentIdx - 1);
                        treep.Index = parentIdx;

                        generations[currentGen + 1].Add(treep);
                        parentIdx++;

                        //if (nextgenParents[1].RelationType.RelationTypeId == 2)
                        if (nextgenParents[1].RelationType == 2)
                        {
                            generations[currentGen][idx].FatherIdx = generations[currentGen + 1].Count - 1;
                        }
                        // else if (nextgenParents[1].RelationType.RelationTypeId == 4)
                        else if (nextgenParents[1].RelationType == 4)
                        {
                            generations[currentGen][idx].MotherIdx = generations[currentGen + 1].Count - 1;
                        }
                    }



                    //     parentIdx += 2;
                    idx++;
                }

                if (generations[currentGen + 1].Count == 0)
                {
                    generations.RemoveAt(currentGen + 1);
                    break;
                }

                currentGen++;



            }



            Debug.WriteLine(s1.EndTimer("end test"));

        }

        #endif

        #endregion
    }



  
    public class TreeModel : TreeBase//, ITreeModel
    {



      

        // not ideal
        // optimize this later
        //Dictionary<Guid, double> nextParents = new Dictionary<Guid, double>();
        //Dictionary<Guid, double> prevParents = new Dictionary<Guid, double>();

        Dictionary<int, double> nextFamily = new Dictionary<int, double>();
        Dictionary<int, double> prevFamily = new Dictionary<int, double>();

        double distancesbetfam;

        double lowerSpan;
        double middleSpan;
        double topSpan;



        public TreeModel()
        {
            //  Debug.WriteLine("created tree model");
            zoomPercentage = 100;

            original_distanceBetweenBoxs = 30;
            original_distanceBetweenGens = 120;
            original_boxWidth = 70;
            original_boxHeight = 50;
            original_distancesbetfam = 100;
            original_lowerStalkHeight = 5;

            original_middleSpan = 40;
            original_topSpan = 20;

            distanceBetweenBoxs = (int)original_distanceBetweenBoxs;
            distanceBetweenGens = (int)original_distanceBetweenGens;
            boxWidth = (int)original_boxWidth;
            boxHeight = (int)original_boxHeight;
            distancesbetfam = (int)original_distancesbetfam;
            halfBox = boxWidth / 2;
            halfBoxHeight = boxHeight / 2;

            lowerSpan = (int)original_lowerStalkHeight;

            middleSpan = (int)original_middleSpan;

            topSpan = (int)original_topSpan;


        }


        public void SetVisibility(TreePerson parent, bool isDisplay)
        {
            TreePerson currentTP = parent;


            Stack<TreePerson> treePersonStack = new Stack<TreePerson>();

            //get first set of children 
            foreach (TreePerson tp in generations[currentTP.GenerationIdx + 1].Where(o => currentTP.ChildLst.Contains(o.PersonId)))
            {
                treePersonStack.Push(tp);
            }



            while (treePersonStack.Count > 0)
            {
                currentTP = treePersonStack.Pop();

                currentTP.IsDisplayed = isDisplay;

                foreach (int spidx in currentTP.SpouseIdxLst)
                {
                    generations[currentTP.GenerationIdx][spidx].IsDisplayed = isDisplay;
                }

                if (generations.Count > currentTP.GenerationIdx + 1)
                {
                    foreach (TreePerson tp in generations[currentTP.GenerationIdx + 1].Where(o => currentTP.ChildLst.Contains(o.PersonId)))
                    {
                        treePersonStack.Push(tp);
                    }
                }


            }



        }



        #region server methods 
        
        public void Refresh100()
        {
            Debug.WriteLine("tree model : refresh");

            SimpleTimer s1 = new SimpleTimer();
            s1.StartTimer();

            // for the future
            // would be faster just to create a dataset for the whole lot 
            // then filter them with linq
#if !SILVERLIGHT

            RelationsBLL relationsBll = new BLL.RelationsBLL();


            //  DsParentMapChildren.uvw_ParentMapChildrenDataTable _relationsDataTable = new DsParentMapChildren.uvw_ParentMapChildrenDataTable();

            IList<uvw_ParentMapChildren> _childTable = null;
            IList<uvw_ParentMapChildren> _relationsDataTable = null;
            Stack<uvw_ParentMapChildren> newStack = new Stack<uvw_ParentMapChildren>();




            var motherTable = relationsBll.GetRelationsByType2(4).ToList();
            var fatherTable = relationsBll.GetRelationsByType2(2).ToList();
            var spouseTable = relationsBll.GetRelationsByType2(8).ToList();


            var relRow0 = fatherTable.FirstOrDefault(o => o.PersonA.Person_id == fatherId);

            while (relRow0 != null)
            {
                relRow0 = fatherTable.FirstOrDefault(o => o.PersonA.Person_id == fatherId);

                if (relRow0 != null)
                    fatherId = relRow0.PersonB.Person_id;
            }


           




            _relationsDataTable = relationsBll.GetRelationsWithPerson2(fatherId).ToList();

            //    Debug.WriteLine("count: "+_relationsDataTable.Count());

            if (_relationsDataTable.Count() == 0)
            {
                Debug.WriteLine("Father ID not found");
                return;
            }




            List<Guid> parents = new List<Guid>();

            parents.Add(Guid.Empty);


            List<Types.TreePerson> lstTree = new List<Types.TreePerson>();

            Types.TreePerson _fatherPerson = new Types.TreePerson();
            _fatherPerson = FillPersonBasicDetails(fatherId);
            _fatherPerson.IsDisplayed = true;

            lstTree.Add(_fatherPerson);
            generations.Add(lstTree);



            parents.Add(fatherId);
            generations.Add(new List<Types.TreePerson>());


            int currentGen = 0;

            foreach (var relRow in _relationsDataTable)
            {

                _fatherPerson.ChildLst.Add(relRow.PersonA.Value);
                // add child
                newStack.Clear();

                newStack.Push(relRow);


                while (newStack.Count > 0)
                {
                    uvw_ParentMapChildren _currentRel = newStack.Pop();
                    // search list of parents

                    currentGen = parents.IndexOf(_currentRel.ParentId.Value);

                    TreePerson treep = new Types.TreePerson();

                    _childTable = relationsBll.GetRelationsWithPerson2(_currentRel.Person_id).OrderByDescending(o => o.BirthInt).ToList();

                    if (_currentRel.RelationType == 2)
                        treep = FillPersonDetails(spouseTable, motherTable, _childTable, _currentRel, currentGen);
                    else
                        treep = FillPersonDetails(spouseTable, fatherTable, _childTable, _currentRel, currentGen);

                    if (treep.ChildCount > 0)
                    {
                        parents.Insert(currentGen + 1, _currentRel.PersonA.Value);

                        if (currentGen + 1 == generations.Count)
                        {
                            generations.Add(new List<Types.TreePerson>());
                        }

                        foreach (uvw_ParentMapChildren _child in _childTable.Where(c => (!generations[currentGen + 1].Any(tp => tp.PersonId == c.Person_id))))
                        {
                            newStack.Push(_child);
                        }
                    }

                    treep.GenerationIdx = currentGen;
                    treep.IsDisplayed = true;
                    generations[currentGen].Add(treep);
                }



            }

            Debug.WriteLine(s1.EndTimer("end section 1"));

            s1.StartTimer();
            int genIdx = 0;
            int personIdx = 0;

            while (genIdx < generations.Count)
            {
                personIdx = 0;

                this.familySpanLines.Add(new List<List<TreePoint>>());

                int familyCount = GetFamilyList(genIdx);

                generations[genIdx][generations[genIdx].Count - 1].IsFamilyEnd = true;

                // if there is just one family dont dick around just divide by 2
                if (familyCount == 1)
                {
                    if (generations[genIdx].Count > 1)
                    {
                        SetParentLink(genIdx, generations[genIdx].Count / 2);
                    }
                    else
                    {
                        SetParentLink(genIdx, generations[genIdx].Count - 1);
                    }
                }
                else
                {


                    // we need to set a isparentlink flag
                    // which tells the computelocations method which person of the family to connect to the 
                    // parents in the diag.


                    // link should be roughly in the middle of a family
                    // needs to be calculated after all the spouses for that family have been 
                    // added. 

                    int familyIdx = 0;
                    // go through each gen counting the family members
                    // and then once we got on to the next family
                    // look back to the previous one and set the parentlink flag,
                    // using the familyIdx to record how many members that family had.
                    while (personIdx < generations[genIdx].Count)
                    {





                        if (generations[genIdx][personIdx].IsFamilyStart)
                        {

                            // if there are family links next to each other then 
                            // obviously the first one is going to be the parent link
                            //or if the family start is for the last person in the list 
                            // obviously that must be the fam link
                            if (
                                ((generations[genIdx].Count > personIdx + 1) && generations[genIdx][personIdx + 1].IsFamilyStart)
                                || (generations[genIdx].Count - 1 == personIdx))
                            {


                                SetParentLink(genIdx, personIdx);

                            }

                            // the last person in a family!
                            // when not the last person in a generation
                            if (familyIdx != 0)
                            {
                                // start of family idx
                                familyIdx = ((personIdx - (personIdx - familyIdx)) / 2) + (personIdx - familyIdx);

                                SetParentLink(genIdx, familyIdx);
                            }

                            //reset the sibling index
                            familyIdx = 0;

                        }
                        else
                        {
                            // count the siblings 
                            // until we get to the total for the family
                            familyIdx++;

                            // the last person in the generation
                            if (personIdx == generations[genIdx].Count - 1)
                            {
                                // start of family
                                familyIdx = ((personIdx - (personIdx - familyIdx)) / 2) + (personIdx - familyIdx);

                                SetParentLink(genIdx, familyIdx);

                                familyIdx = 0;
                            }
                        }

                        personIdx++;
                    }


                }

                //  this.familiesPerGeneration.Add(familyList.Count);

                genIdx++;
            }


            //System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(generations.GetType());

            //System.IO.StringWriter writeStr = new StringWriter();

            //x.Serialize(writeStr, generations);

            //Debug.WriteLine(writeStr.ToString());

            Debug.WriteLine(s1.EndTimer("end section 2"));
#endif

        }


        public override void Refresh()
        {
            Debug.WriteLine("tree model : refresh");

            SimpleTimer s1 = new SimpleTimer();
            s1.StartTimer();

            // for the future
            // would be faster just to create a dataset for the whole lot 
            // then filter them with linq
#if !SILVERLIGHT

            RelationsBLL relationsBll = new BLL.RelationsBLL();


            SimpleTimer t1 = new SimpleTimer();
            t1.StartTimer();

            List<GetDescendants_Result> descendantResults = relationsBll.GetDescendants(fatherId);


            var topPerson = descendantResults.Where(m => m.level == 1).OrderBy(b => b.BirthInt).FirstOrDefault();

            if (topPerson != null && topPerson.ChildId != null) fatherId = topPerson.ChildId.GetValueOrDefault();
            
            if(topPerson != null && topPerson.ParentId != null) fatherId = topPerson.ParentId.GetValueOrDefault();
            


            List<GetDescendantSpouses_Result> descendantSpousesResults = relationsBll.GetDescendantSpouses(fatherId);

            List<GetDescendants_Result> orderedDescendants = new List<GetDescendants_Result>();
         
            List<Guid> requiredPersons = new List<Guid>();


            requiredPersons.AddRange(descendantResults.Select(d => d.ChildId.GetValueOrDefault()).Distinct());

            requiredPersons.AddRange(descendantResults.Select(d => d.ParentId.GetValueOrDefault()).Distinct());
            
            requiredPersons.AddRange(descendantResults.Select(d => d.motherId.GetValueOrDefault()).Distinct());

            requiredPersons.AddRange(descendantSpousesResults.Select(d => d.PersonA.GetValueOrDefault()).Distinct());

            requiredPersons.AddRange(descendantSpousesResults.Select(d => d.PersonB.GetValueOrDefault()).Distinct());

            requiredPersons.RemoveDuplicates();

            DeathsBirthsBLL deathsBirthsBll = new DeathsBirthsBLL();

            List<Person> persons = deathsBirthsBll.GetDeathBirthRecordByIds(requiredPersons).ToList();


            if (descendantResults.Count > 0)
            {

                int orderedIdx = 1;
                int maxcount = descendantResults.Max(a => a.level).GetValueOrDefault();
                orderedDescendants.AddRange(descendantResults.Where(dr => dr.level == 1).ToList());

                while (orderedIdx <= maxcount)
                {
                    List<Guid> results = orderedDescendants.Where(dr => dr.level == orderedIdx).Select(f => f.ChildId.GetValueOrDefault()).ToList();
                    foreach (Guid result in results)
                    {
                        orderedDescendants.AddRange(descendantResults.Where(a => a.ParentId == result && a.level == (orderedIdx + 1)).OrderBy(b => b.BirthInt).ToList());
                    }
                    orderedIdx++;
                }



                descendantResults = orderedDescendants;


                generations = new List<List<TreePerson>>();
                this.familySpanLines.Clear();

                GetDescendants_Result firstPerson0 = new GetDescendants_Result();
                firstPerson0.ChildId = descendantResults[0].ParentId;
                firstPerson0.level = 0;
                descendantResults.Insert(0, firstPerson0);

                int currentGenTest = -1;
                int columnCounter = 0;

                foreach (GetDescendants_Result currentChild in descendantResults)
                {
                    
                    if (currentGenTest != currentChild.level)
                    {
                        // start new generation

                        currentGenTest = currentChild.level.GetValueOrDefault();

                        columnCounter = 0;

                        generations.Add(new List<TreePerson>());
                        this.familySpanLines.Add(new List<List<TreePoint>>());


                    }

                    TreePerson treePerson = FillBasicDetails(currentChild, currentGenTest, Guid.Empty, persons);

                    //foreach (var group in descendantResults.GroupBy(dr => dr.motherId == currentChild.motherId && dr.ParentId == currentChild.ParentId && currentChild.level == dr.level))
                    //{
                        
                    //    this.familySpanLines[currentGenTest].Add(new List<TreePoint>());
                    //}

                    treePerson.ChildLst.AddRange(descendantResults.Where(a => a.ParentId == treePerson.PersonId).Select(b => b.ChildId.GetValueOrDefault()).ToList());

                    treePerson.ChildCount = treePerson.ChildLst.Count;

                    if (generations[currentGenTest].Count(tp => tp.PersonId == treePerson.PersonId
                        &&
                        ((tp.FatherId == treePerson.FatherId && tp.MotherId == treePerson.MotherId) ||
                        (tp.FatherId == treePerson.MotherId && tp.MotherId == treePerson.FatherId))) == 0)
                    {


                        //if new one is a family start
                        //then last must be fam end.
                        //iterate backwards to find start of 
                        //this previous family
                        //then mark start mid and end

                        Guid firstPerson = Guid.Empty;
                        Guid lastPerson = Guid.Empty;
                        Guid parentLink = Guid.Empty;
                        List<Guid> family = new List<Guid>();
                        if (currentGenTest == 1)
                        {
                            family = descendantResults.Where(dr => dr.motherId == currentChild.motherId && dr.ParentId == currentChild.ParentId).Select(s => s.ChildId.GetValueOrDefault()).ToList();
                        }
                        else
                        {
                            family = descendantResults.Where(dr => dr.motherId == currentChild.motherId && dr.ParentId == currentChild.ParentId).OrderBy(o => o.BirthInt).Select(s => s.ChildId.GetValueOrDefault()).ToList();
                        }


                        //get parent idxs
                        //obviously the first person wont have any parents otherwise they'd be in the tree
                        if (currentGenTest > 0)
                        {
                            treePerson.FatherIdx = generations[currentGenTest - 1].FirstIndexOfPerson(treePerson.FatherId);

                            treePerson.MotherIdx = generations[currentGenTest - 1].FirstIndexOfPerson(treePerson.MotherId);

                            if (treePerson.MotherIdx == -1 && treePerson.FatherIdx != -1)
                            {
                                treePerson.MotherIdx = treePerson.FatherIdx;
                            }
                            else if (treePerson.MotherIdx == -1)
                            {
                                treePerson.MotherIdx = 0;
                            }
                        }

                        if (family.Count > 0)
                        {
                            firstPerson = family.First();

                            parentLink = family[family.Count / 2];

                            lastPerson = family.Last();
                        }



                        if (treePerson.PersonId == firstPerson)
                            treePerson.IsFamilyStart = true;

                        if (treePerson.PersonId == parentLink)
                            treePerson.IsParentalLink = true;

                        generations[currentGenTest].Add(treePerson);

                        columnCounter++;



                        if (generations[currentGenTest].Count(tp => tp.PersonId == treePerson.PersonId) == 1)
                        {
                            List<Guid> spouseAndMotherList = descendantResults.Where(dr => dr.ParentId == currentChild.ChildId.GetValueOrDefault() && dr.motherId.GetValueOrDefault() != Guid.Empty).
                                OrderBy(a => a.BirthInt).Select(s => s.motherId.GetValueOrDefault()).ToList();
                            #region add spouses
                            List<Guid> spouseList = new List<Guid>();

                            spouseList.AddRange(descendantSpousesResults.Where(ds => ds.PersonA == currentChild.ChildId.GetValueOrDefault()).Select(sp => sp.PersonB.GetValueOrDefault()).ToList());

                            spouseList.AddRange(descendantSpousesResults.Where(ds => ds.PersonB == currentChild.ChildId.GetValueOrDefault()).Select(sp => sp.PersonA.GetValueOrDefault()).ToList());

                            foreach (Guid person in spouseList)
                            {
                                if (!spouseAndMotherList.Contains(person))
                                    spouseAndMotherList.Add(person);
                            }

                            spouseAndMotherList.RemoveDuplicates();

                            treePerson.SpouseLst.AddRange(spouseAndMotherList);

                            if (treePerson.PersonId.ToString().ToUpper() == "C1AECAE3-4AEF-46ED-BDEC-6FA1EF15BE4E")
                            {

                            }

                            int firstSpouseIdx = generations[currentGenTest].Count - 1;

                            foreach (var spouseId in spouseAndMotherList)
                            {
                                // add all spouses

                                TreePerson spousePerson = FillBasicDetails(currentChild, currentGenTest, spouseId, persons);


                                spousePerson.ChildLst.AddRange(descendantResults.Where(a => a.motherId == spousePerson.PersonId).Select(b => b.ChildId.GetValueOrDefault()).ToList());

                                spousePerson.SpouseIdxLst = new List<int>();
                                spousePerson.SpouseIdxLst.Add(firstSpouseIdx);
                                spousePerson.IsHtmlLink = true;
                                spousePerson.ChildCount = spousePerson.ChildLst.Count;

                                treePerson.SpouseIdxLst.Add(columnCounter);




                                generations[currentGenTest].Add(spousePerson);

                                columnCounter++;
                            }
                            #endregion
                        }
                        else
                        {
                            Debug.WriteLine("exists: " + treePerson.PersonId.ToString());
                        }

                        if (treePerson.PersonId == lastPerson)
                        {
                            generations[currentGenTest].Last().IsFamilyEnd = true;

                        }


                    }

                }


                //System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(generations.GetType());

                //System.IO.StringWriter writeStr = new StringWriter();

                //x.Serialize(writeStr, generations);

                //Debug.WriteLine(writeStr.ToString());


                Debug.WriteLine(t1.EndTimer("end test"));

            }

            //System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(generations.GetType());

            //System.IO.StringWriter writeStr = new StringWriter();

            //x.Serialize(writeStr, generations);



#endif

        }


        private void ValidateDescendantTree(List<List<TreePerson>> testTree)
        {
            // take 2 trees and spot the differences!

            string pathToValidTree = @"C:\Users\george\Documents\treePersons.xml";

            #if !SILVERLIGHT
            List<List<TreePerson>> validTree = new List<List<Types.TreePerson>>();

            XmlSerializer x = new XmlSerializer(validTree.GetType());

            TextReader tr = new StreamReader(pathToValidTree);

            validTree = (List<List<TreePerson>>)x.Deserialize(tr);



            if (testTree.Count == validTree.Count)
            {
                Debug.WriteLine("correct total generation count");

                int genidx = 0;

                while (genidx < validTree.Count)
                {

                    if (validTree[genidx].Count == testTree[genidx].Count)
                    {
                        Debug.WriteLine("correct generation count for :" + genidx.ToString() );


                        int idx = 0;

                        while (idx < testTree[genidx].Count)
                        {

                            if (testTree[genidx][idx].IsFamilyStart != validTree[genidx][idx].IsFamilyStart)
                            {
                                Debug.WriteLine("invalid fam start:" + genidx.ToString() + ":" + idx.ToString());
                               // Debug.WriteLine(testTree[genidx][idx].SpouseLst.Count.ToString() + "    " + validTree[genidx][idx].SpouseLst.Count.ToString());
                            }

                            //if (testTree[genidx][idx].SpouseLst.Count != validTree[genidx][idx].SpouseLst.Count)
                            //{
                            //    Debug.WriteLine("invalid child SpouseLst.Count:" + genidx.ToString() + ":" + idx.ToString());
                            //    Debug.WriteLine(testTree[genidx][idx].SpouseLst.Count.ToString() + "    " + validTree[genidx][idx].SpouseLst.Count.ToString());
                            //}

                            //if (testTree[genidx][idx].ChildLst.Count != validTree[genidx][idx].ChildLst.Count)
                            //{
                            //    Debug.WriteLine("invalid child ChildLst.Count:" + genidx.ToString() + ":" + idx.ToString());
                            //    Debug.WriteLine(testTree[genidx][idx].ChildLst.Count.ToString() + "    " + validTree[genidx][idx].ChildLst.Count.ToString());
                            //}

                            //if (testTree[genidx][idx].ChildIdxLst.Count != validTree[genidx][idx].ChildIdxLst.Count)
                            //{
                            //    Debug.WriteLine("invalid child ChildIdxLst.Count:" + genidx.ToString() + ":" + idx.ToString());
                            //    Debug.WriteLine(testTree[genidx][idx].ChildIdxLst.Count.ToString() + "    " + validTree[genidx][idx].ChildIdxLst.Count.ToString());
                            //}

                            //if (testTree[genidx][idx].ChildCount != validTree[genidx][idx].ChildCount)
                            //{
                            //    Debug.WriteLine("invalid child count:" + genidx.ToString() + ":" + idx.ToString());
                            //    Debug.WriteLine(testTree[genidx][idx].ChildCount.ToString() + "    " + validTree[genidx][idx].ChildCount.ToString());
                            //}

                            //if (testTree[genidx][idx].PersonId != validTree[genidx][idx].PersonId)
                            //{


                            //    Debug.WriteLine("person out of synch:" + genidx.ToString() + ":" + idx.ToString());
                            //    Debug.WriteLine(testTree[genidx][idx].PersonId.ToString() + "    " + validTree[genidx][idx].PersonId.ToString());
                            //}

                            //Debug.WriteLine(testTree[genidx][idx].PersonId.ToString() + "    " + validTree[genidx][idx].PersonId.ToString());

                            idx++;
                        }
                    }
                    else
                    {
                        Debug.WriteLine("incorrect generation count for :" + genidx.ToString());
                        //Debug.WriteLine("validTree");
                        //foreach (TreePerson tp in validTree[genidx])
                        //{
                        //    Debug.WriteLine(tp.PersonId.ToString());
                        //}

                        //Debug.WriteLine("invalidTree");
                        //foreach (TreePerson tp in testTree[genidx])
                        //{
                        //    Debug.WriteLine(tp.PersonId.ToString());
                        //}


                        //Debug.WriteLine("ordered incorrect generation count for :" + genidx.ToString());
                        //Debug.WriteLine("validTree");
                        //foreach (TreePerson tp in validTree[genidx].OrderBy(t=>t.PersonId))
                        //{
                        //    Debug.WriteLine(tp.PersonId.ToString());
                        //}

                        //Debug.WriteLine("invalidTree");
                        //foreach (TreePerson tp in testTree[genidx].OrderBy(t => t.PersonId))
                        //{
                        //    Debug.WriteLine(tp.PersonId.ToString());
                        //}
                    }

                    genidx++;
                }
            }
            else
            {
                Debug.WriteLine("incorrect total generation count");
            }


           

            #endif

        }


        //refresh100
        private int GetFamilyList(int genIdx)
        {
            List<string> familyList = new List<string>();

            // cycle through each person in the generation
            int personIdx = 0;

            while (personIdx < generations[genIdx].Count)
            {
              
                string familyIdentifier = generations[genIdx][personIdx].FatherId.ToString() + generations[genIdx][personIdx].MotherId.ToString();
             
                if (!familyList.Contains(familyIdentifier))
                {
                    familyList.Add(familyIdentifier);

                    this.familySpanLines[genIdx].Add(new List<TreePoint>());

                   // Debug.WriteLine("fam start: " + generations[genIdx][personIdx].Name + generations[genIdx][personIdx].DOB);
                    generations[genIdx][personIdx].IsFamilyStart = true;

                    if (personIdx > 0)
                    {
                        //Debug.WriteLine("fam end: " + generations[genIdx][personIdx].Name + generations[genIdx][personIdx].DOB);
                        generations[genIdx][personIdx - 1].IsFamilyEnd = true;
                    }
                }
                else
                {
                    generations[genIdx][personIdx].IsFamilyStart = false;

                }
                int spouseIdx = InsertSpouses(genIdx, personIdx);

                setParentIdxs(genIdx, personIdx);

                // collection has got bigger because we've potentially added spouses
                personIdx = personIdx + spouseIdx + 1;
            }

            return familyList.Count;
        }




        #if !SILVERLIGHT
        //unused
        private void AddParentsSpouses(List<Relation> spouseTable, int currentGen, int relationshipType, TreePerson treep)
        {
            if (currentGen > 0)
            {

                // if the parent wasnt in the relations table
                // but this person has a spouse make that spouse the parent
                // not strictly speaking correct but will do

                TreePerson parent = generations[currentGen - 1].Last();
                List<Guid> parentSpouses = generations[currentGen - 1].Last().SpouseLst;

                Guid _spouseId = Guid.Empty;

                if (relationshipType == 2)
                {
                   // _spouseId = treep.mother;

                   
                    
                    //wifes
                    parentSpouses.InsertRange(0, spouseTable.Where(o => o.PersonB.Person_id == parent.PersonId)
                        .Where(c=> (!parentSpouses.Contains(c.PersonA.Person_id))).Select(s => s.PersonA.Person_id).ToList());



                    if (treep.MotherId == Guid.Empty)
                    {
                        if (parentSpouses.Count > 0)
                        {
                            treep.MotherId = parentSpouses[0];
                        }
                        else
                        {
                               treep.MotherId = Guid.NewGuid();

                            parentSpouses.Insert(0, treep.MotherId);
                        }
                    }
                    else
                    {
                        if (!parentSpouses.Contains(treep.MotherId))
                        {

                            parentSpouses.Insert(0, treep.MotherId);
                        }
                    }

                   
                   
                }
                else
                {
                 //   _spouseId = treep.father;

                    parentSpouses.InsertRange(0, spouseTable.Where(o => o.PersonA.Person_id == parent.PersonId)
                        .Where(c => (!parentSpouses.Contains(c.PersonB.Person_id))).Select(s => s.PersonB.Person_id).ToList());


                    if (treep.FatherId == Guid.Empty)
                    {
                        if (parentSpouses.Count > 1)
                        {
                            treep.FatherId = parentSpouses[0];
                        }
                        else
                        {
                            treep.FatherId = Guid.NewGuid();

                            parentSpouses.Insert(0, treep.FatherId);
                        }


                    }
                    else
                    {
                        if (!parentSpouses.Contains(treep.FatherId))
                        {
                            parentSpouses.Insert(0, treep.FatherId);
                        }
                    }                                     
                }
                 
                 


     

            }
        }
        #endif









        //refresh100
        private int InsertSpouses(int idx, int genIdx)
        {

            // cycle through all this persons spouses
            // create a new person 
            // and insert that after them in the list
            generations[idx][genIdx].SpouseIdxLst = new List<int>();

            if (generations[idx][genIdx].Name == "Richard Thackray Dove")
            { 
            
            }


            int spouseIdx = 0;
#if !SILVERLIGHT
            while (spouseIdx < generations[idx][genIdx].SpouseLst.Count)
            {

                TreePerson newSpouse = FillPersonBasicDetails(generations[idx][genIdx].SpouseLst[spouseIdx]);
                newSpouse.SpouseIdxLst = new List<int>();
                newSpouse.SpouseIdxLst.Add(genIdx);

                // FIX ME! - the count of children might be different because multiple spouses partners could be involved
                // just put this in like this as a hack to make displaying the tree easier.
                newSpouse.ChildCount = generations[idx][genIdx].ChildCount;
                //insert spouse detail
                newSpouse.IsHtmlLink = true;
                newSpouse.GenerationIdx = idx;
                newSpouse.IsDisplayed = true;
                newSpouse.Index = genIdx;
                generations[idx].Insert(genIdx + 1, newSpouse);

                // generations[idx][genIdx].spouseIdxs.Add(generations[idx].Count - 1);
                generations[idx][genIdx].SpouseIdxLst.Add(genIdx + (spouseIdx + 1));
                spouseIdx++;
            }
#endif
            return spouseIdx;
        }


        //refresh100
        private void setParentIdxs(int genIdx, int personIdx)
        {


            // we have no parents
            if (genIdx < 1) return;


            //if (generations[genIdx][personIdx].name.Contains("John Manners") && generations[genIdx][personIdx].datebirth == "1821")
            //{

            //}


            int parentGenIdx = genIdx - 1;

            Guid fatherGuid = generations[genIdx][personIdx].FatherId;
            Guid motherGuid = generations[genIdx][personIdx].MotherId;


            List<int> mothers = generations[parentGenIdx].Where(o => o.PersonId == motherGuid).Select(o => generations[parentGenIdx].IndexOf(o)).ToList();

            List<int> fathers = generations[parentGenIdx].Where(o => o.PersonId == fatherGuid).Select(o => generations[parentGenIdx].IndexOf(o)).ToList();

            List<int> unknownPeople = new List<int>();


            if (fathers.Count == 0 || mothers.Count == 0)
                unknownPeople = generations[parentGenIdx].Where(o => o.Name.ToLower() == "unknown").Select(o => generations[parentGenIdx].IndexOf(o)).ToList();


            // in some very rare circumstances the same people can appear multiple times in a generation
            // if they got remarried or if people fucked up when entering data into the tree!..
            // so we have to make sure the persons parents are the correct index


            if (fathers.Count > 0)
            {
                if (mothers.Count == 0)
                {


                    unknownPeople = generations[parentGenIdx][fathers[0]].SpouseIdxLst.Intersect(unknownPeople).ToList();

                    if (unknownPeople.Count > 0)
                        generations[genIdx][personIdx].MotherIdx = unknownPeople[0];
                    else
                        generations[genIdx][personIdx].MotherIdx = fathers[0];

                    generations[genIdx][personIdx].FatherIdx = fathers[0];


                }
                else
                {
                    foreach (int midx in mothers)
                    {
                        fathers = generations[parentGenIdx][midx].SpouseIdxLst.Intersect(fathers).ToList();
                        if (fathers.Count > 0)
                        {
                            generations[genIdx][personIdx].FatherIdx = fathers[0];
                            generations[genIdx][personIdx].MotherIdx = midx;
                        }
                    }
                }

            }
            else
            {

                unknownPeople = generations[parentGenIdx][mothers[0]].SpouseIdxLst.Intersect(unknownPeople).ToList();

                if (unknownPeople.Count > 0)
                    generations[genIdx][personIdx].FatherIdx = unknownPeople[0];
                else
                    generations[genIdx][personIdx].FatherIdx = mothers[0];

                generations[genIdx][personIdx].MotherIdx = mothers[0];

            }




        }




        //refresh 100
        private void SetParentLink(int genNum, int linkIdx)
        {



            if (generations[genNum][linkIdx].IsHtmlLink)
            {
                //here im assuming people dont have more than 2 spouses
                if (generations[genNum][linkIdx - 1].IsHtmlLink)
                {
                    generations[genNum][linkIdx - 2].IsParentalLink = true;
                }
                else
                {
                    generations[genNum][linkIdx - 1].IsParentalLink = true;
                }


            }
            else
            {
                generations[genNum][linkIdx].IsParentalLink = true;
            }

        }


        //unused
        private void CreateFamilySpan()
        {
            //public List<List<TreePerson>> generations = new List<List<Types.TreePerson>>();

            int idx = 0;

            while (idx < generations.Count)
            {
                //generations[idx][


                idx++;
            }
        }







        //public void RemoveAllObservers()
        //{
        //    aList = new List<ITreeView>();
        //}

        //public void AddObserver(ITreeView paramView)
        //{
        //    aList.Add(paramView);
        //}

        //public void RemoveObserver(ITreeView paramView)
        //{
        //    aList.Remove(paramView);
        //}

        //public void NotifyObservers()
        //{

        //    foreach (ITreeView view in aList)
        //    {
        //        view.Update(this);
        //    }
        //}


        #endregion



        public override void SetZoom(int percentage)
        {
            double workingtp = 0;

            workingtp = original_distanceBetweenBoxs / 100;

            distancesbetfam = workingtp * zoomPercentage;


            workingtp = original_lowerStalkHeight / 100;
            lowerSpan = workingtp * zoomPercentage;// (int)original_lowerStalkHeight;

            workingtp = original_middleSpan / 100;
            middleSpan = workingtp * zoomPercentage;//(int)original_middleSpan;

            workingtp = original_topSpan / 100;
            topSpan = workingtp * zoomPercentage;//(int)original_topSpan;


            base.SetZoom(percentage);




        }
 


        public override void ComputeBoxLocations()
        {
            if (generations.Count == 0)
            {
                Debug.WriteLine("ComputeBoxLocations no generations found - exiting method");
                return;
            }

            int displayGenCount = 0;
            int genidx = 0;
            double initialCentrePoint = CentrePoint;
            double startx1 = 0;
            double endx2 = 0;
            childlessMarriages.Clear();

            this.drawingX2 = 0;

            while (genidx < generations.Count)
            {
                foreach (List<TreePoint> ltrees in familySpanLines[genidx])
                {
                    ltrees.Clear();
                }
                genidx++;
            }



            genidx = 0;
            double lastPersonY2 = 0;

            // cycle through generations
            while (genidx < generations.Count)
            {

                if (generations[genidx].Where(o => o.IsDisplayed).Count() == 0)
                {

                    genidx++;
                    continue;
                }
                else
                {
                    displayGenCount++;
                }

                #region establish start point for generation
                if (genidx == 0)
                {
                    drawingX1 = startx1;

                    startx1 = initialCentrePoint - (((generations[genidx].Count * boxWidth) + ((generations[genidx].Count - 1) * distanceBetweenBoxs)) / 2);
                }
                else
                {
                    //generations[genidx][0].

                    TreePerson startPerson = generations[genidx - 1].Where(o => o.ChildLst.Count > 0 && o.IsDisplayed).OrderBy(o => o.X1).FirstOrDefault();
                    TreePerson endPerson = generations[genidx - 1].Where(o => o.ChildLst.Count > 0 && o.IsDisplayed).OrderBy(o => o.X1).LastOrDefault();

                   // Debug.WriteLine("first and last:" + startPerson.Name + "  " + endPerson.Name);

                   // Debug.WriteLine("first and last:" + (startPerson.X1 ) + "," + (endPerson.X2 ));

                    startx1 = startPerson.X1 + (boxWidth / 2);
                    endx2 = endPerson.X2 - (boxWidth / 2);

                    int familyCount = generations[genidx].Where(o => o.IsDisplayed && o.IsFamilyStart).Count();
                    int curGenPerCount = generations[genidx].Where(o => o.IsDisplayed).Count();

                    double prevGenLen = endPerson.X2 - startPerson.X1;//generations[genidx - 1].Where(o => o.isDisplayed).Last().x2 - generations[genidx - 1].Where(o => o.isDisplayed).First().x1;

                 //   Debug.WriteLine("prev gen len: " + prevGenLen);
   

                    // required space to display generation
                    double curGenLen = 0;
                    curGenLen = (curGenPerCount * (boxWidth + distanceBetweenBoxs)) - (distanceBetweenBoxs * familyCount);
                //    Debug.WriteLine("curGenLen: " + curGenLen);

                    // calculate distance between families based on prev generation
                    // if its wider then increase the distance between families
                  //  if (generations[genidx - 1].Where(o => o.isDisplayed).Count() > curGenPerCount)
                    if (prevGenLen > curGenLen)
                    {
                        distancesbetfam = (prevGenLen - curGenLen) / familyCount;
                    }
                    else
                    {
                        distancesbetfam = (original_distancesbetfam / 100) * zoomPercentage;
                    }

                    //add in the distances between the families
                    curGenLen = curGenLen + (distancesbetfam * (familyCount - 1));
                    
              //      Debug.WriteLine("curGenLen with fam dist: " + curGenLen);

                    // middle of the families of the previous generation
                    double desiredMidPoint = ((endx2 - startx1) / 2) +startx1;

                  //  Debug.WriteLine("desiredMidPoint: " + desiredMidPoint);
                    
                    
                    //set new start point by subtracting half the total space required for the generation

                    // set new start point by subtracting half the total space required for the generation
                    startx1 = desiredMidPoint - (curGenLen / 2);
               //     Debug.WriteLine("finish startx1: " + startx1);
                }

                #endregion

                double current_gen_upper_y = (genidx * boxHeight) + (genidx * distanceBetweenGens) + this.centreVerticalPoint;

                double increment_temp = 0;

                int famidx = 0;

                fillGenXs(genidx, startx1);

                List<double> familydirectionCounts = new List<double>(createFamilyCountArray(genidx));
              //  familyCounts = ;



                int familyIdx = -1;
                int personIdx = 0;
                foreach (TreePerson genPerson in generations[genidx].Where(o => o.IsDisplayed))
                {
                //    Debug.WriteLine("displaying: "+ genPerson.Name + "-" + genidx.ToString());
                    try
                    {

                        genPerson.X2 = genPerson.X1 + this.boxWidth;

                     //   genPerson.distFromCent = genPerson.Y1;


                        bool isDoubleSpouseEnd = false;
                        bool isSpouse = genPerson.IsHtmlLink;

                        isDoubleSpouseEnd = false;

                        if (isSpouse)
                        {
                            if ((generations[genidx].Count > personIdx + 1) && generations[genidx][personIdx + 1].IsHtmlLink)
                            {
                           //     Debug.WriteLine(genidx + " , " + personIdx);
                                isDoubleSpouseEnd = true;
                            }
                        }

                        double parent_gen_lower_y = 0;
                        if (genPerson.IsFamilyStart)
                        {
                            familyIdx++;
                            familySpanLines[genidx][familyIdx].Clear();
                        }

                        if (genPerson.SpouseIdxLst != null && genPerson.SpouseIdxLst.Count > 0 && genPerson.ChildCount == 0)
                        {
                            int spouseIdx = genPerson.SpouseIdxLst[0];
                            double tp = generations[genidx][spouseIdx].X1;

                            // because sometimes when people marry their cousins
                            // the 2 entries for that family will exist. 
                            // potentially on different sides of the tree

                            if (Math.Abs(spouseIdx - personIdx) <= 2)
                            {
                                if (generations[genidx][spouseIdx].ChildCount == 0)
                                {
                                    childlessMarriages.Add(new TreePoint(genPerson.X1 + halfBox, current_gen_upper_y + this.boxHeight));

                                    childlessMarriages.Add(new TreePoint(genPerson.X1 + halfBox, current_gen_upper_y + this.boxHeight + this.topSpan));

                                    childlessMarriages.Add(new TreePoint(tp + halfBox, current_gen_upper_y + this.boxHeight + this.topSpan));
                                    childlessMarriages.Add(new TreePoint(tp + halfBox, current_gen_upper_y + this.boxHeight));

                                }

                            }
                            // 

                        }



                        double middleParents = 0;
                        double firstPX = 0;
                        double secondPX = 0;

                        double thirdStorkX = 0;
                        //   int middleRow = 0;

                        //this.distanceBetweenGens

                        if (genidx > 0)
                            parent_gen_lower_y = generations[genidx - 1][genPerson.FatherIdx].Y2;

                        double firstRow = current_gen_upper_y - this.lowerSpan;
                        double secondRow = parent_gen_lower_y + this.middleSpan; // changed with increment later on - need to calculate the maximum and minimum this increment will be
                        double thirdRow = parent_gen_lower_y + this.middleSpan;
                        double fourthRow = parent_gen_lower_y + this.topSpan;

                        //distance between firstrow and thirdrow needs to be based on the number of children somehow


                     //   double cur_P_X1 = genPerson.x1;
                        // dont do when is end of family and is a spouse 

                        if ((!(genPerson.IsFamilyEnd && isSpouse)) && genidx > 0)
                        {
                            if (!isDoubleSpouseEnd)
                            {
                             //   Debug.WriteLine(genPerson.Name);

                                familySpanLines[genidx][familyIdx].Add(new TreePoint(genPerson.X1 + halfBox, firstRow));

                                if (!isSpouse)
                                    familySpanLines[genidx][familyIdx].Add(new TreePoint(genPerson.X1 + halfBox, current_gen_upper_y));

                                familySpanLines[genidx][familyIdx].Add(new TreePoint(genPerson.X1 + halfBox, firstRow));
                            }
                        }

                        #region parental links

                        if (genPerson.IsParentalLink && genidx > 0)
                        {

                            middleParents = MiddleParents(genidx, genPerson.FatherIdx, genPerson.MotherIdx, middleParents);
                         
                            double nextParentLink = GetFirst(genidx, genPerson);
                            double prevParentLink = GetPrev(genidx, genPerson);

                            

                            GetParentXs(genidx, genPerson, ref firstPX, ref secondPX);

                            double incSize = 0;

                            incSize = distanceBetweenGens - this.middleSpan - this.lowerSpan;
                            incSize = incSize / familydirectionCounts[famidx];
                            //is start minus or plus
                            if (famidx == 0)
                            {
                                if (genPerson.X1 > middleParents)
                                    increment_temp = distanceBetweenGens - this.middleSpan - this.lowerSpan;
                                else
                                    increment_temp = 0;
                            }

                            
                            if (genPerson.X1 > middleParents)
                            {
                                increment_temp -= incSize;//original

                                if (nextParentLink > genPerson.X2)
                                    thirdStorkX = genPerson.X2;
                                else
                                    thirdStorkX = nextParentLink;

                                if ((genPerson.X1 > middleParents) && (thirdStorkX > genPerson.X1))
                                {
                                    thirdStorkX = genPerson.X1;
                                }
                            }
                            else
                            {
                                increment_temp += incSize;//original
                             
                                if (prevParentLink < genPerson.X1)
                                    thirdStorkX = genPerson.X1;
                                else
                                    thirdStorkX = prevParentLink;
                            
                                if ((genPerson.X1 < middleParents) && (thirdStorkX < genPerson.X1))
                                {
                                    thirdStorkX = genPerson.X1;
                                }
                            }


                            secondRow += increment_temp;

                            //if (genPerson.name == "Charles Swales" && genPerson.datebirth == "1895")
                            //{ 
                            
                            //}

                            #region tweak start of rows

                          //  firstRow.AlmostEquals(secondRow,1)
                            if (firstRow.AlmostEquals(secondRow,1))
                            {
                                //   if (middleParents > nextFamilyStart && middleParents < prevFamilyStart)
                                //   {
                                secondRow -= (incSize / 2);
                                //   }

                            }



                            double secondStorkX = genPerson.X1;


                            
                            if (genPerson.IsFamilyStart && genPerson.IsFamilyEnd)
                            {
                                // only child with no spouses!
                                if (personIdx == 0)
                                {                                    
                                    double nextFamilyStart = 0;
                                    double prevFamilyStart = this.generations[genidx][personIdx].X1;

                                    if (this.generations[genidx].Count > 1)
                                    {
                                        nextFamilyStart = this.generations[genidx][personIdx + 1].X1;
                                    }
                                    else
                                    {
                                        nextFamilyStart = this.generations[genidx][personIdx].X2;
                                    }

                                    if (middleParents < nextFamilyStart && middleParents > prevFamilyStart)
                                    {
                                        secondStorkX = middleParents;
                                        thirdStorkX = middleParents;
                                    }

                                }
                            }
                            else
                            {
                                // handles situations where lines are overlapping the next or prev
                                // family
                                // happens when there are just 1 or 2 families 
                                // and one of them is unusually large or something like that.

                                if (genPerson.IsFamilyStart)
                                {
                                    // tidy up the link to the parents

                                    double sizeToAdd = this.halfBox;

                                    if (!genPerson.IsFamilyEnd)
                                    {
                                        sizeToAdd = this.boxWidth;
                                    }

                                    if (secondStorkX == thirdStorkX)
                                    {
                                        thirdStorkX += sizeToAdd;
                                    }


                                    secondStorkX += sizeToAdd;


                                }

                            }


                            #endregion


                            familySpanLines[genidx][familyIdx].Add(new TreePoint(secondStorkX, firstRow));// starting point  .1

                            familySpanLines[genidx][familyIdx].Add(new TreePoint(secondStorkX, secondRow)); // move to the middle of the family   .2

                            familySpanLines[genidx][familyIdx].Add(new TreePoint(thirdStorkX, secondRow));//bottom of stork .3
                            // familySpanLines[genidx][familyIdx].Add(new TreePoint(middleParents, middleFamSpanY));// 

                            familySpanLines[genidx][familyIdx].Add(new TreePoint(thirdStorkX, thirdRow));//bottom of stork .3

                            familySpanLines[genidx][familyIdx].Add(new TreePoint(middleParents, thirdRow));//bottom of stork .3

                            //  familySpanLines[genidx][familyIdx].Add(new TreePoint(middleParents,  middleFamSpanY - increment_temp));//bottom of stork .3

                            familySpanLines[genidx][familyIdx].Add(new TreePoint(middleParents, fourthRow));// top of stork 4.
                            familySpanLines[genidx][familyIdx].Add(new TreePoint(firstPX, fourthRow));//  take a step to the left .5
                            familySpanLines[genidx][familyIdx].Add(new TreePoint(firstPX, parent_gen_lower_y));//  take a step to the top .6
                            familySpanLines[genidx][familyIdx].Add(new TreePoint(firstPX, fourthRow));//  take a step down .7

                            familySpanLines[genidx][familyIdx].Add(new TreePoint(secondPX, fourthRow));//  take a step to the right .8
                            familySpanLines[genidx][familyIdx].Add(new TreePoint(secondPX, parent_gen_lower_y));//  take a step to the top .9
                            familySpanLines[genidx][familyIdx].Add(new TreePoint(secondPX, fourthRow));//  take a step down .10
                            familySpanLines[genidx][familyIdx].Add(new TreePoint(middleParents, fourthRow));// take a step to the middle 11.

                            familySpanLines[genidx][familyIdx].Add(new TreePoint(middleParents, thirdRow));//take a step down .12


                            //   familySpanLines[genidx][familyIdx].Add(new TreePoint(middleParents, middleFamSpanY - increment_temp));//take a step down .12
                            //   familySpanLines[genidx][familyIdx].Add(new TreePoint(middleParents, middleFamSpanY - increment_temp));//bottom of stork .3

                            familySpanLines[genidx][familyIdx].Add(new TreePoint(thirdStorkX, thirdRow));//bottom of stork .3

                            familySpanLines[genidx][familyIdx].Add(new TreePoint(thirdStorkX, secondRow));//bottom of stork .3




                            familySpanLines[genidx][familyIdx].Add(new TreePoint(secondStorkX, secondRow));//13
                            familySpanLines[genidx][familyIdx].Add(new TreePoint(secondStorkX, firstRow));//14



                            famidx++;
                        }


                        #endregion

                        genPerson.Y1 = current_gen_upper_y;
                        genPerson.Y2 = current_gen_upper_y + boxHeight;

                        lastPersonY2 = genPerson.Y2;

                        CalcTPZoom(genPerson);
                    }
                    catch (Exception ex1)
                    {
                        Debug.WriteLine(ex1.Message);
                    }




                    personIdx++;
                }






                #region calculate drawing width
                if (generations[genidx].Where(o => o.IsDisplayed).Count() > 0)
                {



                    TreePerson currentLastPerson = generations[genidx].Where(o => o.IsDisplayed).LastOrDefault();

                    TreePerson currentFirstPerson = generations[genidx].Where(o => o.IsDisplayed).FirstOrDefault();

                    //  int tpLstIdx = 0;
                    if (genidx < 2)
                    {
                        if (generations[genidx].Count > 0)
                        {
                            //      tpLstIdx = generations[genidx].Where(o => o.isDisplayed).Count()-1;


                            drawingX1 = currentFirstPerson.X1;// generations[genidx][0].x1;

                            drawingX2 = currentLastPerson.X2; //generations[genidx][tpLstIdx].x2;
                        }
                    }
                    else
                    {
                        TreePerson prevFirstPerson = generations[genidx - 1].Where(o => o.IsDisplayed).FirstOrDefault();
                        TreePerson prevLastPerson = generations[genidx - 1].Where(o => o.IsDisplayed).LastOrDefault();

                        if (currentFirstPerson.X1 < prevFirstPerson.X1)
                        {
                            drawingX1 = currentFirstPerson.X1;
                        }
                        else
                        {
                            drawingX1 = prevFirstPerson.X1;
                        }

                        //   tpLstIdx = generations[genidx - 1].Where(o => o.isDisplayed).Count() - 1;
                        //    double lastX2 = 0;

                        if (currentLastPerson.X2 > prevLastPerson.X2)
                        {
                            drawingX2 = currentLastPerson.X2;
                        }
                        else
                        {
                            drawingX2 = prevLastPerson.X2;
                        }
                    }


                }


                #endregion
                //      if ((cur_P_X1 + this.boxWidth) > this.drawingX2)
                //          this.drawingX2 = (cur_P_X1 + this.boxWidth);


                genidx++;
            }



            if (generations.Count > 0)
            {
                drawingY1 = generations[0][0].Y1;
            }
            if (generations[displayGenCount - 1].Count > 0)
            {
                TreePerson heightPerson = generations[displayGenCount - 1].Where(o => o.IsDisplayed && o.IsParentalLink).FirstOrDefault();

                //  if(heightPerson != null)
                drawingY2 = lastPersonY2;
            }
            //drawingCentreVertical = drawingY2 - drawingY1;
            drawingCentre = (drawingX2 - drawingX1) / 2;
            drawingHeight = drawingY2 - drawingY1;
            drawingWidth = drawingX2 - drawingX1;

            //foreach (TreePerson _tp in this.generations[1])
            //{ 
            //    Debug.WriteLine(_tp.X1 + ", " +_tp.X2);
            //}



            //      Debug.WriteLine("3-0:" + this.generations[3][0].x1 + " dif " + Math.Abs(oldVal - this.generations[3][0].x1) + " current zoom " + zoomLevel + " drawing boundary: " + drawingX1 + "," + drawingX2 + " cent: " + drawingCentre);
        }

        //computeboxs
        private void fillGenXs(int genidx, double startx1)
        {
            int idx = 0;
            double currentDistanceBetweenBoxes = 0;
       
            TreePerson prevPerson = null;

            foreach (TreePerson newPerson in generations[genidx].Where(o => o.IsDisplayed))
            {
                if (idx == 0)
                {
                    newPerson.X1 = startx1;
                    newPerson.X2 = startx1 + boxWidth;
          
                }
                else
                { 
                    if (newPerson.IsFamilyStart)
                    {
                        currentDistanceBetweenBoxes = distancesbetfam;
                    }
                    else
                    {
                        currentDistanceBetweenBoxes = distanceBetweenBoxs;
                    }

                    newPerson.X1 = prevPerson.X1 + this.boxWidth + currentDistanceBetweenBoxes;

                    newPerson.X2 = newPerson.X1 + this.boxWidth;

                }
                prevPerson = newPerson;

                idx++;
            }
        }


        /// <summary>
        /// We need to know how many families go together in the same direction
        /// so we can decide how much space to give each horizontal line. 
        /// Each entry in the returned array corresponds to a family in the generation
        /// and indicates how many other family are grouped together going in that particular direction.
        /// </summary>
        //compute boxs
        private List<double> createFamilyCountArray(int genIdx)
        {

            List<double> newswitchs = new List<double>();

            if (genIdx == 0) return newswitchs;

            double leftCounter = 0;
            double rightCounter = 0;

            //if (genIdx == 7)
            //{
            //    Debug.WriteLine("generation: " + genIdx + " families: " + generations[genIdx].Where(o => o.isParentalLink && o.isDisplayed).Count());

            //}


            foreach (TreePerson _tp in generations[genIdx].Where(o => o.IsParentalLink && o.IsDisplayed))
            {
                newswitchs.Add(0);

                if (_tp.X1 > MiddleParents(genIdx, _tp.FatherIdx, _tp.MotherIdx))
                {
                    rightCounter++;

                    if (leftCounter > 0)
                        newswitchs[newswitchs.Count - 2] = leftCounter;



                    leftCounter = 0;
                }
                else
                {
                    leftCounter++;

                    if (rightCounter > 0)
                        newswitchs[newswitchs.Count - 2] = rightCounter;

                    rightCounter = 0;
                }


                

            }



            if (leftCounter != 0) newswitchs[newswitchs.Count - 1] = leftCounter;

            if (rightCounter != 0) newswitchs[newswitchs.Count - 1] = rightCounter;




            int idx = newswitchs.Count - 1;
            while (idx > 0)
            {
                if (newswitchs[idx - 1] == 0)
                    newswitchs[idx - 1] = newswitchs[idx];
                idx--;
            }


            //if (genIdx == 7)
            //{
                
            //    //EnvDTE80.DTE2 ide = (EnvDTE80.DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.8.0");
            //    //ide.ExecuteCommand("Edit.ClearOutputWindow", "");
            //    //System.Runtime.InteropServices.Marshal.ReleaseComObject(ide);

            //    Debug.WriteLine("generation: " + genIdx + " families: " + generations[genIdx].Where(o => o.isParentalLink && o.isDisplayed).Count());

            //    foreach (double _switch in newswitchs)
            //    {
            //        Debug.WriteLine(_switch);
            //    }
            //}

            return newswitchs;
        }

        //unused
        private double GetMiddleGeneration(int genidx)
        {
            int middleRow = 0;
            double ret = 0;
            middleRow = generations[genidx - 1].Count / 2;

            ret = generations[genidx - 1][middleRow].X1 + halfBox;


            return ret;
        }

        //compute box locations
        private double MiddleParents(int genidx, int fatIdx, int motIdx, double middleParents = 0)
        {


            // if mother is fathers second wife 
            // if father is mothers second husband
            if (Math.Abs(fatIdx - motIdx) > 1)
            {
                if (fatIdx < motIdx)
                {
                    middleParents = (generations[genidx - 1][motIdx - 1].X1 + generations[genidx - 1][motIdx].X2) / 2;
                }
                else
                {
                    middleParents = (generations[genidx - 1][fatIdx - 1].X1 + generations[genidx - 1][fatIdx].X2) / 2;
                }
            }
            else
            {
                middleParents = (generations[genidx - 1][fatIdx].X1 + generations[genidx - 1][motIdx].X2) / 2;
            }
            return middleParents;
        }
        
        //compute box locations
        private void GetParentXs(int genidx, TreePerson _treePerson, ref double firstPX, ref double secondPX)
        {
            int fatIdx = _treePerson.FatherIdx;
            int motIdx = _treePerson.MotherIdx;

            if (genidx < 1)
            {
                secondPX = this.centrePoint;
                firstPX = this.centrePoint;
            }
            else
            {
                if (generations[genidx - 1][fatIdx].X1 > generations[genidx - 1][motIdx].X1)
                {
                    secondPX = generations[genidx - 1][fatIdx].X1 + halfBox;
                    firstPX = generations[genidx - 1][motIdx].X1 + halfBox;

                }
                else
                {
                    secondPX = generations[genidx - 1][motIdx].X1 + halfBox;
                    firstPX = generations[genidx - 1][fatIdx].X1 + halfBox;
                }
            }
        }
       
        //compute box locations
        private double GetFirst(int genidx, TreePerson _treePerson)
        {
            int fatIdx = _treePerson.FatherIdx;
            int motIdx = _treePerson.MotherIdx;
         //   int personIdx = this.generations[genidx].IndexOf(_treePerson);


          
            double middleParents = 0;

            middleParents = MiddleParents(genidx, fatIdx, motIdx, middleParents); // (generations[genidx - 1][fatIdx].x1 + generations[genidx - 1][motIdx].x2) / 2;

            double nextParentLink = middleParents;
            //  int prevParentLink = middleParents;

            int idxParentLink = motIdx;


            // if we only have 1 parent, but that parent 
            // later remarries we want the next nextparent setting to the current parents edge
            if (fatIdx == motIdx)
            {
                //remember fatidx and motidx are the same!
                if (generations[genidx - 1][fatIdx].SpouseLst.Count > 0)
                {
                    return generations[genidx - 1][fatIdx].X2;
                }
            }


            // if multiple spouses set next parent as end of first one
            if (generations[genidx - 1][fatIdx].SpouseLst.Count > 1)
            {
                if (Math.Abs(fatIdx - motIdx) == 1)
                {
                    return nextParentLink;
                }
            }


            if (fatIdx > motIdx) idxParentLink = fatIdx;

            double rightX2OfCurrentParent = generations[genidx - 1][idxParentLink].X2;

            _treePerson = generations[genidx - 1].Where(o => o.X1 > rightX2OfCurrentParent && o.IsDisplayed && o.ChildCount > 0).FirstOrDefault();

           

            if (_treePerson != null)
            {
                Debug.WriteLine("first parent: " + _treePerson.Name);
                nextParentLink = _treePerson.X1;
            }

            return nextParentLink;
        }
        
        //compute box locations
        private double GetPrev(int genidx, TreePerson _treePerson)
        {
            int fatIdx = _treePerson.FatherIdx;

            int motIdx = _treePerson.MotherIdx;


          //  int personIdx = this.generations[genidx].IndexOf(_treePerson);

            double middleParents = (generations[genidx - 1][fatIdx].X1 + generations[genidx - 1][motIdx].X2) / 2;

            double prevParentLink = middleParents;

            //left parent
            int idxParentLink = fatIdx;
            if (fatIdx > motIdx) idxParentLink = motIdx;

            double currentParentsLeft = generations[genidx - 1][idxParentLink].X1;

            if (generations[genidx - 1][fatIdx].SpouseLst.Count > 1)
            {
                if (Math.Abs(fatIdx - motIdx) == 2)
                {
                    return prevParentLink;
                }
            }

            _treePerson = generations[genidx - 1].Where(o => o.ChildCount > 0 && o.IsDisplayed && o.X1 < currentParentsLeft).LastOrDefault();

            

            if (_treePerson != null)
            {
                Debug.WriteLine("last parent: " + _treePerson.Name);
                prevParentLink = _treePerson.X2;
            }


            return prevParentLink;
        }




    }





}


#region unused

//public void MoveTo(int x_param, int y_param, Guid personId)
        //{

        //    if (generations.Count == 0)
        //    {
        //        Debug.WriteLine("moveto generations equal zero  method exited");
        //        return;
        //    }

        //    this.ComputeBoxLocations();

        //    int genIdx = 0;
        //    int personIdx = 0;

        //    int selectedGen = 0;
        //    int selectedPers = 0;

        //    while (genIdx < this.generations.Count)
        //    {
        //        personIdx = 0;
        //        while (personIdx < this.generations[genIdx].Count)
        //        {
        //            if (generations[genIdx][personIdx].PersonId == personId)
        //            {

        //                selectedGen = genIdx;
        //                selectedPers = personIdx;
        //                //break out the loop
        //                genIdx = this.generations.Count - 1;
        //            }
        //            personIdx++;
        //        }


        //        genIdx++;
        //    }


        //    //this.centrePoint
        //    //this.centreVerticalPoint


        //    double tempX = (double)x_param;
        //    double tempY = (double)y_param;




        //    while (Math.Abs(tempX - generations[selectedGen][selectedPers].X1) <= 1 ||
        //       Math.Abs(tempY - generations[selectedGen][selectedPers].Y1) <= 1)
        //    {

        //        if (Math.Abs(tempX - generations[selectedGen][selectedPers].X1) <= 1)
        //        {
        //            if (Math.Round(generations[selectedGen][selectedPers].X1, 0) < tempX)
        //                this.centrePoint++;
        //            else if (Math.Round(generations[selectedGen][selectedPers].X1, 0) > tempX)
        //                this.centrePoint--;


        //        }

        //        if (Math.Abs(tempY - generations[selectedGen][selectedPers].Y1) <= 1)
        //        {


        //            if (Math.Round(generations[selectedGen][selectedPers].Y1, 0) < tempY)
        //                this.centreVerticalPoint++;
        //            else if (Math.Round(generations[selectedGen][selectedPers].Y1, 0) > tempY)
        //                this.centreVerticalPoint--;

        //        }

        //        this.ComputeBoxLocations();

        //    }

        //}

#endregion