var dataTable;

$(document).ready(function () {
    loadDataTable();
});

dataTable = $('#myTable').DataTable({
    "ajax": {
        url: '/Book/GetALLBooks', // Ensure correct URL
        dataSrc: 'data', // Ensure correct data source property
        cache: false, // Disable caching
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Cache-Control', 'no-cache');
            xhr.setRequestHeader('Pragma', 'no-cache');
        }
    },
    columns: [
        { data: 'book_Id', "width": "5%" },
        { data: 'title', "width": "10%" },
        { data: 'description', "width": "20%" },
        { data: 'isbn', "width": "10%" },
        { data: 'price', "width": "10%" },
        { data: 'publishDate', "width": "10%" },
        { data: 'publisher', "width": "10%" },
        { data: 'stock', "width": "10%" },
        { data: 'category_ID', "width": "5%" },
        {
            data: 'book_Id',
            "render": function (data) {
                return `<div class="d-flex justify-content-center btn-group" role="group">
                            <a href="/book/edit/${data}" class="btn btn-info">
                                <i class="bi bi-pencil-square"></i> EDIT
                            </a>
                            <a onclick="Delete('/book/delete/${data}')" class="btn btn-danger">
                                <i class="bi bi-trash3"></i> DELETE
                            </a>
                        </div>`;
            },
            "width": "10%"
        }
    ]
});



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
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    });
}