var dataTable;
$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Category/GetAll",
            "type": "Get",
            "DataType": "json"
        },
        "columns": [
            {
                "data": "id",
                "render": function (data) {
                    return `                    
                    <div class="text-lg-left">
                    <a href="/Admin/Category/Upsert/${data}" class="btn btn-info">
                    <i class="fas fa-edit"></i>
                    </a>           
                    </div>
                    `;
                }
            },
            { "data": "name", "width": "50%" },
            {
                "data": "id",
                "render": function (data) {
                    return `    
                    <div class="text-lg-left">
                    <a class="btn btn-danger" onclick=Delete("/Admin/Category/Delete/${data}")>
                    <i class="fas fa-trash"></i>
                    </a>
                    </div>
                    `;
                }
            }
        ]
    })
}


function Delete(url) {
    swal({
        title:"Want to delete data",
        text: "Delete information !",
        icon:"warning",
        buttons:true,
        dangerModel:true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message)
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}
        