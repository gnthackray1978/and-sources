 
var TreeUI = function (screen_width, screen_height, box_width, box_height) {

    this.screen_width = screen_width;
    this.screen_height = screen_height;
    this.canvas = document.getElementById("myCanvas");
    this.context = this.canvas.getContext("2d");
    this.boxWidth = box_width;
    this.boxHeight = box_height;
    
    this.docClose = new Image();
    this.docNew = new Image();
    
    this.docClose.src = '/Images/icons/24x24/plus.png';
    this.docNew.src = '/Images/icons/24x24/minus.png';
};



TreeUI.prototype = {

    DrawLine: function (points) {

        var _pointIdx = 0;
        this.context.beginPath();
        var _validLine = false;
        var _sx1 = -100; //screen left
        var _sx2 = this.screen_width + 100; // screen right 
        var _sy1 = -100;
        var _sy2 = this.screen_height + 100;


        while (_pointIdx < points.length) {
            var _Point = points[_pointIdx];


            if (((_Point[0] > _sx1) && (_Point[0] < _sx2)) && ((_Point[1] > _sy1) && (_Point[1] < _sy2))) {
                _validLine = true;
                break;
            }

            // but its also valid if line crosses the screen!
            if (_pointIdx > 0 && !_validLine) {
                var _prevPoint = points[_pointIdx - 1];
                if ((_prevPoint[0] > _sx1 && _prevPoint[0] < _sx2 && _Point[0] > _sx1 && _Point[0] < _sx2) && ((_prevPoint[1] < _sy2 && _Point > _sy2) || (_prevPoint[1] < _sy1 && _Point[1] > _sy2))) _validLine = true;
                if ((_prevPoint[1] > _sy1 && _prevPoint[1] < _sy2 && _Point[1] > _sy1 && _Point[1] < _sy2) && ((_prevPoint[0] < _sx2 && _Point > _sx2) || (_prevPoint[0] < _sx1 && _Point[0] > _sx1))) _validLine = true;
               
                if(_validLine)
                    break;
            }

            _pointIdx++;
        }


        if (_validLine) {
            _pointIdx = 0;
            while (_pointIdx < points.length) {
                var _Point = points[_pointIdx];
                if (_pointIdx === 0) {
                    this.context.moveTo(_Point[0], _Point[1]);
                }
                else {
                    this.context.lineTo(_Point[0], _Point[1]);
                }
                _pointIdx++;
            }

            this.context.lineWidth = 2;
            this.context.strokeStyle = "black";
            this.context.stroke();
        }
    },
    DrawButton: function (_person, checked) {
        var linkArea = { x1: 0, x2: 0, y1: 0, y2: 0, action: 'box' };
        //
        if (_person.IsDisplayed &&
                    _person.X2 > 0 &&
                    _person.X1 < this.screen_width &&
                    _person.Y2 > -100 &&
                    _person.Y1 < this.screen_height &&
                    _person.ChildLst.length > 0 &&
                    _person.zoom >= 3) {

            // this doesnt correspond to the isdisplayed person of the property
            // because obviously the we want the parent to stay visible so we
            // can turn on and off the childrens visibility. if we cant see it , we cant turn anything on and off..
            if (checked) {
                //  this.context.fillStyle = "red";
                this.context.drawImage(this.docClose, _person.X2 - 24, _person.Y2 - 24);
            }
            else {
                // this.context.fillStyle = "black";
                this.context.drawImage(this.docNew, _person.X2 - 24, _person.Y2 - 24);
            }


            //   this.context.fill();
            //   this.context.lineWidth = 1;
            //   this.context.strokeStyle = "black";
            //   this.context.stroke();


            linkArea.y1 = _person.Y2 - 15;
            linkArea.x1 = _person.X2 - 15;
            linkArea.y2 = _person.Y2;
            linkArea.x2 = _person.X2;
            linkArea.action = _person.PersonId + "," + String(checked);
        }
        else {
            linkArea = null;
        }
        return linkArea;

    },
    DrawPerson: function (_person, sourceId, zoomPerc) {

        var linkArea = { x1: 0, x2: 0, y1: 0, y2: 0, action: '' };

        if (_person.IsDisplayed &&
                    _person.X2 > 0 &&
                    _person.X1 < this.screen_width &&
                    _person.Y2 > -100 &&
                    _person.Y1 < this.screen_height) {


            this.context.beginPath();


            //   this.context.rect(_person.X1, _person.Y1, this.boxWidth, this.boxHeight);

            if (_person.zoom >= 5) {
                var rectX = _person.X1;
                var rectY = _person.Y1;
                var rectWidth = Math.abs(_person.X2 - _person.X1);
                var rectHeight = Math.abs(_person.Y2 - _person.Y1);
                var radius = 10;

                this.context.strokeStyle = "#99003A";
                this.context.lineWidth = 2;
                this.RoundedRect(this.context, rectX, rectY, rectWidth, rectHeight, radius);


            }
            else {
                this.context.rect(_person.X1, _person.Y1, Math.abs(_person.X2 - _person.X1), Math.abs(_person.Y2 - _person.Y1));

                this.context.fillStyle = "white";
                this.context.fill();
                this.context.lineWidth = 1;
                this.context.strokeStyle = "black";
                this.context.stroke();

            }


            var linespacing = 15;

            if (_person.zoom >= 7) {
                linespacing = 30;

            }


            var _y = this.WriteName(_person.X1 + 3, _person.Y1 + 12, _person, 0);

            if (_person.IsHtmlLink) {
                linkArea.y1 = _person.Y1;
                linkArea.x1 = _person.X1 + 3;
                linkArea.y2 = _y - linespacing;
                linkArea.x2 = _person.X2;
                linkArea.action = _person.PersonId;
            }
            else {
                linkArea = null;
            }



            switch (_person.zoom) {

                case 4: //show name
                    this.context.fillText("Dob: " + _person.DOB, _person.X1 + 3, _y);
                    _y += linespacing;
                    if (_y <= _person.Y2 - 10) {
                        _y = this.WriteBLocation(_person.X1 + 3, _y, _person, 1); //+ linespacing
                    }
                    break;
                case 5: //show name
                case 6:
                case 7:
                case 8:

                    this.context.fillText("Dob: " + _person.DOB, _person.X1 + 3, _y);
                    _y += linespacing;

                    if (_y <= _person.Y2 - 10) {
                        _y = this.WriteBLocation(_person.X1 + 3, _y, _person, 2); //+ linespacing
                    }

                    if (_y <= _person.Y2 - 10) {
                        this.context.fillText("Dod: " + _person.DOD, _person.X1 + 3, _y);

                        _y += linespacing;

                        this.WriteDLocation(_person.X1 + 3, _y, _person, 2);
                    }
                    break;

            }



        }

        //typically used in descendanttree drawtree method
        //adds to basetree links collection
        return linkArea;

    },
    RoundedRect: function (ctx, x, y, width, height, radius) {
        ctx.beginPath();
        ctx.moveTo(x, y + radius);
        ctx.lineTo(x, y + height - radius);
        ctx.quadraticCurveTo(x, y + height, x + radius, y + height);
        ctx.lineTo(x + width - radius, y + height);
        ctx.quadraticCurveTo(x + width, y + height, x + width, y + height - radius);
        ctx.lineTo(x + width, y + radius);
        ctx.quadraticCurveTo(x + width, y, x + width - radius, y);
        ctx.lineTo(x + radius, y);
        ctx.quadraticCurveTo(x, y, x, y + radius);
        ctx.stroke();
    },
    WriteName: function (xpos, ypos, _person, maxlines) {
        this.context.font = "bold 8pt Calibri";

        if (_person.IsHtmlLink) {
            this.context.fillStyle = "#1600BF";
        }
        else {
            this.context.fillStyle = "black";
        }

        var _textToDisplay = this.MakeArray(_person, _person.Name);
        var _y = ypos;

        var linespacing = 15;

        if (maxlines === 0 || _textToDisplay.length <= maxlines) {
            maxlines = _textToDisplay.length;
        }

        for (var i = 0; i < maxlines; i++) {
            this.context.fillText(_textToDisplay[i], xpos, _y);
            _y += linespacing;
        }

        //reset to black.
        this.context.font = "8pt Calibri";
        this.context.fillStyle = "black";
        return _y;
    },
    WriteBLocation: function (xpos, ypos, _person, maxlines) {
        this.context.font = "8pt Calibri";
        this.context.fillStyle = "black";

        _person.BirthLocation = _person.BirthLocation.replace(",", " ");
        _person.BirthLocation = _person.BirthLocation.replace("  ", " ");

        var _textToDisplay = this.MakeArray(_person, _person.BirthLocation);
        var _y = ypos;

        var linespacing = 15;


        if (maxlines === 0 || _textToDisplay.length <= maxlines) {
            maxlines = _textToDisplay.length;
        }


        for (var i = 0; i < maxlines; i++) {
            this.context.fillText(_textToDisplay[i], xpos, _y);
            _y += linespacing;
        }
        return _y;
    },
    WriteDLocation: function (xpos, ypos, _person, maxlines) {
        this.context.font = "8pt Calibri";
        this.context.fillStyle = "black";

        var _textToDisplay = this.MakeArray(_person, _person.DeathLocation);

        var _y = ypos;

        var linespacing = 15;

        if (maxlines === 0 || _textToDisplay.length <= maxlines) {
            maxlines = _textToDisplay.length;
        }

        for (var i = 0; i < maxlines; i++) {
            if (_y < _person.Y2 - 10) {
                this.context.fillText(_textToDisplay[i], xpos, _y);
                _y += linespacing;
            }
        }
        return _y;
    },
    MakeArray: function (person, parseStr) {

        var name = '';
        var nameAr = [];
        var i = 0;
        var character_width = 3;
        var max_text_width = Math.abs(person.X2 - person.X1);
        var max_char_count = max_text_width / character_width;

        switch (person.zoom) {

            case 1:
                nameAr.push('');
                break;
            case 2:

                if (parseStr !== '') {
                    var parts = parseStr.split(' ');

                    for (i = 0; i < parts.length; i++) {

                        if (parts[i].length > 0)
                            name += parts[i].slice(0, 1) + " ";
                    }
                }
                nameAr.push(name);
                break;
            case 3:
            case 4:
            case 5:
            case 6:

                nameAr = parseStr.split(' ');

                for (i = 0; i < nameAr.length; i++) {

                    if ((nameAr[i].length * character_width) > max_text_width)
                        nameAr[i] = nameAr[i].slice(0, max_char_count - 1) + " ";
                }
                break;
            case 7:
            case 8:
                nameAr.push(parseStr);
                break;
        }

        return nameAr;
    }
};

 