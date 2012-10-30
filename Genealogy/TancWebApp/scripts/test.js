var template = '<div class="preview">' +
						'<span class="imageHolder">' +
							'<img />' +
							'<span class="uploaded"></span>' +
						'</span>' +
						'<div class="progressHolder">' +
							'<div class="progress"></div>' +
						'</div>' +
					'</div>';


function createImage(file) {

    var preview = $(template),
			image = $('img', preview);

    var reader = new FileReader();

    image.width = 100;
    image.height = 100;

    reader.onload = function (e) {

        // e.target.result holds the DataURL which
        // can be used as a source of the image:

        image.attr('src', e.target.result);
    };

    // Reading the file as a DataURL. When finished,
    // this will trigger the onload function above:
    reader.readAsDataURL(file);

    message.hide();
    preview.appendTo(dropbox);

    // Associating a preview container
    // with the file, using jQuery's $.data():

    $.data(file, preview);
}


$(function () {

    var dropbox = $('#dropbox'),
		message = $('.message', dropbox);

    dropbox.filedrop({
        // The name of the $_FILES entry:
        paramname: 'pic',

        maxfiles: 5,
        maxfilesize: 2, // in mb
        url: 'post_file.php',

        uploadFinished: function (i, file, response) {
            $.data(file).addClass('done');
            // response is the JSON object that post_file.php returns
        },

        error: function (err, file) {
            switch (err) {
                case 'BrowserNotSupported':
                    showMessage('Your browser does not support HTML5 file uploads!');
                    break;
                case 'TooManyFiles':
                    alert('Too many files! Please select 5 at most!');
                    break;
                case 'FileTooLarge':
                    alert(file.name + ' is too large! Please upload files up to 2mb.');
                    break;
                default:
                    break;
            }
        },

        // Called before each upload is started
        beforeEach: function (file) {
            if (!file.type.match(/^image\//)) {
                alert('Only images are allowed!');

                // Returning false will cause the
                // file to be rejected
                return false;
            }
        },

        uploadStarted: function (i, file, len) {
            createImage(file);
        },

        progressUpdated: function (i, file, progress) {
            $.data(file).find('.progress').width(progress);
        }

    });

    var template = '...';

    function createImage(file) {
        // ... see above ...
    }

    function showMessage(msg) {
        message.html(msg);
    }

});











<!DOCTYPE html>
<html lang="en">
<head>
<title>demos... | Multi</title>
<meta charset="UTF-8">
<link rel="stylesheet" href="themes/cleap/css/reset.css" media="screen" charset="utf-8">
<link rel="stylesheet" href="themes/cleap/css/theme.css" media="screen" charset="utf-8">
</head>
<!--[if lt IE 7 ]> <body class="ie6 "> <![endif]-->
<!--[if IE 7 ]> <body class="ie7 "> <![endif]-->
<!--[if IE 8 ]> <body class="ie8 "> <![endif]-->
<!--[if !IE]>--> <body class=""> <!--<![endif]-->
<!-- wondering wtf that ^^^ is?
check: http://paulirish.com/2008/conditional-stylesheets-vs-css-hacks-answer-neither/
-->
<div id="wrapper">
<div id="banner">
<h1 class="logo">
<a href="./"><span>demos...</span></a>
</h1>
</div>
<div id="navigation">
<ul class="level-1"><li class="level-1">
<a class="level-1" href="./"><span>Home</span></a><li class="level-1">
<a class="level-1" href="./idle-timer"><span>Idle Timer</span></a><li class="level-1">
<a class="level-1" href="./fail-blog-pipe"><span>Fail Blog Pipe</span></a><li class="level-1">
<a class="level-1" href="./css-hacks"><span>CSS Hacks</span></a><li class="level-1">
<a class="level-1" href="./log()"><span>log()</span></a><li class="level-1"><a class="level-1" href="./resize">
<span>Resize</span></a><li class="level-1"><a class="level-1" href="./infscr"><span>Infscr</span></a><li class="level-1">
<a class="level-1" href="./annotate"><span>Annotate</span></a><li class="level-1"><a class="level-1" href="./invert"><span>Invert</span></a>
<li class="level-1 selected current"><a class="level-1 selected current" href="./multi"><span>Multi</span></a><li class="level-1"><a class="level-1" href="./inline-svg"><span>Inline SVG</span></a></ul>
</div>
<div id="content-wrapper">
<div id="sub-navigation">
<div id="navigation-breadcrumb">
<a class="level-1 selected current" href="./multi"><span>Multi</span></a>
</div>
<div id="navigation-children">
</div>
</div>
<div id="content">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<h2>Multitouch Demo for <a href="https://dvcs.w3.org/hg/webevents/raw-file/tip/touchevents.html">Touch events</a>-supporting browsers on Canvas</h2>
<style media="screen">
canvas { border: 1px solid #CCC; }
</style>
<canvas id="example" height=450 width=300></canvas>
<script class="jsbin" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
<script>




// canvasDrawr originally from Mike Taylr http://miketaylr.com/
// Tim Branyen massaged it: http://timbranyen.com/
// and i did too. with multi touch.
// and boris fixed some touch identifier stuff to be more specific.
var CanvasDrawr = function(options) {
// grab canvas element


var canvas = document.getElementById(options.id),
ctxt = canvas.getContext("2d");
canvas.style.width = '100%'
canvas.width = canvas.offsetWidth;
canvas.style.width = '';
// set props from options, but the defaults are for the cool kids
ctxt.lineWidth = options.size || Math.ceil(Math.random() * 35);
ctxt.lineCap = options.lineCap || "round";
ctxt.pX = undefined;
ctxt.pY = undefined;



var lines = [,,];
var offset = $(canvas).offset();


var self = 
{
//bind click events


init: function() {
//set pX and pY from first click
canvas.addEventListener('touchstart', self.preDraw, false);
canvas.addEventListener('touchmove', self.draw, false);

},


preDraw: function(event) {

    $.each(event.touches, function(i, touch) {
        var id = touch.identifier;
        lines[id] = { x : this.pageX - offset.left, y : this.pageY - offset.top, color : options.color || ["red", "green", "yellow", "blue", "magenta", "orangered"][Math.floor(Math.random() * 6)]};
    });

    event.preventDefault();
},


draw: function(event) {
            var e = event, hmm = {};
            $.each(event.touches, function(i, touch) {
            var id = touch.identifier,
            moveX = this.pageX - offset.left - lines[id].x,
            moveY = this.pageY - offset.top - lines[id].y;
            var ret = self.move(id, moveX, moveY);
            lines[id].x = ret.x;
            lines[id].y = ret.y;
});
event.preventDefault();
},



move: function(i, changeX, changeY) {
        ctxt.strokeStyle = lines[i].color;
        ctxt.beginPath();
        ctxt.moveTo(lines[i].x, lines[i].y);
        ctxt.lineTo(lines[i].x + changeX, lines[i].y + changeY);
        ctxt.stroke();
        ctxt.closePath();
        return { x: lines[i].x + changeX, y: lines[i].y + changeY };
}
};
return self.init();
};


$(function(){
var super_awesome_multitouch_drawing_canvas_thingy = new CanvasDrawr({id:"example", size: 5 });
});
</script>





<Br><br>
<b>Code by</b> <a href="timbranyen.com">Tim Branyen</a>, <a href="http://miketaylr.com">Mike Taylr</a>, Paul Irish & <a href=//smus.com>Boris Smus</a>
<br><br>
coming sometime? : <a href="https://bugzilla.mozilla.org/show_bug.cgi?id=508906">firefox multitouch</a> support
</div>
</div>
<!--
<div id="footer">
This site was created using <a href="http://cms.thewikies.com/">CMS.txt</a>.
</div>
-->
</div>
<script>




var _gaq=[['_setAccount','UA-692547-2'],['_trackPageview']];
(function(d,t){var g=d.createElement(t),s=d.getElementsByTagName(t)[0];g.async=1;
g.src=('https:'==location.protocol?'//ssl':'//www')+'.google-analytics.com/ga.js';
