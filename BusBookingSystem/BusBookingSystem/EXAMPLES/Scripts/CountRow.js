(document).ready(function ()
{
    CountRow = function (NumRow)
    {
        return (NumRow + " tr").length - 1;
    }

    ("#msg").text("Applicant(s): " + CountRow("#table1"));

});

http://localhost:2847/Scripts/CountRow.js



