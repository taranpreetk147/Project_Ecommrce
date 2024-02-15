var dataTable;
$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "10%" },
            { "data": "streetAddress", "width": "10%" },
            { "data": "city", "width": "10%" },
            { "data": "state", "width": "10%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "postalCode", "width": "15%" },
            {
                "data": "isAuthorizedCompany",
                "render": function (data) {
                    if (data) {
                        return `<input type="checkbox" checked disabled /> `;
                    }
                    else {
                        return '<input type="checkbox" disabled />';
                    }
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `                    
                    <div class="text-lg-left">
                    <a href="/Admin/Company/Upsert/${data}" class="btn btn-info">
                    <i class="fas fa-edit"></i>
                    </a>
                    <a class="btn btn-danger" onclick=Delete("/Admin/Company/Delete/${data}")>
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
    //    alert(url);
    swal({
        title: "Want to Delete data?",
        text: "Delete information !",
        icon: "warning",
        buttons: true,
        dangerModel: true
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