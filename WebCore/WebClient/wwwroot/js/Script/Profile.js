$(document).ready(function () {
    loadData();
});

function loadData() {
    $.ajax({
        url: "/Account/GetUser",
        data: "",
        cache: false,
        type: "GET",
        dataType: "JSON"
    }).then((result) => {  
        debugger;
        $('#Email').html(result.Item1.Email);
        $('#Create').html(result.Item1.CreateTime);
        $('#Update').html(result.Item1.UpdateTime);
        $('#Name').html(result.Item1.Username);
        $('#Address').html(result.Item1.Address);
        $('#Role').html(result.Item1.RoleName);
        $('#Phone').html(result.Item1.Phone);
    });
}