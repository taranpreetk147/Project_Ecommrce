var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll"
        },
        "columns": [
            { "data": "orderId", "width": "15%" },
            { "data": "orderDate", "width": "15%" },
            { "data": "name", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "total", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `                    
                    <div class="text-lg-left">
                    <a href="/Admin/Order/ViewDetails/${data}" class="btn btn-info">
                    </a>                       
                    </div>
                    `;
                }

            }
        ]
    })
}