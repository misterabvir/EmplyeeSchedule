var date = '@ViewBag.Date';

function fill() 
{        
    date;
    $('#wraploading').slideDown(0);
    $.ajax({
        url: "@Url.Action(\"FillMonth\")",
        type: "POST",
        data: "date=" + date,
        success: function () {
            RefreshTable();
        }           
    });      
};


function clear() {

    date;
    var answer = confirm("Вы пытаетесь стереть все данные за этот месяц. Продолжить?")
    if (answer) {
        $('#wraploading').slideDown(0);
        $.ajax({
            url: "@Url.Action(\"ClearMonth\")",
            type: "POST",
            data: "date=" + date,
            success: function () {
                RefreshTable();
            },
        });
    };
}

    function dayConcrete(d) {
        $("#dayyy").slideDown(0);

        $.ajax({
            url: "@Url.Action(\"DayConcrete\")",
            type: "POST",
            data: "date=" + d,
            success: function (response) {
                $("#dayyy").html(response);
            }
        });
    };

    function getData(eid, date) {
        $("#dataaa").slideDown(300);
        $.ajax({
            url: "@Url.Action(\"DayConcreteEm\")",
            type: "POST",
            data: {
                date: date,
                employeId: eid
            },
            success: function (response) {
                $("#dataaa").html(response);
            }
        });
    };
    $(window).load(function () {

        RefreshTable();
        $('#wrapperTable').show(500);
        //$('#filledA').click(function () { fill(); });
        //$('#clearedA').click(function () { clear(); });
    });

    function ref(datenow) {
        date = datenow;
        $.ajax({
            url: "@Url.Action(\"Monthes\")",
            type: "POST",
            data: "date=" + datenow,
            success: function (response) {
                $("#monthdates").html(response);
            }
        });


        $.ajax({
            url: "@Url.Action(\"Tables\")",
            type: "POST",
            data: "date=" + datenow,
            success: function (response) {
                $("#tabledata").html(response);
            }
        });

        $('#wraploading').slideUp(500);
    };


    function RefreshTable() {


        ref(date);


    };
    function closeDiv1() {
        $("#dataaa").slideUp(500);

        RefreshTable();
    };
    function closeDiv2() {
        $("#dayyy").slideUp(500);
        RefreshTable();
    };