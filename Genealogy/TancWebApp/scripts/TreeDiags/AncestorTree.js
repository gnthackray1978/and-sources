var TreeBase;



function AncTree() {

        $.extend(this, new TreeBase());

        this.adjustedDistances = new Array();
        this.adjustedBoxWidths = new Array();
        this.adjustedBoxHeights = new Array();

        this.moveList =  new Array();

        this.newX1 =0.0;
        this.newX2 =0.0;

        this.workingX1 =0.0;
        this.workingX2 =0.0;


        this.DrawTree = function () {

           var canvas = document.getElementById("myCanvas");
           var context = canvas.getContext("2d");
           canvas.width = window.innerWidth;
           canvas.height = window.innerHeight;


           this.ComputeLocations();

           var topLeftCornerX = 188;
           var topLeftCornerY = 50;
           var width = 200;
           var height = 100;



           var _genidx = 0;
           var _personIdx = 0;
           //this.generations.length

           var treeUI = new TreeUI(this.screenWidth, this.screenHeight, this.boxWidth, this.boxHeight);

           this.links = new Array();

           while (_genidx < this.generations.length) {
               _personIdx = 0;

               while (_personIdx < this.generations[_genidx].length) {

                   var _person = this.generations[_genidx][_personIdx];

                   var personLink = treeUI.DrawPerson(_person,this.sourceId, this.zoomPercentage);

                   if(personLink != null)
                    this.links.push(personLink);

                   _personIdx++;
               }
               _genidx++;
           }

           var _fslOuter = 0;
           var _fslInner = 0;
           //   var _pointIdx = 0;


           while (_fslOuter < this.familySpanLines.length) {
               _fslInner = 0;
               while (_fslInner < this.familySpanLines[_fslOuter].length) {
                   treeUI.DrawLine(this.familySpanLines[_fslOuter][_fslInner]);
                   _fslInner++;
               } // end familySpanLines[_fslOuter].length

               _fslOuter++;
           } // end this.familySpanLines.length


       }

        this.ComputeLocations = function () {




        var genidx = 0;
        this.drawingX2 = 0.0;
        this.drawingX1 = 0.0;
        var _y = this.centreVerticalPoint;
        var percentageLess = 0.0;

        //            adjustedDistances = new List<double>(generations.Count);
        //            adjustedBoxWidths = new List<double>(generations.Count);
        //            adjustedBoxHeights = new List<double>(generations.Count);
        this.adjustedDistances = new Array();
        this.adjustedBoxWidths = new Array();
        this.adjustedBoxHeights = new Array();


        this.generations[0][0].X1 = this.centrePoint;
        this.generations[0][0].X2 = this.centrePoint + this.boxWidth;
        this.generations[0][0].Y1 = _y;
        this.generations[0][0].Y2 = _y + this.boxHeight;

        //            var newX1 = 0.0;
        //            var newX2 = 0.0;


        genidx = 0;
        while (genidx < this.generations.length) {


            var personIdx = 0;

            percentageLess += 2;

           // console.log('GENERATION: ' + genidx);

            while (personIdx < this.generations[genidx].length) {

                this.GetNewX(genidx,
                        percentageLess,
                        personIdx); // fills newxs

                var overlap = 0.0;
                var requiredSpace = 0.0;
                if (personIdx > 0) {

                    if (this.generations[genidx][personIdx - 1].X2 > this.newX1) {
                        overlap = this.generations[genidx][personIdx - 1].X2 - this.newX1;
                        overlap += this.adjustedDistances[genidx];
                    }

                    var newChildidx = this.generations[genidx][personIdx].ChildIdx;
                    var oldChildidx = this.generations[genidx][personIdx - 1].ChildIdx;
                    var countPersonSpaces = newChildidx - oldChildidx;

                    if (countPersonSpaces > 1) {

                        countPersonSpaces--;
                        //needed space
                        requiredSpace = (countPersonSpaces * this.adjustedBoxWidths[genidx - 1]) + ((countPersonSpaces + 1) * (this.adjustedDistances[genidx - 1] + 5));

                        var spaceSoFarCreated = (this.generations[genidx - 1][newChildidx].X1 - this.generations[genidx - 1][oldChildidx].X2) + overlap;

                        // we dont have enough space!
                        if (requiredSpace > spaceSoFarCreated) {
                            // increase the overlap so enough space if provided
                            overlap += (requiredSpace - spaceSoFarCreated);
                        }
                        else if (overlap == 0) {
                            overlap = (requiredSpace - spaceSoFarCreated);
                        }

                    }

                }




                if (overlap > 0) {

                   // console.log('overlaped: ' + personIdx);

                    this.getMoveList(personIdx - 1, genidx);

                    this.moveList.SortByGenIdx();

                    //var sorted = moveList.OrderByDescending(o => o.GenerationIdx);

                    var listIdx = 0;
                    // }

                    //                    $.each(this.moveList, function (index, _treePerson) {


                    while (listIdx < this.moveList.length) {
                        var _treePerson = this.moveList[listIdx];
                        var tpPersonIdx = _treePerson.Index;

                        while (tpPersonIdx >= 0) {

                            //      Debug.WriteLine("moving: " + this.generations[_treePerson.generation][tpPersonIdx].name);
                            //console.log("moving: " + _treePerson.Name + " " + _treePerson.X1 + " " + _treePerson.X2);

                            var _movePerson = this.generations[_treePerson.GenerationIdx][tpPersonIdx];

                            var _prevPerson = null;
                            var _nextPerson = null;


                            if (tpPersonIdx > 0)
                                _prevPerson = this.generations[_treePerson.GenerationIdx][tpPersonIdx - 1];

                            if ((tpPersonIdx + 1) < this.generations[_treePerson.GenerationIdx].length)
                                _nextPerson = this.generations[_treePerson.GenerationIdx][tpPersonIdx + 1];

                            this.workingX1 = 0.0;
                            this.workingX2 = 0.0;

                            if ((_movePerson.FatherIdx == -1 && _movePerson.MotherIdx == -1) || (_movePerson.GenerationIdx == genidx)) {
                                if (_movePerson.GenerationIdx == genidx) {
                                    this.workingX1 = _movePerson.X1 - overlap;
                                    this.workingX2 = _movePerson.X2 - overlap;
                                }
                                else {

                                    var parentlessPersonStartX = _movePerson.X1 - overlap; // GetX1ForParentlessPerson(_movePerson.generation, _movePerson.index);

                                    if (parentlessPersonStartX == 0.0) {
                                        parentlessPersonStartX = 15;
                                        this.workingX2 = _nextPerson.X1 - parentlessPersonStartX;
                                        this.workingX1 = this.workingX2 - this.adjustedBoxWidths[_nextPerson.GenerationIdx];
                                    }
                                    else {
                                        this.workingX1 = parentlessPersonStartX;
                                        this.workingX2 = this.workingX1 + this.adjustedBoxWidths[_nextPerson.GenerationIdx];
                                    }
                                }

                            }
                            else {
                                this.CreateChildPositionFromParent(_movePerson); //sets workingXs 
                            }

                         //   if (this.workingX1 == -3830.17696197631 && this.workingX2 == -3773.62983468031 && _treePerson.Name == 'James Reeves') {
                          //      console.log('hello');
                          //  }

                           // console.log('working 1 and 2: ' + this.workingX1 + ' - ' + this.workingX2);

                            _movePerson.X1 = this.workingX1; // -adjustedDistanceApart;
                            _movePerson.X2 = this.workingX2; // -adjustedDistanceApart;

                            tpPersonIdx--;
                        } //end while (tpPersonIdx >= 0)

                        listIdx++;
                    } // end listIdx < this.moveList.length




                }

                this.generations[genidx][personIdx].X1 = this.newX1; // _x - adjustedBoxWidth;
                this.generations[genidx][personIdx].X2 = this.newX2; // _x + adjustedBoxWidth;

                this.generations[genidx][personIdx].Y1 = _y;
                this.generations[genidx][personIdx].Y2 = _y + this.adjustedBoxHeights[genidx];


                this.CalcTPZoom(genidx, personIdx);

                personIdx++;
            }

            _y -= this.distanceBetweenGens;

            genidx++;

        }



        //, ref newX1, ref newX2
        this.CreateChildPositionFromParent(this.generations[0][0]); //sets workingXs 

        this.generations[0][0].X1 = this.workingX1;
        this.generations[0][0].X2 = this.workingX2;

        this.generations[0][0].IsDisplayed =true;

        genidx = 0;

        this.drawingX1 = this.generations[0][0].X1;
        this.drawingX2 = this.generations[0][0].X2;

        while (genidx < this.generations.length) {
            if (this.drawingX1 > this.generations[genidx][0].X1)
                this.drawingX1 = this.generations[genidx][0].X1;

            if (this.drawingX2 < this.generations[genidx][this.generations[genidx].length - 1].X2)
                this.drawingX2 = this.generations[genidx][this.generations[genidx].length - 1].X2;

            genidx++;
        }

        // top of the screen
        this.drawingY1 = this.generations[this.generations.length - 1][0].Y2;

        //bottom of the screen
        this.drawingY2 = this.generations[0][0].Y1;

        this.drawingHeight = this.generations[0][0].Y1 - this.generations[this.generations.length - 1][0].Y2;

        this.drawingCentre = (this.drawingX2 - this.drawingX1) / 2;
        this.drawingWidth = this.drawingX2 - this.drawingX1;




        this.CreateConnectionLines();


    }        //end compute locations

        this.CreateConnectionLines = function () {

        // this.FamilySpanLines = new List<List<List<TreePoint>>>();

        var middleGeneration = 0.0;
        var middleXChild = 0.0;
        var middleParent = 0.0;
        var middleTopChild = 0.0;
        var bottomParent = 0.0;

        var parentHeight = 0.0;
        var distanceBetweenGens = 0.0;


        var genidx = 0;
        while (genidx < this.generations.length) {

            var personIdx = 0;

            if (genidx + 1 >= this.familySpanLines.length) {
                genidx++;
                continue;
            }


            while (personIdx < this.generations[genidx].length) {
                var _family0 = this.familySpanLines[genidx][personIdx];

                //_family0[_family0.length] = new Array(_secondStorkX, _firstRow);
                              
                _family0 = new Array();
                //familySpanLines[genidx][personIdx].Clear();

                middleTopChild = this.generations[genidx][personIdx].Y1;// + 10
                if (this.generations.length > (genidx + 1)) {
                    parentHeight = (this.generations[genidx + 1][0].Y2 - this.generations[genidx + 1][0].Y1);
                    bottomParent = this.generations[genidx + 1][0].Y1 + parentHeight;// + 10
                    distanceBetweenGens = (this.generations[genidx][personIdx].Y1 - this.generations[genidx + 1][0].Y2);

                    if (this.generations[genidx][personIdx].FatherIdx > 0 || this.generations[genidx][personIdx].MotherIdx > 0) {
                        // top middle of child 
                        middleXChild = (this.generations[genidx][personIdx].X1 + this.generations[genidx][personIdx].X2) / 2;
                        middleGeneration = this.generations[genidx][personIdx].Y1 - (distanceBetweenGens / 2) + 10;
                        // move to top and middle of child
                        // familySpanLines[genidx][personIdx].Add(new TreePoint(middleXChild, middleTopChild));
                        _family0[_family0.length] = new Array(middleXChild, middleTopChild);

                        // move to middle of generations about child
                        // familySpanLines[genidx][personIdx].Add(new TreePoint(middleXChild, middleGeneration));
                        _family0[_family0.length] = new Array(middleXChild, middleGeneration);

                        var patIdx = this.generations[genidx][personIdx].FatherIdx;
                        if (patIdx != -1) {
                            // move to middle generation under parent
                            middleParent = (this.generations[genidx + 1][patIdx].X1 + this.generations[genidx + 1][patIdx].X2) / 2;


                            //familySpanLines[genidx][personIdx].Add(new TreePoint(middleParent, middleGeneration));
                            _family0[_family0.length] = new Array(middleParent, middleGeneration);

                            if (this.drawingHeight > 200) {
                                // move to bottom of parent
                                //familySpanLines[genidx][personIdx].Add(new TreePoint(middleParent, bottomParent));
                                _family0[_family0.length] = new Array(middleParent, bottomParent);
                            }
                            else {
                                //familySpanLines[genidx][personIdx].Add(new TreePoint(middleParent, middleGeneration - 4));
                                _family0[_family0.length] = new Array(middleParent, middleGeneration - 4);
                            }
                            // move to middle generation under parent
                            //familySpanLines[genidx][personIdx].Add(new TreePoint(middleParent, middleGeneration));
                            _family0[_family0.length] = new Array(middleParent, middleGeneration);
                            // move to middle of child
                            // familySpanLines[genidx][personIdx].Add(new TreePoint(middleXChild, middleGeneration));
                            _family0[_family0.length] = new Array(middleXChild, middleGeneration);
                            // move to top and middle of child
                            // familySpanLines[genidx][personIdx].Add(new TreePoint(middleXChild, middleTopChild));
                            _family0[_family0.length] = new Array(middleXChild, middleTopChild);
                        }
                        patIdx = this.generations[genidx][personIdx].MotherIdx;
                        if (patIdx != -1) {
                            middleParent = (this.generations[genidx + 1][patIdx].X1 + this.generations[genidx + 1][patIdx].X2) / 2;
                            // move to middle of generations about child
                            // familySpanLines[genidx][personIdx].Add(new TreePoint(middleXChild, middleGeneration));
                            _family0[_family0.length] = new Array(middleXChild, middleGeneration);

                            //familySpanLines[genidx][personIdx].Add(new TreePoint(middleParent, middleGeneration));
                            _family0[_family0.length] = new Array(middleParent, middleGeneration);

                            if (this.drawingHeight > 200) {
                                // move to bottom of parent
                                //familySpanLines[genidx][personIdx].Add(new TreePoint(middleParent, bottomParent));
                                _family0[_family0.length] = new Array(middleParent, bottomParent);
                            }
                            else {
                                //familySpanLines[genidx][personIdx].Add(new TreePoint(middleParent, middleGeneration - 4));
                                _family0[_family0.length] = new Array(middleParent, middleGeneration - 4);
                            }
                            // move to middle generation under parent
                            // familySpanLines[genidx][personIdx].Add(new TreePoint(middleParent, middleGeneration));
                            _family0[_family0.length] = new Array(middleParent, middleGeneration);
                            // move to middle of child
                            // familySpanLines[genidx][personIdx].Add(new TreePoint(middleXChild, middleGeneration));
                            _family0[_family0.length] = new Array(middleXChild, middleGeneration);
                            // move to top and middle of child

                            //familySpanLines[genidx][personIdx].Add(new TreePoint(middleXChild, middleTopChild));
                            _family0[_family0.length] = new Array(middleXChild, middleTopChild);
                        } //end (patIdx != -1)
                    } //end  if (this.generations[genidx][personIdx].FatherIdx > 0 || this.generations[genidx][personIdx].MotherIdx > 0)
                } //if (this.generations.length > (genidx + 1))


                this.familySpanLines[genidx][personIdx] = _family0;

                personIdx++;
            }

            genidx++;
        }



    } //this.CreateConnectionLines

        this.CreateChildPositionFromParent = function (movePerson) {
        
            this.workingX1 = 0.0;
            this.workingX2 = 0.0;
            var boxWidth = 0.0;

            if (this.adjustedBoxWidths.length > movePerson.GenerationIdx)
            {
                this.boxWidth = this.adjustedBoxWidths[movePerson.GenerationIdx];
                }
            else
            {
                this.boxWidth = this.boxWidth;
                }

            if (movePerson.FatherIdx == -1)
            {
                this.workingX1 = ((this.generations[movePerson.GenerationIdx + 1][movePerson.MotherIdx].X1 + this.generations[movePerson.GenerationIdx + 1][movePerson.MotherIdx].X2) / 2) - (this.boxWidth / 2);
                this.workingX2 = this.workingX1 + this.boxWidth;
            }

            if (movePerson.MotherIdx == -1)
            {
                this.workingX1 = ((this.generations[movePerson.GenerationIdx + 1][movePerson.FatherIdx].X1 + this.generations[movePerson.GenerationIdx + 1][movePerson.FatherIdx].X2) / 2) - (this.boxWidth / 2);
                this.workingX2 = this.workingX1 + this.boxWidth;
            }

            var parentX1 = 0.0;
            var parentX2 = 0.0;

            if (movePerson.FatherIdx != -1 && movePerson.MotherIdx != -1)
            {
                parentX2 = this.generations[movePerson.GenerationIdx + 1][movePerson.MotherIdx].X2;
                parentX1 = this.generations[movePerson.GenerationIdx + 1][movePerson.FatherIdx].X1;

                if (movePerson.FatherIdx > movePerson.MotherIdx)
                {
                    parentX2 = this.generations[movePerson.GenerationIdx + 1][movePerson.FatherIdx].X2;
                    parentX1 = this.generations[movePerson.GenerationIdx + 1][movePerson.MotherIdx].X1;
                }

                this.workingX1 = ((parentX2 + parentX1) / 2) - ((movePerson.X2 - movePerson.X1) / 2);
                this.workingX2 = this.workingX1 + (movePerson.X2 - movePerson.X1);
            }
        }

        this.GetNewX = function (genidx, percentageLess, personIdx) {


            var adjustedBoxHeight = 0.0;
            var adjustedDistanceApart = 0.0;
            var adjustedBoxWidth = 0.0;

            var childIdx = this.generations[genidx][personIdx].ChildIdx;

            if (genidx > 0) {
                adjustedBoxHeight = this.boxHeight - ((this.boxHeight / 100) * percentageLess);
                var childBoxWidth = (this.generations[genidx - 1][childIdx].X2 - this.generations[genidx - 1][childIdx].X1);
                var childCentrePoint = this.generations[genidx - 1][childIdx].X1 + (childBoxWidth / 2);
                adjustedDistanceApart = this.distanceBetweenBoxs - ((this.distanceBetweenBoxs / 100) * percentageLess);
                adjustedBoxWidth = childBoxWidth - ((childBoxWidth / 100) * percentageLess);

                var isFirstParent = false;
                var isLastParent = false;
                var isSingleParent = false;

                //trying to determine which of the parents we are refering to
                // because if its the first then x value will be lower than it would be for 2nd 
                if (this.generations[genidx].length > personIdx + 1) {
                    if (this.generations[genidx][personIdx + 1].ChildIdx == this.generations[genidx][personIdx].ChildIdx) {
                        isFirstParent = true;
                    }

                }

                if (personIdx > 0) {
                    if (this.generations[genidx][personIdx].ChildIdx == this.generations[genidx][personIdx - 1].ChildIdx) {
                        isLastParent = true;
                    }
                }

                if (!isFirstParent && !isLastParent) {
                    isSingleParent = true;
                }
                if (isFirstParent) {
                    this.newX1 = childCentrePoint - (adjustedDistanceApart / 2) - adjustedBoxWidth;

                }

                if (isLastParent) {
                    this.newX1 = childCentrePoint + (adjustedDistanceApart / 2);
                }

                if (isSingleParent) {
                    this.newX1 = childCentrePoint - (adjustedBoxWidth / 2);
                }


                // newX1 = initialCentrePoint - newX1;
            }
            else {
                adjustedBoxHeight = this.boxHeight;
                adjustedBoxWidth = this.boxWidth;
                this.newX1 = this.centrePoint;
            }



            if (this.adjustedDistances.length > genidx) {
                this.adjustedDistances[genidx] = adjustedDistanceApart;
            }
            else {
                this.adjustedDistances[this.adjustedDistances.length] = adjustedDistanceApart;
            }

            if (this.adjustedBoxWidths.length > genidx) {
                this.adjustedBoxWidths[genidx] = adjustedBoxWidth;
            }
            else {
                //this.adjustedBoxWidths[this.adjustedBoxWidths.length] = adjustedBoxWidth;
                this.adjustedBoxWidths.push(adjustedBoxWidth);
            }


            if (this.adjustedBoxHeights.length > genidx) {
                this.adjustedBoxHeights[genidx] = adjustedBoxHeight;
            }
            else {
                //this.adjustedBoxHeights[this.adjustedBoxWidths.length] = adjustedBoxHeight;
                this.adjustedBoxHeights.push(adjustedBoxHeight);
            }




            this.newX2 = this.newX1 + adjustedBoxWidth;


        } //end getnewx

        this.getMoveList = function (person, startGen) {

            this.moveList = new Array();
            var moveGenIdx = startGen;

           
            while (moveGenIdx > 0)
            {

                if (!this.moveList.ContainsPerson(this.generations[moveGenIdx][person]))
                {
                    this.moveList.push(this.generations[moveGenIdx][person]);
                }

                person = this.generations[moveGenIdx][person].ChildIdx;

                moveGenIdx--;
            }


        } //end  this.getMoveList = function (person, startGen) {

}