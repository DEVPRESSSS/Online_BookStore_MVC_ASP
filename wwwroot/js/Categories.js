var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#myTable').DataTable({
        "ajax": {
            url: '/Admin/Category/GetAll',
            dataSrc: 'data'
        },
        columns: [
            { data: 'category_ID', "width": "30%" },
            { data: 'category_Name', "width": "40%" },
            {
                data: 'category_ID',
                "render": function (data) {
                    return `<div class="d-flex justify-content-center btn-group" role="group">
                                <a href="/Admin/Category/Edit/${data}" class="btn btn-info">
                                    <i class="bi bi-pencil-square"></i> EDIT
                                </a>
                                <a onclick="Delete('/Admin/Category/Delete/${data}')" class="btn btn-danger">
                                    <i class="bi bi-trash3"></i> DELETE
                                </a>
                            </div>`;
                },
                "width": "30%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                },
                error: function (xhr, status, error) {
                    toastr.error("Error while deleting record.");
                }
            });
        }
    });
}
