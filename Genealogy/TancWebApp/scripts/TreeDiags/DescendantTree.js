var TreeBase,TreeUI;



function Tree() {
    console.log('tree created');
    
    $.extend(this, new TreeBase());

    this.distancesbetfam = 0.0;
    this.lowerSpan = 0.0;
    this.middleSpan = 0.0;
    this.topSpan = 0.0;




    this.startx1 = 0.0;
    this.endx2 = 0.0;
    this.familyCount = 0;
    this.curGenPerCount = 0;
    this.firstPX = 0.0;
    this.secondPX = 0.0;
    this.percX1 = 0.0;
    this.percY1 = 0.0;

    this.BaseSetZoom = this.SetZoom;
}


Tree.prototype  = {

    SetZoom: function (p_percentage) {

        var workingtp = this.original_distanceBetweenBoxs / 100;
      
        this.distancesbetfam = workingtp * this.zoomPercentage;

        workingtp = this.original_lowerStalkHeight / 100;

        this.lowerSpan = workingtp * this.zoomPercentage;// (int)original_lowerStalkHeight;

        workingtp = this.original_middleSpan / 100;

        this.middleSpan = workingtp * this.zoomPercentage;//(int)original_middleSpan;

        workingtp = this.original_topSpan / 100;

        this.topSpan = workingtp * this.zoomPercentage;//(int)original_topSpan;


        this.BaseSetZoom(Number(p_percentage));


    },

    DrawTree:function () {



        var canvas = document.getElementById("myCanvas");
        //var context = 
        canvas.getContext("2d");
        canvas.width = window.innerWidth;
        canvas.height = window.innerHeight;


        this.ComputeLocations();

        //var topLeftCornerX = 188;
        //var topLeftCornerY = 50;
        //var width = 200;
        //var height = 100;



        var _genidx = 0;
        var _personIdx = 0;
        //this.generations.length

        var treeUI = new TreeUI(this.screenWidth, this.screenHeight, this.boxWidth, this.boxHeight);

        this.links = [];
        this.buttonLinks = [];

  //      $("#body").remove(".tree_Links");

                     //html('<span>Downloading Descendant Tree</span>');

        while (_genidx < this.generations.length) {
            _personIdx = 0;

            while (_personIdx < this.generations[_genidx].length) {

                var _person = this.generations[_genidx][_personIdx];

                var personLink = treeUI.DrawPerson(_person, this.sourceId, this.zoomPercentage);

                if(personLink !== null)
                    this.links.push(personLink);

                var buttonLink = treeUI.DrawButton(_person, this.GetChildDisplayStatus(_person));

                if(buttonLink !== null)
                    this.buttonLinks.push(buttonLink);
                
                
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


    },

    ComputeLocations:function () {

        if (this.generations.length === 0) {
            return;
        }

        // unused
        var _displayGenCount = 0;
        var _genIdx = 0;

        this.childlessMarriages = [];

        this.drawingX2 = 0.0;
        
        //initialize familyspan array
        while (_genIdx < this.generations.length) {
            var _innerIdx = 0;

            while (_innerIdx < this.familySpanLines[_genIdx].length) {
                this.familySpanLines[_genIdx][_innerIdx] = [];
                _innerIdx++;
            }

            _genIdx++;
        }

        _genIdx = 0;

        var lastPersonY2 = 0.0;

        while (_genIdx < this.generations.length) {

            if (this.IsGenerationDisplayed(_genIdx)) {
                _displayGenCount++;

                this.SetScheduleVars(_genIdx);

                var _current_gen_upper_y = (_genIdx * this.boxHeight) + (_genIdx * this.distanceBetweenGens) + this.centreVerticalPoint;

                var _increment_temp = 0.0;

                var _famIdx = 0;

                this.fillGenXs(_genIdx);

                var familydirectionCounts = this.createFamilyCountArray(_genIdx);

                var _familyIdx = -1;
                var _personIdx = 0;

                while (_personIdx < this.generations[_genIdx].length) {

                    var genPerson = this.generations[_genIdx][_personIdx];


                    if (genPerson.IsDisplayed) {
                      //  console.log('displaying: ' + genPerson.Name);

                        genPerson.X2 = genPerson.X1 + this.boxWidth;

                        var _isDoubleSpouseEnd = false;
                        var _isSpouse = genPerson.IsHtmlLink;

                        if (_isSpouse) {

                            if ((this.generations[_genIdx].length > _personIdx + 1) && this.generations[_genIdx][_personIdx + 1].IsHtmlLink) {
                                _isDoubleSpouseEnd = true;
                                //  console.log('double spouse: ' + _genIdx + ',' + _personIdx);
                            }

                        }

                        var _parent_gen_lower_y = 0.0;
                        if (genPerson.IsFamilyStart) {
                            _familyIdx++;
                            this.familySpanLines[_genIdx][_familyIdx] = [];
                        }



                        if (genPerson.SpouseIdxLst.length > 0 && genPerson.ChildCount === 0) {
                            var spouseIdx = genPerson.SpouseIdxLst[0];
                            var tp = this.generations[_genIdx][spouseIdx].X1;

                            if (Math.abs(spouseIdx - _personIdx) <= 2) {
                                if (this.generations[_genIdx][spouseIdx].ChildCount === 0) {

                                    var myArray = new Array((genPerson.X1 + this.halfBox), (_current_gen_upper_y + this.boxHeight));
                                    this.childlessMarriages[this.childlessMarriages.length] = myArray;

                                    myArray = new Array((genPerson.X1 + this.halfBox), (_current_gen_upper_y + this.boxHeight + this.topSpan));
                                    this.childlessMarriages[this.childlessMarriages.length] = myArray;

                                    myArray = new Array((tp + this.halfBox), (_current_gen_upper_y + this.boxHeight + this.topSpan));
                                    this.childlessMarriages[this.childlessMarriages.length] = myArray;

                                    myArray = new Array((tp + this.halfBox), (_current_gen_upper_y + this.boxHeight));
                                    this.childlessMarriages[this.childlessMarriages.length] = myArray;
                                }
                            }

                        } //end genPerson.SpouseIdxLst.length > 0 && genPerson.ChildCount


                        var _middleParents = 0.0;
                        //   var firstPX = 0.0;
                        //   var secondPX = 0.0;

                        var _thirdStorkX = 0.0;

                        if (_genIdx > 0)
                            _parent_gen_lower_y = this.generations[_genIdx - 1][genPerson.FatherIdx].Y2;

                        var _firstRow = _current_gen_upper_y - this.lowerSpan;
                        var _secondRow = _parent_gen_lower_y + this.middleSpan; // changed with increment later on - need to calculate the maximum and minimum this increment will be
                        var _thirdRow = _parent_gen_lower_y + this.middleSpan;
                        var _fourthRow = _parent_gen_lower_y + this.topSpan;

                        if ((!(genPerson.IsFamilyEnd && _isSpouse)) && _genIdx > 0) {
                            if (!_isDoubleSpouseEnd) {
                                var _family = this.familySpanLines[_genIdx][_familyIdx];

                                //  console.log(genPerson.Name);

                                _family.push(new Array((genPerson.X1 + this.halfBox), _firstRow));

                                if (!_isSpouse)
                                    _family.push(new Array((genPerson.X1 + this.halfBox), _current_gen_upper_y));

                                _family.push(new Array((genPerson.X1 + this.halfBox), _firstRow));
                            }
                        }




                        //regional parental links


                        if (genPerson.IsParentalLink && _genIdx > 0) {
                            _middleParents = this.MiddleParents(_genIdx, genPerson.FatherIdx, genPerson.MotherIdx);

                            var _nextParentLink = this.GetFirst(_genIdx, genPerson.FatherIdx, genPerson.MotherIdx);
                            var _prevParentLink = this.GetPrev(_genIdx, genPerson.FatherIdx, genPerson.MotherIdx);

                            this.GetParentXs(_genIdx, genPerson.FatherIdx, genPerson.MotherIdx);

                            var incSize = 0;

                            incSize = this.distanceBetweenGens - this.middleSpan - this.lowerSpan;
                            incSize = incSize / familydirectionCounts[_famIdx];


                            if (_famIdx === 0) {
                                if (genPerson.X1 > _middleParents)
                                    _increment_temp = this.distanceBetweenGens - this.middleSpan - this.lowerSpan;
                                else
                                    _increment_temp = 0.0;
                            }


                            if (genPerson.X1 > _middleParents) {
                                _increment_temp -= incSize; //original

                                if (_nextParentLink > genPerson.X2)
                                    _thirdStorkX = genPerson.X2;
                                else
                                    _thirdStorkX = _nextParentLink;

                                if ((genPerson.X1 > _middleParents) && (_thirdStorkX > genPerson.X1)) {
                                    _thirdStorkX = genPerson.X1;
                                }
                            }
                            else {
                                _increment_temp += incSize; //original

                                if (_prevParentLink < genPerson.X1)
                                    _thirdStorkX = genPerson.X1;
                                else
                                    _thirdStorkX = _prevParentLink;

                                if ((genPerson.X1 < _middleParents) && (_thirdStorkX < genPerson.X1)) {
                                    _thirdStorkX = genPerson.X1;
                                }
                            }


                            _secondRow += _increment_temp;


                            //tweak start of rows
                            //(Math.abs(double1 - double2) <= precision)

                            if (Math.abs(_firstRow - _secondRow) <= 1) {
                                _secondRow -= (incSize / 2);
                            }





                            var _secondStorkX = genPerson.X1;

                            if (genPerson.IsFamilyStart && genPerson.IsFamilyEnd) {
                                // only child with no spouses!
                                if (_personIdx === 0) {
                                    var _nextFamilyStart = 0;


                                    if (this.generations[_genIdx].Count > 1) {
                                        _nextFamilyStart = this.generations[_genIdx][_personIdx + 1].X1;
                                    }
                                    else {
                                        _nextFamilyStart = this.generations[_genIdx][_personIdx].X2;
                                    }

                                    if (_middleParents < _nextFamilyStart && _middleParents > this.generations[_genIdx][_personIdx].X1) {
                                        _secondStorkX = _middleParents;
                                        _thirdStorkX = _middleParents;
                                    }

                                }
                            }
                            else {
                                // handles situations where lines are overlapping the next or prev
                                // family
                                // happens when there are just 1 or 2 families 
                                // and one of them is unusually large or something like that.

                                if (genPerson.IsFamilyStart) {
                                    // tidy up the link to the parents

                                    var _sizeToAdd = this.halfBox;

                                    if (!genPerson.IsFamilyEnd) {
                                        _sizeToAdd = this.boxWidth;
                                    }

                                    if (_secondStorkX == _thirdStorkX) {
                                        _thirdStorkX += _sizeToAdd;
                                    }


                                    _secondStorkX += _sizeToAdd;


                                }

                            }

                            //endregion
                            var _family0 = this.familySpanLines[_genIdx][_familyIdx];

                            //_family0.push();
                            _family0.push(new Array(_secondStorkX, _firstRow));
                            _family0.push(new Array(_secondStorkX, _secondRow));
                            _family0.push(new Array(_thirdStorkX, _secondRow));
                            _family0.push(new Array(_thirdStorkX, _thirdRow));
                            _family0.push(new Array(_middleParents, _thirdRow));
                            _family0.push(new Array(_middleParents, _fourthRow));
                            _family0.push(new Array(this.firstPX, _fourthRow));
                            _family0.push(new Array(this.firstPX, _parent_gen_lower_y));
                            _family0.push(new Array(this.firstPX, _fourthRow));

                            _family0.push(new Array(this.secondPX, _fourthRow));
                            _family0.push(new Array(this.secondPX, _parent_gen_lower_y));
                            _family0.push(new Array(this.secondPX, _fourthRow));
                            _family0.push(new Array(_middleParents, _fourthRow));
                            _family0.push(new Array(_middleParents, _thirdRow));

                            _family0.push(new Array(_thirdStorkX, _thirdRow));
                            _family0.push(new Array(_thirdStorkX, _secondRow));

                            _family0.push(new Array(_secondStorkX, _secondRow));
                            _family0.push(new Array(_secondStorkX, _firstRow));

                            _famIdx++;
                        } //end (genPerson.IsParentalLink && _genIdx > 0)


                        genPerson.Y1 = _current_gen_upper_y;
                        genPerson.Y2 = _current_gen_upper_y + this.boxHeight;

                        lastPersonY2 = genPerson.Y2;

                        this.CalcTPZoom(_genIdx, _personIdx);

                    } // end (genPerson.IsDisplayed)


                    _personIdx++;
                } //end while

            }


            _genIdx++;
        }

        if (this.generations.length > 0) {
            this.drawingY1 = this.generations[0][0].Y1;
        }

        if (this.generations[_displayGenCount - 1].length > 0) { 
            this.drawingY2 = lastPersonY2;
        }
        //drawingCentreVertical = drawingY2 - drawingY1;
        this.drawingCentre = (this.drawingX2 - this.drawingX1) / 2;
        this.drawingHeight = this.drawingY2 - this.drawingY1;
        this.drawingWidth = this.drawingX2 - this.drawingX1;

    },
    
    IsGenerationDisplayed: function (genidx) {
        //returns if generation is to be displayed
        //also counts displayed families and displayed person

        var idx = 0;
        var isGenDisplayed = false;
        this.curGenPerCount = 0;
        this.familyCount = 0;

        while (idx < this.generations[genidx].length) {

            if (this.generations[genidx][idx].IsDisplayed) {
                if (this.generations[genidx][idx].IsFamilyStart) {
                    this.familyCount++;
                }

                this.curGenPerCount++;

                isGenDisplayed = true;
            }
            idx++;
        }

        return isGenDisplayed;
    },

    //IsGenerationDisplayed
    //must be run first
    SetScheduleVars:function (genidx) {
        var idx = 0;
        var _firstx1 = 0.0;
        var _lastx2 = 0.0;
        var innercount = 0;



        if (genidx === 0) {
            this.drawingX1 = this.startx1;
            this.startx1 = this.centrePoint - (((this.generations[genidx].length * this.boxWidth) + ((this.generations[genidx].length - 1) * this.distanceBetweenBoxs)) / 2);
        }
        else {
 

            var firstPerson = null;
            var lastPerson = null;

            while (idx < this.generations[genidx - 1].length) {
                // in the preceeding generation there should always be at least one person with 
                // a child!!! 
                if (this.generations[genidx - 1][idx].ChildLst.length > 0 &&
                    this.generations[genidx - 1][idx].IsDisplayed) {

                    if (innercount === 0) {
                        _firstx1 = this.generations[genidx - 1][idx].X1;
                        _lastx2 = this.generations[genidx - 1][idx].X1;

                        firstPerson = this.generations[genidx - 1][idx];
                        lastPerson = this.generations[genidx - 1][idx];
                    }
                    else {
                        if (_firstx1 > this.generations[genidx - 1][idx].X1) {
                            _firstx1 = this.generations[genidx - 1][idx].X1;
                            firstPerson = this.generations[genidx - 1][idx];
                        }
                        if (_lastx2 < this.generations[genidx - 1][idx].X1) {
                            _lastx2 = this.generations[genidx - 1][idx].X1;
                            lastPerson = this.generations[genidx - 1][idx];
                        }
                    }
                    innercount++;
                }
                idx++;
            }

            _lastx2 += this.boxWidth;

          //  console.log(genidx);
           // console.log('first and last: ' + _firstx1 + ' ' + _lastx2);
           // console.log('first and last persons: ' + firstPerson.Name + ' ' + lastPerson.Name);

            this.startx1 = _firstx1 + (this.boxWidth / 2);
            this.endx2 = _lastx2 - (this.boxWidth / 2);

            var _prevGenLen = this.endx2 - this.startx1;
            var _curGenLen = 0.0;

            _curGenLen = (this.curGenPerCount * (this.boxWidth + this.distanceBetweenBoxs)) - (this.distanceBetweenBoxs * this.familyCount);

            if (_prevGenLen > _curGenLen) {
                this.distancesbetfam = (_prevGenLen - _curGenLen) / this.familyCount;
            }
            else {
                this.distancesbetfam = (this.original_distancesbetfam / 100) * this.zoomPercentage;
            }

            //add in the distances between the families
            _curGenLen = _curGenLen + (this.distancesbetfam * (this.familyCount - 1));

            // middle of the families of the previous generation
            var _desiredMidPoint = ((this.endx2 - this.startx1) / 2) + this.startx1;

            // set new start point by subtracting half the total space required for the generation
            this.startx1 = _desiredMidPoint - (_curGenLen / 2);
        }

    },
    
    fillGenXs:function (genidx) {

        var idx = 0;
        var _currentDistanceBetweenBoxes = 0.0;
        var innerIdx = 0;
        var prevPerson = null;
        while (idx < this.generations[genidx].length) {

            if (this.generations[genidx][idx].IsDisplayed) {

                if (innerIdx === 0) {
                    this.generations[genidx][idx].X1 = this.startx1;
                    this.generations[genidx][idx].X2 = this.startx1 + this.boxWidth;
                }
                else {
                    if (this.generations[genidx][idx].IsFamilyStart) {
                        _currentDistanceBetweenBoxes = this.distancesbetfam;
                    }
                    else {
                        _currentDistanceBetweenBoxes = this.distanceBetweenBoxs;
                    }

                    this.generations[genidx][idx].X1 = prevPerson.X1 + this.boxWidth + _currentDistanceBetweenBoxes;
                    this.generations[genidx][idx].X2 = this.generations[genidx][idx].X1 + this.boxWidth;
                }
                prevPerson = this.generations[genidx][idx];
                innerIdx++;

            }

            idx++;
        }
    },
    
    createFamilyCountArray:function (genidx) {

        var newswitchs = Array();
        var leftCounter = 0.0;
        var rightCounter = 0.0;
        var idx = 0;

        if (genidx !== 0) {
            while (idx < this.generations[genidx].length) {
                var _tp = this.generations[genidx][idx];
                if (_tp.IsParentalLink &&
                        _tp.IsDisplayed) {
                    newswitchs.push(0.0);
                    if (_tp.X1 > this.MiddleParents(genidx, _tp.FatherIdx, _tp.MotherIdx)) {
                        rightCounter++;
                        if (leftCounter > 0)
                            newswitchs[newswitchs.length - 2] = leftCounter;
                        leftCounter = 0;
                    }
                    else {
                        leftCounter++;
                        if (rightCounter > 0)
                            newswitchs[newswitchs.length - 2] = rightCounter;
                        rightCounter = 0;
                    }
                }
                idx++;
            }
            if (leftCounter !== 0) newswitchs[newswitchs.length - 1] = leftCounter;
            if (rightCounter !== 0) newswitchs[newswitchs.length - 1] = rightCounter;
            idx = newswitchs.length - 1;
            while (idx > 0) {
                if (newswitchs[idx - 1] === 0)
                    newswitchs[idx - 1] = newswitchs[idx];
                idx--;
            }
        }
        return newswitchs;
    },
    
    MiddleParents:function (genidx, fatIdx, motIdx) {
        // return 
        var middleParents = 0.0;
        
 
        // returns 1 less than the furthest parent to the right!
 
        // midx = 22 
        // fidx = 25 // F25 M24
        
       // midx = 25
       // fidx = 22 // 24 25 
        
        // if the parent had more than 1 spouse then 2 parents might not be next to each other in the generation.
        if (Math.abs(fatIdx - motIdx) > 1) {
            if (fatIdx < motIdx) {
                middleParents = (this.generations[genidx - 1][motIdx - 1].X1 + this.generations[genidx - 1][motIdx].X2) / 2;
            }
            else {
                middleParents = (this.generations[genidx - 1][fatIdx - 1].X1 + this.generations[genidx - 1][fatIdx].X2) / 2;
            }
        }
        else {
            middleParents = (this.generations[genidx - 1][fatIdx].X1 + this.generations[genidx - 1][motIdx].X2) / 2;
        }

        return middleParents;
    },
    
    GetParentXs:function (genidx, fatIdx, motIdx) {

        if (genidx < 1) {
            this.secondPX = this.centrePoint;
            this.firstPX = this.centrePoint;
        }
        else {
            if (this.generations[genidx - 1][fatIdx].X1 > this.generations[genidx - 1][motIdx].X1) {
                this.secondPX = this.generations[genidx - 1][fatIdx].X1 + this.halfBox;
                this.firstPX = this.generations[genidx - 1][motIdx].X1 + this.halfBox;

            }
            else {
                this.secondPX = this.generations[genidx - 1][motIdx].X1 + this.halfBox;
                this.firstPX = this.generations[genidx - 1][fatIdx].X1 + this.halfBox;
            }
        }

    },
    
    GetFirst:function (genidx, fatIdx, motIdx) {
        var middleParents = this.MiddleParents(genidx, fatIdx, motIdx);
        var nextParentLink = middleParents;
        var idxParentLink = motIdx;

        // if we only have 1 parent, but that parent 
        // later remarries we want the next nextparent setting to the current parents edge
        if (fatIdx == motIdx) {
            //remember fatidx and motidx are the same!
            if (this.generations[genidx - 1][fatIdx].SpouseLst.length > 0) {
                return this.generations[genidx - 1][fatIdx].X2;
            }
        }

        // if multiple spouses set next parent as end of first one
        if (this.generations[genidx - 1][fatIdx].SpouseLst.length > 1) {
            if (Math.abs(fatIdx - motIdx) == 1) {
                return nextParentLink;
            }
        }

        if (fatIdx > motIdx) idxParentLink = fatIdx;

        var rightX2OfCurrentParent = this.generations[genidx - 1][idxParentLink].X2;

        var idx = 0;
        
        var _treePerson = null;

        var isFound = false;
        while (idx < this.generations[genidx - 1].length) {
            if (this.generations[genidx - 1][idx].IsDisplayed
                && this.generations[genidx - 1][idx].ChildCount > 0
                && this.generations[genidx - 1][idx].X1 > rightX2OfCurrentParent) {
                isFound = true;
                _treePerson = this.generations[genidx - 1][idx];
                break;
            }
            idx++;
        }

        if (isFound)
            nextParentLink = this.generations[genidx - 1][idx].X1;

        return nextParentLink;
    },
    
    GetPrev:function (genidx, fatIdx, motIdx) {

        var middleParents = (this.generations[genidx - 1][fatIdx].X1 + this.generations[genidx - 1][motIdx].X2) / 2;

        var prevParentLink = middleParents;

        //left parent
        var idxParentLink = fatIdx;
        if (fatIdx > motIdx) idxParentLink = motIdx;

        var currentParentsLeft = this.generations[genidx - 1][idxParentLink].X1;

        if (this.generations[genidx - 1][fatIdx].SpouseLst.length > 1) {

            if (Math.abs(fatIdx - motIdx) == 2) {
                return prevParentLink;
            }

        }


        var idx = 0;

        var _treePerson = null;

        while (idx < this.generations[genidx - 1].length) {
            if (this.generations[genidx - 1][idx].IsDisplayed
                    && this.generations[genidx - 1][idx].ChildCount > 0
                    && this.generations[genidx - 1][idx].X1 < currentParentsLeft) {
                _treePerson = this.generations[genidx - 1][idx];

            }
            idx++;
        }

       // if(_treePerson != null)
     //   console.log('last person ' + _treePerson.Name);

        if (_treePerson != null)
            prevParentLink = _treePerson.X2;


        return prevParentLink;
    }

}