var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#myTable').DataTable({
        "ajax": {
            url: '/Admin/Company/GetAll',
            dataSrc: 'data'
        },
        columns: [
            { data: 'id', "width": "10%" },
            { data: 'companyName', "width": "20%" },
            { data: 'streetAddress', "width": "30%" },
            { data: 'city', "width": "10%" },
            { data: 'state', "width": "10%" },
            { data: 'postalCode', "width": "10%" },
            { data: 'phoneNumber', "width": "10%" },

            {
                data: 'id',
                "render": function (data) {
                    return `<div class="d-flex justify-content-center btn-group" role="group">
                                <a href="/Admin/Company/Edit/${data}" class="btn btn-info">
                                    <i class="bi bi-pencil-square"></i> EDIT
                                </a>
                                <a onclick="Delete('/Admin/Company/Delete/${data}')" class="btn btn-danger">
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
