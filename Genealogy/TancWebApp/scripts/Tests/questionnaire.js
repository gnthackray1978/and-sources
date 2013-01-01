var JSMaster, QryStrUtils, AncUtils;

 

$(document).bind("pageinit", function() {
    var questionnaire = new Questionnaire();
    questionnaire.init();
});

var Questionnaire = function () {
    this.selectedcategory = '';
    this.testcategories = [];
    this.questionset = [];

    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.currentQuestionIdx = 0;

};



Questionnaire.prototype = {

    init: function () {



        var finished = function (result) {
            var headersection = '';
            var rows = result.split('\x0A');
            var idx = 1;
            while (idx < rows.length) {
                var cols = rows[idx].split(',');

                this.testcategories.push(cols[2]);
                idx++;
            }
            this.testcategories = this.testcategories.RemoveDupes();

            if (this.testcategories.length > 0 && this.testcategories[0] != undefined) {
                this.processSelect(this.testcategories[0]);
                this.createquestionset();
            }


        };

        var aburl = this.ancUtils.getHost() + '/HtmlPages/tests/questions.csv';

        $.ajax({
            url: aburl,
            data: "query=search_terms",
            success: $.proxy(finished, this)
        });

        $('#main').bind("vclick",
        $.proxy(
        function () {
            var panels = new Panels();
            panels.masterShowTab(1);
            this.createquestionset();
        }, this));

        $('#select').bind("vclick",
        $.proxy(
        function () {
            var panels = new Panels();
            panels.masterShowTab(2);
            this.listtests();
        }, this));

        $('#next').bind("vclick",
        $.proxy(
        function () {
            this.displayQuestion(1);
        }, this));

        $('#prev').bind("vclick",
        $.proxy(
        function () {
            this.displayQuestion(-1);
        }, this));
    },

    listtests: function () {
        var cats = '';
        var selectEvents = [];
        var idx = 0;
        while (idx < this.testcategories.length) {
            if (this.testcategories[idx] != undefined) {
                cats += '<a id= "s' + idx + '" href="index.html" data-role="button" data-theme="b" data-corners="true" data-shadow="true" data-iconshadow="true" data-wrapperels="span" class="ui-btn ui-shadow ui-btn-corner-all ui-btn-up-b"><span class="ui-btn-inner ui-btn-corner-all"><span class="ui-btn-text">' + this.testcategories[idx] + '</span></span></a>';

                selectEvents.push({ key: 's' + idx, value: this.testcategories[idx] });
            }
            idx++;
        }
        this.ancUtils.addlinks(selectEvents, this.processSelect, this);
        $('#categories').html(cats);
    },

    processSelect: function (cat) {
        this.selectedcategory = cat;
        $('#title').html(cat);
    },

    createquestionset: function () {

        var finished = function (result) {
            var headersection = '';
            var rows = result.split('\x0A');
            var idx = 1;
            this.questionset = [];
            while (idx < rows.length) {
                var cols = rows[idx].split(',');
                if (cols[2] == this.selectedcategory) {
                    this.questionset.push({ question: cols[0], answer: cols[1] });
                }
                idx++;
            }
            this.displayQuestion(0);
        };

        $.ajax({
            url: this.ancUtils.getHost() + '/HtmlPages/tests/questions.csv',
            data: "query=search_terms",
            success: $.proxy(finished, this)
        });
    },

    displayQuestion: function (pos) {
        var content = '';
        // if (this.currentQuestionIdx == 0 && pos == -1 ) pos = 0;

        switch (pos) {
            case 1:
                if (this.questionset.length - 1 > this.currentQuestionIdx)
                    this.currentQuestionIdx++;
                break;
            case -1:
                if (this.currentQuestionIdx > 0)
                    this.currentQuestionIdx--;
                break;
            default:
                this.currentQuestionIdx = 0;
        }

        if (this.questionset != undefined && this.questionset.length > 0) {
            $('#count').html(this.currentQuestionIdx + 1 + ' of ' + this.questionset.length);


            var items = this.questionset[this.currentQuestionIdx].answer.split('|');

            if (items.length > 1) {
                var idx = 1;
                content = '<div id="rqs"><fieldset id = "t1" data-role="controlgroup"><legend>' + this.questionset[this.currentQuestionIdx].question + '</legend>';

                while (idx < items.length) {

                    var chkid = 'radio-choice-' + idx;

                    if (idx == 1)
                        content += '<input type="radio" name="radio-choice" id="' + chkid + '" value="choice-' + idx + '" checked="checked" /><label for="' + chkid + '">' + items[idx - 1] + '</label>';
                    else
                        content += '<input type="radio" name="radio-choice" id="' + chkid + '" value="choice-' + idx + '" /><label for="' + chkid + '">' + items[idx - 1] + '</label>';

                    idx++;
                }

                content += '</fieldset></div>';
                $("#answer").removeClass("displayPanel").addClass("hidePanel");
            }
            else {
                content = this.questionset[this.currentQuestionIdx].question;
                $("#answer").removeClass("hidePanel").addClass("displayPanel");
            }

        }
        else {
            content = 'no questions';
            $("#answer").removeClass("displayPanel").addClass("hidePanel");
        }

        $('#mainbody').html(content);
        //how long did it take to work out i needed to call this - on a containing div not the content!!
        $("#rqs").trigger('create');
       
    }


}









            //<fieldset data-role="controlgroup">
            //	<legend>Choose a pet:</legend>
            //     	<input type="radio" name="radio-choice" id="radio-choice-1" value="choice-1" checked="checked" />
            //     	<label for="radio-choice-1">Cat</label>

            //     	<input type="radio" name="radio-choice" id="radio-choice-2" value="choice-2"  />
            //     	<label for="radio-choice-2">Dog</label>

            //     	<input type="radio" name="radio-choice" id="radio-choice-3" value="choice-3"  />
            //     	<label for="radio-choice-3">Hamster</label>

            //     	<input type="radio" name="radio-choice" id="radio-choice-4" value="choice-4"  />
            //     	<label for="radio-choice-4">Lizard</label>
            //</fieldset>
