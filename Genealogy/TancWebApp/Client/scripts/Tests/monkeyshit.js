
$.prototype.extend  = function extend(){
    for(var i=1; i<arguments.length; i++)
        for(var key in arguments[i])
            if(arguments[i].hasOwnProperty(key))
                arguments[0][key] = arguments[i][key];
    return arguments[0];
};


//module.exports.Gorilla = Gorilla;

//exports.g_cocos = function () {
  
  //var g = new Gorilla();
  //return g.no_eatten_coconuts;
  
//};



var MonkeyBase = function () {
    this.no_eatten_bannanas =0;

};

MonkeyBase.prototype = {
    EatBannana:function(){
        console.log('eatting bannana');
        this.no_eatten_bannanas++;
    }
    
};




var Gorilla = function(){
    this.no_eatten_coconuts =11;
    $.extend(this, new MonkeyBase());
};

Gorilla.prototype = {
    EatCoconut:function(){
        console.log('eatting coconut');
        this.no_eatten_coconuts++;
    }
    
};

var g = new Gorilla();

console.log(g.no_eatten_coconuts);


