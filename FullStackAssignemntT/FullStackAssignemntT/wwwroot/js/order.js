/*code examples from https://datatables.net/ how to use data table API*/
/*01.11.Tatiana display datatabse of Orders */

var dataTable;


$(document).ready(function () {
    //get url data
    var url = window.location.search;
    //add status to filter data by status
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    } else if (url.includes("completed")) {
        loadDataTable("completed");
    } else if (url.includes("approoved")) {
        loadDataTable("approoved");
    } else {
        loadDataTable("all");
    }
});



function loadDataTable(status) {
    dataTable = $('#productTable').DataTable({
        "ajax": {
            "url": "/Order/GetAll?status="+status
        },
        "columns": [
            { "data": "id", "width": "15%" },
            { "data": "name", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "applicationUser.email", "width": "15%" },
            { "data": "orderStatus", "width": "15%" },
            { "data": "orderTotal", "width": "15%" },
            {
                /*display buttons Details to let chekc order details*/

                "data":"id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Order/Details?orderId=${data}"
                        class="btn btn-primary mx-1"> Details</a>                        
					</div>
                        `
                },
                "width": "15%"
            }
            ]
    });
}

