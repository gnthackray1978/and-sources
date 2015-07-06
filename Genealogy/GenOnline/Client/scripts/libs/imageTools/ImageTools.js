var ImageTools = function () {

    this.cookieTools = new CookieTools();

    this.imgIdx = 4;
    //  this.urlroot = 'https://c9.io/gnthackray1978/and-sources/workspace/Genealogy/TancWebApp';


    this.urlroot = '..';

 //   this.setBackground();
    this.ancUtils = new AncUtils();
    this.qryStrUtils = new QryStrUtils();

    this.DEFAULT_BACKGROUND_IMAGES = [
        { url: this.urlroot + '/Images/backgrounds/stone_tree.jpg', name: 'stone_tree', width: 640, height: 800 },
        { url: this.urlroot + '/Images/backgrounds/back_trees.jpg', name: 'back_trees', width: 640, height: 800 },
        { url: this.urlroot + '/Images/backgrounds/back_stones.jpg', name: 'back_stones', width: 640, height: 800 },
        { url: this.urlroot + '/Images/backgrounds/back_hole_stone.jpg', name: 'back_hole_stone', width: 640, height: 800 },
        { url: this.urlroot + '/Images/backgrounds/back_trees_bw.jpg', name: 'back_trees_bw', width: 640, height: 800 }
    ];


    $(window).resize($.proxy(this.setBackground.debounce(250, false), this));
     
};




ImageTools.prototype = {

 
    nextBackground: function () {
        var backIndex = this.cookieTools.getCookie('gnt_back');

        if (backIndex === undefined) {
            this.imgIdx = 1;
        }
        else {
            this.imgIdx = backIndex;

            if (this.imgIdx < this.DEFAULT_BACKGROUND_IMAGES.length - 1)
                this.imgIdx++;
            else
                this.imgIdx = 0;

        }


        this.cookieTools.setCookie("gnt_back", this.imgIdx, 365);

        this.setBackground(this.imgIdx);
    },


    prevBackground: function () {
        var backIndex = this.cookieTools.getCookie('gnt_back');

        if (backIndex === undefined) {
            this.imgIdx = this.DEFAULT_BACKGROUND_IMAGES.length - 1;
        }
        else {
            this.imgIdx = backIndex;

            if (this.imgIdx === 0) {
                this.imgIdx = this.DEFAULT_BACKGROUND_IMAGES.length - 1;
            }
            else {
                this.imgIdx--;
            }
        }

        this.cookieTools.setCookie("gnt_back", this.imgIdx, 365);

        this.setBackground(this.imgIdx);
    },
   
    setBackground: function () {

        //alert('set back');
        this.imgIdx = this.cookieTools.getCookie('gnt_back');

        if (this.imgIdx === undefined)
            this.imgIdx = 0;

        $('.mainbackground').css("background", "url(" + this.DEFAULT_BACKGROUND_IMAGES[this.imgIdx].url + ") no-repeat "); //center top


        $('.mainbackground').css("position", "absolute");
        $('.mainbackground').css("height", "1290px");
        $('.mainbackground').css("z-index", "-1");
        $('.mainbackground').css("background-size", "100% 100%");
        $('.mainbackground').css("background-attachment", "fixed");

        //stops annoying white bars appearing when you scroll around.
        if (window.innerWidth > 1200) {
            $('.mainbackground').css("width", "100%");
        }
        else {
            $('.mainbackground').css("width", "1250px");
        }

        if (this.imgIdx == 7) {

            $('.middle').css("background-color", "");

            $('.middle').css("opacity", "");
            $('.middle').css("filter", "");


            $('.middle').css("border", "thin solid #000000");

            $('.middle').css("background-image", "url('../Images/main_back.gif')");
        }
        else {


            $('.middle').css("background-color", "#FFFFFF");

            $('.middle').css("opacity", "0.9");
            $('.middle').css("filter", "alpha(opacity=90)");

            $('.middle').css("border", "");

            $('.middle').css("background-image", "");
        }
    }



}
