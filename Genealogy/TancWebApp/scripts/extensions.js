
Array.prototype.LinkContainingPoint = function (mx,my) {

    for (var i = 0; i < this.length; i++) {

        if ((this[i].x1 <= mx && this[i].x2 >= mx) 
        && (this[i].y1 <= my && this[i].y2 >= my))                      
        {
            return this[i];
        }
    }



    return null;

};



Array.prototype.ContainsPerson = function (value) {

    for (var i = 0; i < this.length; i++) {

        if (this[i].PersonId == value.PersonId) {
            return true;
        }
    }



    return false;

};


Array.prototype.SortByGenIdx = function()
{
    for(var i=0;i<this.length;i++)
	{
		for(var j=i+1;j<this.length;j++)
		{
			if(Number(this[i].GenerationIdx) < Number(this[j].GenerationIdx))
			{
				var tempValue = this[j];
				this[j] = this[i];
				this[i] = tempValue;
			}
		}
	}
};
