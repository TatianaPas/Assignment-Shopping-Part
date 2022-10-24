/*code examples from https://datatables.net/ how to use data table API*/ 

var dataTable;


$(document).ready(function () {
    loadDataTable();
});



function loadDataTable() {
    dataTable = $('#productTable').DataTable({
        "ajax": {
            "url": "/Product/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "color", "width": "15%" },
            { "data": "listPrice", "width": "15%" },
            { "data": "stock", "width": "15%" },
            { "data": "size.name", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            {
                /*display buttons Edit and Delete and pass product ID too*/

                "data":"id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Product/Upsert?id=${data}"
                        class="btn btn-primary mx-1"> Edit</a>
                        <a onClick=Delete('/Product/Delete/${data}')
                        class="btn btn-danger">  Delete</a>
					</div>
                        `
                },
                "width": "15%"
            }
            ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success == true) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })

}